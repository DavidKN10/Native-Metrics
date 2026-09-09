#include <NativeMetrics/Gpu.hpp>

void getGraphicsAdapters(std::vector<GpuInfo>& gpuList) {
    IDXGIFactory6* factory = nullptr; 
    CreateDXGIFactory2(0, __uuidof(IDXGIFactory6), reinterpret_cast<void**>(&factory) ); 
    
    IDXGIAdapter3* adapter = nullptr;
    u32 index = 0;
    
    while (factory->EnumAdapterByGpuPreference(
        index, 
        DXGI_GPU_PREFERENCE_HIGH_PERFORMANCE, 
        __uuidof(IDXGIAdapter3), 
        reinterpret_cast<void**>(&adapter)) != DXGI_ERROR_NOT_FOUND) 
    {
        GpuInfo currentAdapter{};

        DXGI_ADAPTER_DESC2 desc{};
        adapter->GetDesc2(&desc);
        
        // skip software adapters
        if (!(desc.Flags & DXGI_ADAPTER_FLAG_SOFTWARE)) {
            wcsncpy_s(currentAdapter.description, desc.Description, _TRUNCATE);

            LARGE_INTEGER li{};
            li.LowPart = desc.AdapterLuid.LowPart;
            li.HighPart = desc.AdapterLuid.HighPart;
            currentAdapter.luid = li.QuadPart;

            currentAdapter.dedicatedVideoMemoryBytes = static_cast<u64>(desc.DedicatedVideoMemory);
            currentAdapter.dedicatedSystemMemoryBytes = static_cast<u64>(desc.DedicatedSystemMemory);
            currentAdapter.sharedSystemMemoryBytes = static_cast<u64>(desc.SharedSystemMemory);
           
            gpuList.push_back(currentAdapter);
        }
        
        adapter->Release();
        index++;
    }
    
    factory->Release();
}

std::vector<GpuInfo> collectGpuInfo() {
    std::vector<GpuInfo> adapters{};
    getGraphicsAdapters(adapters);

    return adapters;
}