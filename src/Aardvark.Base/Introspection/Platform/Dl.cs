using System;
using System.Runtime.InteropServices;

namespace Aardvark.Base;

public partial class Aardvark
{
    internal static class Dl
    {
        public const int RTLD_LAZY         = 0x00001;
        public const int RTLD_NOW          = 0x00002;
        public const int RTLD_BINDING_MASK = 0x00003;
        public const int RTLD_NOLOAD       = 0x00004;
        public const int RTLD_DEEPBIND     = 0x00008;
        public const int RTLD_GLOBAL       = 0x00100;
        public const int RTLD_LOCAL        = 0x00000;
        public const int RTLD_NODELETE     = 0x01000;

        [DllImport("libc", SetLastError = false, CharSet = CharSet.Ansi)]
        public static extern IntPtr dlopen(string path, int flag);

        [DllImport("libc", SetLastError = true, CharSet = CharSet.Ansi)]
        public static extern IntPtr dlsym(IntPtr handle, string name);
    }
}