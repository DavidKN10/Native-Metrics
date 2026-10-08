#include <NativeMetrics/Gpu.hpp>

bool sameLuid(LUID& hardwareLuid, LUID& softwareLuid) {
    return hardwareLuid.HighPart == softwareLuid.HighPart && 
        hardwareLuid.LowPart == softwareLuid.LowPart;
}

void getAdapterDesc(GpuInfo& adapter, DXGI_ADAPTER_DESC2& desc) {
    adapter.vendorId = desc.VendorId;

    adapter.dedicatedVideoMemoryBytes = static_cast<f64>(desc.DedicatedVideoMemory);
    adapter.sharedSystemMemoryBytes = static_cast<f64>(desc.SharedSystemMemory);
}

void udpateDedicatedMemoryUsage(GpuInfo& adapter, DXGI_QUERY_VIDEO_MEMORY_INFO& localMemoryInfo) {
    adapter.localCurrentMemoryBytes = static_cast<f64>(localMemoryInfo.CurrentUsage);
    
    adapter.currentUsagePercent = (
        adapter.localCurrentMemoryBytes / 
        adapter.dedicatedVideoMemoryBytes) * 100.0;
}

void updateSharedMemoryUsage(GpuInfo& adapter, DXGI_QUERY_VIDEO_MEMORY_INFO& nonLocalMemoryInfo) {
    adapter.nonLocalCurrentMemoryBytes = static_cast<f64>(nonLocalMemoryInfo.CurrentUsage);
}

void getGraphicsAdapters(std::vector<GpuInfo>& gpuList) {
    /*
        Get hardware adapters first. 
    */
    IDXCoreAdapterFactory* dxCoreFactory = nullptr;
    if (FAILED(DXCoreCreateAdapterFactory(&dxCoreFactory))) {
        return;
    }
  
    // get graphics adapters that are D3D12 capable
    const GUID filterAttributes[]{ DXCORE_ADAPTER_ATTRIBUTE_D3D12_GRAPHICS };
    IDXCoreAdapterList* dxCoreAdapterList = nullptr;
    if (FAILED(dxCoreFactory->CreateAdapterList(_countof(filterAttributes), filterAttributes, IID_PPV_ARGS(&dxCoreAdapterList)))) {
        return;
    }

    // set preferences so hardware adapters are prioritized
    DXCoreAdapterPreference preferences[] = { DXCoreAdapterPreference::Hardware};
    dxCoreAdapterList->Sort(_countof(preferences), preferences);

    u32 totalCount = dxCoreAdapterList->GetAdapterCount();
    std::vector<IDXCoreAdapter*> hardwareAdapters{};
    for (u32 i = 0; i < totalCount; ++i) {
        IDXCoreAdapter* adapter = nullptr;
        
        // extract only hardware adpaters
        if (SUCCEEDED(dxCoreAdapterList->GetAdapter(i, IID_PPV_ARGS(&adapter)))) {

            bool isHardware{};
            if (SUCCEEDED(adapter->GetProperty(DXCoreAdapterProperty::IsHardware, &isHardware))) {
                if (!isHardware) {
                    break;
                }

                hardwareAdapters.push_back(adapter);
            }
        }
    }
    
    for (size_t i = 0; i < hardwareAdapters.size(); ++i) {
        GpuInfo currentAdapter{};
        
        size_t descSize{};
        if (SUCCEEDED(hardwareAdapters[i]->GetPropertySize(DXCoreAdapterProperty::DriverDescription, &descSize))) {
            std::vector<char> description(descSize);
            hardwareAdapters[i]->GetProperty(DXCoreAdapterProperty::DriverDescription, descSize, description.data());
            std::string descriptionStr(description.begin(), description.end());    

            std::wstring descriptionStrW = convertStringToWstring(descriptionStr);
            wcsncpy_s(currentAdapter.description, descriptionStrW.c_str(), _TRUNCATE);
        }

        size_t luidSize{};
        if (SUCCEEDED(hardwareAdapters[i]->GetPropertySize(DXCoreAdapterProperty::InstanceLuid, &luidSize))) {
            LUID luid{};
            hardwareAdapters[i]->GetProperty(DXCoreAdapterProperty::InstanceLuid, luidSize, &luid);
            currentAdapter.luid = luidToU64(luid);
        }
        gpuList.push_back(currentAdapter);
    }

    /*
        Get software adapter that corresponds with the hardware adapter.
    */
    IDXGIFactory6* dxgiFactory = nullptr; 
    if (FAILED(CreateDXGIFactory2(0, IID_PPV_ARGS(&dxgiFactory)))) {
        return;
    }
   
    for (u32 index = 0;; ++index) {
        IDXGIAdapter3* adapter = nullptr;
       
        HRESULT result = dxgiFactory->EnumAdapterByGpuPreference(index, DXGI_GPU_PREFERENCE_HIGH_PERFORMANCE, IID_PPV_ARGS(&adapter));
        if (FAILED(result)) {
            break;
        }

        DXGI_ADAPTER_DESC2 desc{};
        // skip software adapters
        if (FAILED(adapter->GetDesc2(&desc)) || (desc.Flags & DXGI_ADAPTER_FLAG_SOFTWARE)) {
            adapter->Release(); 
            continue;
        } 

        u64 adapterLuid = luidToU64(desc.AdapterLuid);

        // find hardware adapter from gpuList.
        auto it = std::find_if(gpuList.begin(), gpuList.end(), [adapterLuid](const GpuInfo& gpu) { 
            return gpu.luid == adapterLuid;
        });
        
        if (it == gpuList.end()) {
            adapter->Release();
            continue; 
        }
        
        GpuInfo& foundGpu = *it;
        getAdapterDesc(foundGpu, desc);

        // creating a D3D12 device for QueryVideoMemoryInfo()
        ID3D12Device* device = nullptr;
        D3D12CreateDevice(adapter, D3D_FEATURE_LEVEL_11_0, IID_PPV_ARGS(&device));        
        
        DXGI_QUERY_VIDEO_MEMORY_INFO localMemoryInfo{};
        if (SUCCEEDED(adapter->QueryVideoMemoryInfo(0, DXGI_MEMORY_SEGMENT_GROUP_LOCAL, &localMemoryInfo))) {
            udpateDedicatedMemoryUsage(foundGpu, localMemoryInfo); 
        }

        DXGI_QUERY_VIDEO_MEMORY_INFO nonLocalMemoryInfo{};
        if (SUCCEEDED(adapter->QueryVideoMemoryInfo(0, DXGI_MEMORY_SEGMENT_GROUP_NON_LOCAL, &nonLocalMemoryInfo))) {
            updateSharedMemoryUsage(foundGpu, nonLocalMemoryInfo);
        }
        
        device->Release();
        adapter->Release();
    }

    dxgiFactory->Release();
    dxCoreFactory->Release();
}

std::vector<GpuInfo> collectGpuInfo() {
    std::vector<GpuInfo> adapters{};
    getGraphicsAdapters(adapters);

    return adapters;
}