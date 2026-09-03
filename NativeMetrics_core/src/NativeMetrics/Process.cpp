#include <NativeMetrics/Process.hpp>

std::vector<ProcessInfo> collectProcesses() {
    std::vector<ProcessInfo> processes{};

    // take snapshot of all processes in system
    HANDLE hProcessSnap = CreateToolhelp32Snapshot(TH32CS_SNAPPROCESS, 0);
    if (hProcessSnap == INVALID_HANDLE_VALUE) {
        return processes;
    }

    PROCESSENTRY32W pew32{};
    pew32.dwSize = sizeof(PROCESSENTRY32W);

    // retrieve information about first process,
    // and exit if unsuccessful
    if (!Process32FirstW(hProcessSnap, &pew32)) {
        CloseHandle(hProcessSnap);
        return processes;
    }

    // walk the snapshot of processes, collect info,
    // and add to processes vector
    while (Process32NextW(hProcessSnap, &pew32)) {
        // skip system idle process
        if (pew32.th32ProcessID == 0) {
            continue;
        }

        ProcessInfo currentProcess{};
        wcsncpy_s(currentProcess.processName, pew32.szExeFile, _TRUNCATE);
        currentProcess.processId = pew32.th32ProcessID;
        currentProcess.threadsCount = pew32.cntThreads;
        currentProcess.parentProcessId = pew32.th32ParentProcessID;
        currentProcess.priorityClassBase = pew32.pcPriClassBase;

        HANDLE hProcess = OpenProcess(PROCESS_QUERY_LIMITED_INFORMATION, FALSE, pew32.th32ProcessID);
        if (hProcess == nullptr) {
            // protected processes that can't be opened should still be added
            processes.push_back(currentProcess);
            continue;
        }

        // get process exe path if available
        wchar_t exePath[PROCESS_PATH_LENGTH];
        DWORD bufferSize = PROCESS_PATH_LENGTH;

        if (QueryFullProcessImageNameW(hProcess, 0, exePath, &bufferSize)) {
            wcsncpy_s(currentProcess.processPath, exePath, _TRUNCATE);
        }

        // get process memory info
        PROCESS_MEMORY_COUNTERS_EX processMemory{};
        bool processMemoryInfo = GetProcessMemoryInfo(hProcess, reinterpret_cast<PROCESS_MEMORY_COUNTERS*>(&processMemory), sizeof(processMemory));
        if (processMemoryInfo) {
            u64 memoryUsageInt = bytesToMB(static_cast<u64>(processMemory.WorkingSetSize));
            u64 commitSizeInt = bytesToMB(static_cast<u64>(processMemory.PagefileUsage));
            u64 privateMemoryInt = bytesToMB(static_cast<u64>(processMemory.PrivateUsage));

            currentProcess.memoryUsage = static_cast<f64>(memoryUsageInt);
            currentProcess.commitSize = static_cast<f64>(commitSizeInt);
            currentProcess.privateMemory = static_cast<f64>(privateMemoryInt);
        }

        CloseHandle(hProcess);
        processes.push_back(currentProcess);
    }

    CloseHandle(hProcessSnap);
    return processes;
}