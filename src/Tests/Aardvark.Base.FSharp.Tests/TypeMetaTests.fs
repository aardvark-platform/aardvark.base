namespace Aardvark.Base.FSharp.Tests

open System
open Aardvark.Base
open Aardvark.Base.TypeMeta

open FsUnit
open NUnit.Framework

module ``TypeMeta Tests`` =

    [<Test>]
    let ``[Patterns] MatrixOf``() =
        let test43 (t: Type) =
            match t with
            | MatrixOf (s, Integral) -> s |> should equal (V2i(4, 3))
            | _ -> failwith $"failed pattern {t}"

        test43 typeof<M34i>
        test43 typeof<M34l>

    [<Test>]
    let ``[Patterns] ListOf``() =
        let test (t: Type) =
            match t with
            | ListOf Fractional -> ()
            | _ -> failwithf $"failed pattern {t}"

        test typeof<list<float16>>
        test typeof<list<float32>>

    [<Test>]
    let ``[Patterns] ArrayOf``() =
        let test (t: Type) =
            match t with
            | ArrayOf Fractional -> ()
            | _ -> failwithf $"failed pattern {t}"

        test typeof<float16[]>
        test typeof<decimal[]>

    [<Test>]
    let ``[Patterns] ArrOf``() =
        let test (expectedDim: int) (t: Type) =
            match t with
            | ArrOf (d, Fractional) -> d |> should equal expectedDim
            | _ -> failwithf $"failed pattern {t}"

        test 4 typeof<Arr<N<4>, float32>>
        test 32 typeof<Arr<N<32>, float>>

    [<Test>]
    let ``[Patterns] SeqOf``() =
        let test (t: Type) =
            match t with
            | SeqOf Numeric -> ()
            | _ -> failwithf $"failed pattern {t}"

        test typeof<seq<decimal>>
        test typeof<seq<int8>>

        match typeof<array<int8>> with
        | SeqOf _ -> failwith "should not match for array"
        | _ -> ()

    [<Test>]
    let ``[Patterns] EnumerableOf``() =
        let test (t: Type) =
            match t with
            | EnumerableOf (VectorOf (_, Integral)) -> ()
            | _ -> failwithf $"failed pattern {t}"

        test typeof<seq<V2i>>
        test typeof<list<V3ui>>
        test typeof<array<V4l>>
        test typeof<ResizeArray<V4i>>

    [<Test>]
    let ``[Patterns] RefOf``() =
        let test (t: Type) =
            match t with
            | RefOf Vector -> ()
            | _ -> failwithf $"failed pattern {t}"

        test typeof<ref<V3ui>>
        test typeof<ref<V2d>>