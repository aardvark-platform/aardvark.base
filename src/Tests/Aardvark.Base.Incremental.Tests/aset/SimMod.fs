namespace Aardvark.Base.Incremental.Tests

open Aardvark.Base
open Aardvark.Base.Incremental
open NUnit.Framework
open FsCheck
open FsCheck.NUnit


type simmod<'a> =
    inherit IDependent
    abstract member Value : 'a
    abstract member Mod : IMod<'a>

and csimmod<'a>(value : 'a) =
    let id = newId()
    let m = Mod.init value
    let mutable value = value

    interface IDependent with
        member x.AsString = sprintf "simmod%d" id
        member x.Inputs = HSet.ofList [x :> IChangeable]
        
    interface IChangeable with
        member x.RandomChange(rand, addprob) =
            gen {
                let! v = Arb.generate<'a>
                m.Value <- v
                value <- v
                return sprintf "set(%A)" v
            }

    interface simmod<'a> with
        member x.Value = value
        member x.Mod = m :> IMod<_>
        
type SimModGenerator() =
    static member SimMod() =
        { new Arbitrary<simmod<'a>>() with
            override x.Generator =  
                gen {
                    let! v = Arb.generate<'a>
                    return csimmod v :> simmod<_>
                }
        }

