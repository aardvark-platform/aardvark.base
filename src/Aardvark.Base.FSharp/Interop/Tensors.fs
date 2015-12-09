namespace Aardvark.Base

[<AutoOpen>]
module ``Tensor Slice Extensions`` =

    type Vector<'a> with
        member x.GetSlice(min : Option<int>, max : Option<int>) =
            let size = int x.Size
            let min = defaultArg min 0                  // |> clamp 0 (size-1)
            let max = defaultArg max (int x.Size - 1)   // |> clamp 0 (size-1)
            x.SubVector(min, 1 + max - min)

    type Vector<'a, 'b> with
        member x.GetSlice(min : Option<int>, max : Option<int>) =
            let size = int x.Size
            let min = defaultArg min 0                  // |> clamp 0 (size-1)
            let max = defaultArg max (int x.Size - 1)   // |> clamp 0 (size-1)
            x.SubVector(min, 1 + max - min)



    type Matrix<'a> with
        member x.GetSlice(col : int, min : Option<int>, max : Option<int>) =
            x.Col(int64 col).GetSlice(min, max)

        member x.GetSlice(min : Option<int>, max : Option<int>, row : int) =
            x.Row(int64 row).GetSlice(min, max)

        member x.GetSlice(minX : Option<int>, maxX : Option<int>, minY : Option<int>, maxY : Option<int>) =
            let size = V2i x.Size

            let minX = defaultArg minX 0                // |> clamp 0 (size.X-1)
            let maxX = defaultArg maxX (size.X - 1)     // |> clamp 0 (size.X-1)
            let minY = defaultArg minY 0                // |> clamp 0 (size.Y-1)
            let maxY = defaultArg maxY (size.Y - 1)     // |> clamp 0 (size.Y-1)

            x.SubMatrix(int64 minX, int64 minY, 1L + int64 (maxX - minX), 1L + int64 (maxY - minY))

        member x.GetSlice(min : Option<V2i>, max : Option<V2i>) =
            let min = defaultArg min V2i.Zero
            let max = defaultArg max (V2i x.Size - V2i.II)
            x.SubMatrix(int64 min.X, int64 min.Y, 1L + int64 (max.X - min.X), 1L + int64 (max.Y - min.Y))

    type Matrix<'a, 'b> with
        member x.GetSlice(col : int, min : Option<int>, max : Option<int>) =
            x.Col(int64 col).GetSlice(min, max)

        member x.GetSlice(min : Option<int>, max : Option<int>, row : int) =
            x.Row(int64 row).GetSlice(min, max)

        member x.GetSlice(minX : Option<int>, maxX : Option<int>, minY : Option<int>, maxY : Option<int>) =
            let size = V2i x.Size

            let minX = defaultArg minX 0                // |> clamp 0 (size.X-1)
            let maxX = defaultArg maxX (size.X - 1)     // |> clamp 0 (size.X-1)
            let minY = defaultArg minY 0                // |> clamp 0 (size.Y-1)
            let maxY = defaultArg maxY (size.Y - 1)     // |> clamp 0 (size.Y-1)

            x.SubMatrix(int64 minX, int64 minY, 1L + int64 (maxX - minX), 1L + int64 (maxY - minY))

        member x.GetSlice(min : Option<V2i>, max : Option<V2i>) =
            let min = defaultArg min V2i.Zero
            let max = defaultArg max (V2i x.Size - V2i.II)
            x.SubMatrix(int64 min.X, int64 min.Y, 1L + int64 (max.X - min.X), 1L + int64 (max.Y - min.Y))

