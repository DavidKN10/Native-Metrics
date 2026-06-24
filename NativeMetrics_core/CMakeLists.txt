cmake_minimum_required(VERSION 3.20)
project(NativeMetrics_core LANGUAGES CXX)

set(CMAKE_CXX_STANDARD 17)
set(CMAKE_CXX_STANDARD_REQUIRED ON)

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

target_compile_definitions(NativeMetrics_core PRIVATE NATIVEMETRICS_EXPORTS)

if (MSVC)
	target_compile_options(NativeMetrics_core PRIVATE /W4 /permissive-)
endif()
