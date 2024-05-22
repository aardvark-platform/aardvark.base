open System

module Generator =

    let tensorNames =
        Map.ofList [
            1, "Vector"
            2, "Matrix"
            3, "Volume"
        ]

    let tensorReadableNames =
        Map.ofList [
            4, "4D tensor"
        ]

    let tensorPluralNames =
        Map.ofList [
            2, "matrices"
        ]

    let componentNames = [| "X"; "Y"; "Z"; "W" |]

    // We only check for a subset of common permutations (XY swap, last dimension most inner or outer).
    // For all other permutations, iteration will be suboptimal.
    let getOptimizedPermutation (innerZW: bool) (swapXY: bool) (dim: int) =
        let str =
            let xy = if swapXY then "YX" else "XY"
            if dim > 2 then
                let xyz = if dim > 3 then xy + "Z" else xy
                let w = componentNames.[dim - 1]
                if innerZW then w + xyz else xyz + w
            else
                xy

        str.ToCharArray() |> List.ofArray

    let getDelta (size: string) (permutation: char list) =
        let dim = permutation.Length

        let index i =
            componentNames |> Array.findIndex (fun x -> x.[0] = permutation.[i])

        ("1L", permutation) ||> List.scan (fun s c ->
            if s = "1L" then $"{size}{c}"
            else $"{s} * {size}{c}"
        )
        |> List.take dim
        |> List.permute index

    let getName (dim : int) =
        match Map.tryFind dim tensorNames with
        | Some n -> n
        | None -> sprintf "Tensor%d" dim

    let getReadableName (dim: int) =
        match Map.tryFind dim tensorReadableNames with
        | Some n -> n
        | None -> (getName dim).ToLowerInvariant()

    let getReadablePluralName (dim : int) =
        match Map.tryFind dim tensorPluralNames with
        | Some n -> n
        | None -> getReadableName dim + "s"

    let builder = System.Text.StringBuilder()

    let write (str : string) = builder.Append(str) |> ignore

    let mutable indent = ""
    let blank() = write "\r\n"
    let line fmt = Printf.kprintf (fun str -> write indent; write str; write "\r\n") fmt
    let start fmt = Printf.kprintf (fun str -> write indent; write str; write "\r\n"; indent <- indent + "    ") fmt
    let stop() = indent <- indent.Substring(4)

    let forEachPermutation (action: char list -> unit) (dim: int) (t: string) =
        let lc = componentNames.[dim - 1]

        let rec perm (decisions : bool list) =
            match decisions with
            | [] | [_] ->
                let o = if decisions.Length = 1 then "Y" else lc
                start $"if abs {t}.DX < abs {t}.D{o} then"
                perm (false :: decisions)
                stop()
                start "else"
                perm (true :: decisions)
                stop()

            | swapXY :: innerZW :: _ ->
                action <| getOptimizedPermutation innerZW swapXY dim

        if dim > 2 then perm []
        else perm [true]

    let genericIter (components: string[]) (t: string) (iter2: bool)
                    (continueWhile: string option)
                    (sizefail: unit -> bool) (preamble: unit -> unit) (action: unit -> unit) (postamble: unit -> unit) =
        let dim = components.Length
        let sfx1 = if iter2 then "_1" else ""
        let t1 = if iter2 then t + "1" else t
        let t2 = if iter2 then t + "2" else t
        let it1 = if iter2 then "i1" else "i"

        let continueWhile =
            match continueWhile with
            | Some ee -> " && " + ee
            | _ -> ""

        let cont =
            if iter2 then
                start $"if {t}1.Size <> {t}2.Size then"
                let cont = sizefail()
                stop()
                if cont then
                    start "else"
                    true
                else
                    blank()
                    false
            else
                false

        if dim > 1 then
            for i = 0 to dim - 1 do
                line $"let mutable s{i}{sfx1} = 0L"
                line $"let mutable j{i}{sfx1} = 0L"
                if iter2 then
                    line $"let mutable j{i}_2 = 0L"

            blank()

            (dim, t2) ||> forEachPermutation (fun p ->
                p |> List.iteri (fun i c ->
                    let l = if i = 0 then '0' else p.[i - 1]

                    let mutable ass = $"s{i}{sfx1} <- {t1}.DS{c}; j{i}{sfx1} <- {t1}.Info.J{c}{l}"
                    if iter2 then
                        ass <- ass + $"; j{i}_2 <- {t}2.Info.J{c}{l}"

                    line "%s" ass
                )
            )

            blank()
        else
            line $"let s0{sfx1} = {t1}.DSX"
            line $"let j0{sfx1} = {t1}.JX"
            if iter2 then
                line $"let j0_2 = {t2}.JX"

        line $"let mutable {it1} = {t1}.FirstIndex"
        if iter2 then
            line $"let mutable i2 = {t2}.FirstIndex"
        blank()

        preamble()

        let rec buildLoop (i: int) =
            if i < 0 then
                action()
            else
                line $"let e{i}{sfx1} = {it1} + s{i}{sfx1}"
                start $"while {it1} <> e{i}{sfx1}{continueWhile} do"
                buildLoop (i - 1)
                line $"{it1} <- {it1} + j{i}{sfx1}"
                if iter2 then
                    line $"i2 <- i2 + j{i}_2"
                stop()

        buildLoop (dim - 1)

        postamble()

        // End of size check
        if cont then stop()

    let equalsWith (name: string) (components: string[]) (t: string) =
        start $"let inline equalsWith ([<InlineIfLambda>] compare : 'T1 -> 'T2 -> bool) ({t}1: {name}<'T1>) ({t}2: {name}<'T2>) ="

        genericIter components t true
            (Some "result")
            (fun () -> line "false"; true )
            (fun () ->
                line $"let d1 = {t}1.Data"
                line $"let d2 = {t}2.Data"
                line "let mutable result = true"
                blank()
            )
            (fun () -> line "result <- compare d1.[int i1] d2.[int i2]")
            (fun () ->
                blank()
                line "result"
            )

        stop()

    let equals (name: string) (t: string) =
        start $"let inline equals ({t}1: {name}<'T>) ({t}2: {name}<'T>) ="
        line $"equalsWith (=) {t}1 {t}2"
        stop()

    let equalsWithin (name: string) (t: string) =
        start $"let inline equalsWithin (epsilon: 'T) ({t}1: {name}<'T>) ({t}2: {name}<'T>) ="
        line $"equalsWith (fun x y -> if x > y then x - y <= epsilon else y - x <= epsilon) {t}1 {t}2"
        stop()

    let iteri (name: string) (components: string[]) (t: string) =
        start $"let inline iteri ([<InlineIfLambda>] action: int64 -> unit) ({t}: {name}<'T>) ="

        genericIter components t false None
            (fun () -> false)
            (fun () -> ())
            (fun () -> line "action i")
            (fun () -> ())

        stop()

    let iter (name: string) (t: string) =
        start $"let inline iter ([<InlineIfLambda>] action: 'T -> unit) ({t}: {name}<'T>) ="
        line $"let d = {t}.Data"
        line $"{t} |> iteri (fun i -> action d.[int i])"
        stop()

    let map (name: string) (dim: int) (arraySize: string -> string) (t: string) =
        start $"let inline map ([<InlineIfLambda>] mapping: 'T -> 'U) ({t}: {name}<'T>) ="
        line $"let result = Array.zeroCreate <| {arraySize t}"

        if dim > 1 then
            blank()
            start "let delta ="
            (dim, t) ||> forEachPermutation (fun p ->
                let delta = p |> getDelta $"{t}.S" |> String.concat ", "
                line $"V{dim}l({delta})"
            )
            stop()
            blank()

        line "let mutable j = 0"
        blank()

        start $"{t} |> iter (fun x ->"
        line "result.[j] <- mapping x"
        line "j <- j + 1"
        stop()
        line ")"

        let delta = if dim = 1 then "1L" else "delta"
        blank()
        line $"{name}<'U>(result, 0L, {t}.Size, {delta})"
        stop()

    let mapInPlace (name: string) (t: string) =
        start $"let inline mapInPlace ([<InlineIfLambda>] mapping: 'T -> 'T) ({t}: {name}<'T>) ="
        line $"let d = {t}.Data"
        line $"{t} |> iteri (fun i -> d.[int i] <- mapping d.[int i])"
        line $"{t}"
        stop()

    let mapReduce (name: string) (t: string) =
        start $"let inline mapReduce ([<InlineIfLambda>] mapping: 'T -> 'U) ([<InlineIfLambda>] reduction: 'U -> 'U -> 'U) ({t}: {name}<'T>) ="
        line $"let d = {t}.Data"
        line $"let fi = {t}.FirstIndex"
        line $"let j = {t}.JX"
        line $"let e = fi + {t}.DSX"

        line $"let mutable result = mapping d.[int fi]"
        line $"let mutable i = fi + j"

        start $"while i <> e do"
        line $"result <- reduction result (mapping d.[int i])"
        line $"i <- i + j"
        stop()

        line $"result"
        stop()

    let iteri2 (name: string) (components: string[]) (t: string) =
        start $"let inline iteri2 ([<InlineIfLambda>] action: int64 -> int64 -> unit) ({t}1: {name}<'T1>) ({t}2: {name}<'T2>) ="

        genericIter components t true None
            (fun () ->
                line """raise <| ArgumentException($"Mismatching %s size ({%s1.Size} != {%s2.Size})")""" name t t
                false
            )
            (fun () -> ())
            (fun () -> line "action i1 i2")
            (fun () -> ())

        stop()

    let iter2 (name: string) (t: string) =
        start $"let inline iter2 ([<InlineIfLambda>] action: 'T1 -> 'T2 -> unit) ({t}1: {name}<'T1>) ({t}2: {name}<'T2>) ="
        line $"let d1 = {t}1.Data"
        line $"let d2 = {t}2.Data"
        line $"iteri2 (fun i1 i2 -> action d1.[int i1] d2.[int i2]) {t}1 {t}2"
        stop()

    let map2 (name: string) (dim: int) (arraySize: string -> string) (t: string) =
        let t1 = t + "1"
        let t2 = t + "2"
        start $"let inline map2 ([<InlineIfLambda>] mapping: 'T1 -> 'T2 -> 'T3) ({t}1: {name}<'T1>) ({t}2: {name}<'T2>) ="
        line $"let result = Array.zeroCreate <| {arraySize t1}"

        if dim > 1 then
            blank()
            start "let delta ="
            (dim, t2) ||> forEachPermutation (fun p ->
                let delta = p |> getDelta $"{t2}.S" |> String.concat ", "
                line $"V{dim}l({delta})"
            )
            stop()
            blank()

        line "let mutable j = 0"
        blank()

        start $"iter2 (fun x y ->"
        line "result.[j] <- mapping x y"
        line "j <- j + 1"
        stop()
        line $") {t1} {t2}"

        let delta = if dim = 1 then "1L" else "delta"
        blank()
        line $"{name}<'T3>(result, 0L, {t1}.Size, {delta})"
        stop()

    let map2InPlace (name: string) (t: string) =
        start $"let inline map2InPlace ([<InlineIfLambda>] mapping: 'T1 -> 'T2 -> 'T2) ({t}1: {name}<'T1>) ({t}2: {name}<'T2>) ="
        line $"let d1 = {t}1.Data"
        line $"let d2 = {t}2.Data"
        line $"iteri2 (fun i1 i2 -> d2.[int i2] <- mapping d1.[int i1] d2.[int i2]) {t}1 {t}2"
        line $"{t}2"
        stop()

    let mapReduce2 (name: string) (t: string) =
        start $"let inline mapReduce2 ([<InlineIfLambda>] mapping: 'T1 -> 'T2 -> 'T3) ([<InlineIfLambda>] reduction: 'T3 -> 'T3 -> 'T3) ({t}1: {name}<'T1>) ({t}2: {name}<'T2>) ="

        start $"if {t}1.Size <> {t}2.Size then"
        line """raise <| ArgumentException($"Mismatching %s size ({%s1.Size} != {%s2.Size})")""" name t t
        stop()

        line $"let d1 = {t}1.Data"
        line $"let d2 = {t}2.Data"
        line $"let fi1 = {t}1.FirstIndex"
        line $"let fi2 = {t}2.FirstIndex"
        line $"let j1 = {t}1.JX"
        line $"let j2 = {t}2.JX"
        line $"let e1 = fi1 + {t}1.DSX"

        line $"let mutable result = mapping d1.[int fi1] d2.[int fi2]"
        line $"let mutable i1 = fi1 + j1"
        line $"let mutable i2 = fi2 + j2"

        start $"while i1 <> e1 do"
        line $"result <- reduction result (mapping d1.[int i1] d2.[int i2])"
        line $"i1 <- i1 + j1"
        line $"i2 <- i2 + j2"
        stop()

        line $"result"
        stop()

    let tensor (dim: int) =
        let name = getName dim
        let rname = getReadableName dim
        let rnames = getReadablePluralName dim
        let t = string <| Char.ToLowerInvariant name.[0]
        let componentNames = Array.take dim componentNames

        let arraySize (t: string) =
            componentNames
            |> Array.map (fun c -> $"{t}.S{c}")
            |> String.concat " * "
            |> (fun total -> $"int ({total})")

        blank()
        start "module %s =" name

        blank()
        line $"/// Returns if two {rnames} are equal according to the given comparison function."
        equalsWith name componentNames t

        blank()
        line $"/// Returns if two {rnames} are equal."
        equals name t

        blank()
        line $"/// Returns if two {rnames} are equal, i.e. the absolute difference between two corresponding elements does not exceed the given epsilon value."
        equalsWithin name t

        blank()
        line $"/// Invokes the given function for each index of a {rname}."
        iteri name componentNames t

        blank()
        line $"/// Invokes the given function for each element of a {rname}."
        iter name t

        blank()
        line $"/// Computes a new {rname} by applying the given mapping function to each element of a {rname}."
        map name dim arraySize t

        blank()
        line $"/// Applies the given mapping function to each element of a {rname}. Returns the modified input {rname}."
        mapInPlace name t

        if dim = 1 then
            blank()
            line $"/// Applies the given mapping function to each element of a {rname}, accumulating the results using the given reduction function."
            mapReduce name t

        blank()
        line $"/// Invokes the given function for each index of two {rnames}. The {rnames} must match in size."
        iteri2 name componentNames t

        blank()
        line $"/// Invokes the given function for each element of two {rnames}. The {rnames} must match in size."
        iter2 name t

        blank()
        line $"/// Computes a new {rname} by applying the given mapping function to each element of two {rnames}. The {rnames} must match in size."
        map2 name dim arraySize t

        blank()
        line $"/// Applies the given mapping function to each element of two {rnames}. Returns {t}2 where the computed values are stored."
        line $"/// The {rnames} must match in size."
        map2InPlace name t

        if dim = 1 then
            blank()
            line $"/// Applies the given mapping function to each element of two {rnames}, accumulating the results using the given reduction function."
            line $"/// The {rnames} must match in size."
            mapReduce2 name t

        stop()

        ()

    let run() =
        line "namespace Aardvark.Base"
        line ""
        line "open System"
        line ""
        line "[<AutoOpen>]"
        start "module FSharpTensorExtensions ="
        for d in 1 .. 4 do
            tensor d
        stop()

        let str = builder.ToString()
        System.IO.File.WriteAllText(System.IO.Path.Combine(__SOURCE_DIRECTORY__, @"TensorUtilitiesGenerated.fs"), str)

Generator.run()