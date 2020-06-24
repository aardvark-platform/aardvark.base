// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' project for more help.


open System 
open System.Threading
open System.Collections.Generic
open Aardvark.Base
open Aardvark.Base.Runtime
open Aardvark.Base.Monads.State
open FSharp.Data.Adaptive
open System.IO
open System.Runtime.InteropServices
open Microsoft.FSharp.NativeInterop

#nowarn "9"
#nowarn "51"

type NativeStream() =
    inherit Stream()

    let mutable capacity = 128n
    let mutable store = Marshal.AllocHGlobal(capacity)
    let mutable offset = 0n
    let mutable length = 0n

    member x.Pointer = store
        
    override x.Dispose(disposing : bool) =
        base.Dispose(disposing)
        let s = Interlocked.Exchange(&store, 0n)
        if s <> 0n then
            Marshal.FreeHGlobal s
            capacity <- 0n
            offset <- 0n
            length <- 0n

    override x.CanRead = true
    override x.CanSeek = true
    override x.CanWrite = true

    override x.Length = int64 length
    override x.Position
        with get() = int64 offset
        and set o = offset <- nativeint o

    override x.Flush() = ()

    override x.Seek(o, origin) =
        match origin with
            | SeekOrigin.Begin -> offset <- nativeint o
            | SeekOrigin.Current -> offset <- offset + nativeint o
            | SeekOrigin.End -> offset <- length - nativeint o
            | _ -> ()
        int64 offset

    override x.SetLength(l : int64) =
        if l > int64 capacity then
            let newCap = Fun.NextPowerOfTwo l |> nativeint
            store <- Marshal.ReAllocHGlobal(store, newCap)
            capacity <- newCap

        length <- nativeint l

    override x.Read(buffer : byte[], o : int, count : int) =
        let count = min (length - nativeint offset) (nativeint count) |> int
        Marshal.Copy(store + offset, buffer, o, count)
        count

    override x.Write(buffer : byte[], o : int, count : int) =
        let l = offset + nativeint count
        if l > capacity then
            let newCap = Fun.NextPowerOfTwo (int64 l) |> nativeint
            store <- Marshal.ReAllocHGlobal(store, newCap)
            capacity <- newCap

        Marshal.Copy(buffer, o, store + offset, count)
        length <- length + nativeint count
        offset <- offset + nativeint count


module Benchmark =
    open System.Diagnostics

    type MyDelegate = delegate of float32 * float32 * int64 * float32 * int * float32 * int * float32 -> unit

    let callback =
        MyDelegate (fun a b c d e f g h ->
            printfn "a: %A" a
            printfn "b: %A" b
            printfn "c: %A" c
            printfn "d: %A" d
            printfn "e: %A" e
            printfn "f: %A" f
            printfn "g: %A" g
            printfn "h: %A" h
        )

    let pDel = Marshal.PinDelegate(callback)
    let ptr = pDel.Pointer

    let cnt = 1 <<< 12

    let args = [| 1.0f :> obj; 2.0f :> obj; 3L :> obj; 4.0f :> obj; 5 :> obj; 6.0f :> obj; 7 :> obj; 8.0f :> obj |]
    let calls = Array.init cnt (fun _ -> ptr, args)

    let runOld(iter : int) =
        // warmup
        for i in 1 .. 10 do
            Aardvark.Base.Assembler.compileCalls 0 calls |> ignore

        let sw = Stopwatch.StartNew()
        for i in 1 .. iter do
            Aardvark.Base.Assembler.compileCalls 0 calls |> ignore
        sw.Stop()

        sw.MicroTime / (float iter)

    let cc = AMD64.CallingConvention.windows
    let fillNew(cnt) =
        use s = new MemoryStream()
        use w = new AMD64.AssemblerStream(s)

        w.Begin()

        for i in 1 .. cnt do
            w.BeginCall(8)
            w.PushArg(cc, 7.0f)
            w.PushArg(cc, 6u)
            w.PushArg(cc, 5.0f)
            w.PushArg(cc, 4u)
            w.PushArg(cc, 3.0f)
            w.PushArg(cc, 2UL)
            w.PushArg(cc, 1.0f)
            w.PushArg(cc, 0.0f)
            w.Call(cc, ptr)

        w.End()
        w.Ret()
        //s.ToArray()

    let runNew(iter : int) =
        // warmup
        for i in 1 .. 10 do
            fillNew(cnt) |> ignore

        let sw = Stopwatch.StartNew()
        for i in 1 .. iter do
            fillNew(cnt) |> ignore
        sw.Stop()

        sw.MicroTime / (float iter)

    let run() =
