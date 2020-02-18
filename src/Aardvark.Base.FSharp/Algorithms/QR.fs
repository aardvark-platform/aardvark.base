namespace Aardvark.Base

open Microsoft.FSharp.NativeInterop

#nowarn "9"

[<AutoOpen>]
module internal QRHelpers =

    let inline sgn v = if v < LanguagePrimitives.GenericZero then -LanguagePrimitives.GenericOne else LanguagePrimitives.GenericOne

    let inline tiny (eps : 'a) (v : 'a) =
        abs v <= eps

    let inline applyGivensMat (mat : NativeMatrix< ^a >) (c : int) (r : int) (cos : ^a) (sin : ^a) =
#if DEBUG
        let s = min mat.SX mat.SY |> int
        if c < 0 || c >= s || r < 0 || r >= s then failwithf "bad givens (%d, %d)" c r
#endif
        let ptrQ = NativePtr.toNativeInt mat.Pointer
        let dcQ = nativeint sizeof< ^a > * nativeint mat.DX
        let drQ = nativeint sizeof< ^a > * nativeint mat.DY
        let rows = int mat.SY

        let mutable p0 = ptrQ + nativeint c * dcQ 
        let mutable p1 = ptrQ + nativeint r * dcQ      
        // adjust affected elements
        for _ in 0 .. rows - 1 do
            let A = NativeInt.read< ^a > p0 //mat.[ci, c]
            let B = NativeInt.read< ^a > p1 //mat.[ci, r]

            //mat.[ci, c] <-  cos * A + sin * B
            //mat.[ci, r] <- -sin * A + cos * B
            NativeInt.write p0 ( cos * A + sin * B )
            NativeInt.write p1 (-sin * A + cos * B )

            p0 <- p0 + drQ
            p1 <- p1 + drQ
                
    let inline applyGivensTransposedMat (mat : NativeMatrix< ^a >) (c : int) (r : int) (cos : ^a) (sin :  ^a) =
#if DEBUG
        let s = min mat.SX mat.SY |> int
        if c < 0 || c >= s || r < 0 || r >= s then failwithf "bad givens (%d, %d)" c r
#endif
        let ptrQ = NativePtr.toNativeInt mat.Pointer
        let dcQ = nativeint sizeof< ^a > * nativeint mat.DX
        let drQ = nativeint sizeof< ^a > * nativeint mat.DY
        let cols = int mat.SX

        let mutable p0 = ptrQ + nativeint c * drQ 
        let mutable p1 = ptrQ + nativeint r * drQ      
        // adjust affected elements
        for _ in 0 .. cols - 1 do
            let A = NativeInt.read< ^a > p0
            let B = NativeInt.read< ^a > p1
            NativeInt.write p0 ( cos * A + sin * B )
            NativeInt.write p1 (-sin * A + cos * B )

            p0 <- p0 + dcQ
            p1 <- p1 + dcQ
            
    let inline qrDecomposeNative (eps : ^a) (pQ : NativeMatrix< ^a >) (pR : NativeMatrix< ^a >) =
        let rows = int pR.SY
        let cols = int pR.SX

        // pQ <- identity
        pQ.SetByCoord (fun (v : V2i) -> if v.X = v.Y then LanguagePrimitives.GenericOne else LanguagePrimitives.GenericZero)
        
        let sa = nativeint sizeof< ^a >
        let drR = nativeint pR.DY * sa
        let dcR = nativeint pR.DX * sa

        let pr = NativePtr.toNativeInt pR.Pointer

#if DEBUG
        let maxp = nativeint (rows * cols) * sa
#endif
        let inline read (p : nativeint) : ^a =
#if DEBUG    
            if p < 0n || p > maxp then failwithf "[QR] bad read: %A outside %A" p maxp
#endif        
            NativeInt.read< ^a > (pr + p)

        let inline write (p : nativeint) (v : ^a ) : unit =
#if DEBUG    
            if p < 0n || p > maxp then failwithf "[RQ] bad write: %A outside %A" p maxp
#endif        
            NativeInt.write< ^a > (pr + p) v

        let mutable pc0 = 0n
        let mutable pcc = 0n
        for c in 0 .. cols - 1 do
            // wiki performs this loop backwards (should not really matter)
            let mutable prc = pcc + drR
            let mutable pr0 = pc0 + drR
            for r in c + 1 .. rows - 1 do 
                let vcc : ^a = read pcc // important since R.[c,c] changes
                let vrc : ^a = read prc 

                // if the dst-element is not already zero then make it zero
                if not (tiny eps vrc) then

                    // find givens rotation
                    let rho = sgn vcc * sqrt (vcc * vcc + vrc * vrc)
                    let cos = vcc / rho
                    let sin = vrc / rho
                    
                    let mutable p0 = pcc
                    let mutable p1 = prc 
                    // adjust affected elements
                    for ci in c .. cols - 1 do
                        let A = read p0
                        let B = read p1
                        
                        write p0 ( cos * A + sin * B )
                        write p1 (-sin * A + cos * B )
                        
                        p0 <- p0 + dcR
                        p1 <- p1 + dcR

                        
                    // adjust the resulting Q matrix
                    applyGivensMat pQ c r cos sin
                
                pr0 <- pr0 + drR
                prc <- prc + drR
            
            pc0 <- pc0 + drR
            pcc <- pcc + drR + dcR
            
    let inline rqDecomposeNative (eps : ^a) (pR : NativeMatrix< ^a >) (pQ : NativeMatrix< ^a >) =
        let rows = int pR.SY
        let cols = int pR.SX
#if DEBUG    
        if rows > cols then failwithf "cannot RQ decompose matrix with %d rows > %d cols" rows cols
#endif
        // pQ <- identity
        pQ.SetByCoord (fun (v : V2i) -> if v.X = v.Y then LanguagePrimitives.GenericOne else LanguagePrimitives.GenericZero)

        let sa = nativeint sizeof< ^a >
        let drR = nativeint pR.DY * sa
        let dcR = nativeint pR.DX * sa

        let pr = NativePtr.toNativeInt pR.Pointer

#if DEBUG
        let maxp = nativeint (rows * cols) * sa
#endif    
        let inline read (p : nativeint) : ^a = 
#if DEBUG    
            if p < 0n || p > maxp then failwithf "[RQ] bad read: %A outside %A" p maxp
#endif        
            NativeInt.read< ^a > (pr + p)
        
        let inline write (p : nativeint) (v : ^a ) : unit = 
