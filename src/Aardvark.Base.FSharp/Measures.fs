namespace Aardvark.Base

open System
open System.Diagnostics



[<StructuredFormatDisplay("{AsString}")>]
type MicroTime =
    struct
        val mutable public TotalNanoseconds : int64

        member x.TotalSeconds = float x.TotalNanoseconds / 1000000000.0
        member x.TotalMilliseconds = float x.TotalNanoseconds / 1000000.0
        member x.TotalMicroseconds = float x.TotalNanoseconds / 1000.0

        member private x.AsString =
            x.ToString()

        override x.ToString() =
            if x.TotalNanoseconds = 0L then "0"
            elif x.TotalNanoseconds > 1000000000L then sprintf "%.3fs" x.TotalSeconds
            elif x.TotalNanoseconds > 1000000L then sprintf "%.2fms" x.TotalMilliseconds
            elif x.TotalNanoseconds > 1000L then sprintf "%.1fµs" x.TotalMicroseconds
            else sprintf "%dns" x.TotalNanoseconds


        static member (+) (l : MicroTime, r : MicroTime) = MicroTime(l.TotalNanoseconds + r.TotalNanoseconds)
        static member (-) (l : MicroTime, r : MicroTime) = MicroTime(l.TotalNanoseconds - r.TotalNanoseconds)
        static member (~-) (l : MicroTime) = MicroTime(-l.TotalNanoseconds)
        static member (*) (l : MicroTime, r : float) = MicroTime(float l.TotalNanoseconds * r |> int64)
        static member (*) (l : float, r : MicroTime) = MicroTime(l * float r.TotalNanoseconds |> int64)
        static member (*) (l : MicroTime, r : int) = MicroTime(l.TotalNanoseconds * int64 r)
        static member (*) (l : int, r : MicroTime) = MicroTime(int64 l * r.TotalNanoseconds)
        static member (/) (l : MicroTime, r : int) = MicroTime(l.TotalNanoseconds / int64 r)
        static member (/) (l : MicroTime, r : float) = MicroTime(float l.TotalNanoseconds / r |> int64)

        static member (/) (l : MicroTime, r : MicroTime) = float l.TotalNanoseconds / float r.TotalNanoseconds


        static member Zero = MicroTime(0L)


        new (ns : int64) = { TotalNanoseconds = ns }
        new (ts : TimeSpan) = { TotalNanoseconds = (ts.Ticks * 1000000000L) / TimeSpan.TicksPerSecond}
    end

[<StructuredFormatDisplay("{AsString}")>]
type Mem =
    struct
        val mutable public Bytes : int64

        member x.Kilobytes = float x.Bytes / 1024.0
        member x.Megabytes = float x.Bytes / 1048576.0
        member x.Gigabytes = float x.Bytes / 1073741824.0
        member x.Terabytes = float x.Bytes / 1099511627776.0

        member private x.AsString = x.ToString()

        override x.ToString() =
            let b = abs x.Bytes
            if b = 0L then "0"
            elif b > 1099511627776L then sprintf "%.3fTB" x.Terabytes
            elif b > 1073741824L then sprintf "%.3fGB" x.Gigabytes
            elif b > 1048576L then sprintf "%.2fMB" x.Megabytes
            elif b > 1024L then sprintf "%.1fkB" x.Kilobytes
            else sprintf "%db" x.Bytes

        static member Zero = Mem(0L)

        static member (+) (l : Mem, r : Mem) = Mem(l.Bytes + r.Bytes)
        static member (-) (l : Mem, r : Mem) = Mem(l.Bytes - r.Bytes)
        static member (~-) (l : Mem) = Mem(-l.Bytes)

        static member (*) (l : Mem, r : int) = Mem(l.Bytes * int64 r)
        static member (*) (l : Mem, r : float) = Mem(float l.Bytes * r |> int64)
        static member (*) (l : int, r : Mem) = Mem(int64 l * r.Bytes)
        static member (*) (l : float, r : Mem) = Mem(l * float r.Bytes |> int64)
        static member (/) (l : Mem, r : int) = Mem(l.Bytes / int64 r)
        static member (/) (l : Mem, r : float) = Mem(float l.Bytes / r |> int64)
        static member (/) (l : Mem, r : Mem) = float l.Bytes / float r.Bytes

        new(bytes : int64) = { Bytes = bytes }
        new(bytes : int) = { Bytes = int64 bytes }
        new(bytes : uint64) = { Bytes = int64 bytes }
        new(bytes : uint32) = { Bytes = int64 bytes }
        new(bytes : nativeint) = { Bytes = int64 bytes }
        new(bytes : unativeint) = { Bytes = int64 bytes }
    end


[<AutoOpen>]
module ``Stopwatch Extensions`` =
    type Stopwatch with
        member x.MicroTime = MicroTime x.Elapsed