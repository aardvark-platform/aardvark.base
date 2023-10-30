#r "../../../bin/Debug/netstandard2.0/Aardvark.Base.dll"
#r "../../../bin/Debug/netstandard2.0/Aardvark.Base.FSharp.dll"
#r "../../../bin/Debug/net6.0/FSharp.Data.Adaptive.dll"
#r "System.Net.Http.dll"

open Aardvark.Base
open System
open System.IO
open System.Text
open System.Text.RegularExpressions
open System.Net.Http

fsi.ShowDeclarationValues <- false

let source =
    let ws = Regex(@"\s+", RegexOptions.Compiled)

    use client = new HttpClient()
    use task = client.GetStringAsync "https://raw.githubusercontent.com/axismaps/colorbrewer/master/colorbrewer_schemes.js"
    task.Wait()

    task.Result
    |> String.getLines
    |> Array.map (fun str -> ws.Replace(str, ""))

module Regex =

    let Scheme =
        Regex(
            @"^(?<Name>[A-Za-z0-9]+):\{" +
            @"(?<Palettes>(?:[0-9]:\[.+?\],?)+)" +
            @"'properties':\{(?<Properties>.+)\}\},?$",
            RegexOptions.Multiline
        );

    let Palette =
        Regex(@"(?<Size>[0-9]+):\[(?<Colors>[0-9'rgb\(\),]+)\]", RegexOptions.Compiled);

    let Color =
        Regex(@"'(?<Color>rgb\([0-9]{1,3},[0-9]{1,3},[0-9]{1,3}\))'", RegexOptions.Compiled)

    let RGB =
        Regex(@"^rgb\((?<R>[0-9]{1,3}),(?<G>[0-9]{1,3}),(?<B>[0-9]{1,3})\)$", RegexOptions.Compiled)

    let Property =
        Regex(@"'(?<Key>[a-zA-Z]+)':(?:'(?<Value>[a-zA-Z]+)'|\[(?<PerPaletteValue>[0-9,]+)\])", RegexOptions.Compiled);


type Severity =
    | Warning = 0
    | Error = 1

let messages = ResizeArray<Severity * string>()

let log severity fmt =
    Printf.kprintf (fun str -> messages.Add (severity, str)) fmt

let schemes =
    source |> Array.choose (fun str ->
        let m = Regex.Scheme.Match str

        if m.Success then
            let schemeName = m.Groups.["Name"].Value

            try
                let properties =
                    Regex.Property.Matches m.Groups.["Properties"].Value
                    |> Seq.map (fun p ->
                        let value =
                            if p.Groups.["Value"].Success then p.Groups.["Value"].Value
                            else p.Groups.["PerPaletteValue"].Value

                        p.Groups.["Key"].Value, value
                    )
                    |> Map.ofSeq

                let typ =
                    match properties.["type"] with
                    | "div"  -> ColorBrewer.SchemeType.Diverging
                    | "qual" -> ColorBrewer.SchemeType.Qualitative
                    | "seq"  -> ColorBrewer.SchemeType.Sequential
                    | t -> failwithf "Unknown scheme type '%s'" t

                let colors =
                    Regex.Palette.Matches m.Groups.["Palettes"].Value
                    |> Seq.map (fun p ->
                        let size = Int32.Parse p.Groups.["Size"].Value

                        let colors =
                            Regex.Color.Matches p.Groups.["Colors"].Value
                            |> Seq.map (fun m ->
                                let m = Regex.RGB.Match m.Groups.["Color"].Value
                                let r = m.Groups.["R"].Value
                                let g = m.Groups.["G"].Value
                                let b = m.Groups.["B"].Value
                                $"C3b({r}uy,{g}uy,{b}uy)"
                            )
                            |> Array.ofSeq

                        if colors.Length <> size then
                            failwithf "Size mismatch (expected %d but got %d)" size colors.Length

                        colors
                    )
                    |> Array.ofSeq

                let getFlags (propertyName : string) =
                    let values =
                        properties.[propertyName] |> String.split "," |> Array.map (fun str ->
                            Int32.Parse str = 1
                        )

                    if values.Length >= colors.Length then values
                    elif values.Length = 1 then Array.replicate colors.Length values.[0]
                    else
                        let missing = Array.replicate (colors.Length - values.Length) false
                        log Severity.Warning "Property '%s' in scheme '%s' has %d entries but expected %d." propertyName schemeName values.Length colors.Length
                        Array.concat [values; missing]

                let blind  = getFlags "blind"
                let print  = getFlags "print"
                let copy   = getFlags "copy"
                let screen = getFlags "screen"

                let palettes =
                    colors |> Array.mapi (fun i c ->
                        let mutable usage = Set.empty
                        if blind.[i] then usage <- usage |> Set.add ColorBrewer.PaletteUsage.ColorBlind
                        if print.[i] then usage <- usage |> Set.add ColorBrewer.PaletteUsage.Print
                        if copy.[i] then usage <- usage |> Set.add ColorBrewer.PaletteUsage.PhotoCopy
                        if screen.[i] then usage <- usage |> Set.add ColorBrewer.PaletteUsage.LCD

                        c.Length, {| Colors = c; Usage = usage |}
                    )
                    |> Map.ofArray

                Some {| Name = schemeName; Type = typ; Palettes = palettes |}

            with e ->
                log Severity.Error "Failed to parse scheme %s: %s" schemeName e.Message
                None
        else
            None
    )