#if DEBUG
            if p < 0n || p > maxp then failwithf "[RQ] bad write: %A outside %A" p maxp
#endif    
            NativeInt.write< ^a > (pr + p) v


        let diag = min cols rows
        let mutable pdd = (dcR + drR) * nativeint (diag - 1)   //ptr (diag - 1) (diag - 1)
        let mutable pd0 = drR * nativeint (diag - 1)           //ptr (diag - 1) 0
        let mutable p0d = dcR * nativeint (diag - 1)           //ptr 0 (diag - 1)

        for d in 1 .. diag - 1 do
            let d = diag - d

            let mutable pdc = pd0
            let mutable p0c = 0n
            for c in 0 .. d - 1 do
                let vcc : ^a = read pdd // important since R.[d,d] changes
                let vrc : ^a = read pdc
                
                // if the dst-element is not already zero then make it zero
                if not (tiny eps vrc) then

                    // find givens rotation
                    let rho = sgn vcc * sqrt (vcc * vcc + vrc * vrc)
                    let cos = vcc / rho
                    let sin = vrc / rho
                    
                    let mutable p0 = p0d
                    let mutable p1 = p0c

                    // adjust affected elements
                    for ri in 0 .. d do
                        //let p0 = ptr ri d
                        //let p1 = ptr ri c
                        let A = read p0
                        let B = read p1
                        
                        write p0 ( cos * A + sin * B )
                        write p1 (-sin * A + cos * B )
                        p0 <- p0 + drR
                        p1 <- p1 + drR
                        
                    // adjust the resulting Q matrix
                    applyGivensTransposedMat pQ d c cos sin
           
                pdc <- pdc + dcR
                p0c <- p0c + dcR

            pdd <- pdd - drR - dcR
            pd0 <- pd0 - drR
            p0d <- p0d - dcR

            

    /// creates a (in-place) decomposition B = U * B' * Vt where
    /// U and V a orthonormal rotations and B' is upper bidiagonal
    /// returns the "anorm" of the resulting B matrix
    let inline qrBidiagonalizeNative (eps : ^a) (U : NativeMatrix< ^a >) (B : NativeMatrix< ^a >) (Vt : NativeMatrix< ^a >) =
        let rows = int B.SY
        let cols = int B.SX

        // set u and v to identity
        U.SetByCoord(fun (v : V2i) -> if v.X = v.Y then LanguagePrimitives.GenericOne else LanguagePrimitives.GenericZero)
        Vt.SetByCoord(fun (v : V2i) -> if v.X = v.Y then LanguagePrimitives.GenericOne else LanguagePrimitives.GenericZero)

        let pbSize = nativeint B.SX * nativeint B.SY * nativeint sizeof< ^a >

        let sa = nativeint sizeof< ^a >
        let pB = NativePtr.toNativeInt B.Pointer
        let dbr = nativeint B.DY * sa
        let dbc = nativeint B.DX * sa
        
        let inline read (ptr : nativeint) : ^a = 
#if DEBUG
             let dist = ptr - pB
             if dist < 0n || dist >= pbSize then 
                 failwithf "bad offset: %A" (int64 pbSize / int64 sizeof< ^a >)
#endif
             NativeInt.read< ^a> ptr

        let inline write (ptr : nativeint) (value : ^a) =  
#if DEBUG
             let dist = ptr - pB
             if dist < 0n || dist >= pbSize then 
                 failwithf "bad offset: %A" (int64 pbSize / int64 sizeof< ^a >)
#endif
             NativeInt.write< ^a > ptr value

        let mutable anorm = LanguagePrimitives.GenericZero

        let mutable pii = pB
        let s = min rows cols 
        for i in 0 .. s - 1 do

            // make the subdiagonal column 0
            let mutable pi1i = pii + dbr
            for j in i + 1 .. rows - 1 do
                let vcc = read pii     //B.[i,i]
                let vrc = read pi1i    //B.[j,i]

                // if the dst-element is not already zero then make it zero
                if not (tiny eps vrc) then

                    // find givens rotation
                    let rho = sgn vcc * sqrt (vcc * vcc + vrc * vrc)
                    let cos = vcc / rho
                    let sin = vrc / rho
                    
                    // adjust affected elements
                    let mutable pr0 = pii 
                    let mutable pr1 = pi1i

                    // first iteration
                    let a = read pr0 //B.[r0,ci]
                    let b = read pr1 //B.[r1,ci]
                    write pr0 ( cos * a + sin * b )
                    write pr1 LanguagePrimitives.GenericZero
                    pr1 <- pr1 + dbc
                    pr0 <- pr0 + dbc

                    for _ in i+1 .. cols - 1 do
                        let a = read pr0 //B.[r0,ci]
                        let b = read pr1 //B.[r1,ci]
                        write pr0 ( cos * a + sin * b )
                        write pr1 (-sin * a + cos * b )
                        pr1 <- pr1 + dbc
                        pr0 <- pr0 + dbc

                    // adjust the resulting U matrix
                    applyGivensMat U i j cos sin
                    
                pi1i <- pi1i + dbr

                
            // step one to the right
            pii <- pii + dbc
            let i = i + 1
            let mutable pij = pii + dbc
            
            // make the 2nd superdiagonal row 0
            for j in i + 1 .. cols - 1 do
                let vcc = read pii     //B.[i,i+1] // important since R.[c,c] changes
                let vrc = read pij     //B.[i,j]

                // if the dst-element is not already zero then make it zero
                if not (tiny eps vrc) then

                    // find givens rotation
                    let rho = sgn vcc * sqrt (vcc * vcc + vrc * vrc)
                    let sin = vrc / rho
                    let cos = vcc / rho
                    
                    // adjust affected elements
                    let mutable pc0 = pii
                    let mutable pc1 = pij
                    
                    // first iteration
                    let a = read pc0 //B.[r0,ci]
                    let b = read pc1 //B.[r1,ci]
                    write pc0 ( cos * a + sin * b )
                    write pc1 LanguagePrimitives.GenericZero
                    pc0 <- pc0 + dbr
                    pc1 <- pc1 + dbr

                    for _ in i+1 .. rows do
                        let a = read pc0 //B.[r0,ci]
                        let b = read pc1 //B.[r1,ci]
                        write pc0 ( cos * a + sin * b )
                        write pc1 (-sin * a + cos * b )
                        pc0 <- pc0 + dbr
                        pc1 <- pc1 + dbr
                        
                    // adjust the resulting Vt matrix
                    applyGivensTransposedMat Vt i j cos sin
                    
                pij <- pij + dbc
                
            let pd = pii - dbc
            let i = i - 1
            let normv =
                if i > 0 then 
                    // abs B.[i-1,i] + abs B.[i,i]
                    abs (read (pd - dbc)) + abs (read pd)
                else 
                    // abs B.[i,i]
                    abs (read pd)

            anorm <- max anorm normv
            pii <- pii + dbr

        anorm



