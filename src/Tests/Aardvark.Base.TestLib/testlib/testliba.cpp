#include "testliba.h"
#include "testlibb.h"

DllExport(int) foo()
{
    return 2 * bar();
}