printfn "Found %d schemes" schemes.Length

let writeToFile() =
    let builder = StringBuilder()

    let writeln indent fmt =
        Printf.kprintf (fun str -> builder.AppendLine(str |> String.indent indent) |> ignore) fmt

    let schemeTypeComments =
        Map.ofList [
            ColorBrewer.SchemeType.Diverging, "/// Diverging schemes put equal emphasis on mid-range critical values and extremes at both ends of the data range.\n/// The critical class or break in the middle of the legend is emphasized with light colors and low and high extremes are\n/// emphasized with dark colors that have contrasting hues."

            ColorBrewer.SchemeType.Qualitative, "/// Qualitative schemes do not imply magnitude differences between legend classes, and hues are used to\n/// create the primary visual differences between classes. Qualitative schemes are best suited to representing nominal or categorical data."

            ColorBrewer.SchemeType.Sequential, "/// Sequential schemes are suited to ordered data that progress from low to high.\n/// Lightness steps dominate the look of these schemes, with light colors for low data values to dark colors for high data values."
        ]

    writeln 0 "namespace Aardvark.Base"
    writeln 0 ""
    writeln 0 "[<AutoOpen>]"
    writeln 0 "module ColorBrewerSchemes ="
    writeln 1 "open ColorBrewer"
    writeln 0 ""
    writeln 1 "/// Brewer color schemes designed for chloropleth map visualizations."
    writeln 1 "module ColorBrewer ="
    writeln 0 ""
    writeln 2 "module Scheme ="
    writeln 0 ""

    let grouped =
        schemes |> Array.groupBy (fun s -> s.Type)

    for (typ, schemes) in grouped do

        for c in schemeTypeComments.[typ] |> String.getLines do
            writeln 3 "%s" c

        writeln 3 "module %A =" typ
        writeln 0 ""

        for s in schemes do
            writeln 4 "let %s =" s.Name
            writeln 5 "{"
            writeln 6 "Name = Sym.ofString \"%s\"" s.Name
            writeln 6 "Type = SchemeType.%A" s.Type
            writeln 6 "Palettes ="
            writeln 7 "MapExt.ofList ["

            for (KeyValue(n, p)) in s.Palettes do
                let usage =
                    if Set.isEmpty p.Usage then "PaletteUsage.None"
                    else p.Usage |> Seq.map (fun u -> $"PaletteUsage.{u}") |> String.concat " ||| "

                let colors =
                    Array.chunkBySize 4 p.Colors
                    |> Array.map (String.concat "; ")

                colors.[0] <- $"Colors = [| {colors.[0]}"
                colors.[colors.Length - 1] <- $"{colors.[colors.Length - 1]} |]"

                writeln 8 "%d, {" n
                writeln 9 "Usage = %s" usage

                writeln 9 "%s" colors.[0]
                for i = 1 to colors.Length - 1 do
                    writeln 12 "%s" colors.[i]

                writeln 8 "}"
                writeln 0 ""

            writeln 7 "]"
            writeln 5 "}"
            writeln 0 ""

    writeln 3 "/// Array of all available color schemes."
    writeln 3 "let All ="
    writeln 4 "[|"

    let all =
        grouped |> Array.collect (fun (typ, schemes) ->
            schemes |> Array.map (fun s -> $"{typ}.{s.Name}")
        )
        |> Array.chunkBySize 6
        |> Array.map (String.concat "; ")

    for schemes in all do
        writeln 5 "%s" schemes

    writeln 4 "|]"


    let file = Path.Combine(__SOURCE_DIRECTORY__, "ColorBrewerSchemes.fs")
    File.writeAllText file (builder.ToString())

    for s in schemes do
        printfn "%s" s.Name
        printfn "%A" s.Palettes

    for (s, e) in messages do
        printfn "[%A] %s" s e

writeToFile()