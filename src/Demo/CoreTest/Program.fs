// Learn more about F# at http://fsharp.org

open Aardvark.Base


let svd() =
    let rand = RandomSystem()
    let mutable evil = None
    let mk() =
        let morig =
            (Trafo2d.Rotation(rand.UniformDouble()*Constant.PiTimesTwo))
                //Trafo2d.Translation(rand.UniformV2d()))
                .Forward
        let m = morig
        let vs = morig.ToArray()

        match SVD.decompose m with
        | None -> failwith "none"
        | Some (u,s,vt) -> 
            let mutable res = true
            let mt = u * s * vt
            for i in 0..vs.Length-1 do
                let ms = mt.ToArray()
                if not <| (Fun.ApproximateEquals(vs.[i],ms.[i],1E-8) || Fun.ApproximateEquals(vs.[i],-ms.[i],1E-8)) then
                    for j in 0..vs.Length-1 do
                        Log.line "%.5f ~ %.5f" vs.[j] ms.[j]
                    Log.error "wrong %d" i
                    evil <- Some(m)
                    res <- false
            res

                    
    let ct = 100000
    Log.startTimed "Running"
    Report.Progress(0.0)
    let mutable res = true
    let mutable i = 0
    while i < ct && res do
        res <- mk()
        i <- i+1
        Report.Progress(float i/float ct)
    Report.Progress(1.0)
    Log.stop()
    
    let m = evil |> Option.get
    Log.error "EVIL"
    Log.line "%A" (SVD.decompose m |> Option.get |> (fun (u,s,vt) -> u*s*vt))
    Log.error "EVIL"
    Log.line "%A" (SVD.decompose m |> Option.get |> (fun (u,s,vt) -> u*s*vt))
    Log.error "EVIL"
    Log.line "%A" (SVD.decompose m |> Option.get |> (fun (u,s,vt) -> u*s*vt))
    Log.error "EVIL"
    Log.line "%A" (SVD.decompose m |> Option.get |> (fun (u,s,vt) -> u*s*vt))
    Log.error "EVIL"
    Log.line "%A" (SVD.decompose m |> Option.get |> (fun (u,s,vt) -> u*s*vt))
    Log.error "EVIL"
    Log.line "%A" (SVD.decompose m |> Option.get |> (fun (u,s,vt) -> u*s*vt))
    Log.error "EVIL"
    Log.line "%A" (SVD.decompose m |> Option.get |> (fun (u,s,vt) -> u*s*vt))
    Log.error "EVIL"
    Log.line "%A" (SVD.decompose m |> Option.get |> (fun (u,s,vt) -> u*s*vt))


    Log.line "ok"

[<EntryPoint>]
let main argv =
    svd()
    
    0 
