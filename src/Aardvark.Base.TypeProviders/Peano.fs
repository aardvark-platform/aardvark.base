namespace Aardvark.Base
open System
open System.Reflection
open Microsoft.FSharp.Core.CompilerServices
open Microsoft.FSharp.Quotations
open ProviderImplementation
open ProviderImplementation.ProvidedTypes




type INatural = interface end

type Z = class interface INatural end

type S<'a when 'a :> INatural> =  class interface INatural end
type S2<'a when 'a :> INatural> = class interface INatural end
type S4<'a when 'a :> INatural> = class interface INatural end
type S8<'a when 'a :> INatural> = class interface INatural end
type S16<'a when 'a :> INatural> = class interface INatural end
type S32<'a when 'a :> INatural> = class interface INatural end
type S64<'a when 'a :> INatural> = class interface INatural end
type S128<'a when 'a :> INatural> = class interface INatural end
type S256<'a when 'a :> INatural> = class interface INatural end
type S512<'a when 'a :> INatural> = class interface INatural end
type S1024<'a when 'a :> INatural> = class interface INatural end
type S2048<'a when 'a :> INatural> = class interface INatural end
type S4096<'a when 'a :> INatural> = class interface INatural end
type S8192<'a when 'a :> INatural> = class interface INatural end
    
[<AutoOpen>]
module Peano =

    let private cache = System.Collections.Concurrent.ConcurrentDictionary<Type, int>()
    let private typeCache = System.Collections.Concurrent.ConcurrentDictionary<int, Type>()

    let private numbers =
        let steps = System.Collections.Generic.Dictionary<Type, int>()
        steps.[typedefof<S<_>>] <- 1
        steps.[typedefof<S2<_>>] <- 2
        steps.[typedefof<S4<_>>] <- 4
        steps.[typedefof<S8<_>>] <- 8
        steps.[typedefof<S16<_>>] <- 16
        steps.[typedefof<S32<_>>] <- 32
        steps.[typedefof<S64<_>>] <- 64
        steps.[typedefof<S128<_>>] <- 128
        steps.[typedefof<S256<_>>] <- 256
        steps.[typedefof<S512<_>>] <- 512
        steps.[typedefof<S1024<_>>] <- 1024
        steps.[typedefof<S2048<_>>] <- 2048
        steps.[typedefof<S4096<_>>] <- 4096
        steps.[typedefof<S8192<_>>] <- 8192
        steps


    let rec getSize (t : Type) =
        cache.GetOrAdd(t, fun t ->
            if t.IsGenericType then
                match numbers.TryGetValue (t.GetGenericTypeDefinition()) with
                    | (true, step) ->
                        step + getSize (t.GetGenericArguments().[0])
                    | _ ->
                        failwithf "bad numeric type: %A" t

            elif t = typeof<Z> then
                0

            else
                failwithf "bad numeric type: %A" t

        )

    let rec typeSize<'d> =
        getSize typeof<'d>
 
    let rec getPeanoType (v : int) =
        typeCache.GetOrAdd(v, fun v -> 
            if v < 0 then 
                failwith "cannot represent negative numbers"
            elif v = 0 then 
                typeof<Z>
            elif v >= 8192 then
                let inner = getPeanoType (v - 8192)
                typedefof<S8192<_>>.MakeGenericType [| inner |]
            elif v >= 4096 then
                let inner = getPeanoType (v - 4096)
                typedefof<S4096<_>>.MakeGenericType [| inner |]
            elif v >= 2048 then
                let inner = getPeanoType (v - 2048)
                typedefof<S2048<_>>.MakeGenericType [| inner |]
            elif v >= 1024 then
                let inner = getPeanoType (v - 1024)
                typedefof<S1024<_>>.MakeGenericType [| inner |]
            elif v >= 512 then
                let inner = getPeanoType (v - 512)
                typedefof<S512<_>>.MakeGenericType [| inner |]
            elif v >= 256 then
                let inner = getPeanoType (v - 256)
                typedefof<S256<_>>.MakeGenericType [| inner |]
            elif v >= 128 then
                let inner = getPeanoType (v - 128)
                typedefof<S128<_>>.MakeGenericType [| inner |]
            elif v >= 64 then
                let inner = getPeanoType (v - 64)
                typedefof<S64<_>>.MakeGenericType [| inner |]
            elif v >= 32 then
                let inner = getPeanoType (v - 32)
                typedefof<S32<_>>.MakeGenericType [| inner |]
            elif v >= 16 then
                let inner = getPeanoType (v - 16)
                typedefof<S16<_>>.MakeGenericType [| inner |]
            elif v >= 8 then
                let inner = getPeanoType (v - 8)
                typedefof<S8<_>>.MakeGenericType [| inner |]
            elif v >= 4 then
                let inner = getPeanoType (v - 4)
                typedefof<S4<_>>.MakeGenericType [| inner |]
            elif v >= 2 then
                let inner = getPeanoType (v - 2)
                typedefof<S2<_>>.MakeGenericType [| inner |]
            else
                typeof<S<Z>>
        )

       
