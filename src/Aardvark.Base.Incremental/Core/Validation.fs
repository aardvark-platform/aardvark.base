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

    let createFile ext =
        let fileName = System.DateTime.Now.ToString("yyyy_MM_dd_hh_mm_ss") + ext
        let desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)

        let folder = Path.Combine(desktop, "incremental-dumps")
        if not <| Directory.Exists folder then
            Directory.CreateDirectory folder |> ignore

        Path.Combine(folder, fileName)

    [<AutoOpen>]
    module GraphVisualizations =

        let rec dumpTreeDot' (stream : StreamWriter) (seen : HashSet<int>) (depth : int) (x : IAdaptiveObject) =
            if depth < 0 then 
                ()
            else
                lock x (fun () ->
                    if seen.Contains x.Id then ()
                    else
                        seen.Add x.Id |> ignore
                        stream.WriteLine(sprintf "\t%d [label=\"id=%d, level=%d\"];" x.Id x.Id x.Level)
                        try
                            x.Inputs |> Seq.iter Monitor.Enter

                            for i in x.Inputs do
                                stream.WriteLine(sprintf "\t%d -> %d;" x.Id i.Id)

                            if not (Seq.isEmpty x.Inputs) then
                                x.Inputs |> Seq.iter (dumpTreeDot' stream seen (depth - 1)) 
  
                        finally
                            x.Inputs |> Seq.iter Monitor.Exit
                )
    
        let dumpTreeDot (fileName : string) (depth : int) (x : IAdaptiveObject) =
            use file = new StreamWriter ( fileName  )
            file.WriteLine("digraph depGraph {")
            dumpTreeDot' file ( HashSet() ) depth x
            file.WriteLine("}")
            fileName

        let rec dumpDgml' (nodes : StringBuilder) (links : StringBuilder) (seen : HashSet<int>) (depth : int) (x : IAdaptiveObject) =
            if depth < 0 then 
                ()
            else
                lock x (fun () ->
                    if seen.Contains x.Id then ()
                    else
                        seen.Add x.Id |> ignore
                        nodes.AppendLine(sprintf "\t\t<Node Id=\"%d\" Label=\"Id=%d, level=%d\"/>" x.Id x.Id x.Level) |> ignore
                        try
                            x.Inputs |> Seq.iter Monitor.Enter

                            for i in x.Inputs do
                                links.AppendLine(sprintf "\t\t<Link Source=\"%d\" Target=\"%d\"/>" x.Id i.Id) |> ignore

                            if not (Seq.isEmpty x.Inputs) then
                                x.Inputs |> Seq.iter (dumpDgml' nodes links seen (depth - 1)) 
  
                        finally
                            x.Inputs |> Seq.iter Monitor.Exit
                )

        let private properties = """
          <Properties>
            <Property Id="Background" Label="Background" DataType="Brush" />
            <Property Id="Label" Label="Label" DataType="String" />
            <Property Id="Size" DataType="String" />
            <Property Id="Start" DataType="DateTime" />
          </Properties>"""
        
        let dumpDgml (fileName : string) (depth : int) (x : IAdaptiveObject) =
            let nodes = StringBuilder()
            let links = StringBuilder()
            dumpDgml' nodes links (HashSet()) depth x
            use file = new StreamWriter( fileName )
            file.WriteLine("<?xml version='1.0' encoding='utf-8'?>")
            file.WriteLine("<DirectedGraph xmlns=\"http://schemas.microsoft.com/vs/2009/dgml\">")
            file.WriteLine("<Nodes>")
            file.Write(nodes.ToString())
            file.WriteLine(@"</Nodes>")
            file.WriteLine("<Links>")
            file.Write(links.ToString())
            file.WriteLine("</Links>")
            file.WriteLine(properties)
            file.WriteLine("</DirectedGraph>")
            fileName
            

    let rec dumpTree (depth : int) (x : IAdaptiveObject) =
            if depth < 0 then 
                "..."
            else
                lock x (fun () ->
                    try
                        x.Inputs |> Seq.iter Monitor.Enter

                        if not (Seq.isEmpty x.Inputs) then
                            let inputStr = x.Inputs |> Seq.map (dumpTree (depth - 1)) |> String.concat "\r\n"|> String.indent 2
                            sprintf "%s {\r\n    id = %d\r\n    level = %d\r\n    outOfDate = %A\r\n    inputs = [\r\n%s\r\n    ]\r\n}" (x.GetType().PrettyName) x.Id x.Level x.OutOfDate inputStr
                        else
                            sprintf "%s {\r\n    id = %d\r\n    level = %d\r\n    outOfDate = %A\r\n}" (x.GetType().PrettyName) x.Id x.Level x.OutOfDate

                    finally
                        x.Inputs |> Seq.iter Monitor.Exit
                )

    let dump (level : int) (x : IAdaptiveObject) =
        let str = dumpTree level x

        let file = createFile ".crashdump"
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
                        let iAmInTransaction = failwith "not implemented" //Transaction.InAnyOfTheTransactionsInternal x
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
                if not (Seq.isEmpty x.Inputs) then
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

    [<Extension>]
    static member DumpDotFile(x : IAdaptiveObject, depth : int, file : string) =
        dumpTreeDot file depth x

    [<Extension>]
    static member DumpDgml(x : IAdaptiveObject, depth : int, file : string) =
        dumpDgml file depth x
