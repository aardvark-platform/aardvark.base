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
