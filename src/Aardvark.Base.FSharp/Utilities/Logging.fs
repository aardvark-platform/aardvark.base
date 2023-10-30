namespace Aardvark.Base

#nowarn "1337"


[<AbstractClass; Sealed>]
type Log private() =
    static let lineBreakRx = System.Text.RegularExpressions.Regex @"\r?\n"

    static let printLines (print : string -> unit) (str : string) =
        if str.IndexOf '\n' >= 0 then
            let lines = lineBreakRx.Split(str)
            for l in lines do print l
        else
            print str
        

    static let writeLine =
        () 
        fun (str : string) ->
            if str.IndexOf '\n' >= 0 then
                let lines = lineBreakRx.Split(str)
                for l in lines do Report.Line("{0}", l)
            else
                Report.Line("{0}", str)
                
        

    static let writeDebug =
        () 
        fun (str : string) ->
            if str.IndexOf '\n' >= 0 then
                let lines = lineBreakRx.Split(str)
                for l in lines do Report.DebugNoPrefix("{0}", l)
            else
                Report.DebugNoPrefix("{0}", str)

    static let writeWarn =
        () 
        fun (str : string) ->
            if str.IndexOf '\n' >= 0 then
                let lines = lineBreakRx.Split(str)
                Report.WarnNoPrefix("WARNING")
                for l in lines do Report.WarnNoPrefix("  {0}", l)
            else
                Report.Warn("{0}", str)
        
    static let writeError =
        () 
        fun (str : string) ->
            if str.IndexOf '\n' >= 0 then
                let lines = lineBreakRx.Split(str)
                Report.ErrorNoPrefix("ERROR")
                for l in lines do Report.ErrorNoPrefix("  {0}", l)
            else
                Report.Error("{0}", str)
        
    [<CompilerMessage("internal", 1337, IsHidden = true)>]
    static member WriteDebug = writeDebug

    [<CompilerMessage("internal", 1337, IsHidden = true)>]
    static member WriteLine = writeLine

    [<CompilerMessage("internal", 1337, IsHidden = true)>]
    static member WriteWarn = writeWarn
    
    [<CompilerMessage("internal", 1337, IsHidden = true)>]
    static member WriteError = writeError
    
    static member inline debug fmt = Printf.kprintf Log.WriteDebug fmt
    static member inline line fmt = Printf.kprintf Log.WriteLine fmt
    static member inline warn fmt = Printf.kprintf Log.WriteWarn fmt
    static member inline error fmt = Printf.kprintf Log.WriteError fmt

    static member inline startTimed fmt = Printf.kprintf (fun str -> Report.BeginTimed("{0}", str)) fmt
    static member inline start fmt = Printf.kprintf (fun str -> Report.Begin("{0}", str)) fmt
    static member inline stop() = Report.End() |> ignore
    static member inline stop fmt = Printf.kprintf (fun str -> Report.End("{0}", str) |> ignore) fmt

    static member inline section fmt = Printf.kprintf (fun str -> fun c -> Report.Begin("{0}", str); c(); Report.End() |> ignore) fmt

    static member check c fmt = Printf.kprintf (fun str -> if not c then printLines (fun str -> Report.Warn("{0}", str)) str) fmt
