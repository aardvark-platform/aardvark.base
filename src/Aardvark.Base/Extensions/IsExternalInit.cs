#if !NET8_0_OR_GREATER

using System.ComponentModel;

namespace System.Runtime.CompilerServices
{
    // See: https://github.com/dotnet/roslyn/issues/45510
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class IsExternalInit
    {
    }
}

#endif