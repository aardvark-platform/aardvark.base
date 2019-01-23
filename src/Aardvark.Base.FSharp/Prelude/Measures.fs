namespace Aardvark.Base

open System
open System.Diagnostics



[<StructuredFormatDisplay("{AsString}")>]
type MicroTime =
    struct
        val mutable public TotalNanoseconds : int64

        member x.TotalMinutes = float x.TotalNanoseconds / 60000000000.0
        member x.TotalSeconds = float x.TotalNanoseconds / 1000000000.0
        member x.TotalMilliseconds = float x.TotalNanoseconds / 1000000.0
        member x.TotalMicroseconds = float x.TotalNanoseconds / 1000.0
        
        member x.IsZero = x.TotalNanoseconds = 0L

        member x.IsPositiveInfinity =
            x.TotalNanoseconds = Int64.MaxValue

        member x.IsNegativeInfinity =
            x.TotalNanoseconds = Int64.MinValue

        member x.IsInfinite =
            x.TotalNanoseconds = Int64.MaxValue ||
            x.TotalNanoseconds = Int64.MinValue

        member x.IsFinite =
            x.TotalNanoseconds <> Int64.MaxValue &&
            x.TotalNanoseconds <> Int64.MinValue

        member private x.AsString =
            x.ToString()

        override x.ToString() =
            if x.IsPositiveInfinity then "inf"
            elif x.IsNegativeInfinity then "-inf"
            else
                let ns = abs x.TotalNanoseconds
                if ns = 0L then "0"
                elif ns >= 60000000000L then 
                    let m = x.TotalMinutes
                    let sec = abs (60.0 * (m % 1.0))
                    if sec = 0.0 then sprintf "%.0fm" m
                    else sprintf "%.0fm%.0fs" m sec
                elif ns >= 1000000000L then 
                    sprintf "%.3fs" x.TotalSeconds
                elif ns >= 1000000L then 
                    sprintf "%.2fms" x.TotalMilliseconds
                elif ns >= 1000L then 
                    sprintf "%.1fµs" x.TotalMicroseconds
                else 
                    sprintf "%dns" x.TotalNanoseconds


        static member (+) (l : MicroTime, r : MicroTime) = 
            if l.IsPositiveInfinity then MicroTime.PositiveInfinity
            elif l.IsNegativeInfinity then MicroTime.NegativeInfinity
            elif r.IsPositiveInfinity then MicroTime.PositiveInfinity
            elif r.IsNegativeInfinity then MicroTime.NegativeInfinity
            else MicroTime(l.TotalNanoseconds + r.TotalNanoseconds)

        static member (-) (l : MicroTime, r : MicroTime) = 
            if l.IsPositiveInfinity then MicroTime.PositiveInfinity
            elif l.IsNegativeInfinity then MicroTime.NegativeInfinity
            elif r.IsPositiveInfinity then MicroTime.NegativeInfinity
            elif r.IsNegativeInfinity then MicroTime.PositiveInfinity
            else MicroTime(l.TotalNanoseconds - r.TotalNanoseconds)

        static member (~-) (l : MicroTime) = 
            if l.IsPositiveInfinity then MicroTime.NegativeInfinity
            elif l.IsNegativeInfinity then MicroTime.PositiveInfinity
            else MicroTime(-l.TotalNanoseconds)

        static member (*) (l : MicroTime, r : float) = 
            if r = 0.0 then MicroTime.Zero
            else MicroTime(float l.TotalNanoseconds * r |> int64)

        static member (*) (l : float, r : MicroTime) = 
            if l = 0.0 then MicroTime.Zero
            else MicroTime(l * float r.TotalNanoseconds |> int64)

        static member (*) (l : MicroTime, r : int) = 
            if r = 0 then MicroTime.Zero
            else MicroTime(l.TotalNanoseconds * int64 r)

        static member (*) (l : int, r : MicroTime) = 
            if l = 0 then MicroTime.Zero
            else MicroTime(int64 l * r.TotalNanoseconds)

        static member (/) (l : MicroTime, r : int) = 
            if r = 0 then 
                if l.TotalNanoseconds >= 0L then MicroTime.PositiveInfinity
                else MicroTime.NegativeInfinity
            else 
                MicroTime(l.TotalNanoseconds / int64 r)

        static member (/) (l : MicroTime, r : float) = 
            if r = 0.0 then
                if l.TotalNanoseconds >= 0L then MicroTime.PositiveInfinity
                else MicroTime.NegativeInfinity
            else
                MicroTime(float l.TotalNanoseconds / r |> int64)

        static member private ToFloat (t : MicroTime) : float =
            if t.IsPositiveInfinity then Double.PositiveInfinity
            elif t.IsNegativeInfinity then Double.NegativeInfinity
            else float t.TotalNanoseconds

        static member DivideByInt(l : MicroTime, r : int) = l / r

        static member (/) (l : MicroTime, r : MicroTime) = 
            MicroTime.ToFloat l / MicroTime.ToFloat r


        static member PositiveInfinity = MicroTime(Int64.MaxValue)
        static member NegativeInfinity = MicroTime(Int64.MinValue)
        static member Zero = MicroTime(0L)

        static member FromNanoseconds(ns : int64) = MicroTime(ns)
        static member FromMicroseconds(us : float) = MicroTime(int64(us * 1000.0))
        static member FromMilliseconds(ms : float) = MicroTime(int64(ms * 1000000.0))
        static member FromSeconds(s : float) = MicroTime(int64(s * 1000000000.0))
        static member FromMinutes(m : float) = MicroTime(int64(m * 60000000000.0))
        static member FromTicks (t : int64) = MicroTime.FromMicroseconds(float t * float TimeSpan.TicksPerSecond / 1000000.0)

        static member Nanosecond = MicroTime(1L)
        static member Microsecond = MicroTime(1000L)
        static member Millisecond = MicroTime(1000000L)
        static member Second = MicroTime(1000000000L)
        static member Minute = MicroTime(60000000000L)
        
        new (ns : int64) = { TotalNanoseconds = ns }
        new (ts : TimeSpan) = { TotalNanoseconds = ts.Ticks * (1000000000L / TimeSpan.TicksPerSecond) }
    end

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module MicroTime =
    
    let zero = MicroTime.Zero

    let ns = MicroTime.Nanosecond
    let us = MicroTime.Microsecond
    let ms = MicroTime.Millisecond
    let s = MicroTime.Second
    let m = MicroTime.Minute
    
    let inline ofNanoseconds (ns : ^a) = MicroTime(int64 ns)
    let inline ofMicroseconds (us : ^a) = MicroTime.FromMicroseconds(float us)
    let inline ofMilliseconds (us : ^a) = MicroTime.FromMilliseconds(float us)
    let inline ofSeconds (us : ^a) = MicroTime.FromSeconds(float us)
    let inline ofMinutes (us : ^a) = MicroTime.FromMinutes(float us)

    let inline nanoseconds (t : MicroTime) = t.TotalNanoseconds
    let inline microseconds (t : MicroTime) = t.TotalMicroseconds
    let inline milliseconds (t : MicroTime) = t.TotalMilliseconds
    let inline seconds (t : MicroTime) = t.TotalSeconds
    let inline minutes (t : MicroTime) = t.TotalMinutes
    
    let inline isZero (t : MicroTime) = t.IsZero
    let inline isFinite (t : MicroTime) = t.IsFinite
    let inline isPositiveInfinity (t : MicroTime) = t.IsPositiveInfinity
    let inline isNegativeInfinity (t : MicroTime) = t.IsNegativeInfinity



