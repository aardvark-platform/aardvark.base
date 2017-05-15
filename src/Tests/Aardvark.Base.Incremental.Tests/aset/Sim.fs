namespace Aardvark.Base.Incremental.Tests

open Aardvark.Base
open Aardvark.Base.Incremental
open NUnit.Framework
open FsCheck
open FsCheck.NUnit

type IChangeable =
    inherit IDependent
    abstract member RandomChange : rand : RandomSystem * addprob : float -> Gen<string>

and IDependent =
    abstract member Inputs : hset<IChangeable>
    abstract member AsString : string

type IValidator =
    abstract member Validate : list<IChangeable * string> -> unit
    abstract member Release : unit -> unit

module Dependent =
    let arbitraryChange (rand : RandomSystem) (addprob : float) (setProb : float) (sim : IDependent) : Gen<list<IChangeable * string>>=
        let inputs = sim.Inputs
        gen {
            let log = System.Collections.Generic.List<IChangeable * string>()
            use t = new Transaction()
            let old = Transaction.Current
            Transaction.Current <- Some t
            for i in inputs do
                if rand.UniformDouble() < setProb then
                    let! c = i.RandomChange(rand, addprob)
                    log.Add(i, c)

            t.Commit()
            Transaction.Current <- old
            return CSharpList.toList log
        }

    let validate<'a when 'a :> IDependent> (numInstances : int) (numChanges : int) (createValidator : 'a -> IValidator) =
        let rand = RandomSystem()
        gen {
            for i in 1 .. numInstances do
                let! test = Arb.generate<'a>
                printfn "%d: %s" (i-1) test.AsString
                let validator = createValidator test
                validator.Validate []

                for c in 1 .. numChanges do
                    let! log = arbitraryChange rand 0.5 0.3 test
                    validator.Validate log

                validator.Release()

        }
