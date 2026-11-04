#include <NativeMetrics/Util.hpp>

void printError(TCHAR const* message) {
    u32 eNum;              // Windows error code
    TCHAR sysMessage[256]; // buffer for translated error message
    TCHAR* p;              // use to walk through string and remove unwanted characters

    eNum = GetLastError();
    FormatMessage(FORMAT_MESSAGE_FROM_SYSTEM | FORMAT_MESSAGE_IGNORE_INSERTS, NULL, eNum, MAKELANGID(LANG_NEUTRAL, SUBLANG_DEFAULT), sysMessage, 256,
                  NULL);

    p = sysMessage;
    while ((*p > 31) || (*p == 9)) {
        ++p;
    }

    do {
        *p-- = 0;
    } while ((p >= sysMessage) && ((*p == '.') || (*p < 33)));

    _tprintf(TEXT("\n WARNING: %s failed with error %d (%s)\n"), message, eNum, sysMessage);
}

u64 bytesToKB(u64 bytesAmount) {
    return bytesAmount / DIV;
}

u64 bytesToMB(u64 bytesAmount) {
    return bytesAmount / (DIV * DIV);
}

u64 bytesToGB(u64 bytesAmount) {
    return bytesAmount / (DIV * DIV * DIV);
}

u64 fileTimeToU64(const FILETIME& ft) {
    u64 highDateTime = static_cast<u64>(ft.dwHighDateTime);
    u64 result = (highDateTime << 32) | ft.dwLowDateTime;
    return result;
}

u64 luidToU64(LUID& luid) {
    LARGE_INTEGER li{};
    li.HighPart = luid.HighPart;
    li.LowPart = luid.LowPart;

    return li.QuadPart;
}

std::wstring AsciiToWide(const char* narrowStr) {
    if (!narrowStr || *narrowStr == '\0') {
        return L"";
    }
    
    // determine required wide buffer size
    int sizeNeeded = MultiByteToWideChar(CP_ACP, 0, narrowStr, -1, nullptr, 0);
    if (sizeNeeded <= 0) {
        return L"";
    }

    // perform conversion
    std::wstring wideStr(sizeNeeded - 1, L'\0');
    MultiByteToWideChar(CP_ACP, 0, narrowStr, -1, &wideStr[0], sizeNeeded);

    return wideStr;
}

std::wstring convertStringToWstring(const std::string& str) {
    if (str.empty()) {
        return L"";
    }

    i32 size = MultiByteToWideChar(CP_UTF8, 0, &str[0], static_cast<i32>(str.size()), nullptr, 0);

    std::wstring wstr(size, 0);
    MultiByteToWideChar(CP_UTF8, 0, &str[0], static_cast<i32>(str.size()), &wstr[0], size);

    return wstr;
}

std::wstring windowsEdition() {
    DWORD editionNumber{};
    bool result = GetProductInfo(6, 3, 0, 0, &editionNumber);
    if (!result) {
        return L""; 
    }

    switch (editionNumber) {
        case (PRODUCT_CORE):
            return L"Home";
        case (PRODUCT_CORE_N):
            return L"Home N";
        case (PRODUCT_PROFESSIONAL):
            return L"Pro";
        case (PRODUCT_PROFESSIONAL_N):
            return L"Pro N";
        case (PRODUCT_ENTERPRISE):
            return L"Enterprise";
        case (PRODUCT_ENTERPRISE_N):
            return L"Enterprise N";
        case (PRODUCT_EDUCATION):
            return L"Education";
        case (PRODUCT_PRO_FOR_EDUCATION):
            return L"Pro Education";
        case (PRODUCT_PRO_WORKSTATION):
            return L"Pro for Workstations";
        default:
            return L"Unkown";
    }
}
