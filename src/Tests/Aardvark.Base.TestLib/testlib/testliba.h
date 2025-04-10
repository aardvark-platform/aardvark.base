#ifndef TESTLIBA_H
#define TESTLIBA_H

#ifdef __GNUC__
#define DllExport(t) extern "C" t
#else
#define DllExport(t) extern "C"  __declspec( dllexport ) t __cdecl
#endif

DllExport(int) foo();

#endif // TESTLIBA_H
