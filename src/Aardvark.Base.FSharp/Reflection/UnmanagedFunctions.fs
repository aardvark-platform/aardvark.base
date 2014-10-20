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
/// UnmanagedFunctions allows to wrap function pointers as F# functions efficiently.
/// Since MethodInfo.Invoke is extremely slow it uses compiled Linq-Expressions here.
/// </summary>
module UnmanagedFunctions =
    open System.Collections.Generic
    open System.Collections.Concurrent
    //open DynamicLinker

//    let private functions = ConcurrentDictionary()
    let private pointers = ConcurrentDictionary()

    module private DelegateBuilder = 
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

    let private methodCache = System.Collections.Concurrent.ConcurrentDictionary<nativeint, Delegate>()

    [<System.Security.SuppressUnmanagedCodeSecurity>]
    let wrap (executableMemory : IntPtr) : 'a =
        if executableMemory <> 0n then
            let args, ret = FunctionReflection.getMethodSignature typeof<'a>

            let delegateType = DelegateBuilder.buildDelegate (args |> List.toArray) ret
            let d = Marshal.GetDelegateForFunctionPointer(executableMemory, delegateType)

            let invoke = delegateType.GetMethod("Invoke")
            let f = FunctionReflection.buildFunction d invoke

            pointers.TryAdd(f :> obj, executableMemory) |> ignore
            f
        else
            FunctionReflection.makeNAryFunction (fun _ -> failwith "could not get native function (maybe some opengl-driver problem?)")

    let private convert (o : obj) (t : Type) =
        match o with
            | :? int as o ->
                if t = typeof<int64> then o |> int64 :> obj
                elif t = typeof<nativeint> then o |> nativeint :> obj
                elif t = typeof<int> then o :> obj
                else failwith "not implemented"
            | :? int64 as o ->
                if t = typeof<int64> then o :> obj
                elif t = typeof<nativeint> then o |> nativeint :> obj
                elif t = typeof<int> then o |> int :> obj
                else failwith "not implemented"
            | :? nativeint as o ->
                if t = typeof<int64> then o |> int64 :> obj
                elif t = typeof<nativeint> then o :> obj
                elif t = typeof<int> then o |> int :> obj
                else failwith "not implemented"
            | _ -> o

    let private invokeDelegate (d : Delegate) (args : obj[]) =
        let dArgs = d.Method.GetParameters()
        let newArgs = Array.zeroCreate dArgs.Length
        for i in 0..dArgs.Length-1 do
            let a = args.[i]
            let wanted = dArgs.[i]
            let real = a.GetType()

            if wanted.ParameterType <> real then
                newArgs.[i] <- convert a wanted.ParameterType
            else 
                newArgs.[i] <- a

        d.GetMethodInfo().Invoke(d, newArgs)



    [<System.Security.SuppressUnmanagedCodeSecurity>]
    let callFunctionPointer (executableMemory : IntPtr) (arguments : obj[]) : 'a =
        let d = methodCache.GetOrAdd(executableMemory, fun _ ->
            let ret = typeof<'a>
            let args = arguments |> Array.map (fun a -> a.GetType())

            let delegateType = DelegateBuilder.buildDelegate (args) ret
            let del = Marshal.GetDelegateForFunctionPointer(executableMemory, delegateType)
            
            del
        )

        invokeDelegate d arguments |> unbox

    let tryFindFunctionPointer (f : 'a) =
        match pointers.TryGetValue (f :> obj) with
            | (true,f) -> Some f
            | _ -> None
    
    let registerPointer (f : 'a) (ptr : nativeint) =
        pointers.TryAdd(f :> obj, ptr) |> ignore