//        let size = 1 <<< 14
//        while true do
//            fillNew size

        let ot = runOld 100
        let throughput = float cnt / ot.TotalSeconds
        Log.line "old: %A (%.0fc/s)" ot throughput

        let nt = runNew 100
        let throughput = float cnt / nt.TotalSeconds
        Log.line "new: %A (%.0fc/s)" nt throughput

        let speedup = ot / nt
        Log.line "factor: %A" speedup

module Test =

    type MyDelegate = delegate of float32 * float32 * int * float32 * int * float32 * int * float32 -> unit

    let callback =
        MyDelegate (fun a b c d e f g h ->
            Log.start "call"
            Log.line "a: %A" a
            Log.line "b: %A" b
            Log.line "c: %A" c
            Log.line "d: %A" d
            Log.line "e: %A" e
            Log.line "f: %A" f
            Log.line "g: %A" g
            Log.line "h: %A" h
            Log.stop()
        )

    let ptr = Marshal.PinDelegate(callback)

    let run () =
        let store = NativePtr.alloc 1
        NativePtr.write store 100.0f

        use stream = new NativeStream()
        use asm = AssemblerStream.ofStream stream

        asm.BeginFunction()

        asm.BeginCall(8)
        asm.PushFloatArg(NativePtr.toNativeInt store)
        asm.PushArg(6)
        asm.PushArg(5.0f)
        asm.PushArg(4)
        asm.PushArg(3.0f)
        asm.PushArg(2)
        asm.PushArg(1.0f)
        asm.PushArg(0.0f)
        asm.Call(ptr.Pointer)
 
        asm.BeginCall(8)
        asm.PushArg(17.0f)
        asm.PushArg(16)
        asm.PushArg(15.0f)
        asm.PushArg(14)
        asm.PushArg(13.0f)
        asm.PushArg(12)
        asm.PushArg(11.0f)
        asm.PushArg(10.0f)
        asm.Call(ptr.Pointer)

        asm.WriteOutput(1234)
        asm.EndFunction()
        asm.Ret()
        
        let size = Fun.NextPowerOfTwo stream.Length |> nativeint
        let mem = ExecutableMemory.alloc size
        Marshal.Copy(stream.Pointer, mem, stream.Length)

        let managed : int -> int = UnmanagedFunctions.wrap mem
        Log.start "run(3)"

        if sizeof<nativeint> = 4 then Log.warn "32 bit"
        else Log.warn "64 bit"


        let res = managed 3
        Log.line "ret: %A" res
        Log.stop()

        ExecutableMemory.free mem size

        Environment.Exit 0


type MyDelegate = delegate of int * int * int * int64 * int64 -> unit // * int64 * int64 * int64 * int64 -> unit

open AMD64

let testAdd (n : int) =
    let set = HashSetDelta.ofList (List.map Add [ 1 .. n ])

    let iter = 100000
    let results = Array.zeroCreate iter
    let rand = RandomSystem()
    let rems = 
        Array.init iter (fun _ -> 
            let delta = if rand.UniformDouble() > 0.5 then 1 else -1

            if rand.UniformDouble() > 0.5 then
                SetOperation(rand.UniformInt(n) + 1, delta)
            else
                SetOperation(rand.UniformInt(n) + n + 1, delta)
        )
    
    // warmup
    for i in 0 .. iter / 10 do
        results.[i] <- set.Add rems.[i]
        
    let results = Array.zeroCreate iter
    System.GC.Collect()
    System.GC.WaitForFullGCComplete() |> ignore

    let sw = System.Diagnostics.Stopwatch.StartNew()
    for i in 0 .. iter - 1 do
        results.[i] <- set.Add rems.[i]
    sw.Stop()
    sw.MicroTime / iter

let testAddMany (n : int) (m : int) =
    let set = HashSetDelta.ofList (List.map Add [ 1 .. n ])

    let iter = 10000
    let results = Array.zeroCreate iter
    let rand = RandomSystem()
    let rems = 
        Array.init m (fun _ -> 
            let delta = if rand.UniformDouble() > 0.5 then 1 else -1

            if rand.UniformDouble() > 0.5 then
                SetOperation(rand.UniformInt(n) + 1, delta)
            else
                SetOperation(rand.UniformInt(n) + n + 1, delta)
        )
    
    let inline res() =
        let mutable set = set
        for d in rems do
            set <- set.Add d
        set

    // warmup
    for i in 0 .. iter / 10 do
        results.[i] <- res()
        
    let results = Array.zeroCreate iter
    System.GC.Collect()
    System.GC.WaitForFullGCComplete() |> ignore

    let sw = System.Diagnostics.Stopwatch.StartNew()
    for i in 0 .. iter - 1 do
        results.[i] <- res()
    sw.Stop()
    sw.MicroTime / iter

