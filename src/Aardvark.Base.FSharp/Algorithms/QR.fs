namespace Aardvark.Base

open Microsoft.FSharp.NativeInterop

#nowarn "9"

[<AutoOpen>]
module internal QRHelpers =

    let inline sgn v = if v < LanguagePrimitives.GenericZero then -LanguagePrimitives.GenericOne else LanguagePrimitives.GenericOne

    let inline tiny (eps : 'a) (v : 'a) =
        abs v <= eps

    let inline applyGivensMat (mat : NativeMatrix< ^a >) (c : int) (r : int) (cos : ^a) (sin : ^a) =
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

        let mutable pc0 = NativePtr.toNativeInt pR.Pointer
        let mutable pcc = pc0
        for c in 0 .. cols - 1 do
            // wiki performs this loop backwards (should not really matter)
            let mutable prc = pcc + drR
            let mutable pr0 = pc0 + drR
            for r in c + 1 .. rows - 1 do 
                let vcc : ^a = NativeInt.read pcc // important since R.[c,c] changes
                let vrc : ^a = NativeInt.read prc 

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
                        let A = NativeInt.read< ^a > p0
                        let B = NativeInt.read< ^a > p1
                        
                        NativeInt.write p0 ( cos * A + sin * B )
                        NativeInt.write p1 (-sin * A + cos * B )
                        
                        p0 <- p0 + dcR
                        p1 <- p1 + dcR

                        
                    // adjust the resulting Q matrix
                    applyGivensMat pQ c r cos sin
                
                pr0 <- pr0 + drR
                prc <- prc + drR
            
            pc0 <- pc0 + drR
            pcc <- pcc + drR + dcR


    /// creates a (in-place) decomposition B = U * B' * Vt where
    /// U and V a orthonormal rotations and B' is upper bidiagonal
    /// returns the "anorm" of the resulting B matrix
    let inline qrBidiagonalizeNative (eps : ^a) (U : NativeMatrix< ^a >) (B : NativeMatrix< ^a >) (Vt : NativeMatrix< ^a >) =
        let rows = int B.SY
        let cols = int B.SX

        // set u and v to identity
        U.SetByCoord(fun (v : V2i) -> if v.X = v.Y then LanguagePrimitives.GenericOne else LanguagePrimitives.GenericZero)
        Vt.SetByCoord(fun (v : V2i) -> if v.X = v.Y then LanguagePrimitives.GenericOne else LanguagePrimitives.GenericZero)

        let sa = nativeint sizeof< ^a >
        let pB = NativePtr.toNativeInt B.Pointer
        let dbr = nativeint B.DY * sa
        let dbc = nativeint B.DX * sa
        
        let mutable anorm = LanguagePrimitives.GenericZero

        let mutable pii = pB
        for i in 0 .. cols - 1 do

            // make the subdiagonal column 0
            let mutable pi1i = pii + dbr
            for j in i + 1 .. rows - 1 do
                let vcc = NativeInt.read< ^a > pii     //B.[i,i]
                let vrc = NativeInt.read< ^a > pi1i    //B.[j,i]

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
                    let a = NativeInt.read< ^a > pr0 //B.[r0,ci]
                    let b = NativeInt.read< ^a > pr1 //B.[r1,ci]
                    NativeInt.write pr0 ( cos * a + sin * b )
                    NativeInt.write pr1 0.0
                    pr1 <- pr1 + dbc
                    pr0 <- pr0 + dbc

                    for _ in i+1 .. cols - 1 do
                        let a = NativeInt.read< ^a > pr0 //B.[r0,ci]
                        let b = NativeInt.read< ^a > pr1 //B.[r1,ci]
                        NativeInt.write pr0 ( cos * a + sin * b )
                        NativeInt.write pr1 (-sin * a + cos * b )
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
                let vcc = NativeInt.read< ^a > pii     //B.[i,i+1] // important since R.[c,c] changes
                let vrc = NativeInt.read< ^a > pij     //B.[i,j]

                // if the dst-element is not already zero then make it zero
                if not (tiny eps vrc) then

                    // find givens rotation
                    let rho = if vcc = LanguagePrimitives.GenericZero then abs vrc else sgn vcc * sqrt (vcc * vcc + vrc * vrc)
                    let sin = vrc / rho
                    let cos = vcc / rho
                    
                    // adjust affected elements
                    let mutable pc0 = pii
                    let mutable pc1 = pij
                    
                    // first iteration
                    let a = NativeInt.read< ^a > pc0 //B.[r0,ci]
                    let b = NativeInt.read< ^a > pc1 //B.[r1,ci]
                    NativeInt.write pc0 ( cos * a + sin * b )
                    NativeInt.write pc1 0.0
                    pc0 <- pc0 + dbr
                    pc1 <- pc1 + dbr


                    for _ in i .. rows - 1 do
                        let a = NativeInt.read< ^a > pc0 //B.[r0,ci]
                        let b = NativeInt.read< ^a > pc1 //B.[r1,ci]
                        NativeInt.write pc0 ( cos * a + sin * b )
                        NativeInt.write pc1 (-sin * a + cos * b )
                        pc0 <- pc0 + dbr
                        pc1 <- pc1 + dbr
                        
                    // adjust the resulting Vt matrix
                    applyGivensTransposedMat Vt i j cos sin
                    
                pij <- pij + dbc
                
            let normv =
                if i > 0 then 
                    // abs B.[i-1,i] + abs B.[i,i]
                    abs (NativeInt.read< ^a > (pii - dbc - dbr)) + abs (NativeInt.read< ^a > (pii - dbc))
                else 
                    // abs B.[i,i]
                    abs (NativeInt.read< ^a > (pii - dbc))

            anorm <- max anorm normv
            pii <- pii + dbr

        anorm


