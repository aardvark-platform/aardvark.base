namespace Aardvark.Base.FSharp.Tests



module Test =
    [<AbstractClass>]
    type Monoid<'a> =
        class
            val mutable public mempty : 'a
            abstract member mappend : 'a -> 'a -> 'a
            new(a) = { mempty = a }

        end

    let test() =
        { new Monoid<string>("") with
            member x.mappend a b =
                a + b
        }

    let sepp() =
        test().mappend (test().mempty) "sadsad"

module Main =

    [<EntryPoint>]
    let main args = 
        Aardvark.Base.FSharp.Tests.``Control tests``.``[Control] stateful step var cancellation test``()
        1