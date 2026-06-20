#pragma once

#include <iostream>

#include <NativeMetrics/Types.hpp>

#ifdef NATIVEMETRICS_EXPORTS
#define NATIVEMETRICS_API __declspec(dllexport)
#else 
#define NATIVEMETRICS_API __declspec(dllimport)
#endif

extern "C" {
	NATIVEMETRICS_API void test();
}

