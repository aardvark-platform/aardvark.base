module Aardvark.Base.Fonts.Tests.PathSegment

open Expecto
open Aardvark.Base
open Aardvark.Base.Fonts
open FsCheck

[<Tests>]
let smartConstructors =
    testList "Fonts.PathSegment" [
        testProperty "tryLine degenerate" <| fun (p1 : V2d) ->
            let line = PathSegment.tryLine p1 p1
            Expect.isNone line "impossible to create degenerate line"
    
        testProperty "tryBezier2 line" <| fun (p0 : V2d) (p2 : V2d) (ZeroOne t) ->
            let p1 = lerp p0 p2 t
            match PathSegment.tryBezier2 p0 p1 p2 with
            | Some (Line(a,b)) ->
                Expect.equal a p0 "line start"
                Expect.equal b p2 "line end"
            | None ->
                Expect.equal p0 p2 "degenerate bezier2"
            | Some other ->
                failwithf "degenerate bezier2 should be a line but is: %A" other
    
        testProperty "tryBezier2 degenerate" <| fun (p0 : V2d) ->
            let bezier2 = PathSegment.tryBezier2 p0 p0 p0
            Expect.isNone bezier2 "impossible to create degenerate bezier2"
    
        testProperty "tryArcSegment line" <| fun (p0 : V2d) (p2 : V2d) (ZeroOne t) ->
            let p1 = lerp p0 p2 t
            match PathSegment.tryArcSegment p0 p1 p2 with
            | Some (Line(a,b)) ->
                Expect.equal a p0 "line start"
                Expect.equal b p2 "line end"
            | None ->
                Expect.equal p0 p2 "degenerate arc"
            | Some other ->
                failwithf "degenerate arc should be a line but is: %A" other
                
        testProperty "tryArcSegment degenerate" <| fun (p0 : V2d) ->
            let arc = PathSegment.tryArcSegment p0 p0 p0
            Expect.isNone arc "impossible to create degenerate arc"
            
        testProperty "tryBezier3 with bezier2 points" <| fun (p0 : V2d) (p1 : V2d) (p2 : V2d) ->
            let a = lerp p0 p1 (2.0 / 3.0)
            let b = lerp p1 p2 (1.0 / 3.0)
            
            match PathSegment.tryBezier3 p0 a b p2 with
            | Some (Line(a, b)) ->
                Expect.equal a p0 "start"
                Expect.equal b p2 "end"
            | None ->
                Expect.equal p0 p1 "degenerate bezier3"
                Expect.equal p0 p2 "degenerate bezier3"
            | Some (Bezier2(a,b,c)) ->
                Expect.equal a p0 "start"
                Expect.approxEquals b p1 1E-13 "control"
                Expect.equal c p2 "end"
            | Some other ->
                failwithf "degenerate bezier3 should be a bezier2 but is: %A" other
            
        testProperty "tryBezier3 with line points" <| fun (p0 : V2d) (p3 : V2d) (ZeroOne t1) (ZeroOne t2) ->
            
            let p1 = lerp p0 p3 (min t1 t2)
            let p2 = lerp p0 p3 (max t1 t2)
            
            match PathSegment.tryBezier3 p0 p1 p2 p3 with
            | Some (Line(a, b)) ->
                Expect.equal a p0 "start"
                Expect.equal b p3 "end"
            | None ->
                Expect.approxEquals p0 p1 1E-14 "degenerate bezier3"
            | Some other ->
                failwithf "degenerate bezier3 should be a line but is: %A" other
                
        testProperty "tryBezier3 degenerate" <| fun (p0 : V2d) ->
            let arc = PathSegment.tryBezier3 p0 p0 p0 p0
            Expect.isNone arc "impossible to create degenerate arc"
            
        testProperty "line degenerate" <| fun (p1 : V2d) ->
            let line = try Some (PathSegment.line p1 p1) with _ -> None
            Expect.isNone line "impossible to create degenerate line"
    
        testProperty "bezier2 line" <| fun (p0 : V2d) (p2 : V2d) (ZeroOne t) ->
            let p1 = lerp p0 p2 t
            
            let segment = try Some (PathSegment.bezier2 p0 p1 p2) with _ -> None
            
            match segment with
            | Some (Line(a,b)) ->
                Expect.equal a p0 "line start"
                Expect.equal b p2 "line end"
            | None ->
                Expect.equal p0 p2 "degenerate bezier2"
            | Some other ->
                failwithf "degenerate bezier2 should be a line but is: %A" other
    
        testProperty "bezier2 degenerate" <| fun (p0 : V2d) ->
            let bezier2 = try Some (PathSegment.bezier2 p0 p0 p0) with _ -> None
            Expect.isNone bezier2 "impossible to create degenerate bezier2"
    
        testProperty "arcSegment line" <| fun (p0 : V2d) (p2 : V2d) (ZeroOne t) ->
            let p1 = lerp p0 p2 t
            let segment = try Some (PathSegment.arcSegment p0 p1 p2) with _ -> None
            match segment with
            | Some (Line(a,b)) ->
                Expect.equal a p0 "line start"
                Expect.equal b p2 "line end"
            | None ->
                Expect.equal p0 p2 "degenerate arc"
            | Some other ->
                failwithf "degenerate arc should be a line but is: %A" other
                
        testProperty "arcSegment degenerate" <| fun (p0 : V2d) ->
            let arc = try Some (PathSegment.arcSegment p0 p0 p0) with _ -> None
            Expect.isNone arc "impossible to create degenerate arc"
            
        testProperty "bezier3 with bezier2 points" <| fun (p0 : V2d) (p1 : V2d) (p2 : V2d) ->
            let a = lerp p0 p1 (2.0 / 3.0)
            let b = lerp p1 p2 (1.0 / 3.0)
            
            let segment = try Some (PathSegment.bezier3 p0 a b p2) with _ -> None
            match segment with
            | Some (Line(a, b)) ->
                Expect.equal a p0 "start"
                Expect.equal b p2 "end"
            | None ->
                Expect.equal p0 p1 "degenerate bezier3"
                Expect.equal p0 p2 "degenerate bezier3"
            | Some (Bezier2(a,b,c)) ->
                Expect.equal a p0 "start"
                Expect.approxEquals b p1 1E-13 "control"
                Expect.equal c p2 "end"
            | Some other ->
                failwithf "degenerate bezier3 should be a bezier2 but is: %A" other
            
        testProperty "bezier3 with line points" <| fun (p0 : V2d) (p3 : V2d) (ZeroOne t1) (ZeroOne t2) ->
            
            let p1 = lerp p0 p3 (min t1 t2)
            let p2 = lerp p0 p3 (max t1 t2)
            
            let segment = try Some (PathSegment.bezier3 p0 p1 p2 p3) with _ -> None
            match segment with
            | Some (Line(a, b)) ->
                Expect.equal a p0 "start"
                Expect.equal b p3 "end"
            | None ->
                Expect.approxEquals p0 p1 1E-14 "degenerate bezier3"
            | Some other ->
                failwithf "degenerate bezier3 should be a line but is: %A" other
                
        testProperty "bezier3 degenerate" <| fun (p0 : V2d) ->
            let arc = try Some (PathSegment.bezier3 p0 p0 p0 p0) with _ -> None
            Expect.isNone arc "impossible to create degenerate arc"
            
            
            
            
    ]

