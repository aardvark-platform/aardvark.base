#include "testlibb.h"
#include "testlibc.h"

DllExport(int) bar()
{
    return 42 + hugo();
}