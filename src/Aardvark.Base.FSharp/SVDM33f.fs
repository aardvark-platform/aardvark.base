namespace Aardvark.Base

// Fast 3x3 float matrix SVD
// Source: McAdams, Aleka, et al. "Computing the singular value decomposition of 3× 3 matrices with minimal branching and elementary floating point operations." University of Wisconsin Madison (2011).
// Implementation on: 
// https://github.com/ericjang/svd3/blob/master/svd3.h 
// by https://github.com/ericjang

module M33f =

    let gamma = 5.828427124f
    let cstar = 0.923879532f
    let sstar = 0.3826834323f
    let epsilon = float32 1E-6

    let inline invSqrt x = 
        1.0f / (sqrt x)

    let inline quatToM33 (qV : V4f) =
        let w = qV.W
        let x = qV.X
        let y = qV.Y
        let z = qV.Z

        let qxx = x*x
        let qyy = y*y
        let qzz = z*z
        let qxz = x*z
        let qxy = x*y
        let qyz = y*z
        let qwx = w*x
        let qwy = w*y
        let qwz = w*z

        M33f(
            1.0f - 2.0f * (qyy + qzz),      2.0f * (qxy - qwz),             2.0f * (qxz + qwy),
            2.0f * (qxy + qwz),             1.0f - 2.0f * (qxx + qzz),      2.0f * (qyz - qwx),
            2.0f * (qxz - qwy),             2.0f * (qyz + qwx),             1.0f - 2.0f * (qxx + qyy)
        )

    let inline givens m11 m12 m22 =
        let ch = 2.0f * (m11 - m22)
        let sh = m12
        let b = gamma*sh*sh < ch*ch
        let w = invSqrt(ch*ch + sh*sh)
        let och = if b then w*ch else cstar
        let osh = if b then w*sh else sstar
        och, osh

    let inline diagonal m11 m21 m22 m31 m32 m33 =
        M33f(m11, m21, m31, m21, m22, m32, m31, m32, m33)

    let inline conjugate (x : int) (y : int) (z : int) (m : M33f) (qV : V4f) =
        let (ch,sh) = givens m.M00 m.M10 m.M11

        let scale = ch*ch + sh*sh
        let a = (ch*ch-sh*sh)/scale
        let b = (2.0f*sh*ch)/scale

        let m11 =  a*(a*m.M00+b*m.M10)  + b*(a*m.M10 + b*m.M11)
        let m21 =  a*(-b*m.M00+a*m.M10) + b*(-b*m.M10 + a*m.M11)
        let m22 = -b*(-b*m.M00+a*m.M10) + a*(-b*m.M10 + a*m.M11)
        let m31 = a*m.M20 + b*m.M21
        let m32 = -b*m.M20 + a*m.M21
        let m33 = m.M22
        
        let tmp = V3f(qV.X*sh, qV.Y*sh, qV.Z*sh)
        let shScaled = qV.W * sh

        let q1 = qV.X * ch
        let q2 = qV.Y * ch
        let q3 = qV.Z * ch
        let q4 = qV.W * ch

        let q = [|q1; q2; q3; q4|]

        q.[z] <- q.[z] + shScaled
        q.[3] <- q.[3] - tmp.[z]
        q.[x] <- q.[x] + tmp.[y]
        q.[y] <- q.[y] - tmp.[x]

        let ma = diagonal m22 m32 m33 m21 m31 m11
        let va = V4f(q.[0], q.[1], q.[2], q.[3])
        
        ma,va


    let inline diagonalizeSymmetric (m : M33f) =
        
        let mutable m = m
        let mutable qV = V4f(0, 0, 0, 1)

        let doIt (mm,mqV) =
            m <- mm
            qV <- mqV
            
        for _ in 0..4 do
            doIt (conjugate 0 1 2 m qV)
            doIt (conjugate 1 2 0 m qV)
            doIt (conjugate 2 0 1 m qV)
        m,qV

    let inline swap c x y =
        if c then 
            (y,-x)
        else
            (x,y)

    let inline fswap c x y =
        if c then
            (y,x)
        else
            (x,y)

    let inline decomposeAndSort (m : M33f) (q : M33f) =
        let rho1 = m.C0.LengthSquared
        let rho2 = m.C1.LengthSquared
        let rho3 = m.C2.LengthSquared

        let c = rho1 < rho2
        let (m11,m12) = swap c m.M00 m.M01
        let (m21,m22) = swap c m.M10 m.M11
        let (m31,m32) = swap c m.M20 m.M21
        
        let (q11,q12) = swap c q.M00 q.M01
        let (q21,q22) = swap c q.M10 q.M11
        let (q31,q32) = swap c q.M20 q.M21
        
        let (rho1,rho2) = fswap c rho1 rho2

        let c = rho1 < rho3
        let (m11,m13) = swap c m11 m.M02
        let (m21,m23) = swap c m21 m.M12
        let (m31,m33) = swap c m31 m.M22
        
        let (q11,q13) = swap c q11 q.M02
        let (q21,q23) = swap c q21 q.M12
        let (q31,q33) = swap c q31 q.M22

        let (rho1,rho3) = fswap c rho1 rho3

        let c = rho2 < rho3
        let (m12,m13) = swap c m12 m13
        let (m22,m23) = swap c m22 m23
        let (m32,m33) = swap c m32 m33
        
        let (q12,q13) = swap c q12 q13
        let (q22,q23) = swap c q22 q23
        let (q32,q33) = swap c q32 q33
        
        let M = M33f(m11,m12,m13,m21,m22,m23,m31,m32,m33)
        let V = M33f(q11,q12,q13,q21,q22,q23,q31,q32,q33)

        M,V

    let inline QRgivens p a =
        let rho = sqrt(p*p + a*a)
        
        let ch = (abs p) + max rho epsilon
        let sh = if rho > epsilon then a else 0.0f
        let (sh,ch) = fswap (p<0.0f) sh ch
        let w = invSqrt(ch*ch + sh*sh)
        let ch = w * ch
        let sh = w * sh
        ch,sh

    let inline QRdecompose (m : M33f) =
        let (ch1,sh1) = QRgivens m.M00 m.M10
        let a = 1.0f - 2.0f * sh1 * sh1
        let b = 2.0f * ch1 * sh1

        let r11 = a * m.M00 + b * m.M10
        let r12 = a * m.M01 + b * m.M11
        let r13 = a * m.M02 + b * m.M12 
        let r21 = -b* m.M00 + a * m.M10
        let r22 = -b* m.M01 + a * m.M11
        let r23 = -b* m.M02 + a * m.M12
        let r31 = m.M20
        let r32 = m.M21
        let r33 = m.M22

        let (ch2,sh2) = QRgivens r11 r31
        let a = 1.0f - 2.0f * sh2 * sh2
        let b = 2.0f * ch2 * sh2

        let b11 = a * r11 + b * r31
        let b12 = a * r12 + b * r32
        let b13 = a * r13 + b * r33
        let b21 = r21
        let b22 = r22
        let b23 = r23
        let b31 = -b* r11 + a * r31
        let b32 = -b* r12 + a * r32
        let b33 = -b* r13 + a * r33

        let (ch3,sh3) = QRgivens b22 b32
        let a = 1.0f - 2.0f * sh3 * sh3
        let b = 2.0f * ch3 * sh3

        let r11 = b11
        let r12 = b12
        let r13 = b13
        let r21 = a * b21 + b * b31
        let r22 = a * b22 + b * b32
        let r23 = a * b23 + b * b33
        let r31 = -b* b21 + a * b31
        let r32 = -b* b22 + a * b32
        let r33 = -b* b23 + a * b33
        
        let sh1s = sh1*sh1
        let sh2s = sh2*sh2
        let sh3s = sh3*sh3

        let q11 = (-1.0f + 2.0f * sh1s) * (-1.0f + 2.0f * sh2s)
        let q12 = 4.0f * ch2 * ch3 * (-1.0f + 2.0f * sh1s) * sh2 * sh3 + 2.0f * ch1 * sh1 * (-1.0f + 2.0f * sh3s)
        let q13 = 4.0f * ch1 * ch3 * sh1 * sh3 - 2.0f * ch2 * (-1.0f + 2.0f * sh1s) * sh2 * (-1.0f + 2.0f * sh3s)
        
        let q21 = 2.0f * ch1 * sh1 * (1.0f - 2.0f * sh2s)
        let q22 = -8.0f * ch1 * ch2 * ch3 * sh1 * sh2 * sh3 + (-1.0f + 2.0f * sh1s) * (-1.0f + 2.0f * sh3s)
        let q23 = -2.0f * ch3 * sh3 + 4.0f * sh1 * (ch3 * sh1 * sh3 + ch1 * ch2 * sh2 * (-1.0f + 2.0f * sh3s))

        let q31 = 2.0f * ch2 * sh2
        let q32 = 2.0f * ch3 * (1.0f - 2.0f * sh2s) * sh3
        let q33 = (-1.0f + 2.0f * sh2s) * (-1.0f + 2.0f * sh3s)

        let Q = M33f(q11,q12,q13,q21,q22,q23,q31,q32,q33)
        let R = M33f(r11,r12,r13,r21,r22,r23,r31,r32,r33)

        (Q,R)

    let inline svd (m : M33f) =
        
        let norm = m.Transposed * m

        let (_, qV) = diagonalizeSymmetric norm

        let quat = quatToM33 qV

        let B = m * quat

        let (M,V) = decomposeAndSort B quat

        let (U,S) = QRdecompose M

        (U,S,V)



