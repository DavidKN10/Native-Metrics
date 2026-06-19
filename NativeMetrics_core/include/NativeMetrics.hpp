#pragma once

#include <iostream>

#ifdef NATIVEMETRICS_EXPORTS
#define NATIVEMETRICS_API __declspec(dllexport)
#else 
#define NATIVEMETRICS_API __declspec(dllimport)
#endif

extern "C" {
 NATIVEMETRICS_API void test();
}