[<AbstractClass; Sealed>]
type QR private() =

    static let doubleEps = 1E-20
    static let floatEps =  float32 1E-6


    static member DecomposeInPlace(Q : float[,], R : float[,]) =
        let mutable Q = Q
        let mutable R = R
        tensor {
            let! pQ = &Q
            let! pR = &R
            qrDecomposeNative doubleEps pQ pR
        }

    static member DecomposeInPlace(Q : float32[,], R : float32[,]) =
        let mutable Q = Q
        let mutable R = R
        tensor {
            let! pQ = &Q
            let! pR = &R
            qrDecomposeNative floatEps pQ pR
        }

    static member DecomposeInPlace(Q : NativeMatrix<float>, R : NativeMatrix<float>) =
        qrDecomposeNative doubleEps Q R
        
    static member DecomposeInPlace(Q : NativeMatrix<float32>, R : NativeMatrix<float32>) =
        qrDecomposeNative floatEps Q R
        
    static member DecomposeInPlace(Q : Matrix<float>, R : Matrix<float>) =
        let mutable Q = Q
        let mutable R = R
        tensor {
            let! pQ = &Q
            let! pR = &R
            qrDecomposeNative doubleEps pQ pR
        }
        
    static member DecomposeInPlace(Q : Matrix<float32>, R : Matrix<float32>) =
        let mutable Q = Q
        let mutable R = R
        tensor {
            let! pQ = &Q
            let! pR = &R
            qrDecomposeNative floatEps pQ pR
        }
        
    static member Decompose (m : float[,]) =
        let rows = m.GetLength(0)
        let R = Array2D.copy m
        let Q = Array2D.zeroCreate rows rows
        QR.DecomposeInPlace(Q, R)
        Q, R
        
    static member Decompose (m : Matrix<float>) =
        let R = m.Copy()
        let Q = Matrix<float>(m.SY, m.SY)
        QR.DecomposeInPlace(Q, R)
        Q, R
        
    static member Decompose (m : M22d) =
        let aTmp = [|M22d(); m|]

        use pTmp = fixed aTmp
        let pR = NativePtr.add pTmp 1

        let tQ = NativeMatrix<float>(NativePtr.cast pTmp, MatrixInfo(0L,V2l(2,2),V2l(1,2)))
        let tR = NativeMatrix<float>(NativePtr.cast pR,   MatrixInfo(0L,V2l(2,2),V2l(1,2)))

        QR.DecomposeInPlace(tQ, tR)

        aTmp.[0], aTmp.[1]
        
        //let mutable R = m
        //let mutable Q = M22d()
        //tensor {
        //    let! pR = &R
        //    let! pQ = &Q
        //    QR.DecomposeInPlace(pQ, pR)
        //}
        //Q, R
        
    static member Decompose (m : M23d) =
        let aQ = [|M22d()|]
        let aR = [|m|]
        
        use pQ = fixed aQ
        use pR = fixed aR
        
        let tQ = NativeMatrix<float>(NativePtr.cast pQ, MatrixInfo(0L,V2l(2,2),V2l(1,2)))
        let tR = NativeMatrix<float>(NativePtr.cast pR, MatrixInfo(0L,V2l(3,2),V2l(1,3)))
        
        QR.DecomposeInPlace(tQ, tR)
        
        aQ.[0], aR.[0]

        //let mutable R = m
        //let mutable Q = M22d()
        //tensor {
        //    let! pR = &R
        //    let! pQ = &Q
        //    QR.DecomposeInPlace(pQ, pR)
        //}
        //Q, R
        
    static member Decompose (m : M33d) =
        let aTmp = [|M33d(); m|]

        use pTmp = fixed aTmp
        let pR = NativePtr.add pTmp 1

        let tQ = NativeMatrix<float>(NativePtr.cast pTmp, MatrixInfo(0L,V2l(3,3),V2l(1,3)))
        let tR = NativeMatrix<float>(NativePtr.cast pR,   MatrixInfo(0L,V2l(3,3),V2l(1,3)))

        QR.DecomposeInPlace(tQ, tR)

        aTmp.[0], aTmp.[1]
        //let mutable R = m
        //let mutable Q = M33d()
        //tensor {
        //    let! pR = &R
        //    let! pQ = &Q
        //    QR.DecomposeInPlace(pQ, pR)
        //}
        //Q, R

    static member Decompose (m : M34d) =
        let aQ = [|M33d()|]
        let aR = [|m|]
        
        use pQ = fixed aQ
        use pR = fixed aR
        
        let tQ = NativeMatrix<float>(NativePtr.cast pQ, MatrixInfo(0L,V2l(3,3),V2l(1,3)))
        let tR = NativeMatrix<float>(NativePtr.cast pR, MatrixInfo(0L,V2l(4,3),V2l(1,4)))
        
        QR.DecomposeInPlace(tQ, tR)
        
        aQ.[0], aR.[0]
        //let mutable R = m
        //let mutable Q = M33d()
        //tensor {
        //    let! pR = &R
        //    let! pQ = &Q
        //    QR.DecomposeInPlace(pQ, pR)
        //}
        //Q, R

    static member Decompose (m : M44d) =
        let aTmp = [|M44d(); m|]

        use pTmp = fixed aTmp
        let pR = NativePtr.add pTmp 1

        let tQ = NativeMatrix<float>(NativePtr.cast pTmp, MatrixInfo(0L,V2l(4,4),V2l(1,4)))
        let tR = NativeMatrix<float>(NativePtr.cast pR,   MatrixInfo(0L,V2l(4,4),V2l(1,4)))

        QR.DecomposeInPlace(tQ, tR)

        aTmp.[0], aTmp.[1]
        //let mutable R = m
        //let mutable Q = M44d()
        //tensor {
        //    let! pR = &R
        //    let! pQ = &Q
        //    QR.DecomposeInPlace(pQ, pR)
        //}
        //Q, R
        
    static member Decompose (m : float32[,]) =
        let rows = m.GetLength(0)
        let R = Array2D.copy m
        let Q = Array2D.zeroCreate rows rows
        QR.DecomposeInPlace(Q, R)
        Q, R
        
    static member Decompose (m : Matrix<float32>) =
        let R = m.Copy()
        let Q = Matrix<float32>(m.SY, m.SY)
        QR.DecomposeInPlace(Q, R)
        Q, R
       
    static member Decompose (m : M22f) =
        let aTmp = [|M22f(); m|]

        use pTmp = fixed aTmp
        let pR = NativePtr.add pTmp 1

        let tQ = NativeMatrix<float32>(NativePtr.cast pTmp, MatrixInfo(0L,V2l(2,2),V2l(1,2)))
        let tR = NativeMatrix<float32>(NativePtr.cast pR,   MatrixInfo(0L,V2l(2,2),V2l(1,2)))

        QR.DecomposeInPlace(tQ, tR)

        aTmp.[0], aTmp.[1]
        //let mutable R = m
        //let mutable Q = M22f()
        //tensor {
        //    let! pR = &R
        //    let! pQ = &Q
        //    QR.DecomposeInPlace(pQ, pR)
        //}
        //Q, R
        
    static member Decompose (m : M23f) =
        let aQ = [|M22f()|]
        let aR = [|m|]
        
        use pQ = fixed aQ
        use pR = fixed aR
        
        let tQ = NativeMatrix<float32>(NativePtr.cast pQ, MatrixInfo(0L,V2l(2,2),V2l(1,2)))
        let tR = NativeMatrix<float32>(NativePtr.cast pR, MatrixInfo(0L,V2l(3,2),V2l(1,3)))
        
        QR.DecomposeInPlace(tQ, tR)
        
        aQ.[0], aR.[0]

        //let mutable R = m
        //let mutable Q = M22f()
        //tensor {
        //    let! pR = &R
        //    let! pQ = &Q
        //    QR.DecomposeInPlace(pQ, pR)
        //}
        //Q, R
        
    static member Decompose (m : M33f) =
        let aTmp = [|M33f(); m|]

        use pTmp = fixed aTmp
        let pR = NativePtr.add pTmp 1

        let tQ = NativeMatrix<float32>(NativePtr.cast pTmp, MatrixInfo(0L,V2l(3,3),V2l(1,3)))
        let tR = NativeMatrix<float32>(NativePtr.cast pR,   MatrixInfo(0L,V2l(3,3),V2l(1,3)))

        QR.DecomposeInPlace(tQ, tR)

        aTmp.[0], aTmp.[1]
        //let mutable R = m
        //let mutable Q = M33f()
        //tensor {
        //    let! pR = &R
        //    let! pQ = &Q
        //    QR.DecomposeInPlace(pQ, pR)
        //}
        //Q, R

    static member Decompose (m : M34f) =
        let aQ = [|M33f()|]
        let aR = [|m|]
        
        use pQ = fixed aQ
        use pR = fixed aR
        
        let tQ = NativeMatrix<float32>(NativePtr.cast pQ, MatrixInfo(0L,V2l(3,3),V2l(1,3)))
        let tR = NativeMatrix<float32>(NativePtr.cast pR, MatrixInfo(0L,V2l(4,3),V2l(1,4)))
        
        QR.DecomposeInPlace(tQ, tR)
        
        aQ.[0], aR.[0]
        //let mutable R = m
        //let mutable Q = M33f()
        //tensor {
        //    let! pR = &R
        //    let! pQ = &Q
        //    QR.DecomposeInPlace(pQ, pR)
        //}
        //Q, R

    static member Decompose (m : M44f) =
        let aTmp = [|M44f(); m|]

        use pTmp = fixed aTmp
        let pR = NativePtr.add pTmp 1

        let tQ = NativeMatrix<float32>(NativePtr.cast pTmp, MatrixInfo(0L,V2l(4,4),V2l(1,4)))
        let tR = NativeMatrix<float32>(NativePtr.cast pR,   MatrixInfo(0L,V2l(4,4),V2l(1,4)))

        QR.DecomposeInPlace(tQ, tR)

        aTmp.[0], aTmp.[1]
        //let mutable R = m
        //let mutable Q = M44f()
        //tensor {
        //    let! pR = &R
        //    let! pQ = &Q
        //    QR.DecomposeInPlace(pQ, pR)
        //}
        //Q, R
        
        
    static member BidiagonalizeInPlaceWithNorm(U : NativeMatrix<float>, B : NativeMatrix<float>, Vt : NativeMatrix<float>) =
        qrBidiagonalizeNative doubleEps U B Vt
        
    static member BidiagonalizeInPlace(U : NativeMatrix<float>, B : NativeMatrix<float>, Vt : NativeMatrix<float>) =
        qrBidiagonalizeNative doubleEps U B Vt |> ignore
        
    static member BidiagonalizeInPlace(U : NativeMatrix<float32>, B : NativeMatrix<float32>, Vt : NativeMatrix<float32>) =
        qrBidiagonalizeNative floatEps U B Vt |> ignore

    static member BidiagonalizeInPlace(U : Matrix<float>, B : Matrix<float>, Vt : Matrix<float>) =
        let mutable U = U
        let mutable B = B
        let mutable Vt = Vt

        tensor {
            let! pU = &U
            let! pB = &B
            let! pVt = &Vt
            return qrBidiagonalizeNative doubleEps pU pB pVt |> ignore
        }

    static member BidiagonalizeInPlace(U : Matrix<float32>, B : Matrix<float32>, Vt : Matrix<float32>) =
        let mutable U = U
        let mutable B = B
        let mutable Vt = Vt

        tensor {
            let! pU = &U
            let! pB = &B
            let! pVt = &Vt
            return qrBidiagonalizeNative floatEps pU pB pVt |> ignore
        }

    static member BidiagonalizeInPlace(U : float[,], B : float[,], Vt : float[,]) =
        let mutable U = U
        let mutable B = B
        let mutable Vt = Vt

        tensor {
            let! pU = &U
            let! pB = &B
            let! pVt = &Vt
            return qrBidiagonalizeNative doubleEps pU pB pVt |> ignore
        }

    static member BidiagonalizeInPlace(U : float32[,], B : float32[,], Vt : float32[,]) =
        let mutable U = U
        let mutable B = B
        let mutable Vt = Vt

        tensor {
            let! pU = &U
            let! pB = &B
            let! pVt = &Vt
            return qrBidiagonalizeNative floatEps pU pB pVt |> ignore
        }

   
    static member Bidiagonalize(m : Matrix<float>) =
        let U = Matrix<float>(m.SY, m.SY)
        let B = m.Copy()
        let Vt = Matrix<float>(m.SX, m.SX)
        QR.BidiagonalizeInPlace(U, B, Vt) |> ignore
        U, B, Vt

    static member Bidiagonalize(m : float[,]) =
        let r = m.GetLength(0)
        let c = m.GetLength(1)
        let U = Array2D.zeroCreate r r
        let B = Array2D.copy m
        let Vt = Array2D.zeroCreate c c
        QR.BidiagonalizeInPlace(U, B, Vt) |> ignore
        U, B, Vt       

    static member Bidiagonalize (m : M22d) =
        let aTmp = [| M22d(); m; M22d()|]

        use pTmp = fixed aTmp
        let pB =  NativePtr.add pTmp 1
        let pVt = NativePtr.add pTmp 2

        let tU  = NativeMatrix<float>(NativePtr.cast pTmp,  MatrixInfo(0L, V2l(2,2), V2l(1, 2)))
        let tB  = NativeMatrix<float>(NativePtr.cast pB,    MatrixInfo(0L, V2l(2,2), V2l(1, 2)))
        let tVt = NativeMatrix<float>(NativePtr.cast pVt,   MatrixInfo(0L, V2l(2,2), V2l(1, 2)))

        QR.BidiagonalizeInPlace(tU,tB,tVt)

        aTmp.[0], aTmp.[1], aTmp.[2]
        //let mutable U = M22d()
        //let mutable B = m
        //let mutable Vt = M22d()
        //tensor {
        //    let! pU = &U
        //    let! pB = &B
        //    let! pVt = &Vt
        //    QR.BidiagonalizeInPlace(pU, pB, pVt)
        //}
        //U, B, Vt

    static member Bidiagonalize (m : M23d) =
        let aU  = [|M22d()|]
        let aB  = [|m|]
        let aVt = [|M33d()|]

        use pU =  fixed aU
        use pB =  fixed aB
        use pVt = fixed aVt

        let tU  = NativeMatrix<float>(NativePtr.cast pU,    MatrixInfo(0L, V2l(2,2), V2l(1, 2)))
        let tB  = NativeMatrix<float>(NativePtr.cast pB,    MatrixInfo(0L, V2l(3,2), V2l(1, 3)))
        let tVt = NativeMatrix<float>(NativePtr.cast pVt,   MatrixInfo(0L, V2l(3,3), V2l(1, 3)))

        QR.BidiagonalizeInPlace(tU,tB,tVt)

        aU.[0], aB.[0], aVt.[0]
        //let mutable U = M22d()
        //let mutable B = m
        //let mutable Vt = M33d()
        //tensor {
        //    let! pU = &U
        //    let! pB = &B
        //    let! pVt = &Vt
        //    QR.BidiagonalizeInPlace(pU, pB, pVt)
        //}
        //U, B, Vt

    static member Bidiagonalize (m : M33d) =
        let aTmp = [| M33d(); m; M33d()|]

        use pTmp = fixed aTmp
        let pB =  NativePtr.add pTmp 1
        let pVt = NativePtr.add pTmp 2

        let tU  = NativeMatrix<float>(NativePtr.cast pTmp,  MatrixInfo(0L, V2l(3,3), V2l(1, 3)))
        let tB  = NativeMatrix<float>(NativePtr.cast pB,    MatrixInfo(0L, V2l(3,3), V2l(1, 3)))
        let tVt = NativeMatrix<float>(NativePtr.cast pVt,   MatrixInfo(0L, V2l(3,3), V2l(1, 3)))

        QR.BidiagonalizeInPlace(tU,tB,tVt)

        aTmp.[0], aTmp.[1], aTmp.[2]
        //let mutable U = M33d()
        //let mutable B = m
        //let mutable Vt = M33d()
        //tensor {
        //    let! pU = &U
        //    let! pB = &B
        //    let! pVt = &Vt
        //    QR.BidiagonalizeInPlace(pU, pB, pVt)
        //}
        //U, B, Vt

    static member Bidiagonalize (m : M34d) =
        let aU  = [|M33d()|]
        let aB  = [|m|]
        let aVt = [|M44d()|]

        use pU =  fixed aU
        use pB =  fixed aB
        use pVt = fixed aVt

        let tU  = NativeMatrix<float>(NativePtr.cast pU,    MatrixInfo(0L, V2l(3,3), V2l(1, 3)))
        let tB  = NativeMatrix<float>(NativePtr.cast pB,    MatrixInfo(0L, V2l(4,3), V2l(1, 4)))
        let tVt = NativeMatrix<float>(NativePtr.cast pVt,   MatrixInfo(0L, V2l(4,4), V2l(1, 4)))

        QR.BidiagonalizeInPlace(tU,tB,tVt)

        aU.[0], aB.[0], aVt.[0]
        //let mutable U = M33d()
        //let mutable B = m
        //let mutable Vt = M44d()
        //tensor {
        //    let! pU = &U
        //    let! pB = &B
        //    let! pVt = &Vt
        //    QR.BidiagonalizeInPlace(pU, pB, pVt)
        //}
        //U, B, Vt

    static member Bidiagonalize (m : M44d) =
        let aTmp = [| M44d(); m; M44d()|]

        use pTmp = fixed aTmp
        let pB =  NativePtr.add pTmp 1
        let pVt = NativePtr.add pTmp 2

        let tU  = NativeMatrix<float>(NativePtr.cast pTmp,  MatrixInfo(0L, V2l(4,4), V2l(1, 4)))
        let tB  = NativeMatrix<float>(NativePtr.cast pB,    MatrixInfo(0L, V2l(4,4), V2l(1, 4)))
        let tVt = NativeMatrix<float>(NativePtr.cast pVt,   MatrixInfo(0L, V2l(4,4), V2l(1, 4)))

        QR.BidiagonalizeInPlace(tU,tB,tVt)

        aTmp.[0], aTmp.[1], aTmp.[2]
        //let mutable U = M44d()
        //let mutable B = m
        //let mutable Vt = M44d()
        //tensor {
        //    let! pU = &U
        //    let! pB = &B
        //    let! pVt = &Vt
        //    QR.BidiagonalizeInPlace(pU, pB, pVt)
        //}
        //U, B, Vt

    static member Bidiagonalize(m : Matrix<float32>) =
        let U = Matrix<float32>(m.SY, m.SY)
        let B = m.Copy()
        let Vt = Matrix<float32>(m.SX, m.SX)
        QR.BidiagonalizeInPlace(U, B, Vt) |> ignore
        U, B, Vt

    static member Bidiagonalize(m : float32[,]) =
        let r = m.GetLength(0)
        let c = m.GetLength(1)
        let U = Array2D.zeroCreate r r
        let B = Array2D.copy m
        let Vt = Array2D.zeroCreate c c
        QR.BidiagonalizeInPlace(U, B, Vt) |> ignore
        U, B, Vt       
       
    static member Bidiagonalize (m : M22f) =
        let aTmp = [| M22f(); m; M22f()|]

        use pTmp = fixed aTmp
        let pB =  NativePtr.add pTmp 1
        let pVt = NativePtr.add pTmp 2

        let tU  = NativeMatrix<float32>(NativePtr.cast pTmp,  MatrixInfo(0L, V2l(2,2), V2l(1, 2)))
        let tB  = NativeMatrix<float32>(NativePtr.cast pB,    MatrixInfo(0L, V2l(2,2), V2l(1, 2)))
        let tVt = NativeMatrix<float32>(NativePtr.cast pVt,   MatrixInfo(0L, V2l(2,2), V2l(1, 2)))

        QR.BidiagonalizeInPlace(tU,tB,tVt)

        aTmp.[0], aTmp.[1], aTmp.[2]
        //let mutable U = M22f()
        //let mutable B = m
        //let mutable Vt = M22f()
        //tensor {
        //    let! pU = &U
        //    let! pB = &B
        //    let! pVt = &Vt
        //    QR.BidiagonalizeInPlace(pU, pB, pVt)
        //}
        //U, B, Vt

    static member Bidiagonalize (m : M23f) =
        let aU  = [|M22f()|]
        let aB  = [|m|]
        let aVt = [|M33f()|]

        use pU =  fixed aU
        use pB =  fixed aB
        use pVt = fixed aVt

        let tU  = NativeMatrix<float32>(NativePtr.cast pU,    MatrixInfo(0L, V2l(2,2), V2l(1, 2)))
        let tB  = NativeMatrix<float32>(NativePtr.cast pB,    MatrixInfo(0L, V2l(3,2), V2l(1, 3)))
        let tVt = NativeMatrix<float32>(NativePtr.cast pVt,   MatrixInfo(0L, V2l(3,3), V2l(1, 3)))

        QR.BidiagonalizeInPlace(tU,tB,tVt)

        aU.[0], aB.[0], aVt.[0]
        //let mutable U = M22f()
        //let mutable B = m
        //let mutable Vt = M33f()
        //tensor {
        //    let! pU = &U
        //    let! pB = &B
        //    let! pVt = &Vt
        //    QR.BidiagonalizeInPlace(pU, pB, pVt)
        //}
        //U, B, Vt

    static member Bidiagonalize (m : M33f) =
        let aTmp = [| M33f(); m; M33f()|]

        use pTmp = fixed aTmp
        let pB =  NativePtr.add pTmp 1
        let pVt = NativePtr.add pTmp 2

        let tU  = NativeMatrix<float32>(NativePtr.cast pTmp,  MatrixInfo(0L, V2l(3,3), V2l(1, 3)))
        let tB  = NativeMatrix<float32>(NativePtr.cast pB,    MatrixInfo(0L, V2l(3,3), V2l(1, 3)))
        let tVt = NativeMatrix<float32>(NativePtr.cast pVt,   MatrixInfo(0L, V2l(3,3), V2l(1, 3)))

        QR.BidiagonalizeInPlace(tU,tB,tVt)

        aTmp.[0], aTmp.[1], aTmp.[2]
        //let mutable U = M33f()
        //let mutable B = m
        //let mutable Vt = M33f()
        //tensor {
        //    let! pU = &U
        //    let! pB = &B
        //    let! pVt = &Vt
        //    QR.BidiagonalizeInPlace(pU, pB, pVt)
        //}
        //U, B, Vt

    static member Bidiagonalize (m : M34f) =
        let aU  = [|M33f()|]
        let aB  = [|m|]
        let aVt = [|M44f()|]

        use pU =  fixed aU
        use pB =  fixed aB
        use pVt = fixed aVt

        let tU  = NativeMatrix<float32>(NativePtr.cast pU,    MatrixInfo(0L, V2l(3,3), V2l(1, 3)))
        let tB  = NativeMatrix<float32>(NativePtr.cast pB,    MatrixInfo(0L, V2l(4,3), V2l(1, 4)))
        let tVt = NativeMatrix<float32>(NativePtr.cast pVt,   MatrixInfo(0L, V2l(4,4), V2l(1, 4)))

        QR.BidiagonalizeInPlace(tU,tB,tVt)

        aU.[0], aB.[0], aVt.[0]
        //let mutable U = M33f()
        //let mutable B = m
        //let mutable Vt = M44f()
        //tensor {
        //    let! pU = &U
        //    let! pB = &B
        //    let! pVt = &Vt
        //    QR.BidiagonalizeInPlace(pU, pB, pVt)
        //}
        //U, B, Vt

    static member Bidiagonalize (m : M44f) =
        let aTmp = [| M44f(); m; M44f()|]

        use pTmp = fixed aTmp
        let pB =  NativePtr.add pTmp 1
        let pVt = NativePtr.add pTmp 2

        let tU  = NativeMatrix<float32>(NativePtr.cast pTmp,  MatrixInfo(0L, V2l(4,4), V2l(1, 4)))
        let tB  = NativeMatrix<float32>(NativePtr.cast pB,    MatrixInfo(0L, V2l(4,4), V2l(1, 4)))
        let tVt = NativeMatrix<float32>(NativePtr.cast pVt,   MatrixInfo(0L, V2l(4,4), V2l(1, 4)))

        QR.BidiagonalizeInPlace(tU,tB,tVt)

        aTmp.[0], aTmp.[1], aTmp.[2]
        //let mutable U = M44f()
        //let mutable B = m
        //let mutable Vt = M44f()
        //tensor {
        //    let! pU = &U
        //    let! pB = &B
        //    let! pVt = &Vt
        //    QR.BidiagonalizeInPlace(pU, pB, pVt)
        //}
        //U, B, Vt

