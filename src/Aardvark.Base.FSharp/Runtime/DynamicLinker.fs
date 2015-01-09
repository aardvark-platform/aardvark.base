#if INTERACTIVE
#r "..\\..\\Bin\\Debug\\Aardvark.Base.dll"
open Aardvark.Base
#else
namespace Aardvark.Base
#endif

open System.Linq.Expressions
open System.Runtime.InteropServices
open Microsoft.FSharp.Reflection
open System.Reflection
open System.Reflection.Emit
open System

/// <summary>
/// Declares the basic interface for a dynamic linker provided by the OS.
/// </summary>
module DynamicLinkerTypes =
    type IDynamicLinker =
        abstract member LoadLibrary : string -> nativeint
        abstract member FreeLibrary : nativeint -> unit
        abstract member GetProcAddress : nativeint -> string -> nativeint

    type Function internal(ptr : nativeint) =
        member x.Handle = ptr

    type Library internal(ptr : nativeint, linker : IDynamicLinker) =
        let mutable ptr = ptr

        interface IDisposable with
            member x.Dispose() = x.Dispose()


        member x.Dispose() =
            if ptr <> 0n then
                linker.FreeLibrary ptr |> ignore
                ptr <- 0n

        member x.Handle = ptr

        member x.TryFindFunction (name : string) =
            if ptr = 0n then
                None
            else
                let ptr = linker.GetProcAddress ptr name
                if ptr <> 0n then
                    Function(ptr) |> Some
                else
                    None

        member x.GetFunction (name : string) =
            if ptr <> 0n then
                Function (linker.GetProcAddress ptr name)
            else
                raise <| ObjectDisposedException("Library")

/// <summary>
/// Wraps some methods from Kernel32.dll on Windows and provides an implementation
/// of IDynamicLinker declared above.
/// </summary>
module private Kernel32 =
    open System
    open System.Runtime.InteropServices
    open DynamicLinkerTypes

    type LoadLibraryFlags = Default                           = 0x00000000
                            | DontResolveDllReferences        = 0x00000001
                            | LoadLibraryAsDatafile           = 0x00000002
                            | LoadWithAlteredSearchPath       = 0x00000008
                            | LoadIgnoreCodeAuthZLevel        = 0x00000010
                            | LoadLibraryAsImageResource      = 0x00000020
                            | LoadLibraryAsDatafileExclusive  = 0x00000040
                            | LoadLibrarySearchDllLoadDir     = 0x00000100
                            | LoadLibrarySearchApplicationDir = 0x00000200
                            | LoadLibrarySearchUserDirs       = 0x00000400
                            | LoadLibrarySearchSystem32       = 0x00000800
                            | LoadLibrarySearchDefaultDirs    = 0x00001000

    type AllocationType =
        | Commit=0x1000u

    type MemoryProtection =
        | ExecuteReadWrite = 0x40u

    type FreeType =
        | Decommit = 0x4000u


    [<DllImport("kernel32.dll")>]
    extern bool private FreeLibrary (nativeint handle)

    [<DllImport("kernel32.dll")>]
    extern nativeint private LoadLibrary (string path)

    [<DllImport("kernel32.dll")>]
    extern nativeint private LoadLibraryEx (string path, IntPtr hFile, LoadLibraryFlags flags)

    [<DllImport("kernel32.dll")>]
    extern nativeint private GetProcAddress(nativeint library, string name)
    
    [<DllImport("kernel32.dll", SetLastError=true)>]
    extern IntPtr private VirtualAlloc(IntPtr lpAddress, UIntPtr dwSize, AllocationType flAllocationType, MemoryProtection flProtect);

    [<DllImport("kernel32.dll", SetLastError=true)>]
    extern bool private VirtualFree(IntPtr lpAddress, UIntPtr dwSize, FreeType freeType);

    let private linker = 
        { new IDynamicLinker with
            member x.LoadLibrary(name : string) = LoadLibrary(name)
            member x.FreeLibrary(address : nativeint) = FreeLibrary(address) |> ignore
            member x.GetProcAddress (handle : nativeint) (name : string) = GetProcAddress(handle, name) }

    module private DelegateTypeBuilder = 
        let assembly = new AssemblyName();
        assembly.Version <- new Version(1, 0, 0, 0);
        assembly.Name <- "ReflectionEmitDelegateTest";
        let assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(assembly, AssemblyBuilderAccess.RunAndSave);
        let modbuilder = assemblyBuilder.DefineDynamicModule("MyModule", "ReflectionEmitDelegateTest.dll", true);

        let mutable delegateIndex = 0
        let buildDelegate (argTypes : Type[]) (ret : Type) =
            let delegateIndex = System.Threading.Interlocked.Increment(&delegateIndex) 
            let name = sprintf "DelegateType%d" delegateIndex

            let typeBuilder = modbuilder.DefineType(
                                name, 
                                TypeAttributes.Class ||| TypeAttributes.Public ||| TypeAttributes.Sealed ||| TypeAttributes.AnsiClass ||| TypeAttributes.AutoClass, 
                                typeof<System.MulticastDelegate>)

        
            let unmanagedAtt = typeof<UnmanagedFunctionPointerAttribute>.GetConstructor([|typeof<CallingConvention>|])
            let attBuilder = CustomAttributeBuilder(unmanagedAtt, [| CallingConvention.Cdecl :> obj |])
            typeBuilder.SetCustomAttribute(attBuilder)

            let constructorBuilder = typeBuilder.DefineConstructor(MethodAttributes.RTSpecialName ||| MethodAttributes.HideBySig ||| MethodAttributes.Public, CallingConventions.Standard, [| typeof<obj>; typeof<System.IntPtr> |])
            constructorBuilder.SetImplementationFlags(MethodImplAttributes.Runtime ||| MethodImplAttributes.Managed);

            let methodBuilder = typeBuilder.DefineMethod("Invoke", MethodAttributes.Public ||| MethodAttributes.HideBySig, ret, argTypes);
            methodBuilder.SetImplementationFlags(MethodImplAttributes.Runtime ||| MethodImplAttributes.Managed ||| MethodImplAttributes.Native);

            let supress = typeof<System.Security.SuppressUnmanagedCodeSecurityAttribute>.GetConstructor([||])
            let supressSecAtt = CustomAttributeBuilder(supress, [||])
            typeBuilder.SetCustomAttribute(supressSecAtt)
            methodBuilder.SetCustomAttribute(supressSecAtt)

            typeBuilder.CreateType()

    let tryLoadLibrary (path : string) =
        let ptr = LoadLibrary(path)
        if ptr <> 0n then
            Some(new Library(ptr, linker))
        else
            None

    let loadLibrary (path : string) =
        new Library (LoadLibrary(path), linker)

    let wrapFunction' (t : Type) (f : Function) =
        let args, ret = FunctionReflection.getMethodSignature t
        let dType = DelegateTypeBuilder.buildDelegate (args |> List.toArray) ret
        Marshal.GetDelegateForFunctionPointer(f.Handle, dType)

    let wrapFunction (f : Function) : 'a =
        let d = wrapFunction' typeof<'a> f
        let invoke = d.GetMethodInfo()
        FunctionReflection.buildFunction d invoke

