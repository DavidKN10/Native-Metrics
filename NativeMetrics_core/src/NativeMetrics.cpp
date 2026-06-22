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
