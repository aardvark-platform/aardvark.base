namespace Aardvark.Base

open Microsoft.FSharp.NativeInterop
open System.Runtime.CompilerServices

#nowarn "9"

[<AutoOpen>]
module private SVDHelpers =
    let inline pythag a b = sqrt (a*a + b*b)
    
    let inline svdBidiagonalNative (anorm : ^a) (U : NativeMatrix< ^a >) (B : NativeMatrix< ^a >) (Vt : NativeMatrix< ^a >) : bool =
        let two = LanguagePrimitives.GenericOne + LanguagePrimitives.GenericOne
        let mutable c : ^a = LanguagePrimitives.GenericZero
        let mutable s = LanguagePrimitives.GenericZero
        let mutable rvl0 = LanguagePrimitives.GenericZero
        let mutable suc = true

        let inline istiny v =
            anorm + v = anorm

        let n = int B.SY
        let m = min (int B.SX) n

        let pB = NativePtr.toNativeInt B.Pointer
        let sa = nativeint sizeof< ^a >
        let dbc = nativeint B.DX * sa
        let dbr = nativeint B.DY * sa

        let pw = pB
        let dw = dbc + dbr

        let prvl = pB + dbc
        let drvl = dw

        let inline rvl (i : int) : ^a =
#if DEBUG
            if i < 0 || i >= n then failwithf "rvl %d" i
#endif
            if i = 0 then rvl0
            else NativeInt.read< ^a > (prvl + nativeint (i - 1) * drvl) //B.[i-1,i]
                
        let inline setrvl (i : int) (v : ^a) =
#if DEBUG
            if i < 0 || i >= n then failwithf "setrvl %d %A" i v
#endif
            if i = 0 then rvl0 <- v
            else NativeInt.write (prvl + nativeint (i - 1) * drvl) v //B.[i-1,i] <- v

        let inline w (i : int) : ^a =
#if DEBUG
            if i < 0 || i >= n then failwithf "w %d" i
#endif
            NativeInt.read< ^a > (pw + nativeint i * dw)
            
        let inline setw (i : int) (v :  ^a) =
#if DEBUG
            if i < 0 || i >= n then failwithf "setw %d %A" i v