type N0 = Z
type N1 = S<N0> 
type N2 = S2<Z>
type N3 = S<N2>
type N4 = S4<Z>
type N5 = S<N4>
type N6 = S2<N4>
type N7 = S<N6>
type N8 = S8<Z>
type N9 = S<N8>
type N10 = S2<N8>
type N11 = S<N10>
type N12 = S4<N8>
type N13 = S<N12>
type N14 = S2<N12>
type N15 = S<N14>
type N16 = S16<Z>

// This type defines the type provider. When compiled to a DLL, it can be added
// as a reference to an F# command-line compilation, script, or project.
[<TypeProvider>]
type PeanoTypeProvider(config: TypeProviderConfig) as this = 
//TypeProviderConfig * namespaceName:string * types: ProvidedTypeDefinition list
    inherit TypeProviderForNamespaces(config)

    let namespaceName = "Aardvark.Base"
    let thisAssembly = Assembly.GetExecutingAssembly()

    let natType = ProvidedTypeDefinition(thisAssembly, namespaceName, "N", None)


    let rec getPeanoType (v : int) =
        if v < 0 then 
            failwith "cannot represent negative numbers"
        elif v = 0 then 
            typeof<Z>
        elif v >= 8192 then
            let inner = getPeanoType (v - 8192)
            typedefof<S8192<_>>.MakeGenericType [| inner |]
        elif v >= 4096 then
            let inner = getPeanoType (v - 4096)
            typedefof<S4096<_>>.MakeGenericType [| inner |]
        elif v >= 2048 then
            let inner = getPeanoType (v - 2048)
            typedefof<S2048<_>>.MakeGenericType [| inner |]
        elif v >= 1024 then
            let inner = getPeanoType (v - 1024)
            typedefof<S1024<_>>.MakeGenericType [| inner |]
        elif v >= 512 then
            let inner = getPeanoType (v - 512)
            typedefof<S512<_>>.MakeGenericType [| inner |]
        elif v >= 256 then
            let inner = getPeanoType (v - 256)
            typedefof<S256<_>>.MakeGenericType [| inner |]
        elif v >= 128 then
            let inner = getPeanoType (v - 128)
            typedefof<S128<_>>.MakeGenericType [| inner |]
        elif v >= 64 then
            let inner = getPeanoType (v - 64)
            typedefof<S64<_>>.MakeGenericType [| inner |]
        elif v >= 32 then
            let inner = getPeanoType (v - 32)
            typedefof<S32<_>>.MakeGenericType [| inner |]
        elif v >= 16 then
            let inner = getPeanoType (v - 16)
            typedefof<S16<_>>.MakeGenericType [| inner |]
        elif v >= 8 then
            let inner = getPeanoType (v - 8)
            typedefof<S8<_>>.MakeGenericType [| inner |]
        elif v >= 4 then
            let inner = getPeanoType (v - 4)
            typedefof<S4<_>>.MakeGenericType [| inner |]
        elif v >= 2 then
            let inner = getPeanoType (v - 2)
            typedefof<S2<_>>.MakeGenericType [| inner |]
        else
            typeof<S<Z>>


    do natType.DefineStaticParameters(
        [ProvidedStaticParameter("number", typeof<int>)],
        fun typeName parameterValues ->
            match parameterValues with
                | [|:? int as number|] ->

                    if number < 0 then
                        failwith "cannot represent negative numbers"

                    let baseType = getPeanoType number
                    let ty = ProvidedTypeDefinition(thisAssembly, namespaceName, typeName, Some baseType)

                    ty

                | _ -> 
                    failwith "invalid args"
        )


    // And add them to the namespace
    do this.AddNamespace(namespaceName, [natType])

[<assembly:TypeProviderAssembly>] 
do()