module QR =
    let inline private dec< ^a, ^c, ^d when (^a or ^d) : (static member Decompose : ^a -> ^c) > (d : ^d) (m : ^a) : ^c =
        ((^a or ^d) : (static member Decompose : ^a -> ^c) (m))
        
    let inline private decip< ^a, ^b, ^c, ^d when (^a or ^d) : (static member DecomposeInPlace : ^a * ^b -> ^c) > (d : ^d) (m1 : ^a) (m2 : ^b) : ^c =
        ((^a or ^d) : (static member DecomposeInPlace : ^a * ^b -> ^c) (m1, m2))
        
    let inline private bidiag< ^a, ^c, ^d when (^a or ^d) : (static member Bidiagonalize : ^a -> ^c) > (d : ^d) (m : ^a) : ^c =
        ((^a or ^d) : (static member Bidiagonalize : ^a -> ^c) (m))
        
    let inline private bidiagip< ^a, ^b, ^c, ^d, ^x when (^a or ^d) : (static member BidiagonalizeInPlace : ^a * ^b * ^c -> ^x) > (d : ^d) (m1 : ^a) (m2 : ^b) (m3 : ^c) : ^x =
        ((^a or ^d) : (static member BidiagonalizeInPlace : ^a * ^b * ^c -> ^x) (m1, m2, m3))
        
    let inline decompose m = dec Unchecked.defaultof<QR> m
    let inline decomposeInPlace a b  = decip Unchecked.defaultof<QR> a b
    let inline bidiagonalize m = bidiag Unchecked.defaultof<QR> m
    let inline bidiagonalizeInPlace a b c = bidiagip Unchecked.defaultof<QR> a b c