#endif
            NativeInt.write (pw + nativeint i * dw) v
             
        let mutable f : ^a = LanguagePrimitives.GenericZero
        let mutable g : ^a = LanguagePrimitives.GenericZero
        let mutable h : ^a = LanguagePrimitives.GenericZero
        
        let mutable x : ^a = LanguagePrimitives.GenericZero
        let mutable y : ^a = LanguagePrimitives.GenericZero
        let mutable z : ^a = LanguagePrimitives.GenericZero

        for k in m - 1 .. -1 .. 0 do 
            let mutable conv = false
            let mutable iterations = 0
            while not conv && iterations <= 30 do
                inc &iterations
                let mutable flag = true
                let mutable nm = 0
                let mutable l = k
                let mutable testing = true
                while testing && l >= 0 do
                    nm <- l - 1
                    if l = 0 || abs (rvl l) + anorm = anorm then
                        flag <- false
                        testing <- false
                    elif (abs (w nm) + anorm = anorm) then
                        testing <- false    
                    else
                        dec &l

                    
                if flag then
                    c <- LanguagePrimitives.GenericZero
                    s <- LanguagePrimitives.GenericOne
                    let mutable i = l
                    while i <= k do
                        f <- s * rvl i
                        setrvl i (c * rvl i)
                        if abs f + anorm <> anorm then
                            g <- w i //B.[i,i]
                            h <- pythag f g
                            setw i h // B.[i,i] <- h
                            h <- LanguagePrimitives.GenericOne / h
                            c <- g * h
                            s <- -f * h

                            applyGivensMat U nm i c s
                            i <- i + 1
                        else
                            i <- k+1

                z <- w k //B.[k,k] 
                if l = k then
                    conv <- true
                else

                    if (iterations >= 30) then
                        //failwith "no convergence after 30 iterations"
                        suc <- false

                    // shift from bottom 2 x 2 minor
                    x <- w l //B.[l,l];
                    nm <- k - 1;
                    y <- w nm //B.[nm, nm];
                    
                    g <- rvl nm;
                    h <- rvl k;
                    f <- ((y - z) * (y + z) + (g - h) * (g + h)) / (two * h * y)
                    g <- sqrt (f*f + LanguagePrimitives.GenericOne)
                    f <- ((x - z) * (x + z) + h * ((y / (f + (if (f >= LanguagePrimitives.GenericZero) then abs(g) else -abs(g)))) - h)) / x

                    // next QR transformation
                    c <- LanguagePrimitives.GenericOne //1.0
                    s <- LanguagePrimitives.GenericOne //1.0

                    for j in l .. nm do
                        let i = j + 1
                        g <- rvl i
                        y <- w i //B.[i,i]
                        h <- s * g
                        g <- c * g
                        z <- pythag f h //Fun.Pythag(f, h)
                        setrvl j z
                        c <- f / z
                        s <- h / z
                        f <- x * c + g * s
                        g <- g * c - x * s
                        h <- y * s
                        y <- y * c
                        applyGivensTransposedMat Vt j i c s

                        z <- pythag f h // Fun.Pythag(f, h)
                        setw j z //B.[j,j] <- z

                        if not (istiny z) then
                            z <- LanguagePrimitives.GenericOne / z
                            c <- f * z
                            s <- h * z

                        f <- (c * g) + (s * y)
                        x <- (c * y) - (s * g)
                        applyGivensMat U j i c s
                    
                    setrvl l LanguagePrimitives.GenericZero
                    setrvl k f
                    setw k x // B.[k,k] <- x
        

        for i in 1 .. m - 1 do
            setrvl i LanguagePrimitives.GenericZero

        let inline swap i0 i1 =
            applyGivensMat U i0 i1 LanguagePrimitives.GenericZero LanguagePrimitives.GenericOne
            let t = w i0
            setw i0 (w i1)
            setw i1 t
            applyGivensTransposedMat Vt i0 i1 LanguagePrimitives.GenericZero LanguagePrimitives.GenericOne

        let cmp =
            { new System.Collections.Generic.IComparer< ^a > with
                member x.Compare(l, r) =
                    compare (abs l) (abs r)
            }

        let values = 
            //SortedSetExt< ^a * _ >(cmp)
            System.Collections.Generic.SortedDictionary< ^a, _ >(cmp)
        for i in 0 .. m - 1 do
            let v = w i //B.[i,i]
            match values.TryGetValue(v) with
            | (true,o) -> 
                values.[v] <- i::o
            | _ -> 
                values.[v] <- [i]
                // values <- 
                //     MapExt.alter v (fun o ->
                //         let o = defaultArg o []
                //         Some (i :: o)
                //     ) values
                
        for i0 in 0..m-1 do
            let biggestIdx =
                let (KeyValue(key,indices)) = values |> Seq.last
                //let (key, indices) = MapExt.tryItem (values.Count - 1) values |> Option.get
                match indices with
                    | [i0] -> 
                        values.Remove(key) |> ignore
                        //values <- MapExt.remove key values
                        i0
                    | i0 :: r ->
                        values.[key] <- r
                        //values <- MapExt.add key r values
                        i0
                    | _ ->
                        failwith ""
                
            if biggestIdx <> i0 then
                let v0 = w i0 //B.[i0, i0]
                swap i0 biggestIdx
               
                match values.TryGetValue(v0) with
                | (true,o) -> 
                    values.[v0] <- o |> List.map (fun ii -> if ii = i0 then biggestIdx else ii)
                | _ -> 
                    values.[v0] <- []                
                // values <- 
                //     MapExt.alter v0 (fun o ->
                //         let o = Option.defaultValue [] o
                //         o |> List.map (fun ii -> if ii = i0 then biggestIdx else ii) |> Some
                //     ) values

    
        for i0 in 0..m-2 do
            if w i0 < LanguagePrimitives.GenericZero then
                let i1 = m-1
                setw i0 (-w i0)
                setw i1 (-w i1)
                applyGivensTransposedMat Vt i0 i1 -LanguagePrimitives.GenericOne LanguagePrimitives.GenericZero

        suc

