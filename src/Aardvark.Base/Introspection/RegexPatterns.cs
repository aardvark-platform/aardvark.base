using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace Aardvark.Base;

public partial class Aardvark
{
    private partial class RegexPatterns
    {
        public static partial class NativeLibrary
        {
            private const string EXT_WIN32 = @"\.dll$";
            private const string EXT_MACOS = @"\.dylib$";
            private const string EXT_LINUX = @"\.so(?:\.[0-9]+)*$";

#if NET8_0_OR_GREATER
            [GeneratedRegex(EXT_WIN32, RegexOptions.IgnoreCase)]
            private static partial Regex _ExtensionWin32();
            private static Regex ExtensionWin32 => _ExtensionWin32();

            [GeneratedRegex(EXT_MACOS, RegexOptions.IgnoreCase)]
            private static partial Regex _ExtensionMacOS();
            private static Regex ExtensionMacOS => _ExtensionMacOS();

            [GeneratedRegex(EXT_LINUX, RegexOptions.IgnoreCase)]
            private static partial Regex _ExtensionLinux();
            private static Regex ExtensionLinux => _ExtensionLinux();
#else
            private static Regex ExtensionWin32 { get; } = new(EXT_WIN32, RegexOptions.IgnoreCase | RegexOptions.Compiled);
            private static Regex ExtensionMacOS { get; } = new(EXT_MACOS, RegexOptions.IgnoreCase | RegexOptions.Compiled);
            private static Regex ExtensionLinux { get; } = new(EXT_LINUX, RegexOptions.IgnoreCase | RegexOptions.Compiled);
#endif
            public static Regex Extension
                => GetOS() switch
                {
                    OS.Win32 => ExtensionWin32,
                    OS.MacOS => ExtensionMacOS,
                    _ => ExtensionLinux
                };
        }

        public static partial class LdConfig
        {
            private const string ENTRY_X64   = @"[ \t]*(?<name>[^ \t]+)[ \t]+\((?<libc>[^,]+)\,x86-64[^\)]*\)[ \t]*\=\>[ \t]*(?<path>.*)";
            private const string ENTRY_ARM64 = @"[ \t]*(?<name>[^ \t]+)[ \t]+\((?<libc>[^,]+)\,arm-64[^\)]*\)[ \t]*\=\>[ \t]*(?<path>.*)";

#if NET8_0_OR_GREATER
            [GeneratedRegex(ENTRY_X64, RegexOptions.IgnoreCase)]
            private static partial Regex _EntryX64();
            private static Regex EntryX64 => _EntryX64();

            [GeneratedRegex(ENTRY_ARM64, RegexOptions.IgnoreCase)]
            private static partial Regex _EntryArm64();
            private static Regex EntryArm64 => _EntryArm64();
#else
            private static Regex EntryX64   { get; } = new(ENTRY_X64,   RegexOptions.IgnoreCase | RegexOptions.Compiled);
            private static Regex EntryArm64 { get; } = new(ENTRY_ARM64, RegexOptions.IgnoreCase | RegexOptions.Compiled);
#endif

            public static Regex Entry
                => RuntimeInformation.ProcessArchitecture switch
                {
                    Architecture.X64   => EntryX64,
                    Architecture.Arm64 => EntryArm64,
                    _ => null
                };
        }

        public static partial class Assembly
        {
            private const string TARGET_FRAMEWORK = @"^[ \t]*(?<name>[\.A-Za-z_0-9]+)[ \t]*,[ \t]*(v|V)ersion[ \t]*=[ \t]*(?<version>[\.A-Za-z_0-9]+)$";

#if NET8_0_OR_GREATER
            [GeneratedRegex(TARGET_FRAMEWORK)]
            private static partial Regex _TargetFramework();
            public static Regex TargetFramework => _TargetFramework();
#else
            public static Regex TargetFramework { get; } = new(TARGET_FRAMEWORK, RegexOptions.Compiled);
#endif
        }
    }
}