[<AbstractClass; Sealed>]
type RQ private() =

    static let doubleEps = 1E-20
    static let floatEps =  float32 1E-6


    static member DecomposeInPlace(R : float[,], Q : float[,]) =
        let mutable R = R
        let mutable Q = Q
        tensor {
            let! pQ = &Q
            let! pR = &R
            rqDecomposeNative doubleEps pR pQ
        }

    static member DecomposeInPlace(R : float32[,], Q : float32[,]) =
        let mutable R = R
        let mutable Q = Q
        tensor {
            let! pQ = &Q
            let! pR = &R
            rqDecomposeNative floatEps pR pQ
        }

    static member DecomposeInPlace(R : NativeMatrix<float>, Q : NativeMatrix<float>) =
        rqDecomposeNative doubleEps R Q
        
    static member DecomposeInPlace(R : NativeMatrix<float32>, Q : NativeMatrix<float32>) =
        rqDecomposeNative floatEps R Q
        
    static member DecomposeInPlace(R : Matrix<float>, Q : Matrix<float>) =
        let mutable R = R
        let mutable Q = Q
        tensor {
            let! pR = &R
            let! pQ = &Q
            rqDecomposeNative doubleEps pR pQ
        }
        
    static member DecomposeInPlace(R : Matrix<float32>, Q : Matrix<float32>) =
        let mutable R = R
        let mutable Q = Q
        tensor {
            let! pR = &R
            let! pQ = &Q
            rqDecomposeNative floatEps pR pQ
        }
        
    static member Decompose (m : float[,]) =
        let cols = m.GetLength(1)
        let R = Array2D.copy m
        let Q = Array2D.zeroCreate cols cols
        RQ.DecomposeInPlace(R, Q)
        R, Q
        
    static member Decompose (m : Matrix<float>) =
        let R = m.Copy()
        let Q = Matrix<float>(m.SX, m.SX)
        RQ.DecomposeInPlace(R, Q)
        R, Q
        
    static member Decompose (m : M22d) =
        let tmp = [|m; M22d()|]

        use pTmp = fixed tmp
        let pQ = NativePtr.add pTmp 1

        let tR = NativeMatrix<float>(NativePtr.cast pTmp, MatrixInfo(0L,V2l(2,2),V2l(1,2)))
        let tQ = NativeMatrix<float>(NativePtr.cast pQ,   MatrixInfo(0L,V2l(2,2),V2l(1,2)))

        RQ.DecomposeInPlace(tR, tQ)

        tmp.[0], tmp.[1]

        //let mutable R = m
        //let mutable Q = M22d()
        //tensor {
        //    let! pR = &R
        //    let! pQ = &Q
        //    RQ.DecomposeInPlace(pR, pQ)
        //}
        //R, Q
        
    static member Decompose (m : M23d) =
        let aR = [|m|]
        let aQ = [|M33d()|]
        
        use pR = fixed aR
        use pQ = fixed aQ
        
        let tR = NativeMatrix<float>(NativePtr.cast pR, MatrixInfo(0L,V2l(3,2),V2l(1,3)))
        let tQ = NativeMatrix<float>(NativePtr.cast pQ, MatrixInfo(0L,V2l(3,3),V2l(1,3)))
        
        RQ.DecomposeInPlace(tR, tQ)
        
        aR.[0], aQ.[0]

        //let mutable R = m
        //let mutable Q = M33d()
        //tensor {
        //    let! pR = &R
        //    let! pQ = &Q
        //    RQ.DecomposeInPlace(pR, pQ)
        //}
        //R, Q
        
    static member Decompose (m : M33d) =
        let tmp = [|m; M33d()|]

        use pTmp = fixed tmp
        let pQ = NativePtr.add pTmp 1

        let tR = NativeMatrix<float>(NativePtr.cast pTmp, MatrixInfo(0L,V2l(3,3),V2l(1,3)))
        let tQ = NativeMatrix<float>(NativePtr.cast pQ,   MatrixInfo(0L,V2l(3,3),V2l(1,3)))

        RQ.DecomposeInPlace(tR, tQ)

        tmp.[0], tmp.[1]

        //let mutable R = m
        //let mutable Q = M33d()
        //tensor {
        //    let! pR = &R
        //    let! pQ = &Q
        //    RQ.DecomposeInPlace(pR, pQ)
        //}
        //R, Q

    static member Decompose (m : M34d) =
        let aR = [|m|]
        let aQ = [|M44d()|]
        
        use pR = fixed aR
        use pQ = fixed aQ
        
        let tR = NativeMatrix<float>(NativePtr.cast pR, MatrixInfo(0L,V2l(4,3),V2l(1,4)))
        let tQ = NativeMatrix<float>(NativePtr.cast pQ, MatrixInfo(0L,V2l(4,4),V2l(1,4)))
        
        RQ.DecomposeInPlace(tR, tQ)
        
        aR.[0], aQ.[0]

        //let mutable R = m
        //let mutable Q = M44d()
        //tensor {
        //    let! pR = &R
        //    let! pQ = &Q
        //    RQ.DecomposeInPlace(pR, pQ)
        //}
        //R, Q

    static member Decompose (m : M44d) =
        let tmp = [|m; M44d()|]

        use pTmp = fixed tmp
        let pQ = NativePtr.add pTmp 1

        let tR = NativeMatrix<float>(NativePtr.cast pTmp, MatrixInfo(0L,V2l(4,4),V2l(1,4)))
        let tQ = NativeMatrix<float>(NativePtr.cast pQ,   MatrixInfo(0L,V2l(4,4),V2l(1,4)))

        RQ.DecomposeInPlace(tR, tQ)

        tmp.[0], tmp.[1]

        //let mutable R = m
        //let mutable Q = M44d()
        //tensor {
        //    let! pR = &R
        //    let! pQ = &Q
        //    RQ.DecomposeInPlace(pR, pQ)
        //}
        //R, Q
        
    static member Decompose (m : float32[,]) =
        let cols = m.GetLength(1)
        let R = Array2D.copy m
        let Q = Array2D.zeroCreate cols cols
        RQ.DecomposeInPlace(R, Q)
        R, Q
        
    static member Decompose (m : Matrix<float32>) =
        let R = m.Copy()
        let Q = Matrix<float32>(m.SX, m.SX)
        RQ.DecomposeInPlace(R, Q)
        R, Q
        
    static member Decompose (m : M22f) =
        let tmp = [|m; M22f()|]

        use pTmp = fixed tmp
        let pQ = NativePtr.add pTmp 1

        let tR = NativeMatrix<float32>(NativePtr.cast pTmp, MatrixInfo(0L,V2l(2,2),V2l(1,2)))
        let tQ = NativeMatrix<float32>(NativePtr.cast pQ,   MatrixInfo(0L,V2l(2,2),V2l(1,2)))

        RQ.DecomposeInPlace(tR, tQ)

        tmp.[0], tmp.[1]

        //let mutable R = m
        //let mutable Q = M22f()
        //tensor {
        //    let! pR = &R
        //    let! pQ = &Q
        //    RQ.DecomposeInPlace(pR, pQ)
        //}
        //R, Q
        
    static member Decompose (m : M23f) =
        let aR = [|m|]
        let aQ = [|M33f()|]
        
        use pR = fixed aR
        use pQ = fixed aQ
        
        let tR = NativeMatrix<float32>(NativePtr.cast pR, MatrixInfo(0L,V2l(3,2),V2l(1,3)))
        let tQ = NativeMatrix<float32>(NativePtr.cast pQ, MatrixInfo(0L,V2l(3,3),V2l(1,3)))
        
        RQ.DecomposeInPlace(tR, tQ)
        
        aR.[0], aQ.[0]

        //let mutable R = m
        //let mutable Q = M33f()
        //tensor {
        //    let! pR = &R
        //    let! pQ = &Q
        //    RQ.DecomposeInPlace(pR, pQ)
        //}
        //R, Q
        
    static member Decompose (m : M33f) =
        let tmp = [|m; M33f()|]

        use pTmp = fixed tmp
        let pQ = NativePtr.add pTmp 1

        let tR = NativeMatrix<float32>(NativePtr.cast pTmp, MatrixInfo(0L,V2l(3,3),V2l(1,3)))
        let tQ = NativeMatrix<float32>(NativePtr.cast pQ,   MatrixInfo(0L,V2l(3,3),V2l(1,3)))

        RQ.DecomposeInPlace(tR, tQ)

        tmp.[0], tmp.[1]

        //let mutable R = m
        //let mutable Q = M33f()
        //tensor {
        //    let! pR = &R
        //    let! pQ = &Q
        //    RQ.DecomposeInPlace(pR, pQ)
        //}
        //R, Q

    static member Decompose (m : M34f) =
        let aR = [|m|]
        let aQ = [|M44f()|]
        
        use pR = fixed aR
        use pQ = fixed aQ
        
        let tR = NativeMatrix<float32>(NativePtr.cast pR, MatrixInfo(0L,V2l(4,3),V2l(1,4)))
        let tQ = NativeMatrix<float32>(NativePtr.cast pQ, MatrixInfo(0L,V2l(4,4),V2l(1,4)))
        
        RQ.DecomposeInPlace(tR, tQ)
        
        aR.[0], aQ.[0]

        //let mutable R = m
        //let mutable Q = M44f()
        //tensor {
        //    let! pR = &R
        //    let! pQ = &Q
        //    RQ.DecomposeInPlace(pR, pQ)
        //}
        //R, Q

    static member Decompose (m : M44f) =
        let tmp = [|m; M44f()|]

        use pTmp = fixed tmp
        let pQ = NativePtr.add pTmp 1

        let tR = NativeMatrix<float32>(NativePtr.cast pTmp, MatrixInfo(0L,V2l(4,4),V2l(1,4)))
        let tQ = NativeMatrix<float32>(NativePtr.cast pQ,   MatrixInfo(0L,V2l(4,4),V2l(1,4)))

        RQ.DecomposeInPlace(tR, tQ)

        tmp.[0], tmp.[1]

        //let mutable R = m
        //let mutable Q = M44f()
        //tensor {
        //    let! pR = &R
        //    let! pQ = &Q
        //    RQ.DecomposeInPlace(pR, pQ)
        //}
        //R, Q
        
        
