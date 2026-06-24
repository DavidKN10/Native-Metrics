#include <NativeMetrics/NativeMetrics.hpp>

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
	do {
		ProcessInfo currentProcess{};
		wcsncpy_s(currentProcess.processName, pew32.szExeFile, _TRUNCATE);
		currentProcess.parentProcessId = pew32.th32ProcessID;
		currentProcess.threadsCount = pew32.cntThreads;
		currentProcess.parentProcessId = pew32.th32ParentProcessID;
		currentProcess.priorityClassBase = pew32.pcPriClassBase;

		u32 dwPriorityClass = 0;
		HANDLE hProcess = OpenProcess(PROCESS_QUERY_LIMITED_INFORMATION, FALSE, pew32.th32ProcessID);
		if (hProcess == NULL) {
			printError(TEXT("OpenProcess"));
		}
		else {
			dwPriorityClass = GetPriorityClass(hProcess);
			if (!dwPriorityClass) {
				printError(TEXT("GetPriorityClass"));
				CloseHandle(hProcess);
				return processes;
			}
			else {
				currentProcess.priorityClass = dwPriorityClass;
			}

			PROCESS_MEMORY_COUNTERS_EX processMemory{};
			bool processMemoryInfo = GetProcessMemoryInfo(hProcess, reinterpret_cast<PROCESS_MEMORY_COUNTERS*>(&processMemory), sizeof(processMemory));
			if (processMemoryInfo == NULL) {
				printError(TEXT("GetProcessMemoryInfo"));
			}
			else {
				u64 memoryUsageInt = bytesToMB(static_cast<u64>(processMemory.WorkingSetSize));
				u64 commitSizeInt = bytesToMB(static_cast<u64>(processMemory.PagefileUsage));
				u64 privateMemoryInt = bytesToMB(static_cast<u64>(processMemory.PrivateUsage));

				currentProcess.memoryUsage = static_cast<f64>(memoryUsageInt);
				currentProcess.commitSize = static_cast<f64>(commitSizeInt);
				currentProcess.privateMemory = static_cast<f64>(privateMemoryInt);
			}
		}
		processes.push_back(currentProcess);
	} 
	while (Process32NextW(hProcessSnap, &pew32));

	CloseHandle(hProcessSnap);
	return processes;
}

std::vector<NetworkAdapterInfo> collectNetworkAdapters() {
    std::vector<NetworkAdapterInfo> networkAdapters{};

    PMIB_IF_TABLE2 table{};
    i32 returnValue = GetIfTable2(&table);

    if (returnValue != NO_ERROR || table == nullptr) {
        printError(TEXT("GetIfTable2"));
        return networkAdapters;
    } else {
        for (u64 i = 0; i < table->NumEntries; i++) {
            const MIB_IF_ROW2 &currentRow = table->Table[i];

            NetworkAdapterInfo currentAdapter{};
            wcsncpy_s(currentAdapter.alias, currentRow.Alias, _TRUNCATE);
            wcsncpy_s(currentAdapter.description, currentRow.Description, _TRUNCATE);
            currentAdapter.luid = currentRow.InterfaceLuid.Value;
            currentAdapter.isConnected = currentRow.MediaConnectState == MediaConnectStateConnected;
            currentAdapter.isOperational = currentRow.OperStatus == IfOperStatusUp;
            currentAdapter.receiveLinkSpeedBits = currentRow.ReceiveLinkSpeed;
            currentAdapter.transmitLinkSpeedBits = currentRow.TransmitLinkSpeed;
            currentAdapter.receivedBytes = currentRow.InOctets;
            currentAdapter.sentBytes = currentRow.OutOctets;

            // updating download and upload speed
            auto it = networkHistory.find(currentAdapter.luid);
            if (it == networkHistory.end()) {
                networkHistory[currentAdapter.luid] = {currentAdapter.receivedBytes, currentAdapter.sentBytes, std::chrono::steady_clock::now()};
                currentAdapter.downloadBytesPerSec = 0.0;
                currentAdapter.uploadBytesPerSec = 0.0;
            } else {
                auto &previous = it->second;
                f64 elapsedSeconds = std::chrono::duration<f64>(std::chrono::steady_clock::now() - previous.timestamp).count();

                u64 bytesReceivedDelta = currentAdapter.receivedBytes - previous.receivedBytes;
                u64 bytesSentDelta = currentAdapter.sentBytes - previous.sentBytes;

                currentAdapter.downloadBytesPerSec = bytesReceivedDelta / elapsedSeconds;
                currentAdapter.uploadBytesPerSec = bytesSentDelta / elapsedSeconds;

                previous.receivedBytes = currentAdapter.receivedBytes;
                previous.sentBytes = currentAdapter.sentBytes;
                previous.timestamp = std::chrono::steady_clock::now();
            }

            networkAdapters.push_back(currentAdapter);
        }
    }
    FreeMibTable(table);
    return networkAdapters;
}