[<AbstractClass; Sealed>]
type SVD private() =

    static let doubleEps = 1E-20
    static let floatEps = float32 1E-6

    static member DecomposeInPlace(U : NativeMatrix<float>, S : NativeMatrix<float>, Vt : NativeMatrix<float>) =
        if S.SX <= S.SY then
            let anorm = qrBidiagonalizeNative doubleEps U S Vt
            svdBidiagonalNative anorm U S Vt
        else
            // B = U * B' * Vt
            // Bt = V * Bt' * Ut
            let Ut = U.Transposed
            let V = Vt.Transposed
            let St = S.Transposed
            let anorm = qrBidiagonalizeNative doubleEps V St Ut
            svdBidiagonalNative anorm V St Ut

    static member DecomposeInPlace(U : NativeMatrix<float32>, S : NativeMatrix<float32>, Vt : NativeMatrix<float32>) =
        if S.SX <= S.SY then
            let anorm = qrBidiagonalizeNative floatEps U S Vt
            svdBidiagonalNative anorm U S Vt
        else
            // B = U * B' * Vt
            // Bt = V * Bt' * Ut
            let Ut = U.Transposed
            let V = Vt.Transposed
            let St = S.Transposed
            let anorm = qrBidiagonalizeNative floatEps V St Ut
            svdBidiagonalNative anorm V St Ut
            
    static member DecomposeInPlace(U : Matrix<float>, S : Matrix<float>, Vt : Matrix<float>) =
        let mutable U = U
        let mutable S = S
        let mutable Vt = Vt
        let mutable suc = false
        tensor {
            let! pU = &U
            let! pS = &S
            let! pVt = &Vt
            suc <- SVD.DecomposeInPlace(pU, pS, pVt)
        }
        suc
        
    static member DecomposeInPlace(U : Matrix<float32>, S : Matrix<float32>, Vt : Matrix<float32>) =
        let mutable U = U
        let mutable S = S
        let mutable Vt = Vt
        let mutable suc = false
        tensor {
            let! pU = &U
            let! pS = &S
            let! pVt = &Vt
            suc <- SVD.DecomposeInPlace(pU, pS, pVt)
        }
        suc
        
    static member DecomposeInPlace(U : float[,], S : float[,], Vt : float[,]) =
        let mutable U = U
        let mutable S = S
        let mutable Vt = Vt
        let mutable suc = false
        tensor {
            let! pU = &U
            let! pS = &S
            let! pVt = &Vt
            suc <- SVD.DecomposeInPlace(pU, pS, pVt)
        }    
        suc     

    static member DecomposeInPlace(U : float32[,], S : float32[,], Vt : float32[,]) =
        let mutable U = U
        let mutable S = S
        let mutable Vt = Vt
        let mutable suc = false
        tensor {
            let! pU = &U
            let! pS = &S
            let! pVt = &Vt
            suc <- SVD.DecomposeInPlace(pU, pS, pVt)
        }
        suc

    static member Decompose(m : Matrix<float>) =
        let r = m.SY
        let c = m.SX
        let U = Matrix<float>(r,r)
        let S = m.Copy()
        let Vt = Matrix<float>(c,c)
        if SVD.DecomposeInPlace(U, S, Vt) then
            Some (U, S, Vt)
        else
            None

    static member Decompose(m : Matrix<float32>) =
        let r = m.SY
        let c = m.SX
        let U = Matrix<float32>(r,r)
        let S = m.Copy()
        let Vt = Matrix<float32>(c,c)
        if SVD.DecomposeInPlace(U, S, Vt) then
            Some (U, S, Vt)
        else
            None

    static member Decompose(m : float[,]) =
        let r = m.GetLength(0)
        let c = m.GetLength(1)
        let U = Array2D.zeroCreate r r
        let S = Array2D.copy m
        let Vt = Array2D.zeroCreate c c
        if SVD.DecomposeInPlace(U, S, Vt) then
            Some (U, S, Vt)
        else
            None
        
    static member Decompose(m : float32[,]) =
        let r = m.GetLength(0)
        let c = m.GetLength(1)
        let U = Array2D.zeroCreate r r
        let S = Array2D.copy m
        let Vt = Array2D.zeroCreate c c
        if SVD.DecomposeInPlace(U, S, Vt) then
            Some (U, S, Vt)
        else
            None
        
        
    // __MATRIX_SVD_DECOMPOSE__
    [<MethodImpl(MethodImplOptions.NoInlining)>]
    static member DecomposeV(m : M22f) = 
        let pU  = NativePtr.stackalloc<M22f> 1
        let pS  = NativePtr.stackalloc<M22f> 1
        let pVt = NativePtr.stackalloc<M22f> 1
        NativePtr.write pS m
        let tU  = NativeMatrix<float32>(NativePtr.cast pU,  MatrixInfo(0L, V2l(2,2), V2l(1, 2)))
        let tS  = NativeMatrix<float32>(NativePtr.cast pS,  MatrixInfo(0L, V2l(2,2), V2l(1, 2)))
        let tVt = NativeMatrix<float32>(NativePtr.cast pVt, MatrixInfo(0L, V2l(2,2), V2l(1, 2)))
        if SVD.DecomposeInPlace(tU,tS,tVt) then
            ValueSome(struct(NativePtr.read pU, NativePtr.read pS, NativePtr.read pVt))
        else
            ValueNone
    
    static member Decompose(m : M22f) = 
        match SVD.DecomposeV(m) with
        | ValueSome(struct(u, s, vt)) -> Some(u, s, vt)
        | ValueNone -> None
    
    [<MethodImpl(MethodImplOptions.NoInlining)>]
    static member DecomposeV(m : M23f) = 
        let pU  = NativePtr.stackalloc<M22f> 1
        let pS  = NativePtr.stackalloc<M23f> 1
        let pVt = NativePtr.stackalloc<M33f> 1
        NativePtr.write pS m
        let tU  = NativeMatrix<float32>(NativePtr.cast pU,  MatrixInfo(0L, V2l(2,2), V2l(1, 2)))
        let tS  = NativeMatrix<float32>(NativePtr.cast pS,  MatrixInfo(0L, V2l(3,2), V2l(1, 3)))
        let tVt = NativeMatrix<float32>(NativePtr.cast pVt, MatrixInfo(0L, V2l(3,3), V2l(1, 3)))
        if SVD.DecomposeInPlace(tU,tS,tVt) then
            ValueSome(struct(NativePtr.read pU, NativePtr.read pS, NativePtr.read pVt))
        else
            ValueNone
    
    static member Decompose(m : M23f) = 
        match SVD.DecomposeV(m) with
        | ValueSome(struct(u, s, vt)) -> Some(u, s, vt)
        | ValueNone -> None
    
    [<MethodImpl(MethodImplOptions.NoInlining)>]
    static member DecomposeV(m : M33f) = 
        let pU  = NativePtr.stackalloc<M33f> 1
        let pS  = NativePtr.stackalloc<M33f> 1
        let pVt = NativePtr.stackalloc<M33f> 1
        NativePtr.write pS m
        let tU  = NativeMatrix<float32>(NativePtr.cast pU,  MatrixInfo(0L, V2l(3,3), V2l(1, 3)))
        let tS  = NativeMatrix<float32>(NativePtr.cast pS,  MatrixInfo(0L, V2l(3,3), V2l(1, 3)))
        let tVt = NativeMatrix<float32>(NativePtr.cast pVt, MatrixInfo(0L, V2l(3,3), V2l(1, 3)))
        if SVD.DecomposeInPlace(tU,tS,tVt) then
            ValueSome(struct(NativePtr.read pU, NativePtr.read pS, NativePtr.read pVt))
        else
            ValueNone
    
    static member Decompose(m : M33f) = 
        match SVD.DecomposeV(m) with
        | ValueSome(struct(u, s, vt)) -> Some(u, s, vt)
        | ValueNone -> None
    
    [<MethodImpl(MethodImplOptions.NoInlining)>]
    static member DecomposeV(m : M34f) = 
        let pU  = NativePtr.stackalloc<M33f> 1
        let pS  = NativePtr.stackalloc<M34f> 1
        let pVt = NativePtr.stackalloc<M44f> 1
        NativePtr.write pS m
        let tU  = NativeMatrix<float32>(NativePtr.cast pU,  MatrixInfo(0L, V2l(3,3), V2l(1, 3)))
        let tS  = NativeMatrix<float32>(NativePtr.cast pS,  MatrixInfo(0L, V2l(4,3), V2l(1, 4)))
        let tVt = NativeMatrix<float32>(NativePtr.cast pVt, MatrixInfo(0L, V2l(4,4), V2l(1, 4)))
        if SVD.DecomposeInPlace(tU,tS,tVt) then
            ValueSome(struct(NativePtr.read pU, NativePtr.read pS, NativePtr.read pVt))
        else
            ValueNone
    
    static member Decompose(m : M34f) = 
        match SVD.DecomposeV(m) with
        | ValueSome(struct(u, s, vt)) -> Some(u, s, vt)
        | ValueNone -> None
    
    [<MethodImpl(MethodImplOptions.NoInlining)>]
    static member DecomposeV(m : M44f) = 
        let pU  = NativePtr.stackalloc<M44f> 1
        let pS  = NativePtr.stackalloc<M44f> 1
        let pVt = NativePtr.stackalloc<M44f> 1
        NativePtr.write pS m
        let tU  = NativeMatrix<float32>(NativePtr.cast pU,  MatrixInfo(0L, V2l(4,4), V2l(1, 4)))
        let tS  = NativeMatrix<float32>(NativePtr.cast pS,  MatrixInfo(0L, V2l(4,4), V2l(1, 4)))
        let tVt = NativeMatrix<float32>(NativePtr.cast pVt, MatrixInfo(0L, V2l(4,4), V2l(1, 4)))
        if SVD.DecomposeInPlace(tU,tS,tVt) then
            ValueSome(struct(NativePtr.read pU, NativePtr.read pS, NativePtr.read pVt))
        else
            ValueNone
    
    static member Decompose(m : M44f) = 
        match SVD.DecomposeV(m) with
        | ValueSome(struct(u, s, vt)) -> Some(u, s, vt)
        | ValueNone -> None
    
    [<MethodImpl(MethodImplOptions.NoInlining)>]
    static member DecomposeV(m : M22d) = 
        let pU  = NativePtr.stackalloc<M22d> 1
        let pS  = NativePtr.stackalloc<M22d> 1
        let pVt = NativePtr.stackalloc<M22d> 1
        NativePtr.write pS m
        let tU  = NativeMatrix<float>(NativePtr.cast pU,  MatrixInfo(0L, V2l(2,2), V2l(1, 2)))
        let tS  = NativeMatrix<float>(NativePtr.cast pS,  MatrixInfo(0L, V2l(2,2), V2l(1, 2)))
        let tVt = NativeMatrix<float>(NativePtr.cast pVt, MatrixInfo(0L, V2l(2,2), V2l(1, 2)))
        if SVD.DecomposeInPlace(tU,tS,tVt) then
            ValueSome(struct(NativePtr.read pU, NativePtr.read pS, NativePtr.read pVt))
        else
            ValueNone
    
    static member Decompose(m : M22d) = 
        match SVD.DecomposeV(m) with
        | ValueSome(struct(u, s, vt)) -> Some(u, s, vt)
        | ValueNone -> None
    
    [<MethodImpl(MethodImplOptions.NoInlining)>]
    static member DecomposeV(m : M23d) = 
        let pU  = NativePtr.stackalloc<M22d> 1
        let pS  = NativePtr.stackalloc<M23d> 1
        let pVt = NativePtr.stackalloc<M33d> 1
        NativePtr.write pS m
        let tU  = NativeMatrix<float>(NativePtr.cast pU,  MatrixInfo(0L, V2l(2,2), V2l(1, 2)))
        let tS  = NativeMatrix<float>(NativePtr.cast pS,  MatrixInfo(0L, V2l(3,2), V2l(1, 3)))
        let tVt = NativeMatrix<float>(NativePtr.cast pVt, MatrixInfo(0L, V2l(3,3), V2l(1, 3)))
        if SVD.DecomposeInPlace(tU,tS,tVt) then
            ValueSome(struct(NativePtr.read pU, NativePtr.read pS, NativePtr.read pVt))
        else
            ValueNone
    
    static member Decompose(m : M23d) = 
        match SVD.DecomposeV(m) with
        | ValueSome(struct(u, s, vt)) -> Some(u, s, vt)
        | ValueNone -> None
    
    [<MethodImpl(MethodImplOptions.NoInlining)>]
    static member DecomposeV(m : M33d) = 
        let pU  = NativePtr.stackalloc<M33d> 1
        let pS  = NativePtr.stackalloc<M33d> 1
        let pVt = NativePtr.stackalloc<M33d> 1
        NativePtr.write pS m
        let tU  = NativeMatrix<float>(NativePtr.cast pU,  MatrixInfo(0L, V2l(3,3), V2l(1, 3)))
        let tS  = NativeMatrix<float>(NativePtr.cast pS,  MatrixInfo(0L, V2l(3,3), V2l(1, 3)))
        let tVt = NativeMatrix<float>(NativePtr.cast pVt, MatrixInfo(0L, V2l(3,3), V2l(1, 3)))
        if SVD.DecomposeInPlace(tU,tS,tVt) then
            ValueSome(struct(NativePtr.read pU, NativePtr.read pS, NativePtr.read pVt))
        else
            ValueNone
    
    static member Decompose(m : M33d) = 
        match SVD.DecomposeV(m) with
        | ValueSome(struct(u, s, vt)) -> Some(u, s, vt)
        | ValueNone -> None
    
    [<MethodImpl(MethodImplOptions.NoInlining)>]
    static member DecomposeV(m : M34d) = 
        let pU  = NativePtr.stackalloc<M33d> 1
        let pS  = NativePtr.stackalloc<M34d> 1
        let pVt = NativePtr.stackalloc<M44d> 1
        NativePtr.write pS m
        let tU  = NativeMatrix<float>(NativePtr.cast pU,  MatrixInfo(0L, V2l(3,3), V2l(1, 3)))
        let tS  = NativeMatrix<float>(NativePtr.cast pS,  MatrixInfo(0L, V2l(4,3), V2l(1, 4)))
        let tVt = NativeMatrix<float>(NativePtr.cast pVt, MatrixInfo(0L, V2l(4,4), V2l(1, 4)))
        if SVD.DecomposeInPlace(tU,tS,tVt) then
            ValueSome(struct(NativePtr.read pU, NativePtr.read pS, NativePtr.read pVt))
        else
            ValueNone
    
    static member Decompose(m : M34d) = 
        match SVD.DecomposeV(m) with
        | ValueSome(struct(u, s, vt)) -> Some(u, s, vt)
        | ValueNone -> None
    
    [<MethodImpl(MethodImplOptions.NoInlining)>]
    static member DecomposeV(m : M44d) = 
        let pU  = NativePtr.stackalloc<M44d> 1
        let pS  = NativePtr.stackalloc<M44d> 1
        let pVt = NativePtr.stackalloc<M44d> 1
        NativePtr.write pS m
        let tU  = NativeMatrix<float>(NativePtr.cast pU,  MatrixInfo(0L, V2l(4,4), V2l(1, 4)))
        let tS  = NativeMatrix<float>(NativePtr.cast pS,  MatrixInfo(0L, V2l(4,4), V2l(1, 4)))
        let tVt = NativeMatrix<float>(NativePtr.cast pVt, MatrixInfo(0L, V2l(4,4), V2l(1, 4)))
        if SVD.DecomposeInPlace(tU,tS,tVt) then
            ValueSome(struct(NativePtr.read pU, NativePtr.read pS, NativePtr.read pVt))
        else
            ValueNone
    
    static member Decompose(m : M44d) = 
        match SVD.DecomposeV(m) with
        | ValueSome(struct(u, s, vt)) -> Some(u, s, vt)
        | ValueNone -> None
    
    // __MATRIX_SVD_DECOMPOSE__
        

module SVD =
    let inline private dec< ^a, ^c, ^d when (^a or ^d) : (static member Decompose : ^a -> ^c) > (d : ^d) (m : ^a) : ^c =
        ((^a or ^d) : (static member Decompose : ^a -> ^c) (m))
        
    let inline private decip< ^a, ^b, ^c, ^d, ^x when (^a or ^d) : (static member DecomposeInPlace : ^a * ^b * ^c -> ^x) > (d : ^d) (m1 : ^a) (m2 : ^b) (m3 : ^c) : ^x =
        ((^a or ^d) : (static member DecomposeInPlace : ^a * ^b * ^c -> ^x) (m1, m2, m3))
        
    let inline decompose m = dec Unchecked.defaultof<SVD> m
    let inline decomposeInPlace a b  = decip Unchecked.defaultof<SVD> a b


