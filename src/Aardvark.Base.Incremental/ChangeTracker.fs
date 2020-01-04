namespace Aardvark.Base.Incremental


module ChangeTracker =
    type private ChangeTracker<'a>() =
        static let createSimpleTracker : (Option<'a -> 'a -> bool>) -> 'a -> bool =
            fun eq ->
                let eq = defaultArg eq (fun a b -> System.Object.Equals(a,b))
                let old = ref None
                fun n ->
                    match !old with
                        | None -> 
                            old := Some n
                            true
                        | Some o ->
                            if eq o n then 
                                false
                            else 
                                old := Some n
                                true


        static member CreateTracker eq = createSimpleTracker eq
        static member CreateDefaultTracker() = createSimpleTracker None
        static member CreateCustomTracker eq = createSimpleTracker (Some eq)

    let track<'a> : 'a -> bool =
        ChangeTracker<'a>.CreateDefaultTracker()

    let trackCustom<'a> (eq : Option<'a -> 'a -> bool>) : 'a -> bool =
        ChangeTracker<'a>.CreateTracker eq

