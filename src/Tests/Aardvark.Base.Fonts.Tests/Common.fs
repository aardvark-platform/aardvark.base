namespace Expecto


open Expecto
open Aardvark.Base
open Aardvark.Base.Fonts
open FsCheck
open FsCheck.TypeClass

type ZeroOne = ZeroOne of float

type Generators() =
    static member V2d =
        { new Arbitrary<V2d>() with
            override x.Generator =
                gen {
                    let! (NormalFloat a) = Arb.generate<NormalFloat>
                    let! (NormalFloat b)= Arb.generate<NormalFloat>
                    return V2d(a,b)
                }
        }
        
    static member ZeroOne =
        { new Arbitrary<ZeroOne>() with
            override x.Generator =
                gen {
                    let! (NormalFloat a) = Arb.generate<NormalFloat>
                    return ZeroOne (abs a % 1.0)
                }
        }

    static member PathSegment =
        { new Arbitrary<PathSegment>() with
            override x.Generator =
                gen {
                    let! kind = Gen.elements [0;1;2;3]
                    
                    match kind with
                    | 0 ->
                        let! p0 = Arb.generate<V2d>
                        let! p1 = Arb.generate<V2d> |> Gen.filter (fun p -> not (Fun.ApproximateEquals(p, p0, 1E-8)))
                        return PathSegment.line p0 p1
                    | 1 ->
                        let! p0 = Arb.generate<V2d>
                        let! p1 = Arb.generate<V2d> |> Gen.filter (fun p -> not (Fun.ApproximateEquals(p, p0, 1E-8)))
                        let! p2 = Arb.generate<V2d> |> Gen.filter (fun p -> not (Fun.ApproximateEquals(p, p0, 1E-8)) && not (Fun.ApproximateEquals(p, p1, 1E-8)))
                        return PathSegment.bezier2 p0 p1 p2
                    | 2 ->
                        let! center = Arb.generate<V2d>
                        let! a0 = Arb.generate<V2d>
                        let! a1 = Arb.generate<V2d> |> Gen.filter (fun p -> not (Fun.IsTiny(Vec.AngleBetween(Vec.normalize a0, Vec.normalize p), 1E-5)))
                        
                        let! (NormalFloat alpha0) = Arb.generate
                        let! (NormalFloat dAlpha) = Arb.generate
                        
                        let dAlpha = dAlpha % Constant.PiHalf
                        
                        return PathSegment.arc alpha0 dAlpha (Ellipse2d(center, a0, a1))
                    | _ ->
                        let! p0 = Arb.generate<V2d>
                        let! p1 = Arb.generate<V2d> |> Gen.filter (fun p -> not (Fun.ApproximateEquals(p, p0, 1E-8)))
                        let! p2 = Arb.generate<V2d> |> Gen.filter (fun p -> not (Fun.ApproximateEquals(p, p0, 1E-8)) && not (Fun.ApproximateEquals(p, p1, 1E-8)))
                        let! p3 = Arb.generate<V2d> |> Gen.filter (fun p -> not (Fun.ApproximateEquals(p, p0, 1E-8)) && not (Fun.ApproximateEquals(p, p1, 1E-8)) && not (Fun.ApproximateEquals(p, p2, 1E-8)))
                        return PathSegment.bezier3 p0 p1 p2 p3
                }
        }
module Expect =
    let inline approxEqualAux< ^a, ^b when (^a or ^b) : (static member ApproximateEquals : ^a * ^a * float -> bool)> (foo : ^ b) (a : 'a) (b : 'a) (eps : float) =
        (((^a or ^b) : (static member ApproximateEquals : ^a * ^a * float -> bool) (a, b, eps)))
    
    
    let inline approxEquals a b eps msg =
        if not (approxEqualAux Unchecked.defaultof<Fun> a b eps) then
            Expect.equal a b msg
    
    let inline relativeApproxEquals (a : float) (b : float) eps msg =
        
        let scale = max (abs a) (abs b)
        if not (Fun.IsTiny(scale, eps)) then
            let ra = a / scale
            let rb = b / scale
            
            if not (Fun.ApproximateEquals(ra, rb, eps)) then
                Expect.equal a b msg
    
    
    
[<AutoOpen>]
module ExpectoOverrides =
        
    let config =
        { 
            FsCheckConfig.defaultConfig with 
                arbitrary = [ typeof<Generators> ]
                maxTest = 100000
        }
            
            
    let testProperty a b =
        testPropertyWithConfig config a b