[<Tests>]
let splitMerge =
    testList "Fonts.PathSegment" [
        testProperty "split at 0" <| fun (seg : PathSegment) ->
            let (l, r) = PathSegment.split 0.0 seg
            Expect.isNone l "left part should be empty"
            Expect.equal r (Some seg) "right part should be entire segment"
            
        testProperty "split at 1" <| fun (seg : PathSegment) ->
            let (l, r) = PathSegment.split 1.0 seg
            Expect.isNone r "right part should be empty"
            Expect.equal l (Some seg) "left part should be entire segment"
        
        testProperty "tryGetT" <| fun (seg : PathSegment) (ZeroOne t) ->
            let pt = PathSegment.point t seg
            match PathSegment.tryGetT 1E-7 pt seg with
            | Some res ->
                Expect.approxEquals res t 1E-8 "tryGetT"
            | None ->
                failwithf "could not get t for: %A (%A)" t pt
        
        testProperty "withT0" <| fun (seg : PathSegment) (ZeroOne t) ->
            let t = min t 0.95
            let sub = PathSegment.withT0 t seg
            Expect.isSome sub "sub should exist"
            let sub = sub.Value
            
            Expect.approxEquals (PathSegment.point t seg) (PathSegment.startPoint sub) 1E-8 "split point equal"
            Expect.approxEquals (PathSegment.tangent t seg) (PathSegment.tangent 0.0 sub) 1E-5 "split tangent equal"
            Expect.relativeApproxEquals (PathSegment.curvature t seg) (PathSegment.curvature 0.0 sub) 1E-6 "split curvature equal"
            Expect.relativeApproxEquals (PathSegment.curvatureDerivative t seg) (PathSegment.curvatureDerivative 0.0 sub) 1E-6 "split curvature derivative equal"
            
        testProperty "withT1" <| fun (seg : PathSegment) (ZeroOne t) ->
            let t = max t 0.05
            let sub = PathSegment.withT1 t seg
            Expect.isSome sub "sub should exist"
            let sub = sub.Value
            
            Expect.approxEquals (PathSegment.point t seg) (PathSegment.endPoint sub) 1E-8 "split point equal"
            Expect.approxEquals (PathSegment.tangent t seg) (PathSegment.tangent 1.0 sub) 1E-5 "split tangent equal"
            Expect.relativeApproxEquals (PathSegment.curvature t seg) (PathSegment.curvature 1.0 sub) 1E-6 "split curvature equal"
            Expect.relativeApproxEquals (PathSegment.curvatureDerivative t seg) (PathSegment.curvatureDerivative 1.0 sub) 1E-6 "split curvature derivative equal"
            
            
        
        testProperty "split" <| fun (seg : PathSegment) (ZeroOne t) (NonEmptyArray(tests : ZeroOne[]))->
            let t = clamp 1E-2 (1.0 - 1E-2) t
            let (l, r) = PathSegment.split t seg
            
            
            Expect.isSome l "left part should exist"
            Expect.isSome r "right part should exist"
            
            match l with
            | Some l ->
                match r with
                | Some r ->
                    Expect.equal (PathSegment.endPoint l) (PathSegment.startPoint r) "split point equal"
                    Expect.approxEquals (PathSegment.tangent 1.0 l) (PathSegment.tangent 0.0 r) 1E-5 "split tangent equal"
                    Expect.relativeApproxEquals (PathSegment.curvature 1.0 l) (PathSegment.curvature 0.0 r) 1E-6 "split curvature equal"
                    Expect.relativeApproxEquals (PathSegment.curvatureDerivative 1.0 l) (PathSegment.curvatureDerivative 0.0 r) 1E-6 "split curvature derivative equal"
                    
                    for (ZeroOne tt) in tests do
                        let pt = PathSegment.point tt seg
                        
                        let tl = PathSegment.tryGetT 1E-7 pt l |> Option.map (fun t -> PathSegment.point t l)
                        let tr = PathSegment.tryGetT 1E-7 pt r |> Option.map (fun t -> PathSegment.point t r)
                        
                        match tl with
                        | Some pl -> Expect.approxEquals pl pt 1E-7 "point on left part"
                        | None ->
                            match tr with
                            | Some pr -> Expect.approxEquals pr pt 1E-7 "point on left part"
                            | None -> failwith "different curves"
                        
                    
                | None ->
                    ()
                    //Expect.equal l seg "split at end"
            | None ->
                match r with
                | Some r ->
                    ()
                    //Expect.equal r seg "split at start"
                | None ->
                    failwith "should be impossible (both split parts empty)"
            
        
        
    ]