[<AbstractClass; Sealed>]
type QR private() =

    static let doubleEps = 1E-20
    static let floatEps = 1E-15f

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

    static member Decompose (m : Matrix<float>) =
        let R = m.Copy()
        let Q = Matrix<float>(m.SY, m.SY)
        QR.DecomposeInPlace(Q, R)
        Q, R
        
    static member Decompose (m : M22d) =
        let mutable R = m
        let mutable Q = M22d()
        tensor {
            let! pR = &R
            let! pQ = &Q
            QR.DecomposeInPlace(pQ, pR)
        }
        Q, R
        
    static member Decompose (m : M23d) =
        let mutable R = m
        let mutable Q = M22d()
        tensor {
            let! pR = &R
            let! pQ = &Q
            QR.DecomposeInPlace(pQ, pR)
        }
        Q, R
        
    static member Decompose (m : M33d) =
        let mutable R = m
        let mutable Q = M33d()
        tensor {
            let! pR = &R
            let! pQ = &Q
            QR.DecomposeInPlace(pQ, pR)
        }
        Q, R

    static member Decompose (m : M34d) =
        let mutable R = m
        let mutable Q = M33d()
        tensor {
            let! pR = &R
            let! pQ = &Q
            QR.DecomposeInPlace(pQ, pR)
        }
        Q, R

    static member Decompose (m : M44d) =
        let mutable R = m
        let mutable Q = M44d()
        tensor {
            let! pR = &R
            let! pQ = &Q
            QR.DecomposeInPlace(pQ, pR)
        }
        Q, R
        
    static member Decompose (m : Matrix<float32>) =
        let R = m.Copy()
        let Q = Matrix<float32>(m.SY, m.SY)
        QR.DecomposeInPlace(Q, R)
        Q, R
        
    static member Decompose (m : M22f) =
        let mutable R = m
        let mutable Q = M22f()
        tensor {
            let! pR = &R
            let! pQ = &Q
            QR.DecomposeInPlace(pQ, pR)
        }
        Q, R
        
    static member Decompose (m : M23f) =
        let mutable R = m
        let mutable Q = M22f()
        tensor {
            let! pR = &R
            let! pQ = &Q
            QR.DecomposeInPlace(pQ, pR)
        }
        Q, R
        
    static member Decompose (m : M33f) =
        let mutable R = m
        let mutable Q = M33f()
        tensor {
            let! pR = &R
            let! pQ = &Q
            QR.DecomposeInPlace(pQ, pR)
        }
        Q, R

    static member Decompose (m : M34f) =
        let mutable R = m
        let mutable Q = M33f()
        tensor {
            let! pR = &R
            let! pQ = &Q
            QR.DecomposeInPlace(pQ, pR)
        }
        Q, R

    static member Decompose (m : M44f) =
        let mutable R = m
        let mutable Q = M44f()
        tensor {
            let! pR = &R
            let! pQ = &Q
            QR.DecomposeInPlace(pQ, pR)
        }
        Q, R
        

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


    static member Bidiagonalize(m : Matrix<float>) =
        let U = Matrix<float>(m.SY, m.SY)
        let B = m.Copy()
        let Vt = Matrix<float>(m.SX, m.SX)
        QR.BidiagonalizeInPlace(U, B, Vt) |> ignore
        U, B, Vt
        
    static member Bidiagonalize (m : M22d) =
        let mutable U = M22d()
        let mutable B = m
        let mutable Vt = M22d()
        tensor {
            let! pU = &U
            let! pB = &B
            let! pVt = &Vt
            QR.BidiagonalizeInPlace(pU, pB, pVt)
        }
        U, B, Vt

    static member Bidiagonalize (m : M23d) =
        let mutable U = M22d()
        let mutable B = m
        let mutable Vt = M33d()
        tensor {
            let! pU = &U
            let! pB = &B
            let! pVt = &Vt
            QR.BidiagonalizeInPlace(pU, pB, pVt)
        }
        U, B, Vt

    static member Bidiagonalize (m : M33d) =
        let mutable U = M33d()
        let mutable B = m
        let mutable Vt = M33d()
        tensor {
            let! pU = &U
            let! pB = &B
            let! pVt = &Vt
            QR.BidiagonalizeInPlace(pU, pB, pVt)
        }
        U, B, Vt

    static member Bidiagonalize (m : M34d) =
        let mutable U = M33d()
        let mutable B = m
        let mutable Vt = M44d()
        tensor {
            let! pU = &U
            let! pB = &B
            let! pVt = &Vt
            QR.BidiagonalizeInPlace(pU, pB, pVt)
        }
        U, B, Vt

    static member Bidiagonalize (m : M44d) =
        let mutable U = M44d()
        let mutable B = m
        let mutable Vt = M44d()
        tensor {
            let! pU = &U
            let! pB = &B
            let! pVt = &Vt
            QR.BidiagonalizeInPlace(pU, pB, pVt)
        }
        U, B, Vt

    static member Bidiagonalize(m : Matrix<float32>) =
        let U = Matrix<float32>(m.SY, m.SY)
        let B = m.Copy()
        let Vt = Matrix<float32>(m.SX, m.SX)
        QR.BidiagonalizeInPlace(U, B, Vt) |> ignore
        U, B, Vt
        
    static member Bidiagonalize (m : M22f) =
        let mutable U = M22f()
        let mutable B = m
        let mutable Vt = M22f()
        tensor {
            let! pU = &U
            let! pB = &B
            let! pVt = &Vt
            QR.BidiagonalizeInPlace(pU, pB, pVt)
        }
        U, B, Vt

    static member Bidiagonalize (m : M23f) =
        let mutable U = M22f()
        let mutable B = m
        let mutable Vt = M33f()
        tensor {
            let! pU = &U
            let! pB = &B
            let! pVt = &Vt
            QR.BidiagonalizeInPlace(pU, pB, pVt)
        }
        U, B, Vt

    static member Bidiagonalize (m : M33f) =
        let mutable U = M33f()
        let mutable B = m
        let mutable Vt = M33f()
        tensor {
            let! pU = &U
            let! pB = &B
            let! pVt = &Vt
            QR.BidiagonalizeInPlace(pU, pB, pVt)
        }
        U, B, Vt

    static member Bidiagonalize (m : M34f) =
        let mutable U = M33f()
        let mutable B = m
        let mutable Vt = M44f()
        tensor {
            let! pU = &U
            let! pB = &B
            let! pVt = &Vt
            QR.BidiagonalizeInPlace(pU, pB, pVt)
        }
        U, B, Vt

    static member Bidiagonalize (m : M44f) =
        let mutable U = M44f()
        let mutable B = m
        let mutable Vt = M44f()
        tensor {
            let! pU = &U
            let! pB = &B
            let! pVt = &Vt
            QR.BidiagonalizeInPlace(pU, pB, pVt)
        }
        U, B, Vt