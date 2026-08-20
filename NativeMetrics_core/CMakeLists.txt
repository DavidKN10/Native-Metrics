cmake_minimum_required(VERSION 3.20)
project(NativeMetrics_core LANGUAGES CXX)

set(CMAKE_CXX_STANDARD 17)
set(CMAKE_CXX_STANDARD_REQUIRED ON)
set(CMAKE_CXX_EXTENSIONS OFF)

set(SOURCE_FILES
  "src/NativeMetrics.cpp"
)

set(HEADER_FILES
  "include/NativeMetrics/NativeMetrics.hpp"
  "include/NativeMetrics/Types.hpp"
  "include/NativeMetrics/Util.hpp"
  "include/NativeMetrics/Models/ProcessInfo.hpp"
  "include/NativeMetrics/Models/NetworkAdapterInfo.hpp"
)

add_library(NativeMetrics_core SHARED
	${SOURCE_FILES}
)

target_link_libraries(NativeMetrics_core PRIVATE
	Iphlpapi
	Ws2_32
	Psapi
)

target_include_directories(NativeMetrics_core PUBLIC
	${CMAKE_CURRENT_SOURCE_DIR}/include
)

add_executable(CpuTest
  "tests/CpuTest.cpp"
)

target_link_libraries(CpuTest PRIVATE
  NativeMetrics_core
)

add_executable(ProcessTest	
	"tests/ProcessTest.cpp"
)

target_link_libraries(ProcessTest PRIVATE
	NativeMetrics_core
)

add_executable(NetworkTest
	"tests/NetworkTest.cpp"
)

target_link_libraries(NetworkTest PRIVATE
	NativeMetrics_core
)

target_compile_definitions(NativeMetrics_core PRIVATE NATIVEMETRICS_EXPORTS)
