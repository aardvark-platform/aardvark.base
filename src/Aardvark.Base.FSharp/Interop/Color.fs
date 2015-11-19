namespace Aardvark.Base

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module ColorConv =
    
    type private ColorConversion<'a, 'b>() =

        static let convert : 'a -> 'b =
            let a = typeof<'a>
            let b = typeof<'b>

            if a = b then id |> unbox

            //color => color
            elif a = typeof<C3f> && b = typeof<C4f> then (fun (a : C3f) -> a.ToC4f()) |> unbox
            elif a = typeof<C3f> && b = typeof<C3b> then (fun (a : C3f) -> a.ToC3b()) |> unbox
            elif a = typeof<C3f> && b = typeof<C4b> then (fun (a : C3f) -> a.ToC4b()) |> unbox

            elif a = typeof<C4f> && b = typeof<C3f> then (fun (a : C4f) -> a.ToC3f()) |> unbox
            elif a = typeof<C4f> && b = typeof<C3b> then (fun (a : C4f) -> a.ToC3b()) |> unbox
            elif a = typeof<C4f> && b = typeof<C4b> then (fun (a : C4f) -> a.ToC4b()) |> unbox

            elif a = typeof<C3b> && b = typeof<C4f> then (fun (a : C3b) -> a.ToC4f()) |> unbox
            elif a = typeof<C3b> && b = typeof<C3f> then (fun (a : C3b) -> a.ToC3f()) |> unbox
            elif a = typeof<C3b> && b = typeof<C4b> then (fun (a : C3b) -> a.ToC4b()) |> unbox

            elif a = typeof<C4b> && b = typeof<C3b> then (fun (a : C4b) -> a.ToC3b()) |> unbox
            elif a = typeof<C4b> && b = typeof<C3f> then (fun (a : C4b) -> a.ToC3f()) |> unbox
            elif a = typeof<C4b> && b = typeof<C4f> then (fun (a : C4b) -> a.ToC4f()) |> unbox

            //vector => float color
            elif a = typeof<V3i> && b = typeof<C4f> then (fun (a : V3i) -> a.ToC4b().ToC4f()) |> unbox
            elif a = typeof<V3i> && b = typeof<C3f> then (fun (a : V3i) -> a.ToC3b().ToC3f()) |> unbox
            elif a = typeof<V4i> && b = typeof<C4f> then (fun (a : V4i) -> a.ToC4b().ToC4f()) |> unbox
            elif a = typeof<V4i> && b = typeof<C3f> then (fun (a : V4i) -> a.ToC3b().ToC3f()) |> unbox

            elif a = typeof<V3f> && b = typeof<C4f> then (fun (a : V3f) -> a.ToC4f()) |> unbox
            elif a = typeof<V3f> && b = typeof<C3f> then (fun (a : V3f) -> a.ToC3f()) |> unbox
            elif a = typeof<V4f> && b = typeof<C4f> then (fun (a : V4f) -> a.ToC4f()) |> unbox
            elif a = typeof<V4f> && b = typeof<C3f> then (fun (a : V4f) -> a.ToC3f()) |> unbox

            elif a = typeof<V3d> && b = typeof<C4f> then (fun (a : V3d) -> a.ToC4f()) |> unbox
            elif a = typeof<V3d> && b = typeof<C3f> then (fun (a : V3d) -> a.ToC3f()) |> unbox
            elif a = typeof<V4d> && b = typeof<C4f> then (fun (a : V4d) -> a.ToC4f()) |> unbox
            elif a = typeof<V4d> && b = typeof<C3f> then (fun (a : V4d) -> a.ToC3f()) |> unbox

            //vector => byte color
            elif a = typeof<V3i> && b = typeof<C4b> then (fun (a : V3i) -> a.ToC4b()) |> unbox
            elif a = typeof<V3i> && b = typeof<C3b> then (fun (a : V3i) -> a.ToC3b()) |> unbox
            elif a = typeof<V4i> && b = typeof<C4b> then (fun (a : V4i) -> a.ToC4b()) |> unbox
            elif a = typeof<V4i> && b = typeof<C3b> then (fun (a : V4i) -> a.ToC3b()) |> unbox

            elif a = typeof<V3f> && b = typeof<C4b> then (fun (a : V3f) -> a.ToC4f().ToC4b()) |> unbox
            elif a = typeof<V3f> && b = typeof<C3b> then (fun (a : V3f) -> a.ToC3f().ToC3b()) |> unbox
            elif a = typeof<V4f> && b = typeof<C4b> then (fun (a : V4f) -> a.ToC4f().ToC4b()) |> unbox
            elif a = typeof<V4f> && b = typeof<C3b> then (fun (a : V4f) -> a.ToC3f().ToC3b()) |> unbox

            elif a = typeof<V3d> && b = typeof<C4b> then (fun (a : V3d) -> a.ToC4f().ToC4b()) |> unbox
            elif a = typeof<V3d> && b = typeof<C3b> then (fun (a : V3d) -> a.ToC3f().ToC3b()) |> unbox
            elif a = typeof<V4d> && b = typeof<C4b> then (fun (a : V4d) -> a.ToC4f().ToC4b()) |> unbox
            elif a = typeof<V4d> && b = typeof<C3b> then (fun (a : V4d) -> a.ToC3f().ToC3b()) |> unbox


            else failwithf "unsupported color conversion from %A to %A" a.Name b.Name

        static member inline Convert(v : 'a) = convert v

    let toC4f (col : 'a) =
        ColorConversion<'a, C4f>.Convert col

    let toC3f (col : 'a) =
        ColorConversion<'a, C3f>.Convert col

    let toC4b (col : 'a) =
        ColorConversion<'a, C4b>.Convert col

    let toC3b (col : 'a) =
        ColorConversion<'a, C3b>.Convert col