let testUnion (n : int) (m : int) =
    
    let rand = RandomSystem()
    let randomOp () =
        let delta = if rand.UniformDouble() > 0.5 then 1 else -1
        if rand.UniformDouble() > 0.5 then
            SetOperation(rand.UniformInt(n) + 1, delta)
        else
            SetOperation(rand.UniformInt(n) + n + 1, delta)

    let a = HashSetDelta.ofList (List.map Add [ 1 .. n ]) |> HashSetDelta.toHashMap
    let b = HashSetDelta.ofList (List.init m (fun _ -> randomOp())) |> HashSetDelta.toHashMap
    
    let iter = 100
    let results = Array.zeroCreate iter
    let rand = RandomSystem()
    let inline res() =
        HashMap.choose2 (fun _ l r -> 
            match l, r with
                | None, r -> r
                | l, None -> l
                | Some l, Some r ->
                    let r = l + r
                    if r <> 0 then Some r
                    else None
        ) a b
        |> HashSetDelta.ofHashMap

    // warmup
    for i in 0 .. iter / 10 do
        results.[i] <- res()
        
    let results = Array.zeroCreate iter
    System.GC.Collect()
    System.GC.WaitForFullGCComplete() |> ignore

    
    let sw = System.Diagnostics.Stopwatch.StartNew()
    for i in 0 .. iter - 1 do
        results.[i] <- res()
    sw.Stop()
    sw.MicroTime / iter



open System
open Aardvark.Geometry
open System.Runtime.Serialization.Formatters.Binary
open MBrace.FsPickler
open MBrace.FsPickler.Json
open System.Runtime.CompilerServices