module RQ =
    let inline private dec< ^a, ^c, ^d when (^a or ^d) : (static member Decompose : ^a -> ^c) > (d : ^d) (m : ^a) : ^c =
        ((^a or ^d) : (static member Decompose : ^a -> ^c) (m))
        
    let inline private decip< ^a, ^b, ^c, ^d when (^a or ^d) : (static member DecomposeInPlace : ^a * ^b -> ^c) > (d : ^d) (m1 : ^a) (m2 : ^b) : ^c =
        ((^a or ^d) : (static member DecomposeInPlace : ^a * ^b -> ^c) (m1, m2))
        
    let inline decompose m = dec Unchecked.defaultof<RQ> m
    let inline decomposeInPlace a b  = decip Unchecked.defaultof<RQ> a b


module private QROverloadTest = 
    let test() =
        let a = QR.bidiagonalize M34d.Zero
        let a = QR.decompose M44d.Identity

        let a = Matrix<float>()
        let b = Matrix<float>()
        let c = Matrix<float>()

        QR.decomposeInPlace a b
        QR.bidiagonalizeInPlace a b c

        let (R, Q) = RQ.decompose (M23d())

        let arr = Array2D.init 10 10 (fun i j -> 0.1)
        let (U, B, Vt) = QR.bidiagonalize arr

        ()