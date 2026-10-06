#include <NativeMetrics/Gpu.hpp>

void getAdapterDesc(GpuInfo& adapter, DXGI_ADAPTER_DESC2& desc) {
    wcsncpy_s(adapter.description, desc.Description, _TRUNCATE);

    LARGE_INTEGER li{};
    li.LowPart = desc.AdapterLuid.LowPart;
    li.HighPart = desc.AdapterLuid.HighPart;
    adapter.luid = li.QuadPart;

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
    IDXGIFactory6* factory = nullptr; 
    if (FAILED(CreateDXGIFactory2(0, IID_PPV_ARGS(&factory)))) {
        return;
    }
   
    for (u32 index = 0;; ++index) {
        IDXGIAdapter3* adapter = nullptr;
       
        HRESULT result = factory->EnumAdapterByGpuPreference(index, DXGI_GPU_PREFERENCE_HIGH_PERFORMANCE, IID_PPV_ARGS(&adapter));
        if (FAILED(result)) {
            break;
        }

        DXGI_ADAPTER_DESC2 desc{};
        // skip software adapters
        if (FAILED(adapter->GetDesc2(&desc)) || (desc.Flags & DXGI_ADAPTER_FLAG_SOFTWARE)) {
            adapter->Release(); 
            continue;
        } 
        
        GpuInfo currentAdapter{};
        getAdapterDesc(currentAdapter, desc);

        // creating a D3D12 device for QueryVideoMemoryInfo()
        ID3D12Device* device = nullptr;
        D3D12CreateDevice(adapter, D3D_FEATURE_LEVEL_11_0, IID_PPV_ARGS(&device));        
        
        DXGI_QUERY_VIDEO_MEMORY_INFO localMemoryInfo{};
        if (SUCCEEDED(adapter->QueryVideoMemoryInfo(0, DXGI_MEMORY_SEGMENT_GROUP_LOCAL, &localMemoryInfo))) {
            udpateDedicatedMemoryUsage(currentAdapter, localMemoryInfo); 
        }

        DXGI_QUERY_VIDEO_MEMORY_INFO nonLocalMemoryInfo{};
        if (SUCCEEDED(adapter->QueryVideoMemoryInfo(0, DXGI_MEMORY_SEGMENT_GROUP_NON_LOCAL, &nonLocalMemoryInfo))) {
            updateSharedMemoryUsage(currentAdapter, nonLocalMemoryInfo);
        }
        
        gpuList.push_back(currentAdapter);
        device->Release();
        adapter->Release();
    }
    
    factory->Release();
}

std::vector<GpuInfo> collectGpuInfo() {
    std::vector<GpuInfo> adapters{};
    getGraphicsAdapters(adapters);

    return adapters;
}