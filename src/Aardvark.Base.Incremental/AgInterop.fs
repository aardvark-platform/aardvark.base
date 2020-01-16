namespace FSharp.Data.Adaptive

open Aardvark.Base
open FSharp.Data.Traceable

[<AutoOpen>]
module AgInterop = 

    type private ScopedReader<'d>(scope : Ag.Scope, empty : 'd, inner : IOpReader<'d>) =
        inherit AbstractReader<'d>(empty)

        override x.Compute(token : AdaptiveToken) =
            Ag.useScope scope (fun () ->
                inner.GetChanges token
            )



    module AVal =
        let withScope (scope : Ag.Scope) (value : aval<'a>) =
            AVal.custom (fun token ->
                Ag.useScope scope (fun () ->
                    value.GetValue token
                )
            )

        let scoped (value : aval<'a>) =
            let scope = Ag.getContext()
            withScope scope value
            
    module ASet =
        let withScope (scope : Ag.Scope) (value : aset<'a>) =
            ASet.ofReader (fun () ->
                ScopedReader(scope, HashSetDelta.empty, value.GetReader())
            )

        let scoped (value : aset<'a>) =
            let scope = Ag.getContext()
            withScope scope value
            
    module AMap =
        let withScope (scope : Ag.Scope) (value : amap<'a, 'b>) =
            AMap.ofReader (fun () ->
                ScopedReader(scope, HashMapDelta.empty, value.GetReader())
            )

        let scoped (value : amap<'a, 'b>) =
            let scope = Ag.getContext()
            withScope scope value

    module AList =
        let withScope (scope : Ag.Scope) (value : alist<'a>) =
            AList.ofReader (fun () ->
                ScopedReader(scope, IndexListDelta.empty, value.GetReader())
            )

        let scoped (value : alist<'a>) =
            let scope = Ag.getContext()
            withScope scope value