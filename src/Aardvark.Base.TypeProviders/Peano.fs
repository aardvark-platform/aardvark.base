namespace Aardvark.Base
open System
open System.Reflection
open Microsoft.FSharp.Core.CompilerServices
open Microsoft.FSharp.Quotations
open ProviderImplementation
open ProviderImplementation.ProvidedTypes




type INatural = interface end
type Z = class interface INatural end
type S<'a> = class interface INatural end
    
[<AutoOpen>]
module Peano =
    let rec getSize (t : Type) =
        if t.IsGenericType then
            1 + getSize (t.GetGenericArguments().[0])
        else
            0

    let rec typeSize<'d> =
        getSize typeof<'d>
        
type N0 = Z
type N1 = S<N0> 
type N2 = S<N1>
type N3 = S<N2>
type N4 = S<N3>
type N5 = S<N4>
type N6 = S<N5>
type N7 = S<N6>
type N8 = S<N7>
type N9 = S<N8>
type N10 = S<N9>
type N11 = S<N10>
type N12 = S<N11>
type N13 = S<N12>
type N14 = S<N13>
type N15 = S<N14>
type N16 = S<N15>

// This type defines the type provider. When compiled to a DLL, it can be added
// as a reference to an F# command-line compilation, script, or project.
[<TypeProvider>]
type PeanoTypeProvider(config: TypeProviderConfig) as this = 
//TypeProviderConfig * namespaceName:string * types: ProvidedTypeDefinition list
    inherit TypeProviderForNamespaces(config)

    let rec buildPeanoType (n : int) =
        match n with
            | 0 -> typeof<Z>
            | n ->
                let inner = buildPeanoType (n-1)
                typedefof<S<_>>.MakeGenericType [|inner|]

    let namespaceName = "Aardvark.Base"
    let thisAssembly = Assembly.GetExecutingAssembly()

    let natType = ProvidedTypeDefinition(thisAssembly, namespaceName, "N", None)

    do natType.DefineStaticParameters(
        [ProvidedStaticParameter("number", typeof<int>)],
        fun typeName parameterValues ->
            match parameterValues with
                | [|:? int as number|] ->

                    if number < 0 then
                        failwith "cannot represent negative numbers"

                    let baseType = buildPeanoType number

                    let ty = ProvidedTypeDefinition(thisAssembly, namespaceName, typeName, Some baseType)

                    ty

                | _ -> 
                    failwith "invalid args"
        )


    // And add them to the namespace
    do this.AddNamespace(namespaceName, [natType])

[<assembly:TypeProviderAssembly>] 
do()