/// <summary>
/// Wraps some methods from libdl.so on Linux-like systems and provides an implementation
/// of IDynamicLinker declared above.
/// </summary>
module private Dl =
    open DynamicLinkerTypes

    type Protection = None    = 0x00
                    | Read    = 0x01
                    | Write   = 0x02
                    | Execute = 0x04
                    | ReadWriteExecute = 0x07

    

    [<DllImport("libdl", SetLastError=false, CharSet=CharSet.Ansi)>]
    extern nativeint dlopen (string path, int flag)

    [<DllImport("libdl", SetLastError=false, CharSet=CharSet.Ansi)>]
    extern nativeint dlsym(nativeint library, string name)

    [<DllImport("libdl", SetLastError=false)>]
    extern int dlclose(nativeint library)
    

    [<DllImport("libc", SetLastError=true)>]
    extern int getpagesize()

    [<DllImport("libc", SetLastError=true)>]
    extern nativeint memalign(nativeint align, nativeint size)

    [<DllImport("libc", SetLastError=true)>]
    extern int mprotect(IntPtr addr, nativeint size, Protection prot);

    [<DllImport("libc", SetLastError=false)>]
    extern IntPtr malloc(nativeint size);

    [<DllImport("libc", SetLastError=false)>]
    extern void free(nativeint ptr);

    let private linker =
        { new IDynamicLinker with
            member x.LoadLibrary(name : string) = dlopen(name, 1) // RTLD_LAZY = 1
            member x.FreeLibrary(address : nativeint) = dlclose(address) |> ignore
            member x.GetProcAddress (handle : nativeint) (name : string) = dlsym(handle, name) }


    let tryLoadLibrary (path : string) =
        let ptr = dlopen(path, 1) // RTLD_LAZY = 1
        if ptr <> 0n then
            Some(new Library(ptr, linker))
        else
            None

    let loadLibrary (path : string) =
        new Library (dlopen(path, 1), linker) // RTLD_LAZY = 1

/// <summary>
/// DynamicLinker provides platform independent functions for loading libraries and
/// resolving function-pointers within those libraries.
/// Note that MacOSX is currently not supported
/// </summary>
module DynamicLinker =
    open DynamicLinkerTypes

    let private os = System.Environment.OSVersion

    let notimp() = raise <| NotImplementedException()

    let tryLoadLibrary (name : string) =
        match os with
            | Windows -> Kernel32.tryLoadLibrary name
            | Linux -> Dl.tryLoadLibrary name
            | Mac -> notimp()

    let loadLibrary (name : string) =
        match os with
            | Windows -> Kernel32.loadLibrary name
            | Linux -> Dl.loadLibrary name
            | Mac -> notimp()    

    let tryLoadFunction (name : string) (lib : Library) =
        lib.TryFindFunction name