u64 getTotalMemory() {
	MEMORYSTATUSEX statex{};
	statex.dwLength = sizeof(statex);
	GlobalMemoryStatusEx(&statex);

	u64 totalPhysicalMemory = bytesToGB(statex.ullTotalPhys);
	
	return totalPhysicalMemory;
}

u64 getAvailableMemory() {
	MEMORYSTATUSEX statex{};
	statex.dwLength = sizeof(statex);
	GlobalMemoryStatusEx(&statex);

	u64 availablePhysicalMemory = bytesToGB(statex.ullAvailPhys);

	return availablePhysicalMemory;
}

u64 getApproxPercentInUse() {
	MEMORYSTATUSEX statex{};
	statex.dwLength = sizeof(statex);
	GlobalMemoryStatusEx(&statex);
	
	u64 approxMemPercent = static_cast<u64>(statex.dwMemoryLoad);

	return approxMemPercent;
}

f64 getCpuUsage() {
	FILETIME idle1, kernel1, user1;
	FILETIME idle2, kernel2, user2;

	// capture initial values
	GetSystemTimes(&idle1, &kernel1, &user1);
	
	Sleep(ONE_SEC);

	// capture second values
	GetSystemTimes(&idle2, &kernel2, &user2);

	u64 idleTime1 = fileTimeToU64(idle1);
	u64 kernelTime1 = fileTimeToU64(kernel1);
	u64 userTime1 = fileTimeToU64(user1);

	u64 idleTime2 = fileTimeToU64(idle2);
	u64 kernelTime2 = fileTimeToU64(kernel2);
	u64 userTime2 = fileTimeToU64(user2);
	
	u64 idleDelta = idleTime2 - idleTime1;
	u64 kernelDelta = kernelTime2 - kernelTime1;
	u64 userDelta = userTime2 - userTime1;

	u64 totalDelta = kernelDelta + userDelta;	// kernel time already contains idle time

	f64 cpuUsage = (1.0 - (static_cast<f64>(idleDelta)) / (static_cast<f64>(totalDelta))) * 100.0;
	return cpuUsage;
}

bool getProcessList(ProcessInfo* buffer, i32 bufferSize, i32* processesWritten) {
	if (!buffer || !processesWritten || bufferSize <= 0) {
		return false;
	}

	auto processes = collectProcesses();

	i32 count = static_cast<i32>(processes.size());
	i32 toCopy = min(count, bufferSize);

	for (int i = 0; i < toCopy; i++) {
		buffer[i] = processes[i];
	}
	*processesWritten = toCopy;
	return true;
}

bool getNetworkAdapterInfo(NetworkAdapterInfo *buffer, i32 bufferSize, i32* adaptersWritten) {
    if (!buffer || !adaptersWritten || bufferSize <= 0) {
        return false;
    }

    auto networkAdapters = collectNetworkAdapters();

    i32 count = static_cast<i32>(networkAdapters.size());
    i32 toCopy = min(count, bufferSize);

    for (int i = 0; i < toCopy; i++) {
        buffer[i] = networkAdapters[i];
    }
    *adaptersWritten = toCopy;
    return true;
}