[<StructuredFormatDisplay("{AsString}")>]
type Mem =
    struct
        val mutable public Bytes : int64

        member x.IsZero = x.Bytes = 0L

        member x.Kibibytes = float x.Bytes / 1024.0
        member x.Mebibytes = float x.Bytes / 1048576.0
        member x.Gibibytes = float x.Bytes / 1073741824.0
        member x.Tebibytes = float x.Bytes / 1099511627776.0
        member x.Pebibytes = float x.Bytes / 1125899906842624.0
        member x.Exbibytes = float x.Bytes / 1152921504606846976.0
        member x.Kilobytes = float x.Bytes / 1000.0
        member x.Megabytes = float x.Bytes / 1000000.0
        member x.Gigabytes = float x.Bytes / 1000000000.0
        member x.Terabytes = float x.Bytes / 1000000000000.0
        member x.Petabytes = float x.Bytes / 1000000000000000.0
        member x.Exabytes  = float x.Bytes / 1000000000000000000.0
        
        static member Byte      = Mem(1L)
        static member Kibibyte  = Mem(1024L)
        static member Mebibyte  = Mem(1048576L)
        static member Gibibyte  = Mem(1073741824L)
        static member Tebibyte  = Mem(1099511627776L)
        static member Pebibyte  = Mem(1125899906842624L)
        static member Exbibyte  = Mem(1152921504606846976L)
        static member Kilobyte  = Mem(1000L)
        static member Megabyte  = Mem(1000000L)
        static member Gigabyte  = Mem(1000000000L)
        static member Terabyte  = Mem(1000000000000L)
        static member Petabyte  = Mem(1000000000000000L)
        static member Exabyte   = Mem(1000000000000000000L)

        member private x.AsString = x.ToString()
        
        member x.ToStringDecimal() =
            let b = abs x.Bytes
            if b = 0L then "0"
            elif b >= 1000000000000000000L then sprintf "%.3fEB" x.Exabytes
            elif b >= 1000000000000000L then sprintf "%.3fPB" x.Petabytes
            elif b >= 1000000000000L then sprintf "%.3fTB" x.Terabytes
            elif b >= 1000000000L then sprintf "%.3fGB" x.Gigabytes
            elif b >= 1000000L then sprintf "%.2fMB" x.Megabytes
            elif b >= 1000L then sprintf "%.1fKiB" x.Kilobytes
            else sprintf "%dB" x.Bytes

        member x.ToStringBinary() =
            let b = abs x.Bytes
            if b = 0L then "0"
            elif b >= 1152921504606846976L then sprintf "%.3fEiB" x.Exbibytes
            elif b >= 1125899906842624L then sprintf "%.3fPiB" x.Pebibytes
            elif b >= 1099511627776L then sprintf "%.3fTiB" x.Tebibytes
            elif b >= 1073741824L then sprintf "%.3fGiB" x.Gibibytes
            elif b >= 1048576L then sprintf "%.2fMiB" x.Mebibytes
            elif b >= 1024L then sprintf "%.1fKiB" x.Kibibytes
            else sprintf "%dB" x.Bytes

        member x.ToString(decimal : bool) =
            if decimal then x.ToStringDecimal()
            else x.ToStringBinary()

        override x.ToString() = x.ToStringBinary()

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


        static member FromKibibytes(kib : float) = Mem(int64 (1024.0 * kib))
        static member FromMebibytes(mib : float) = Mem(int64 (1048576.0 * mib))
        static member FromGibibytes(gib : float) = Mem(int64 (1073741824.0 * gib))
        static member FromTebibytes(tib : float) = Mem(int64 (1099511627776.0 * tib))
        static member FromPebibytes(pib : float) = Mem(int64 (1125899906842624.0 * pib))
        static member FromExbibytes(eib : float) = Mem(int64 (1152921504606846976.0 * eib))
        
        static member FromKilobytes(kb : float) = Mem(int64 (1000.0 * kb))
        static member FromMegabytes(mb : float) = Mem(int64 (1000000.0 * mb))
        static member FromGigabytes(gm : float) = Mem(int64 (1000000000.0 * gm))
        static member FromTerabytes(tb : float) = Mem(int64 (1000000000000.0 * tb))
        static member FromPetabytes(pb : float) = Mem(int64 (1000000000000000.0 * pb))
        static member FromExabytes(eb : float)  = Mem(int64 (1000000000000000000.0 * eb))

        new(bytes : int64) = { Bytes = bytes }
        new(bytes : int) = { Bytes = int64 bytes }
        new(bytes : uint64) = { Bytes = int64 bytes }
        new(bytes : uint32) = { Bytes = int64 bytes }
        new(bytes : nativeint) = { Bytes = int64 bytes }
        new(bytes : unativeint) = { Bytes = int64 bytes }
    end

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Mem =
    let zero = Mem.Zero
    
    let B = Mem.Byte
    let kB = Mem.Kilobyte
    let MB = Mem.Megabyte
    let GB = Mem.Gigabyte
    let TB = Mem.Terabyte
    let PB = Mem.Petabyte
    let EB = Mem.Exabyte

    let kiB = Mem.Kibibyte
    let MiB = Mem.Mebibyte
    let GiB = Mem.Gibibyte
    let TiB = Mem.Tebibyte
    let PiB = Mem.Pebibyte
    let EiB = Mem.Exbibyte

    
    let inline ofBytes (b : ^a) = Mem(int64 b)

    let inline ofKibibytes (kib : ^a) = Mem.FromKibibytes(float kib)
    let inline ofMebibytes (mib : ^a) = Mem.FromMebibytes(float mib)
    let inline ofGibibytes (gib : ^a) = Mem.FromGibibytes(float gib)
    let inline ofTebibytes (tib : ^a) = Mem.FromTebibytes(float tib)
    let inline ofPebibytes (pib : ^a) = Mem.FromPebibytes(float pib)
    let inline ofExbibytes (eib : ^a) = Mem.FromExbibytes(float eib)
    
    let inline ofKilobytes (kb : ^a) = Mem.FromKilobytes(float kb)
    let inline ofMegabytes (mb : ^a) = Mem.FromMegabytes(float mb)
    let inline ofGigabytes (gb : ^a) = Mem.FromGigabytes(float gb)
    let inline ofTerabytes (tb : ^a) = Mem.FromTerabytes(float tb)
    let inline ofPetabytes (pb : ^a) = Mem.FromPetabytes(float pb)
    let inline ofExabytes  (eb : ^a) = Mem.FromExabytes(float eb)


[<AutoOpen>]
module ``Stopwatch Extensions`` =

    type TimeSpan with
        member x.MicroTime = MicroTime x

    type Stopwatch with
        member x.MicroTime = MicroTime x.Elapsed