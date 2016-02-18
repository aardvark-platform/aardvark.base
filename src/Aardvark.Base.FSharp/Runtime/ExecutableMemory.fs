#nowarn "51"
#if INTERACTIVE
#r "..\\..\\Bin\\Debug\\Aardvark.Base.dll"
open Aardvark.Base
#else
namespace Aardvark.Base
#endif

module ExecutableMemory =
    open System
    let private os = System.Environment.OSVersion

    let alloc (size : int) =
        match os with
            | Windows -> 
                Kernel32.Imports.VirtualAlloc(0n, UIntPtr (uint32 size), Kernel32.AllocationType.Commit, Kernel32.MemoryProtection.ExecuteReadWrite)
            | Linux | Mac  -> 
                let pageSize = Dl.Imports.getpagesize()
                let s = nativeint size
                let mutable mem = 0n
                let r = Dl.Imports.posix_memalign(&&mem, nativeint pageSize, s)
                if r<>0 then failwith "could not alloc aligned memory"

                let stat = Dl.Imports.mprotect(mem, s, Dl.Protection.ReadWriteExecute)
                if stat <> 0 then failwith "mprotect failed"

                mem

    let free (ptr : nativeint) (size : int) =
        match os with
            | Windows -> 
                Kernel32.Imports.VirtualFree(ptr, UIntPtr (uint32 size), Kernel32.FreeType.Decommit) |> ignore
            | Linux | Mac ->
                Dl.Imports.free(ptr)


    let init (data : byte[]) =
        let ptr = alloc data.Length
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