type NativeMatrix =
    static member inline Pin(matrix : inref<M44d>, action : NativeMatrix<float> -> 'r) =
        action (NativeMatrix<float>(NativePtr.cast &&matrix, MatrixInfo(0L, V2l(4,4), V2l(1,4))))
        
    static member inline Pin(matrix : inref<M33d>, action : NativeMatrix<float> -> 'r) =
        action (NativeMatrix<float>(NativePtr.cast &&matrix, MatrixInfo(0L, V2l(3,3), V2l(1,3))))

    static member inline Pin(matrix : inref<M22d>, action : NativeMatrix<float> -> 'r) =
        action (NativeMatrix<float>(NativePtr.cast &&matrix, MatrixInfo(0L, V2l(2,2), V2l(1,2))))

    static member inline Pin(matrix : inref<M34d>, action : NativeMatrix<float> -> 'r) =
        action (NativeMatrix<float>(NativePtr.cast &&matrix, MatrixInfo(0L, V2l(4,3), V2l(1,4))))

    static member inline Pin(matrix : inref<M23d>, action : NativeMatrix<float> -> 'r) =
        action (NativeMatrix<float>(NativePtr.cast &&matrix, MatrixInfo(0L, V2l(3,2), V2l(1,3))))

type NativeVector =
    static member inline Pin(vector : inref<V4d>, action : NativeVector<float> -> 'r) =
        action (NativeVector<float>(NativePtr.cast &&vector, VectorInfo(0L, 4L, 1L)))

    static member inline Pin(vector : inref<V3d>, action : NativeVector<float> -> 'r) =
        action (NativeVector<float>(NativePtr.cast &&vector, VectorInfo(0L, 3L, 1L)))

    static member inline Pin(vector : inref<V2d>, action : NativeVector<float> -> 'r) =
        action (NativeVector<float>(NativePtr.cast &&vector, VectorInfo(0L, 2L, 1L)))
    
    static member inline Pin(vector : inref<V4f>, action : NativeVector<float32> -> 'r) =
        action (NativeVector<float32>(NativePtr.cast &&vector, VectorInfo(0L, 4L, 1L)))

    static member inline Pin(vector : inref<V3f>, action : NativeVector<float32> -> 'r) =
        action (NativeVector<float32>(NativePtr.cast &&vector, VectorInfo(0L, 3L, 1L)))

    static member inline Pin(vector : inref<V2f>, action : NativeVector<float32> -> 'r) =
        action (NativeVector<float32>(NativePtr.cast &&vector, VectorInfo(0L, 2L, 1L)))
    
    static member inline Pin(vector : inref<V4i>, action : NativeVector<int> -> 'r) =
        action (NativeVector<int>(NativePtr.cast &&vector, VectorInfo(0L, 4L, 1L)))

    static member inline Pin(vector : inref<V3i>, action : NativeVector<int> -> 'r) =
        action (NativeVector<int>(NativePtr.cast &&vector, VectorInfo(0L, 3L, 1L)))

    static member inline Pin(vector : inref<V2i>, action : NativeVector<int> -> 'r) =
        action (NativeVector<int>(NativePtr.cast &&vector, VectorInfo(0L, 2L, 1L)))

let inline printMat (name : string) (fmt : string) (m : NativeMatrix< ^a >) =
    let res = 
        Array.init (int m.SX) (fun c -> Array.init (int m.SY) (fun r -> 
            let str = (float m.[c,r]).ToString fmt
            if str.StartsWith "-" then str
            else " " + str
        ))
    let lens = res |> Array.map (fun col -> col |> Seq.map String.length |> Seq.max)
    let pad (len : int) (s : string) =
        if s.Length < len then s + (System.String(' ',len-s.Length))
        else s

    let padded =
        res |> Array.mapi (fun i col -> 
            col |> Array.map (pad lens.[i])
        )
         
    let section = not (String.IsNullOrWhiteSpace name)
    if section then Report.Begin("{0}", name)
    for r in 0..int m.SY-1 do
        let mutable line = ""
        for c in 0..int m.SX-1 do
            if c > 0 then line <- line + " "
            line <- line + padded.[c].[r]
        Report.Line("{0}", line.TrimEnd())
    if section then Report.End() |> ignore
    
[<AbstractClass; Sealed; Extension>]
type NativeMatrixExtensions private() =
    [<Extension>]
    static member LuFactorize(mat : NativeMatrix<float>, perm : NativeVector<int>) =
        if mat.SX <> mat.SY then raise <| IndexOutOfRangeException()

        perm.SetByCoord id

        let n1 = int mat.SX - 1

        let inline get (r : int) (c : int) =
            mat.[c,r]

        let inline set (r : int) (c : int) (v : float) =
            mat.[c,r] <- v
                
        let inline swapRows (ri : int) (rj : int) =
            if ri <> rj then
                for c in 0 .. n1 do
                    let t = get ri c
                    set ri c (get rj c)
                    set rj c t

                let t = perm.[ri]
                perm.[ri] <- perm.[rj]
                perm.[rj] <- t

        let mutable i = 0
        while i < n1 do
            // pivoting
            let vii = 
                let mutable j = i
                let mutable vmax = get i i
                let mutable vmaxAbs = abs vmax
                for ii in i+1 .. n1 do
                    let vii = get ii i
                    let viiAbs = abs vii
                    if viiAbs > vmaxAbs then
                        j <- ii
                        vmax <- vii
                        vmaxAbs <- viiAbs

                swapRows i j
                vmax

            // elimination
            if Fun.IsTiny vii then
                // singular matrix
                i <- Int32.MaxValue
            else
                for j in i + 1 .. n1 do
                    let vji = get j i
                    let f = vji / vii

                    set j i f
                    for c in i + 1 .. n1 do
                        set j c (get j c - get i c * f) 
                i <- i + 1

        if i > n1 then   
            false
        else
            true

    [<Extension>]
    static member LuSolve(mat : NativeMatrix<float>, perm : NativeVector<int>, rhs : NativeVector<float>, res : NativeVector<float>) =
        if mat.SX <> mat.SY || mat.SX <> perm.Size || mat.SX <> rhs.Size || mat.SX <> res.Size then raise <| IndexOutOfRangeException()
        
        let n1 = int mat.SX - 1
        res.SetByCoord(fun (i : int) -> rhs.[perm.[i]])

        for i in 0 .. n1 do
            let pi = perm.[i]
            for j in i + 1 .. n1 do
                let f = mat.[i, j]
                let pj = perm.[j]
                rhs.[pj] <- rhs.[pj] - f * rhs.[pi]


        // back substitution
        for ri in 0 .. n1 do
            let ri = n1 - ri

            let mutable s = rhs.[perm.[ri]]
            for c in ri + 1 .. n1 do
                s <- s - mat.[c, ri] * res.[c]

            res.[ri] <- s / mat.[ri, ri]

        ()
        
    [<Extension>]
    static member LuSolve(mat : NativeMatrix<float>, rhs : NativeVector<float>, res : NativeVector<float>) =
        if mat.SX <> mat.SY || mat.SX <> rhs.Size || mat.SX <> res.Size then raise <| IndexOutOfRangeException()
        
        let n1 = int mat.SX - 1
        res.SetByCoord(fun (i : int) -> rhs.[i])

        for i in 0 .. n1 do
            let pi = i
            for j in i + 1 .. n1 do
                let f = mat.[i, j]
                let pj = j
                rhs.[pj] <- rhs.[pj] - f * rhs.[pi]


        // back substitution
        for ri in 0 .. n1 do
            let ri = n1 - ri

            let mutable s = rhs.[ri]
            for c in ri + 1 .. n1 do
                s <- s - mat.[c, ri] * res.[c]

            res.[ri] <- s / mat.[ri, ri]

        ()
        
    [<Extension>]
    static member LuInvert(mat : NativeMatrix<float>, inv : NativeMatrix<float>) =
        if mat.SX <> mat.SY || mat.SX <> inv.SX || inv.SX <> inv.SY then raise <| IndexOutOfRangeException()
        
        let n = int mat.SY
        let perm = NativePtr.stackalloc n
        let pv = NativeVector<int>(perm, VectorInfo(n))
        if mat.LuFactorize(pv) then
            inv.SetByCoord(fun (c : V2i) ->
                if c.X = NativePtr.get perm c.Y then 1.0
                else 0.0
            )

            for c in 0 .. n - 1 do
                mat.LuSolve(inv.[c, *], inv.[c, *])

            true

        else
            false

        
    [<Extension; MethodImpl(MethodImplOptions.NoInlining)>]
    static member LuFactorizeNew(m : byref<M44d>) =
        NativeMatrix.Pin(&m, fun (tm : NativeMatrix<float>) ->
            let pp = NativePtr.stackalloc<V4i> 1
            let tp = NativeVector<int>(NativePtr.cast pp, VectorInfo(4))

            if tm.LuFactorize(tp) then
                NativePtr.read pp
            else
                V4i.Zero
        )
              
    [<Extension; MethodImpl(MethodImplOptions.NoInlining)>]
    static member LuSolveNew(m : inref<M44d>, perm : V4i, rhs : V4d) =
        let mutable rhs = rhs
        let mutable perm = perm

        NativeMatrix.Pin(&m, fun (tm : NativeMatrix<float>) ->
            NativeVector.Pin(&perm, fun (tp : NativeVector<int>) ->
                NativeVector.Pin(&rhs, fun (tb : NativeVector<float>) ->
                    let pr = NativePtr.stackalloc<V4d> 1
                    let tr = NativeVector<float>(NativePtr.cast pr, VectorInfo(4))
                    tm.LuSolve(tp, tb, tr)
                    NativePtr.read pr
                )
            )
        )
        
    [<Extension; MethodImpl(MethodImplOptions.NoInlining)>]
    static member LuInvertNew(m : M44d) =
        let pm = NativePtr.stackalloc<M44d> 1
        NativePtr.write pm m
        let tm = NativeMatrix<float>(NativePtr.cast pm, MatrixInfo(0L, V2l(4,4), V2l(1,4)))

        let pi = NativePtr.stackalloc<M44d> 1
        let ti = NativeMatrix<float>(NativePtr.cast pi, MatrixInfo(0L, V2l(4,4), V2l(1,4)))

        if tm.LuInvert(ti) then
            NativePtr.read pi
        else
            M44d.Zero
              



let inline printM44d (name : string) (fmt : string) (mat : M44d) =
    let mb = Matrix<float>(mat.ToArray(), V2l(4,4))
    NativeMatrix.using mb (printMat name fmt)


let ellipsoidTest() =
    let trafo =
        Trafo3d.Scale(3.0, 2.0, 1.0) //*
        //Trafo3d.Rotation(V3d.III.Normalized, 1.21312)
        

    let err = 0.0
    let rand = RandomSystem()
    let pts =
        Array.init 2000 (fun _ ->
            rand.UniformV3dDirection() * (1.0 + (rand.UniformDouble() - 0.5) * 2.0 * err) |> trafo.Forward.TransformPos
        )


    //let pts = 
    //    let rx = Regex @"(-?[0-9]+)[ \t]+(-?[0-9]+)[ \t]+(-?[0-9]+)"
    //    File.readAllLines @"C:\Users\Schorsch\Development\ellipsoid_fit_python\mag_out.txt"
    //    |> Array.choose (fun l ->
    //        let m = rx.Match l
    //        if m.Success then Some (V3d(float m.Groups.[1].Value, float m.Groups.[2].Value, float m.Groups.[3].Value))
    //        else None
    //    )

    let bb = Box3d(pts)
    let f = 3.0 / bb.Size.NormMax
    let c = bb.Center
    let ellipsoid = EllipsoidRegression3d.ofArray pts |> EllipsoidRegression3d.getEllipsoid

    let mutable minError = System.Double.PositiveInfinity
    let mutable maxError = 0.0
    let mutable sum = 0.0
    let mutable sumSq = 0.0
    let mutable cnt = 0
    for p in pts do
        let struct(p0, n0) = ellipsoid.GetClosestPointAndNormal(p)
        let h = Vec.distance p p0
        let dn = Vec.dot n0 (p - p0)
        
        minError <- min minError h
        maxError <- max maxError h
        sum <- sum + h
        sumSq <- sumSq + sqr h
        cnt <- cnt + 1
        
    let q = trafo.Forward.TransformPos (V3d(1,0,0))

    let pt = q
    let ps = ellipsoid.Euclidean.TransformPos ((ellipsoid.Euclidean.InvTransformPos(q) / ellipsoid.Radii).Normalized * ellipsoid.Radii)
    let p0 = ellipsoid.GetClosestPoint(pt)



    Log.start "ellipsoid"
    Log.line "r: %.5f %.5f %.5f" ellipsoid.Radii.X ellipsoid.Radii.Y ellipsoid.Radii.Z
    Log.line "x: %s" ((ellipsoid.Euclidean.TransformDir V3d.IOO).ToString "0.00000")
    Log.line "y: %s" ((ellipsoid.Euclidean.TransformDir V3d.OIO).ToString "0.00000")
    Log.line "z: %s" ((ellipsoid.Euclidean.TransformDir V3d.OOI).ToString "0.00000")
    Log.stop()

    Log.start "errors"
    Log.line "min: %.6e" minError
    Log.line "max: %.6e" maxError
    Log.line "avg: %.6e" (sum / float cnt)
    Log.line "rms: %.6e" (sqrt (sumSq / float cnt))
    Log.stop()

    
let sphereTest() =
    let trafo =
        Trafo3d.Scale(5.0) *
        Trafo3d.Translation(1.0, 2.0, 3.0)
        

    let err = 0.1
    let rand = RandomSystem()
    let pts =
        Array.init 1000 (fun _ ->
            let dir = rand.UniformV3dDirection() |> trafo.Forward.TransformDir
            let pos = trafo.Forward.C3.XYZ + dir

            pos + Vec.normalize dir * ((rand.UniformDouble() - 0.5) * 2.0 * err)

        )


    //let pts = 
    //    let rx = Regex @"(-?[0-9]+)[ \t]+(-?[0-9]+)[ \t]+(-?[0-9]+)"
    //    File.readAllLines @"C:\Users\Schorsch\Development\ellipsoid_fit_python\mag_out.txt"
    //    |> Array.choose (fun l ->
    //        let m = rx.Match l
    //        if m.Success then Some (V3d(float m.Groups.[1].Value, float m.Groups.[2].Value, float m.Groups.[3].Value))
    //        else None
    //    )

    let bb = Box3d(pts)
    let f = 3.0 / bb.Size.NormMax
    let c = bb.Center
    let sphere = 
        let r = (SphereRegression3d.Empty, pts) ||> Array.fold (fun r p -> r.Add p)
        r.GetSphere()

    let mutable minError = System.Double.PositiveInfinity
    let mutable maxError = 0.0
    let mutable sum = 0.0
    let mutable sumSq = 0.0
    let mutable cnt = 0
    for p in pts do
        let n0 = Vec.normalize (p - sphere.Center)
        let p0 = n0 * sphere.Radius + sphere.Center
        let h = Vec.distance p p0
        let dn = Vec.dot n0 (p - p0)
        
        minError <- min minError h
        maxError <- max maxError h
        sum <- sum + h
        sumSq <- sumSq + sqr h
        cnt <- cnt + 1
        
    let q = trafo.Forward.TransformPos (V3d(1,0,0))


    Log.start "sphere"
    Log.line "r: %.5f" sphere.Radius
    Log.line "c: %s" (sphere.Center.ToString "0.00000")
    Log.stop()

    Log.start "errors"
    Log.line "min: %.6e" minError
    Log.line "max: %.6e" maxError
    Log.line "avg: %.6e" (sum / float cnt)
    Log.line "rms: %.6e" (sqrt (sumSq / float cnt))
    Log.stop()




[<EntryPoint; STAThread>]
let main argv = 
    sphereTest()
    exit 0

    let s = MapExt.ofList [1,1;2,2;3,2;4,4]
   
    let pickler = FsPickler.CreateJsonSerializer(true, false)

    let str = pickler.PickleToString(s)
    printfn "%s" str
    let test = pickler.UnPickleOfString<MapExt<int, int>> str
    printfn "%A" test
    
    Environment.Exit 0



    let shapeVertices1 = 
        [| 
            V2d(5.13737496260736, 1.65204532553747)
            V2d(5.88809174207466, 1.16673346810404)
            V2d(5.59715606644129, 0.71669234485868)
            V2d(4.84643928697398, 1.20200420229211)
            V2d(5.13737496260736, 1.65204532553747) 
        |]

    let r = PolyRegion.ofArray shapeVertices1
    //let res = PolygonTessellator.Combine([shapeVertices1], TessellationRule.EvenOdd)
    
    match r.Polygons with
        | [l] -> printfn "%A" l.PointCount
        | _ -> printfn "error"
    Environment.Exit 0

    let set = cset [1;2;3]

    let m = cval (Some 10)
    let d = m |> AVal.map (fun v -> printfn "eval %A" v; v)
    let b = set |> ASet.chooseA (fun _ -> d)

    
    let r = b.GetReader()
    r.GetChanges(AdaptiveToken.Top) |> printfn "%A"

    transact (fun () -> set.Remove 1 |> ignore)
    r.GetChanges(AdaptiveToken.Top) |> printfn "%A"

    transact (fun () -> set.Remove 2 |> ignore)
    r.GetChanges(AdaptiveToken.Top) |> printfn "%A"

    transact (fun () -> m.Value <- Some 1000)
    r.GetChanges(AdaptiveToken.Top) |> printfn "%A"
    
    transact (fun () -> set.Remove 3 |> ignore; m.Value <- Some 100)
    r.GetChanges(AdaptiveToken.Top) |> printfn "%A"


    System.Environment.Exit 0

//    let a = Func<int, int, int>(fun a b -> printfn "%A" (a,b); a + b)
//    let f : int -> int -> int = DelegateAdapters.wrap a
//    f 0 1 |> printfn "%A"
//    Environment.Exit 0




//    let output = @"C:\Users\Schorsch\Desktop\bla.csv"
//
//    File.WriteAllText(output, "n;m;ta;tu\r\n")
//    let n = 1000
//    for i in 1 .. 50 do
//        let n = i * 1000
//        let mutable l = 0
//        let mutable r = n / 3
//
//        while r - l > 0 do
//            let m = (l + r) / 2
//            let ta = testAddMany n m
//            let tu = testUnion n m
//            Log.line "%d/%d: %A %A" n m ta tu
//            if ta > tu then
//                r <- m - 1
//            else
//                l <- m + 1
//        let m = l
//        printf " 0: %d/%d: " n m
//        let ta = testAddMany n m
//        printf "%A" ta
//        let tu = testUnion n m
//        printfn " %A" tu
//
//        File.AppendAllLines(output, [sprintf "%d;%d;%.3f;%.3f" n l ta.TotalMicroseconds tu.TotalMicroseconds])
//
//    Environment.Exit 0

    let sw = System.Diagnostics.Stopwatch.StartNew()
    for iter in 1..50 do
        let cnt = iter * 1000
        let arr = ASet.ofArray(Array.init(cnt) (fun i -> cval i :> aval<_>))

        let arrr = arr |> ASet.collect (fun x -> 
                            x |> ASet.bind (fun y -> ASet.single y))

        let r = arrr.GetReader()
   
        System.GC.Collect()
        System.GC.WaitForFullGCComplete() |> ignore
        printf "%d " cnt

//        let worked = System.GC.TryStartNoGCRegion(1L <<< 30)

        let sw = System.Diagnostics.Stopwatch.StartNew()
        r.GetChanges AdaptiveToken.Top |> ignore
        sw.Stop()
//
//        if worked then
//            System.GC.EndNoGCRegion()
//        else
//            printf " (BAD) "

        printfn "took: %A" sw.MicroTime

        System.GC.Collect()
        System.GC.WaitForFullGCComplete() |> ignore

    Environment.Exit 0

//    Program.test()

    
    let m = cval 10
    
    let sw = System.Diagnostics.Stopwatch.StartNew()
    let set = cset [1 .. 100000]
    sw.Stop()
    Log.line "build: %A" sw.MicroTime

    let op = 
        m |> ASet.bind (fun a ->
            printfn "bind"
            set 
                |> ASet.collect (fun b -> if a <> b then ASet.ofList [a] else ASet.empty)
                |> fun a -> printfn "collect rebuild"; a
                |> ASet.collect ASet.single
                |> ASet.collect ASet.single
                |> ASet.collect ASet.single
                |> ASet.collect ASet.single
                |> ASet.collect ASet.single
                |> ASet.collect ASet.single
                |> ASet.collect ASet.single
        )

    let r = op.GetReader()

    let sw = System.Diagnostics.Stopwatch.StartNew()
    r.GetChanges AdaptiveToken.Top |> ignore
    sw.Stop()
    Log.line "took: %A" sw.MicroTime

    
    let sw = System.Diagnostics.Stopwatch.StartNew()
    transact (fun () -> m.Value <- 1)
    sw.Stop()
    Log.line "transact: %A" sw.MicroTime
    
    let sw = System.Diagnostics.Stopwatch.StartNew()
    r.GetChanges AdaptiveToken.Top |> ignore
    sw.Stop()
    Log.line "took: %A" sw.MicroTime
    
    
    let sw = System.Diagnostics.Stopwatch.StartNew()
    transact (fun () -> set.Add 0 |> ignore)
    sw.Stop()
    Log.line "transact: %A" sw.MicroTime
    
    let sw = System.Diagnostics.Stopwatch.StartNew()
    r.GetChanges AdaptiveToken.Top |> ignore
    sw.Stop()


    Log.line "took: %A" sw.MicroTime
    Environment.Exit 0








    let callback =
        MyDelegate (fun a b c d e ->
            printfn "a: %A" a
            printfn "b: %A" b
            printfn "c: %A" c
            printfn "d: %A" d
            printfn "e: %A" e
        )

    let ptr = Marshal.PinDelegate(callback)

    let store = NativePtr.alloc 1

    use stream = new NativeStream()
    let asm = new AssemblerStream(stream, true)

    let data = NativePtr.alloc 1
    NativePtr.write data 12321.0

    let cc = CallingConvention.windows

    let l = asm.NewLabel()

    asm.Begin()
    
    asm.Load(Register.Rax, NativePtr.toNativeInt store, false)
    asm.Cmp(Register.Rax, 0u)
    asm.Jump(JumpCondition.Equal, l)

    asm.BeginCall(5)
    asm.PushArg(cc, 1234UL)
    asm.PushArg(cc, 4321UL)
    asm.PushArg(cc, 3u)
    asm.PushArg(cc, 2u)
    asm.PushArg(cc, 1u)
    asm.Call(cc, ptr.Pointer)

    asm.Mark(l)

    asm.BeginCall(5)
    asm.PushArg(cc, 81234UL)
    asm.PushArg(cc, 74321UL)
    asm.PushArg(cc, 6u)
    asm.PushArg(cc, 5u)
    asm.PushArg(cc, 4u)
    asm.Call(cc, ptr.Pointer)

    asm.End()
    asm.Ret()
    asm.Dispose()

    let size = Fun.NextPowerOfTwo stream.Length |> nativeint
    let mem = ExecutableMemory.alloc size
    Marshal.Copy(stream.Pointer, mem, stream.Length)

    let managed : int  -> float32 = UnmanagedFunctions.wrap mem
    
    NativePtr.write store 0
    let res = managed 1234
    printfn "res = %A" res
    ExecutableMemory.free mem size

    Environment.Exit 0

    let rand = RandomSystem()
    let g = UndirectedGraph.ofNodes (Set.ofList [0..127]) (fun li ri -> float (ri - li) |> Some)

    let tree = UndirectedGraph.maximumSpanningTree compare g
    printfn "%A" tree

    printfn "%A" (Tree.weight tree / float (Tree.count tree))



    //React.Test.run()
    Environment.Exit 0

    0 // return an integer exit code
