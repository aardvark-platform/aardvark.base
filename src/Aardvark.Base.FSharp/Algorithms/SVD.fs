namespace Aardvark.Base

open Microsoft.FSharp.NativeInterop

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

        let n = int B.SY
        let m = int B.SX

        let pB = NativePtr.toNativeInt B.Pointer
        let sa = nativeint sizeof< ^a >
        let dbc = nativeint B.DX * sa
        let dbr = nativeint B.DY * sa

        let pw = pB
        let dw = dbc + dbr

        let prvl = pB + dbc
        let drvl = dw
        
        let inline rvl i =
            if i = 0 then rvl0
            else NativeInt.read< ^a > (prvl + nativeint (i - 1) * drvl) //B.[i-1,i]
                
        let inline setrvl i v =
            if i = 0 then rvl0 <- v
            else NativeInt.write (prvl + nativeint (i - 1) * drvl) v //B.[i-1,i] <- v

        let inline w i =
            NativeInt.read< ^a > (pw + nativeint i * dw)
            
        let inline setw i (v :  ^a) =
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
                    if abs (rvl l) + anorm = anorm then
                        flag <- false
                        testing <- false
                    elif abs (w nm) + anorm = anorm then
                        testing <- false
                    else
                        dec &l

                    
                if flag then
                    c <- LanguagePrimitives.GenericZero
                    s <- LanguagePrimitives.GenericOne
                    for i in l .. k do
                        f <- s * rvl i
                        if abs f + anorm <> anorm then
                            g <- w i //B.[i,i]
                            h <- pythag f g
                            setw i h // B.[i,i] <- h
                            h <- LanguagePrimitives.GenericOne / h
                            c <- g * h
                            s <- -f * h

                            applyGivensMat U nm i c s
    
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

                        if z <> LanguagePrimitives.GenericZero then
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

        let mutable values = MapExt<_,_>(cmp, MapExtImplementation.MapEmpty)
        for i in 0 .. m - 1 do
            let v = w i //B.[i,i]
            values <- 
                MapExt.alter v (fun o ->
                    let o = defaultArg o []
                    Some (i :: o)
                ) values
                
        for i0 in 0..m-1 do
            let biggestIdx =
                let (key, indices) = MapExt.tryItem (values.Count - 1) values |> Option.get
                match indices with
                    | [i0] -> 
                        values <- MapExt.remove key values
                        i0
                    | i0 :: r ->
                        values <- MapExt.add key r values
                        i0
                    | _ ->
                        failwith ""
                
            if biggestIdx <> i0 then
                let v0 = w i0 //B.[i0, i0]
                swap i0 biggestIdx
               
                values <- 
                    MapExt.alter v0 (fun o ->
                        let o = Option.defaultValue [] o
                        o |> List.map (fun ii -> if ii = i0 then biggestIdx else ii) |> Some
                    ) values

    
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
    static let floatEps = 1E-15f

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
        

    static member Decompose(m : M22d) =
        let mutable S = m
        let mutable U = M22d()
        let mutable Vt = M22d()
        let mutable suc = false
        tensor {
            let! pS = &S
            let! pU = &U
            let! pVt = &Vt
            suc <- SVD.DecomposeInPlace(pU, pS, pVt)
        }
        if suc then
            Some (U, S, Vt)
        else
            None

    static member Decompose(m : M23d) =
        let mutable S = m
        let mutable U = M22d()
        let mutable Vt = M33d()
        let mutable suc = false
        tensor {
            let! pS = &S
            let! pU = &U
            let! pVt = &Vt
            suc <- SVD.DecomposeInPlace(pU, pS, pVt)
        }
        if suc then
            Some (U, S, Vt)
        else
            None

    static member Decompose(m : M33d) =
        let mutable S = m
        let mutable U = M33d()
        let mutable Vt = M33d()
        let mutable suc = false
        tensor {
            let! pS = &S
            let! pU = &U
            let! pVt = &Vt
            suc <- SVD.DecomposeInPlace(pU, pS, pVt)
        }
        if suc then
            Some (U, S, Vt)
        else
            None

    static member Decompose(m : M34d) =
        let mutable S = m
        let mutable U = M33d()
        let mutable Vt = M44d()
        let mutable suc = false
        tensor {
            let! pS = &S
            let! pU = &U
            let! pVt = &Vt
            suc <- SVD.DecomposeInPlace(pU, pS, pVt)
        }
        if suc then
            Some (U, S, Vt)
        else
            None

    static member Decompose(m : M44d) =
        let mutable S = m
        let mutable U = M44d()
        let mutable Vt = M44d()
        let mutable suc = false
        tensor {
            let! pS = &S
            let! pU = &U
            let! pVt = &Vt
            suc <- SVD.DecomposeInPlace(pU, pS, pVt)
        }
        if suc then
            Some (U, S, Vt)
        else
            None


    static member Decompose(m : M22f) =
        let mutable S = m
        let mutable U = M22f()
        let mutable Vt = M22f()
        let mutable suc = false
        tensor {
            let! pS = &S
            let! pU = &U
            let! pVt = &Vt
            suc <- SVD.DecomposeInPlace(pU, pS, pVt)
        }
        if suc then
            Some (U, S, Vt)
        else
            None

    static member Decompose(m : M23f) =
        let mutable S = m
        let mutable U = M22f()
        let mutable Vt = M33f()
        let mutable suc = false
        tensor {
            let! pS = &S
            let! pU = &U
            let! pVt = &Vt
            suc <- SVD.DecomposeInPlace(pU, pS, pVt)
        }
        if suc then
            Some (U, S, Vt)
        else
            None

    static member Decompose(m : M33f) =
        let mutable S = m
        let mutable U = M33f()
        let mutable Vt = M33f()
        let mutable suc = false
        tensor {
            let! pS = &S
            let! pU = &U
            let! pVt = &Vt
            suc <- SVD.DecomposeInPlace(pU, pS, pVt)
        }
        if suc then
            Some (U, S, Vt)
        else
            None

    static member Decompose(m : M34f) =
        let mutable S = m
        let mutable U = M33f()
        let mutable Vt = M44f()
        let mutable suc = false
        tensor {
            let! pS = &S
            let! pU = &U
            let! pVt = &Vt
            suc <- SVD.DecomposeInPlace(pU, pS, pVt)
        }
        if suc then
            Some (U, S, Vt)
        else
            None

    static member Decompose(m : M44f) =
        let mutable S = m
        let mutable U = M44f()
        let mutable Vt = M44f()
        let mutable suc = false
        tensor {
            let! pS = &S
            let! pU = &U
            let! pVt = &Vt
            suc <- SVD.DecomposeInPlace(pU, pS, pVt)
        }
        if suc then
            Some (U, S, Vt)
        else
            None
        


module SVD =
    let inline private dec< ^a, ^c, ^d when (^a or ^d) : (static member Decompose : ^a -> ^c) > (d : ^d) (m : ^a) : ^c =
        ((^a or ^d) : (static member Decompose : ^a -> ^c) (m))
        
    let inline private decip< ^a, ^b, ^c, ^d, ^x when (^a or ^d) : (static member DecomposeInPlace : ^a * ^b * ^c -> ^x) > (d : ^d) (m1 : ^a) (m2 : ^b) (m3 : ^c) : ^x =
        ((^a or ^d) : (static member DecomposeInPlace : ^a * ^b * ^c -> ^x) (m1, m2, m3))
        
    let inline decompose m = dec Unchecked.defaultof<SVD> m
    let inline decomposeInPlace a b  = decip Unchecked.defaultof<SVD> a b

        

module private SVDOverloadTest = 
    let test() =
        let a = SVD.decompose M34d.Identity
        let a = SVD.decompose M44d.Identity

        let a = Matrix<float>()
        let b = Matrix<float>()
        let c = Matrix<float>()

        SVD.decomposeInPlace a b c

        let arr = Array2D.init 10 10 (fun i j -> 0.1)
        let a = SVD.decompose arr

        ()
