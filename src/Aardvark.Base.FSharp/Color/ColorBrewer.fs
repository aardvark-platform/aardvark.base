namespace Aardvark.Base

open System
open System.Collections
open System.Collections.Generic

/// Brewer color schemes designed for choropleth map visualizations.
module ColorBrewer =

    [<Flags>]
    type PaletteUsage =
        | None       = 0

        /// Does not confuse people with red-green color blindness.
        | ColorBlind = 1

        /// Suitable for desktop color printing.
        | Print      = 2

        /// Suitable for viewing on a laptop LCD display.
        /// Small, portable LCD monitors tend to wash-out colors which results in noticeable differences from computer-to-computer.
        | LCD        = 4

        /// Withstands black and white photocopying.
        /// Diverging schemes can not be photocopied successfully.
        /// Differences in lightness should be preserved with sequential schemes.
        | PhotoCopy  = 8

    [<Struct>]
    type Palette =
        {
            /// The color values of the palette.
            Colors : C3b[]

            /// Usage properties of the palette.
            Usage  : PaletteUsage
        }

        member inline x.Length =
            x.Colors.Length

        member inline x.Item (index : int) =
            x.Colors.[index]

        interface IEnumerable with
            member x.GetEnumerator() = x.Colors.GetEnumerator()

        interface IEnumerable<C3b> with
            member x.GetEnumerator() = (x.Colors :> IEnumerable<C3b>).GetEnumerator()


    type SchemeType =

        /// Diverging schemes put equal emphasis on mid-range critical values and extremes at both ends of the data range.
        /// The critical class or break in the middle of the legend is emphasized with light colors and low and high extremes are
        /// emphasized with dark colors that have contrasting hues.
        | Diverging   = 0

        /// Qualitative schemes do not imply magnitude differences between legend classes, and hues are used to
        /// create the primary visual differences between classes. Qualitative schemes are best suited to representing nominal or categorical data.
        | Qualitative = 1

        /// Sequential schemes are suited to ordered data that progress from low to high.
        /// Lightness steps dominate the look of these schemes, with light colors for low data values to dark colors for high data values.
        | Sequential  = 2

    /// A color scheme containing palettes of various size.
    [<Struct>]
    type Scheme =
        {
            /// Name of the scheme.
            Name     : Symbol

            /// Type of the scheme.
            Type     : SchemeType

            /// The palettes of the scheme according to their size.
            Palettes : MapExt<int, Palette>
        }

        /// Returns whether the scheme is empty (i.e. has no palettes).
        member inline x.IsEmpty =
            x.Palettes.IsEmpty

        /// Size of the smallest palette.
        member inline x.MinSize =
            x.Palettes.TryMinKeyV |> ValueOption.defaultValue 0

        /// Size of the largest palette.
        member inline x.MaxSize =
            x.Palettes.TryMaxKeyV |> ValueOption.defaultValue 0

        /// Gets the palette with the given size.
        /// If the scheme is not defined for the requested size, gets the next larger palette.
        /// Throws an exception if the requested size is greater than the maximum size.
        member inline x.Item (requestedSize : int) =
            match x.Palettes |> MapExt.neighboursV requestedSize with
            | struct (_, ValueSome (struct (_, palette)), _)
            | struct (_, _, ValueSome (struct (_, palette))) ->
                palette

            | struct (ValueSome (struct (max, _)), _, _) ->
                raise <| ArgumentOutOfRangeException("requestedSize", $"Scheme {x.Name} has a maximum palette size of {max} (requested {requestedSize}).")

            | struct (ValueNone, ValueNone, ValueNone) ->
                raise <| ArgumentException($"Scheme {x.Name} is empty.")

    [<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
    module Scheme =

        /// Returns whether the scheme is empty (i.e. has no palettes).
        let inline isEmpty (scheme : Scheme) =
            scheme.IsEmpty

        /// Return the size of the smallest palette for the given scheme.
        let inline minSize (scheme : Scheme) =
            scheme.MinSize

        /// Return the size of the largest palette for the given scheme.
        let inline maxSize (scheme : Scheme) =
            scheme.MaxSize

        /// Gets the palette with the given size.
        /// If the scheme is not defined for the requested size, gets the next larger palette.
        /// Throws an exception if the requested size is greater than the maximum size.
        let inline getPalette (requestedSize : int) (scheme : Scheme) =
            scheme.[requestedSize]

        /// Returns a new scheme containing only the palettes for which the predicate returns true.
        let inline filter (predicate : Palette -> bool) (scheme : Scheme) =
            { scheme with Palettes = scheme.Palettes |> MapExt.filter (fun _ -> predicate) }

        /// Returns a new scheme containing only the palettes with the given usage flags.
        let inline filterUsage (usage : PaletteUsage) (scheme : Scheme) =
            scheme |> filter (fun p -> p.Usage &&& usage = usage)