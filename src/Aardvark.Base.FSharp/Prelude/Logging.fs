namespace Aardvark.Base



type Log =
    
    static member inline line fmt = Printf.kprintf (fun str -> Report.Line("{0}", str)) fmt
    static member inline warn fmt = Printf.kprintf (fun str -> Report.Warn("{0}", str)) fmt
    static member inline error fmt = Printf.kprintf (fun str -> Report.Error("{0}", str)) fmt

    static member inline startTimed fmt = Printf.kprintf (fun str -> Report.BeginTimed("{0}", str)) fmt
    static member inline start fmt = Printf.kprintf (fun str -> Report.Begin("{0}", str)) fmt
    static member inline stop() = Report.End() |> ignore
    static member inline stop fmt = Printf.kprintf (fun str -> Report.End("{0}", str) |> ignore) fmt

    static member inline section fmt = Printf.kprintf (fun str -> fun c -> Report.Begin("{0}", str); c(); Report.End() |> ignore) fmt

    static member check c fmt = Printf.kprintf (fun str -> if not c then Report.Warn("{0}", str)) fmt

open NUnit.Framework

[<TestFixture>]
module LogTests =
    open System
    open System.IO

 
    let private capture (f : unit -> unit) =
        let builder = System.Text.StringBuilder()
        let myTarget = new TextLogTarget(new System.Action<int, LogType, int, string>(fun _ _ _ str -> builder.Append(str) |> ignore))

        Report.Targets.Add(myTarget)

        f()

        Report.Targets.Remove(myTarget)

        myTarget.Dispose()
       

        builder.ToString()

    let private check (expected : string) (real : string) =
        if expected <> real then
            let real = Array.zip (real.ToCharArray()) (System.Text.ASCIIEncoding.Default.GetBytes(real))
            let realRep = real |> Array.map (fun (c,b) -> sprintf "0x%X (%c)" b (if c <> '\n' then c else ' ')) |> String.concat " "
            
            let expected = Array.zip (expected.ToCharArray()) (System.Text.ASCIIEncoding.Default.GetBytes(expected))
            let expectedRep = expected |> Array.map (fun (c,b) -> sprintf "0x%X (%c)" b (if c <> '\n' then c else ' ')) |> String.concat " "
            

            failwithf "unexpected output: %A vs. %A" realRep expectedRep

    [<Test>]
    let ``simple line``() =
        let str =
            capture (fun () ->
                Log.line "a"
            )

        check " 0: a\n" str

    [<Test>]
    let ``begin and end``() =
        let str =
            capture (fun () ->
                Log.start "a"
                Log.line "b"
                Log.stop()
                Log.line "c"
            )

        check " 0: a\n 0:   b\n 0: c\n" str