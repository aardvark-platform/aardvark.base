namespace Aardvark.Base.FSharp.Tests

open Aardvark.Base

module Main =

    [<EntryPoint>]
    let main args = 
        let prin (m : M33f) name =
            printfn "%s" name
            printfn "%A %A %A" m.M00 m.M01 m.M02
            printfn "%A %A %A" m.M10 m.M11 m.M12
            printfn "%A %A %A" m.M20 m.M21 m.M22
            printfn " "

        let mat = M33f(1.0f,0.0f,0.0f,0.0f,0.0f,-1.0f,0.0f,1.0f,0.0f)

        prin mat "mat"
        
        let (s,v,d) = M33f.svd mat
        prin s "s"
        prin v "v"
        prin d "d"

        prin (s*v*d.Transposed) "mul"

        printfn "Second"

        let m2 = M33f(2.0f, 0.0f, 2.0f, 0.0f, 1.0f, 0.0f, 0.0f, 0.0f, 0.0f)//Trafo3d.RotationX(Constant.PiHalf).Forward.UpperLeftM33() |> M33f.op_Explicit
        prin m2 "m2" 
        
        let (u,s,v) = M33f.svd m2
        prin u "u"
        prin s "s"
        prin v "v"
        prin (u*s*v.Transposed) "mul"

        printfn "Third"

        let m3 = M33f(1.0f,2.0f,3.0f,4.0f,5.0f,6.0f,7.0f,8.0f,9.0f)

        let (u2,s2,v2) = M33f.svd m3
        prin u2 "u2"
        prin s2 "s2"
        prin v2 "v2"
        prin (u2*s2*v2.Transposed) "multiplied"

        1