#nowarn "51"
#if INTERACTIVE
#r "..\\..\\Bin\\Debug\\Aardvark.Base.dll"
open Aardvark.Base
#else
namespace Aardvark.Base

open System

#endif



module internal Kernel32 =
    open System
    open System.Runtime.InteropServices

    type AllocationType =
        | Commit=0x1000u

    type MemoryProtection =
        | ExecuteReadWrite = 0x40u

    type FreeType =
        | Decommit = 0x4000u

    
    module Imports = 
        [<DllImport("kernel32.dll", SetLastError=true)>]
        extern IntPtr VirtualAlloc(IntPtr lpAddress, UIntPtr dwSize, AllocationType flAllocationType, MemoryProtection flProtect);

        [<DllImport("kernel32.dll", SetLastError=true)>]
        extern bool VirtualFree(IntPtr lpAddress, UIntPtr dwSize, FreeType freeType);


module private Dl =
    open System
    open System.Runtime.InteropServices

    type Protection = None    = 0x00
                    | Read    = 0x01
                    | Write   = 0x02
                    | Execute = 0x04
                    | ReadWriteExecute = 0x07

    
    module Imports =
        [<DllImport("libc", SetLastError=true)>]
        extern int getpagesize()

        [<DllImport("libc", SetLastError=true)>]
        extern int posix_memalign(nativeint* ptr, nativeint p, nativeint size)

        [<DllImport("libc", SetLastError=true)>]
        extern int mprotect(IntPtr addr, nativeint size, Protection prot);

        [<DllImport("libc", SetLastError=false)>]
        extern IntPtr malloc(nativeint size)

        [<DllImport("libc", SetLastError=false)>]
        extern void free(nativeint ptr)



[<Obsolete("use Aardvark.Assembler JitMem instead")>]
module ExecutableMemory =
    open System
    let private os = System.Environment.OSVersion

    let alloc (size : nativeint) =
        match os with
            | Windows -> 
                Kernel32.Imports.VirtualAlloc(0n, unativeint size, Kernel32.AllocationType.Commit, Kernel32.MemoryProtection.ExecuteReadWrite)
            | Linux | Mac  -> 
                let pageSize = Dl.Imports.getpagesize()
                let s = size
                let mutable mem = 0n
                let r = Dl.Imports.posix_memalign(&&mem, nativeint pageSize, s)
                if r<>0 then failwith "could not alloc aligned memory"

                let stat = Dl.Imports.mprotect(mem, s, Dl.Protection.ReadWriteExecute)
                if stat <> 0 then failwith "mprotect failed"

                mem

    let free (ptr : nativeint) (size : nativeint) =
        match os with
            | Windows -> 
                Kernel32.Imports.VirtualFree(ptr, unativeint size, Kernel32.FreeType.Decommit) |> ignore
            | Linux | Mac ->
                Dl.Imports.free(ptr)


    let init (data : byte[]) =
        let ptr = alloc (nativeint data.Length)
        System.Runtime.InteropServices.Marshal.Copy(data, 0, ptr, data.Length)
        ptr


    // simple c function "int test(int a int b) { return a + b }" 
    // compiled using gcc with no flags
    let simpleAdd = [| 0x55uy;                      //  push   %rbp
                       0x48uy; 0x89uy; 0xe5uy;      //  mov    %rsp,%rbp
                       0x89uy; 0x7duy; 0xfcuy;      //  mov    %edi,-0x4(%rbp)
                       0x89uy; 0x75uy; 0xf8uy;      //  mov    %esi,-0x8(%rbp)
                       0x8buy; 0x45uy; 0xf8uy;      //  mov    -0x8(%rbp),%eax
                       0x8buy; 0x55uy; 0xfcuy;      //  mov    -0x4(%rbp),%edx
                       0x8duy; 0x04uy; 0x02uy;      //  lea    (%rdx,%rax,1),%eax
                       0xc9uy;                      //  leaveq
                       0xc3uy                       //  retq
                    |]