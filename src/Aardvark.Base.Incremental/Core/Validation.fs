namespace Aardvark.Base.Incremental.Validation

open System
open System.Collections.Generic
open Aardvark.Base
open System.Collections.Concurrent
open System.Threading
open System.Diagnostics
open System.Text
open System.IO
open Aardvark.Base.ReflectionHelpers
open System.Runtime.CompilerServices
open Aardvark.Base.Incremental

[<AutoOpen>]
module private ValidationModule =
    let str (a : IAdaptiveObject) =
        sprintf "%s { id = %d; level = %d; outOfDate = %A }" (a.GetType().PrettyName) a.Id a.Level a.OutOfDate

    let rec dumpTree (depth : int) (x : IAdaptiveObject) =
        if depth < 0 then 
            "..."
        else
            lock x (fun () ->
                try
                    x.Inputs |> Seq.iter Monitor.Enter

                    if x.Inputs.Count > 0 then
                        let inputStr = x.Inputs |> Seq.map (dumpTree (depth - 1)) |> String.concat "\r\n"|> String.indent 2
                        sprintf "%s {\r\n    id = %d\r\n    level = %d\r\n    outOfDate = %A\r\n    inputs = [\r\n%s\r\n    ]\r\n}" (x.GetType().PrettyName) x.Id x.Level x.OutOfDate inputStr
                    else
                        sprintf "%s {\r\n    id = %d\r\n    level = %d\r\n    outOfDate = %A\r\n}" (x.GetType().PrettyName) x.Id x.Level x.OutOfDate

                finally
                    x.Inputs |> Seq.iter Monitor.Exit
            )

    let dump (level : int) (x : IAdaptiveObject) =
        let str = dumpTree level x

        let fileName = System.DateTime.Now.ToString("yyyy_MM_dd_hh_mm_ss") + ".crashdump"
        let desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)

        let folder = Path.Combine(desktop, "incremental-dumps")
        if not <| Directory.Exists folder then
            Directory.CreateDirectory folder |> ignore

        let file = Path.Combine(folder, fileName)
        File.WriteAllText(file, str)
        file


    let rec validate (x : IAdaptiveObject) =
        lock x (fun () ->
            try
                x.Inputs |> Seq.iter Monitor.Enter

                // validate OutOfDate stuff
                if not x.OutOfDate then
                    let invalid = x.Inputs |> Seq.exists (fun i -> i.OutOfDate) && not x.OutOfDate
                    if invalid then
                        let iAmInTransaction = Transaction.InAnyOfTheTransactionsInternal x
                        if not iAmInTransaction then
                            let outdatedInputs = 
                                x.Inputs 
                                    |> Seq.filter (fun i -> i.OutOfDate) 
                                    |> Seq.map str
                                    |> String.concat "; "
                                    |> sprintf "[%s]"

                            let dumpFile = dump Int32.MaxValue x

                            Log.error "inpus %s are out-of-date but their output %s is up-to-date" outdatedInputs (str x)
                            Log.error "  therefore the system will \"miss\" changes!"
                            Log.error "  a crashdump has been stored to: %s" dumpFile
                            Log.error "  please send the file to krauthaufen@awx.at for further investigation"


                            if Debugger.IsAttached then Debugger.Break()


                // validate levels
                if x.Inputs.Count > 0 then
                    let maxLevel = x.Inputs |> Seq.map (fun i -> i.Level) |> Seq.max
                    if x.Level <= maxLevel then
                            
                        let dumpFile = dump Int32.MaxValue x

                        let badInputs = 
                            x.Inputs 
                                |> Seq.filter (fun i -> i.Level >= x.Level) 
                                |> Seq.map str 
                                |> String.concat "; " 
                                |> sprintf "[%s]"

                        Log.error "cell (%s) has an invalid level which is smaller or equal to one or more input-levels %s" (str x) badInputs
                        Log.error "  therefore the system will \"miss\" changes!"
                        Log.error "  a crashdump has been stored to: %s" dumpFile
                        Log.error "  please send the file to krauthaufen@awx.at for further investigation"


                        if Debugger.IsAttached then Debugger.Break()


                

            finally
                x.Inputs |> Seq.iter Monitor.Exit
        )

        x.Inputs |> Seq.iter validate

[<Extension>]
type IAdaptiveObjectValidationExtensions() =
    
    [<Extension>]
    static member Validate (x : IAdaptiveObject) =
        Log.startTimed "Incremental Validation"
        validate x
        Log.stop()

    [<Extension>]
    static member Dump(x : IAdaptiveObject) =
        dump Int32.MaxValue x

    [<Extension>]
    static member Dump(x : IAdaptiveObject, depth : int) =
        dump depth x
