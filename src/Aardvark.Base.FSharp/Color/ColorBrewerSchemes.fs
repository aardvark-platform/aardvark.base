namespace Aardvark.Base

[<AutoOpen>]
module ColorBrewerSchemes =
    open ColorBrewer

    /// Brewer color schemes designed for chloropleth map visualizations.
    module ColorBrewer =

        module Scheme =

            /// Diverging schemes put equal emphasis on mid-range critical values and extremes at both ends of the data range.
            /// The critical class or break in the middle of the legend is emphasized with light colors and low and high extremes are
            /// emphasized with dark colors that have contrasting hues.
            module Diverging =

                let Spectral =
                    {
                        Name = Sym.ofString "Spectral"
                        Type = SchemeType.Diverging
                        Palettes =
                            MapExt.ofList [
                                3, {
                                    Usage = PaletteUsage.Print ||| PaletteUsage.LCD ||| PaletteUsage.PhotoCopy
                                    Colors = [| C3b(252uy,141uy,89uy); C3b(255uy,255uy,191uy); C3b(153uy,213uy,148uy) |]
                                }

                                4, {
                                    Usage = PaletteUsage.Print ||| PaletteUsage.LCD ||| PaletteUsage.PhotoCopy
                                    Colors = [| C3b(215uy,25uy,28uy); C3b(253uy,174uy,97uy); C3b(171uy,221uy,164uy); C3b(43uy,131uy,186uy) |]
                                }

                                5, {
                                    Usage = PaletteUsage.Print ||| PaletteUsage.PhotoCopy
                                    Colors = [| C3b(215uy,25uy,28uy); C3b(253uy,174uy,97uy); C3b(255uy,255uy,191uy); C3b(171uy,221uy,164uy)
                                                C3b(43uy,131uy,186uy) |]
                                }

                                6, {
                                    Usage = PaletteUsage.None
                                    Colors = [| C3b(213uy,62uy,79uy); C3b(252uy,141uy,89uy); C3b(254uy,224uy,139uy); C3b(230uy,245uy,152uy)
                                                C3b(153uy,213uy,148uy); C3b(50uy,136uy,189uy) |]
                                }

                                7, {
                                    Usage = PaletteUsage.None
                                    Colors = [| C3b(213uy,62uy,79uy); C3b(252uy,141uy,89uy); C3b(254uy,224uy,139uy); C3b(255uy,255uy,191uy)
                                                C3b(230uy,245uy,152uy); C3b(153uy,213uy,148uy); C3b(50uy,136uy,189uy) |]
                                }

                                8, {
                                    Usage = PaletteUsage.None
                                    Colors = [| C3b(213uy,62uy,79uy); C3b(244uy,109uy,67uy); C3b(253uy,174uy,97uy); C3b(254uy,224uy,139uy)
                                                C3b(230uy,245uy,152uy); C3b(171uy,221uy,164uy); C3b(102uy,194uy,165uy); C3b(50uy,136uy,189uy) |]
                                }

                                9, {
                                    Usage = PaletteUsage.None
                                    Colors = [| C3b(213uy,62uy,79uy); C3b(244uy,109uy,67uy); C3b(253uy,174uy,97uy); C3b(254uy,224uy,139uy)
                                                C3b(255uy,255uy,191uy); C3b(230uy,245uy,152uy); C3b(171uy,221uy,164uy); C3b(102uy,194uy,165uy)
                                                C3b(50uy,136uy,189uy) |]
                                }

                                10, {
                                    Usage = PaletteUsage.None
                                    Colors = [| C3b(158uy,1uy,66uy); C3b(213uy,62uy,79uy); C3b(244uy,109uy,67uy); C3b(253uy,174uy,97uy)
                                                C3b(254uy,224uy,139uy); C3b(230uy,245uy,152uy); C3b(171uy,221uy,164uy); C3b(102uy,194uy,165uy)
                                                C3b(50uy,136uy,189uy); C3b(94uy,79uy,162uy) |]
                                }

                                11, {
                                    Usage = PaletteUsage.None
                                    Colors = [| C3b(158uy,1uy,66uy); C3b(213uy,62uy,79uy); C3b(244uy,109uy,67uy); C3b(253uy,174uy,97uy)
                                                C3b(254uy,224uy,139uy); C3b(255uy,255uy,191uy); C3b(230uy,245uy,152uy); C3b(171uy,221uy,164uy)
                                                C3b(102uy,194uy,165uy); C3b(50uy,136uy,189uy); C3b(94uy,79uy,162uy) |]
                                }

                            ]
                    }

                let RdYlGn =
                    {
                        Name = Sym.ofString "RdYlGn"
                        Type = SchemeType.Diverging
                        Palettes =
                            MapExt.ofList [
                                3, {
                                    Usage = PaletteUsage.Print ||| PaletteUsage.LCD
                                    Colors = [| C3b(252uy,141uy,89uy); C3b(255uy,255uy,191uy); C3b(145uy,207uy,96uy) |]
                                }

                                4, {
                                    Usage = PaletteUsage.Print ||| PaletteUsage.LCD
                                    Colors = [| C3b(215uy,25uy,28uy); C3b(253uy,174uy,97uy); C3b(166uy,217uy,106uy); C3b(26uy,150uy,65uy) |]
                                }

                                5, {
                                    Usage = PaletteUsage.Print ||| PaletteUsage.LCD
                                    Colors = [| C3b(215uy,25uy,28uy); C3b(253uy,174uy,97uy); C3b(255uy,255uy,191uy); C3b(166uy,217uy,106uy)
                                                C3b(26uy,150uy,65uy) |]
                                }

                                6, {
                                    Usage = PaletteUsage.None
                                    Colors = [| C3b(215uy,48uy,39uy); C3b(252uy,141uy,89uy); C3b(254uy,224uy,139uy); C3b(217uy,239uy,139uy)
                                                C3b(145uy,207uy,96uy); C3b(26uy,152uy,80uy) |]
                                }

                                7, {
                                    Usage = PaletteUsage.None
                                    Colors = [| C3b(215uy,48uy,39uy); C3b(252uy,141uy,89uy); C3b(254uy,224uy,139uy); C3b(255uy,255uy,191uy)
                                                C3b(217uy,239uy,139uy); C3b(145uy,207uy,96uy); C3b(26uy,152uy,80uy) |]
                                }

                                8, {
                                    Usage = PaletteUsage.None
                                    Colors = [| C3b(215uy,48uy,39uy); C3b(244uy,109uy,67uy); C3b(253uy,174uy,97uy); C3b(254uy,224uy,139uy)
                                                C3b(217uy,239uy,139uy); C3b(166uy,217uy,106uy); C3b(102uy,189uy,99uy); C3b(26uy,152uy,80uy) |]
                                }

                                9, {
                                    Usage = PaletteUsage.None
                                    Colors = [| C3b(215uy,48uy,39uy); C3b(244uy,109uy,67uy); C3b(253uy,174uy,97uy); C3b(254uy,224uy,139uy)
                                                C3b(255uy,255uy,191uy); C3b(217uy,239uy,139uy); C3b(166uy,217uy,106uy); C3b(102uy,189uy,99uy)
                                                C3b(26uy,152uy,80uy) |]
                                }

                                10, {
                                    Usage = PaletteUsage.None
                                    Colors = [| C3b(165uy,0uy,38uy); C3b(215uy,48uy,39uy); C3b(244uy,109uy,67uy); C3b(253uy,174uy,97uy)
                                                C3b(254uy,224uy,139uy); C3b(217uy,239uy,139uy); C3b(166uy,217uy,106uy); C3b(102uy,189uy,99uy)
                                                C3b(26uy,152uy,80uy); C3b(0uy,104uy,55uy) |]
                                }

                                11, {
                                    Usage = PaletteUsage.None
                                    Colors = [| C3b(165uy,0uy,38uy); C3b(215uy,48uy,39uy); C3b(244uy,109uy,67uy); C3b(253uy,174uy,97uy)
                                                C3b(254uy,224uy,139uy); C3b(255uy,255uy,191uy); C3b(217uy,239uy,139uy); C3b(166uy,217uy,106uy)
                                                C3b(102uy,189uy,99uy); C3b(26uy,152uy,80uy); C3b(0uy,104uy,55uy) |]
                                }

                            ]
                    }

                let RdBu =
                    {
                        Name = Sym.ofString "RdBu"
                        Type = SchemeType.Diverging
                        Palettes =
                            MapExt.ofList [
                                3, {
                                    Usage = PaletteUsage.ColorBlind ||| PaletteUsage.Print ||| PaletteUsage.LCD
                                    Colors = [| C3b(239uy,138uy,98uy); C3b(247uy,247uy,247uy); C3b(103uy,169uy,207uy) |]
                                }

                                4, {
                                    Usage = PaletteUsage.ColorBlind ||| PaletteUsage.Print ||| PaletteUsage.LCD
                                    Colors = [| C3b(202uy,0uy,32uy); C3b(244uy,165uy,130uy); C3b(146uy,197uy,222uy); C3b(5uy,113uy,176uy) |]
                                }

                                5, {
                                    Usage = PaletteUsage.ColorBlind ||| PaletteUsage.Print ||| PaletteUsage.LCD
                                    Colors = [| C3b(202uy,0uy,32uy); C3b(244uy,165uy,130uy); C3b(247uy,247uy,247uy); C3b(146uy,197uy,222uy)
                                                C3b(5uy,113uy,176uy) |]
                                }

                                6, {
                                    Usage = PaletteUsage.ColorBlind ||| PaletteUsage.Print
                                    Colors = [| C3b(178uy,24uy,43uy); C3b(239uy,138uy,98uy); C3b(253uy,219uy,199uy); C3b(209uy,229uy,240uy)
                                                C3b(103uy,169uy,207uy); C3b(33uy,102uy,172uy) |]
                                }

                                7, {
                                    Usage = PaletteUsage.ColorBlind
                                    Colors = [| C3b(178uy,24uy,43uy); C3b(239uy,138uy,98uy); C3b(253uy,219uy,199uy); C3b(247uy,247uy,247uy)
                                                C3b(209uy,229uy,240uy); C3b(103uy,169uy,207uy); C3b(33uy,102uy,172uy) |]
                                }

                                8, {
                                    Usage = PaletteUsage.ColorBlind
                                    Colors = [| C3b(178uy,24uy,43uy); C3b(214uy,96uy,77uy); C3b(244uy,165uy,130uy); C3b(253uy,219uy,199uy)
                                                C3b(209uy,229uy,240uy); C3b(146uy,197uy,222uy); C3b(67uy,147uy,195uy); C3b(33uy,102uy,172uy) |]
                                }

                                9, {
                                    Usage = PaletteUsage.ColorBlind
                                    Colors = [| C3b(178uy,24uy,43uy); C3b(214uy,96uy,77uy); C3b(244uy,165uy,130uy); C3b(253uy,219uy,199uy)
                                                C3b(247uy,247uy,247uy); C3b(209uy,229uy,240uy); C3b(146uy,197uy,222uy); C3b(67uy,147uy,195uy)
                                                C3b(33uy,102uy,172uy) |]
                                }

                                10, {
                                    Usage = PaletteUsage.ColorBlind
                                    Colors = [| C3b(103uy,0uy,31uy); C3b(178uy,24uy,43uy); C3b(214uy,96uy,77uy); C3b(244uy,165uy,130uy)
                                                C3b(253uy,219uy,199uy); C3b(209uy,229uy,240uy); C3b(146uy,197uy,222uy); C3b(67uy,147uy,195uy)
                                                C3b(33uy,102uy,172uy); C3b(5uy,48uy,97uy) |]
                                }

                                11, {
                                    Usage = PaletteUsage.ColorBlind
                                    Colors = [| C3b(103uy,0uy,31uy); C3b(178uy,24uy,43uy); C3b(214uy,96uy,77uy); C3b(244uy,165uy,130uy)
                                                C3b(253uy,219uy,199uy); C3b(247uy,247uy,247uy); C3b(209uy,229uy,240uy); C3b(146uy,197uy,222uy)
                                                C3b(67uy,147uy,195uy); C3b(33uy,102uy,172uy); C3b(5uy,48uy,97uy) |]
                                }

                            ]
                    }

                let PiYG =
                    {
                        Name = Sym.ofString "PiYG"
                        Type = SchemeType.Diverging
                        Palettes =
                            MapExt.ofList [
                                3, {
                                    Usage = PaletteUsage.ColorBlind ||| PaletteUsage.Print ||| PaletteUsage.LCD
                                    Colors = [| C3b(233uy,163uy,201uy); C3b(247uy,247uy,247uy); C3b(161uy,215uy,106uy) |]
                                }

                                4, {
                                    Usage = PaletteUsage.ColorBlind ||| PaletteUsage.Print ||| PaletteUsage.LCD
                                    Colors = [| C3b(208uy,28uy,139uy); C3b(241uy,182uy,218uy); C3b(184uy,225uy,134uy); C3b(77uy,172uy,38uy) |]
                                }

                                5, {
                                    Usage = PaletteUsage.ColorBlind
                                    Colors = [| C3b(208uy,28uy,139uy); C3b(241uy,182uy,218uy); C3b(247uy,247uy,247uy); C3b(184uy,225uy,134uy)
                                                C3b(77uy,172uy,38uy) |]
                                }

                                6, {
                                    Usage = PaletteUsage.ColorBlind
                                    Colors = [| C3b(197uy,27uy,125uy); C3b(233uy,163uy,201uy); C3b(253uy,224uy,239uy); C3b(230uy,245uy,208uy)
                                                C3b(161uy,215uy,106uy); C3b(77uy,146uy,33uy) |]
                                }

                                7, {
                                    Usage = PaletteUsage.ColorBlind
                                    Colors = [| C3b(197uy,27uy,125uy); C3b(233uy,163uy,201uy); C3b(253uy,224uy,239uy); C3b(247uy,247uy,247uy)
                                                C3b(230uy,245uy,208uy); C3b(161uy,215uy,106uy); C3b(77uy,146uy,33uy) |]
                                }

                                8, {
                                    Usage = PaletteUsage.ColorBlind
                                    Colors = [| C3b(197uy,27uy,125uy); C3b(222uy,119uy,174uy); C3b(241uy,182uy,218uy); C3b(253uy,224uy,239uy)
                                                C3b(230uy,245uy,208uy); C3b(184uy,225uy,134uy); C3b(127uy,188uy,65uy); C3b(77uy,146uy,33uy) |]
                                }

                                9, {
                                    Usage = PaletteUsage.ColorBlind
                                    Colors = [| C3b(197uy,27uy,125uy); C3b(222uy,119uy,174uy); C3b(241uy,182uy,218uy); C3b(253uy,224uy,239uy)
                                                C3b(247uy,247uy,247uy); C3b(230uy,245uy,208uy); C3b(184uy,225uy,134uy); C3b(127uy,188uy,65uy)
                                                C3b(77uy,146uy,33uy) |]
                                }

                                10, {
                                    Usage = PaletteUsage.ColorBlind
                                    Colors = [| C3b(142uy,1uy,82uy); C3b(197uy,27uy,125uy); C3b(222uy,119uy,174uy); C3b(241uy,182uy,218uy)
                                                C3b(253uy,224uy,239uy); C3b(230uy,245uy,208uy); C3b(184uy,225uy,134uy); C3b(127uy,188uy,65uy)
                                                C3b(77uy,146uy,33uy); C3b(39uy,100uy,25uy) |]
                                }

                                11, {
                                    Usage = PaletteUsage.ColorBlind
                                    Colors = [| C3b(142uy,1uy,82uy); C3b(197uy,27uy,125uy); C3b(222uy,119uy,174uy); C3b(241uy,182uy,218uy)
                                                C3b(253uy,224uy,239uy); C3b(247uy,247uy,247uy); C3b(230uy,245uy,208uy); C3b(184uy,225uy,134uy)
                                                C3b(127uy,188uy,65uy); C3b(77uy,146uy,33uy); C3b(39uy,100uy,25uy) |]
                                }

                            ]
                    }

                let PRGn =
                    {
                        Name = Sym.ofString "PRGn"
                        Type = SchemeType.Diverging
                        Palettes =
                            MapExt.ofList [
                                3, {
                                    Usage = PaletteUsage.ColorBlind ||| PaletteUsage.Print ||| PaletteUsage.LCD
                                    Colors = [| C3b(175uy,141uy,195uy); C3b(247uy,247uy,247uy); C3b(127uy,191uy,123uy) |]
                                }

                                4, {
                                    Usage = PaletteUsage.ColorBlind ||| PaletteUsage.Print ||| PaletteUsage.LCD
                                    Colors = [| C3b(123uy,50uy,148uy); C3b(194uy,165uy,207uy); C3b(166uy,219uy,160uy); C3b(0uy,136uy,55uy) |]
                                }

                                5, {
                                    Usage = PaletteUsage.ColorBlind ||| PaletteUsage.Print
                                    Colors = [| C3b(123uy,50uy,148uy); C3b(194uy,165uy,207uy); C3b(247uy,247uy,247uy); C3b(166uy,219uy,160uy)
                                                C3b(0uy,136uy,55uy) |]
                                }

                                6, {
                                    Usage = PaletteUsage.ColorBlind ||| PaletteUsage.Print
                                    Colors = [| C3b(118uy,42uy,131uy); C3b(175uy,141uy,195uy); C3b(231uy,212uy,232uy); C3b(217uy,240uy,211uy)
                                                C3b(127uy,191uy,123uy); C3b(27uy,120uy,55uy) |]
                                }

                                7, {
                                    Usage = PaletteUsage.ColorBlind
                                    Colors = [| C3b(118uy,42uy,131uy); C3b(175uy,141uy,195uy); C3b(231uy,212uy,232uy); C3b(247uy,247uy,247uy)
                                                C3b(217uy,240uy,211uy); C3b(127uy,191uy,123uy); C3b(27uy,120uy,55uy) |]
                                }

                                8, {
                                    Usage = PaletteUsage.ColorBlind
                                    Colors = [| C3b(118uy,42uy,131uy); C3b(153uy,112uy,171uy); C3b(194uy,165uy,207uy); C3b(231uy,212uy,232uy)
                                                C3b(217uy,240uy,211uy); C3b(166uy,219uy,160uy); C3b(90uy,174uy,97uy); C3b(27uy,120uy,55uy) |]
                                }

                                9, {
                                    Usage = PaletteUsage.ColorBlind
                                    Colors = [| C3b(118uy,42uy,131uy); C3b(153uy,112uy,171uy); C3b(194uy,165uy,207uy); C3b(231uy,212uy,232uy)
                                                C3b(247uy,247uy,247uy); C3b(217uy,240uy,211uy); C3b(166uy,219uy,160uy); C3b(90uy,174uy,97uy)
                                                C3b(27uy,120uy,55uy) |]
                                }

                                10, {
                                    Usage = PaletteUsage.ColorBlind
                                    Colors = [| C3b(64uy,0uy,75uy); C3b(118uy,42uy,131uy); C3b(153uy,112uy,171uy); C3b(194uy,165uy,207uy)
                                                C3b(231uy,212uy,232uy); C3b(217uy,240uy,211uy); C3b(166uy,219uy,160uy); C3b(90uy,174uy,97uy)
                                                C3b(27uy,120uy,55uy); C3b(0uy,68uy,27uy) |]
                                }

                                11, {
                                    Usage = PaletteUsage.ColorBlind
                                    Colors = [| C3b(64uy,0uy,75uy); C3b(118uy,42uy,131uy); C3b(153uy,112uy,171uy); C3b(194uy,165uy,207uy)
                                                C3b(231uy,212uy,232uy); C3b(247uy,247uy,247uy); C3b(217uy,240uy,211uy); C3b(166uy,219uy,160uy)
                                                C3b(90uy,174uy,97uy); C3b(27uy,120uy,55uy); C3b(0uy,68uy,27uy) |]
                                }

                            ]
                    }

                let RdYlBu =
                    {
                        Name = Sym.ofString "RdYlBu"
                        Type = SchemeType.Diverging
                        Palettes =
                            MapExt.ofList [
                                3, {
                                    Usage = PaletteUsage.ColorBlind ||| PaletteUsage.Print ||| PaletteUsage.LCD
                                    Colors = [| C3b(252uy,141uy,89uy); C3b(255uy,255uy,191uy); C3b(145uy,191uy,219uy) |]
                                }

                                4, {
                                    Usage = PaletteUsage.ColorBlind ||| PaletteUsage.Print ||| PaletteUsage.LCD
                                    Colors = [| C3b(215uy,25uy,28uy); C3b(253uy,174uy,97uy); C3b(171uy,217uy,233uy); C3b(44uy,123uy,182uy) |]
                                }

                                5, {
                                    Usage = PaletteUsage.ColorBlind ||| PaletteUsage.Print ||| PaletteUsage.LCD
                                    Colors = [| C3b(215uy,25uy,28uy); C3b(253uy,174uy,97uy); C3b(255uy,255uy,191uy); C3b(171uy,217uy,233uy)
                                                C3b(44uy,123uy,182uy) |]
                                }

                                6, {
                                    Usage = PaletteUsage.ColorBlind ||| PaletteUsage.Print
                                    Colors = [| C3b(215uy,48uy,39uy); C3b(252uy,141uy,89uy); C3b(254uy,224uy,144uy); C3b(224uy,243uy,248uy)
                                                C3b(145uy,191uy,219uy); C3b(69uy,117uy,180uy) |]
                                }

                                7, {
                                    Usage = PaletteUsage.ColorBlind
                                    Colors = [| C3b(215uy,48uy,39uy); C3b(252uy,141uy,89uy); C3b(254uy,224uy,144uy); C3b(255uy,255uy,191uy)
                                                C3b(224uy,243uy,248uy); C3b(145uy,191uy,219uy); C3b(69uy,117uy,180uy) |]
                                }

                                8, {
                                    Usage = PaletteUsage.ColorBlind
                                    Colors = [| C3b(215uy,48uy,39uy); C3b(244uy,109uy,67uy); C3b(253uy,174uy,97uy); C3b(254uy,224uy,144uy)
                                                C3b(224uy,243uy,248uy); C3b(171uy,217uy,233uy); C3b(116uy,173uy,209uy); C3b(69uy,117uy,180uy) |]
                                }

                                9, {
                                    Usage = PaletteUsage.ColorBlind
                                    Colors = [| C3b(215uy,48uy,39uy); C3b(244uy,109uy,67uy); C3b(253uy,174uy,97uy); C3b(254uy,224uy,144uy)
                                                C3b(255uy,255uy,191uy); C3b(224uy,243uy,248uy); C3b(171uy,217uy,233uy); C3b(116uy,173uy,209uy)
                                                C3b(69uy,117uy,180uy) |]
                                }

                                10, {
                                    Usage = PaletteUsage.ColorBlind
                                    Colors = [| C3b(165uy,0uy,38uy); C3b(215uy,48uy,39uy); C3b(244uy,109uy,67uy); C3b(253uy,174uy,97uy)
                                                C3b(254uy,224uy,144uy); C3b(224uy,243uy,248uy); C3b(171uy,217uy,233uy); C3b(116uy,173uy,209uy)
                                                C3b(69uy,117uy,180uy); C3b(49uy,54uy,149uy) |]
                                }

                                11, {
                                    Usage = PaletteUsage.ColorBlind
                                    Colors = [| C3b(165uy,0uy,38uy); C3b(215uy,48uy,39uy); C3b(244uy,109uy,67uy); C3b(253uy,174uy,97uy)
                                                C3b(254uy,224uy,144uy); C3b(255uy,255uy,191uy); C3b(224uy,243uy,248uy); C3b(171uy,217uy,233uy)
                                                C3b(116uy,173uy,209uy); C3b(69uy,117uy,180uy); C3b(49uy,54uy,149uy) |]
                                }

                            ]
                    }

                let BrBG =
                    {
                        Name = Sym.ofString "BrBG"
                        Type = SchemeType.Diverging
                        Palettes =
                            MapExt.ofList [
                                3, {
                                    Usage = PaletteUsage.ColorBlind ||| PaletteUsage.Print ||| PaletteUsage.LCD
                                    Colors = [| C3b(216uy,179uy,101uy); C3b(245uy,245uy,245uy); C3b(90uy,180uy,172uy) |]
                                }

                                4, {
                                    Usage = PaletteUsage.ColorBlind ||| PaletteUsage.Print ||| PaletteUsage.LCD
                                    Colors = [| C3b(166uy,97uy,26uy); C3b(223uy,194uy,125uy); C3b(128uy,205uy,193uy); C3b(1uy,133uy,113uy) |]
                                }

                                5, {
                                    Usage = PaletteUsage.ColorBlind ||| PaletteUsage.Print ||| PaletteUsage.LCD
                                    Colors = [| C3b(166uy,97uy,26uy); C3b(223uy,194uy,125uy); C3b(245uy,245uy,245uy); C3b(128uy,205uy,193uy)
                                                C3b(1uy,133uy,113uy) |]
                                }

                                6, {
                                    Usage = PaletteUsage.ColorBlind ||| PaletteUsage.Print ||| PaletteUsage.LCD
                                    Colors = [| C3b(140uy,81uy,10uy); C3b(216uy,179uy,101uy); C3b(246uy,232uy,195uy); C3b(199uy,234uy,229uy)
                                                C3b(90uy,180uy,172uy); C3b(1uy,102uy,94uy) |]
                                }

                                7, {
                                    Usage = PaletteUsage.ColorBlind
                                    Colors = [| C3b(140uy,81uy,10uy); C3b(216uy,179uy,101uy); C3b(246uy,232uy,195uy); C3b(245uy,245uy,245uy)
                                                C3b(199uy,234uy,229uy); C3b(90uy,180uy,172uy); C3b(1uy,102uy,94uy) |]
                                }

                                8, {
                                    Usage = PaletteUsage.ColorBlind
                                    Colors = [| C3b(140uy,81uy,10uy); C3b(191uy,129uy,45uy); C3b(223uy,194uy,125uy); C3b(246uy,232uy,195uy)
                                                C3b(199uy,234uy,229uy); C3b(128uy,205uy,193uy); C3b(53uy,151uy,143uy); C3b(1uy,102uy,94uy) |]
                                }

                                9, {
                                    Usage = PaletteUsage.ColorBlind
                                    Colors = [| C3b(140uy,81uy,10uy); C3b(191uy,129uy,45uy); C3b(223uy,194uy,125uy); C3b(246uy,232uy,195uy)
                                                C3b(245uy,245uy,245uy); C3b(199uy,234uy,229uy); C3b(128uy,205uy,193uy); C3b(53uy,151uy,143uy)
                                                C3b(1uy,102uy,94uy) |]
                                }

                                10, {
                                    Usage = PaletteUsage.ColorBlind
                                    Colors = [| C3b(84uy,48uy,5uy); C3b(140uy,81uy,10uy); C3b(191uy,129uy,45uy); C3b(223uy,194uy,125uy)
                                                C3b(246uy,232uy,195uy); C3b(199uy,234uy,229uy); C3b(128uy,205uy,193uy); C3b(53uy,151uy,143uy)
                                                C3b(1uy,102uy,94uy); C3b(0uy,60uy,48uy) |]
                                }

                                11, {
                                    Usage = PaletteUsage.ColorBlind
                                    Colors = [| C3b(84uy,48uy,5uy); C3b(140uy,81uy,10uy); C3b(191uy,129uy,45uy); C3b(223uy,194uy,125uy)
                                                C3b(246uy,232uy,195uy); C3b(245uy,245uy,245uy); C3b(199uy,234uy,229uy); C3b(128uy,205uy,193uy)
                                                C3b(53uy,151uy,143uy); C3b(1uy,102uy,94uy); C3b(0uy,60uy,48uy) |]
                                }

                            ]
                    }

                let RdGy =
                    {
                        Name = Sym.ofString "RdGy"
                        Type = SchemeType.Diverging
                        Palettes =
                            MapExt.ofList [
                                3, {
                                    Usage = PaletteUsage.Print ||| PaletteUsage.LCD
                                    Colors = [| C3b(239uy,138uy,98uy); C3b(255uy,255uy,255uy); C3b(153uy,153uy,153uy) |]
                                }

                                4, {
                                    Usage = PaletteUsage.Print ||| PaletteUsage.LCD
                                    Colors = [| C3b(202uy,0uy,32uy); C3b(244uy,165uy,130uy); C3b(186uy,186uy,186uy); C3b(64uy,64uy,64uy) |]
                                }

                                5, {
                                    Usage = PaletteUsage.Print
                                    Colors = [| C3b(202uy,0uy,32uy); C3b(244uy,165uy,130uy); C3b(255uy,255uy,255uy); C3b(186uy,186uy,186uy)
                                                C3b(64uy,64uy,64uy) |]
                                }

                                6, {
                                    Usage = PaletteUsage.None
                                    Colors = [| C3b(178uy,24uy,43uy); C3b(239uy,138uy,98uy); C3b(253uy,219uy,199uy); C3b(224uy,224uy,224uy)
                                                C3b(153uy,153uy,153uy); C3b(77uy,77uy,77uy) |]
                                }

                                7, {
                                    Usage = PaletteUsage.None
                                    Colors = [| C3b(178uy,24uy,43uy); C3b(239uy,138uy,98uy); C3b(253uy,219uy,199uy); C3b(255uy,255uy,255uy)
                                                C3b(224uy,224uy,224uy); C3b(153uy,153uy,153uy); C3b(77uy,77uy,77uy) |]
                                }

                                8, {
                                    Usage = PaletteUsage.None
                                    Colors = [| C3b(178uy,24uy,43uy); C3b(214uy,96uy,77uy); C3b(244uy,165uy,130uy); C3b(253uy,219uy,199uy)
                                                C3b(224uy,224uy,224uy); C3b(186uy,186uy,186uy); C3b(135uy,135uy,135uy); C3b(77uy,77uy,77uy) |]
                                }

                                9, {
                                    Usage = PaletteUsage.None
                                    Colors = [| C3b(178uy,24uy,43uy); C3b(214uy,96uy,77uy); C3b(244uy,165uy,130uy); C3b(253uy,219uy,199uy)
                                                C3b(255uy,255uy,255uy); C3b(224uy,224uy,224uy); C3b(186uy,186uy,186uy); C3b(135uy,135uy,135uy)
                                                C3b(77uy,77uy,77uy) |]
                                }

                                10, {
                                    Usage = PaletteUsage.None
                                    Colors = [| C3b(103uy,0uy,31uy); C3b(178uy,24uy,43uy); C3b(214uy,96uy,77uy); C3b(244uy,165uy,130uy)
                                                C3b(253uy,219uy,199uy); C3b(224uy,224uy,224uy); C3b(186uy,186uy,186uy); C3b(135uy,135uy,135uy)
                                                C3b(77uy,77uy,77uy); C3b(26uy,26uy,26uy) |]
                                }

                                11, {
                                    Usage = PaletteUsage.None
                                    Colors = [| C3b(103uy,0uy,31uy); C3b(178uy,24uy,43uy); C3b(214uy,96uy,77uy); C3b(244uy,165uy,130uy)
                                                C3b(253uy,219uy,199uy); C3b(255uy,255uy,255uy); C3b(224uy,224uy,224uy); C3b(186uy,186uy,186uy)
                                                C3b(135uy,135uy,135uy); C3b(77uy,77uy,77uy); C3b(26uy,26uy,26uy) |]
                                }

                            ]
                    }

                let PuOr =
                    {
                        Name = Sym.ofString "PuOr"
                        Type = SchemeType.Diverging
                        Palettes =
                            MapExt.ofList [
                                3, {
                                    Usage = PaletteUsage.ColorBlind ||| PaletteUsage.Print ||| PaletteUsage.LCD ||| PaletteUsage.PhotoCopy
                                    Colors = [| C3b(241uy,163uy,64uy); C3b(247uy,247uy,247uy); C3b(153uy,142uy,195uy) |]
                                }

                                4, {
                                    Usage = PaletteUsage.ColorBlind ||| PaletteUsage.Print ||| PaletteUsage.LCD ||| PaletteUsage.PhotoCopy
                                    Colors = [| C3b(230uy,97uy,1uy); C3b(253uy,184uy,99uy); C3b(178uy,171uy,210uy); C3b(94uy,60uy,153uy) |]
                                }

                                5, {
                                    Usage = PaletteUsage.ColorBlind ||| PaletteUsage.LCD
                                    Colors = [| C3b(230uy,97uy,1uy); C3b(253uy,184uy,99uy); C3b(247uy,247uy,247uy); C3b(178uy,171uy,210uy)
                                                C3b(94uy,60uy,153uy) |]
                                }

                                6, {
                                    Usage = PaletteUsage.ColorBlind ||| PaletteUsage.LCD
                                    Colors = [| C3b(179uy,88uy,6uy); C3b(241uy,163uy,64uy); C3b(254uy,224uy,182uy); C3b(216uy,218uy,235uy)
                                                C3b(153uy,142uy,195uy); C3b(84uy,39uy,136uy) |]
                                }

                                7, {
                                    Usage = PaletteUsage.ColorBlind
                                    Colors = [| C3b(179uy,88uy,6uy); C3b(241uy,163uy,64uy); C3b(254uy,224uy,182uy); C3b(247uy,247uy,247uy)
                                                C3b(216uy,218uy,235uy); C3b(153uy,142uy,195uy); C3b(84uy,39uy,136uy) |]
                                }

                                8, {
                                    Usage = PaletteUsage.ColorBlind
                                    Colors = [| C3b(179uy,88uy,6uy); C3b(224uy,130uy,20uy); C3b(253uy,184uy,99uy); C3b(254uy,224uy,182uy)
                                                C3b(216uy,218uy,235uy); C3b(178uy,171uy,210uy); C3b(128uy,115uy,172uy); C3b(84uy,39uy,136uy) |]
                                }

                                9, {
                                    Usage = PaletteUsage.ColorBlind
                                    Colors = [| C3b(179uy,88uy,6uy); C3b(224uy,130uy,20uy); C3b(253uy,184uy,99uy); C3b(254uy,224uy,182uy)
                                                C3b(247uy,247uy,247uy); C3b(216uy,218uy,235uy); C3b(178uy,171uy,210uy); C3b(128uy,115uy,172uy)
                                                C3b(84uy,39uy,136uy) |]
                                }

                                10, {
                                    Usage = PaletteUsage.ColorBlind
                                    Colors = [| C3b(127uy,59uy,8uy); C3b(179uy,88uy,6uy); C3b(224uy,130uy,20uy); C3b(253uy,184uy,99uy)
                                                C3b(254uy,224uy,182uy); C3b(216uy,218uy,235uy); C3b(178uy,171uy,210uy); C3b(128uy,115uy,172uy)
                                                C3b(84uy,39uy,136uy); C3b(45uy,0uy,75uy) |]
                                }

                                11, {
                                    Usage = PaletteUsage.ColorBlind
                                    Colors = [| C3b(127uy,59uy,8uy); C3b(179uy,88uy,6uy); C3b(224uy,130uy,20uy); C3b(253uy,184uy,99uy)
                                                C3b(254uy,224uy,182uy); C3b(247uy,247uy,247uy); C3b(216uy,218uy,235uy); C3b(178uy,171uy,210uy)
                                                C3b(128uy,115uy,172uy); C3b(84uy,39uy,136uy); C3b(45uy,0uy,75uy) |]
                                }

                            ]
                    }

            /// Qualitative schemes do not imply magnitude differences between legend classes, and hues are used to
            /// create the primary visual differences between classes. Qualitative schemes are best suited to representing nominal or categorical data.
            module Qualitative =

                let Set2 =
                    {
                        Name = Sym.ofString "Set2"
                        Type = SchemeType.Qualitative
                        Palettes =
                            MapExt.ofList [
                                3, {
                                    Usage = PaletteUsage.ColorBlind ||| PaletteUsage.Print ||| PaletteUsage.LCD
                                    Colors = [| C3b(102uy,194uy,165uy); C3b(252uy,141uy,98uy); C3b(141uy,160uy,203uy) |]
                                }

                                4, {
                                    Usage = PaletteUsage.Print ||| PaletteUsage.LCD
                                    Colors = [| C3b(102uy,194uy,165uy); C3b(252uy,141uy,98uy); C3b(141uy,160uy,203uy); C3b(231uy,138uy,195uy) |]
                                }

                                5, {
                                    Usage = PaletteUsage.Print
                                    Colors = [| C3b(102uy,194uy,165uy); C3b(252uy,141uy,98uy); C3b(141uy,160uy,203uy); C3b(231uy,138uy,195uy)
                                                C3b(166uy,216uy,84uy) |]
                                }

                                6, {
                                    Usage = PaletteUsage.None
                                    Colors = [| C3b(102uy,194uy,165uy); C3b(252uy,141uy,98uy); C3b(141uy,160uy,203uy); C3b(231uy,138uy,195uy)
                                                C3b(166uy,216uy,84uy); C3b(255uy,217uy,47uy) |]
                                }

                                7, {
                                    Usage = PaletteUsage.None
                                    Colors = [| C3b(102uy,194uy,165uy); C3b(252uy,141uy,98uy); C3b(141uy,160uy,203uy); C3b(231uy,138uy,195uy)
                                                C3b(166uy,216uy,84uy); C3b(255uy,217uy,47uy); C3b(229uy,196uy,148uy) |]
                                }

                                8, {
                                    Usage = PaletteUsage.None
                                    Colors = [| C3b(102uy,194uy,165uy); C3b(252uy,141uy,98uy); C3b(141uy,160uy,203uy); C3b(231uy,138uy,195uy)
                                                C3b(166uy,216uy,84uy); C3b(255uy,217uy,47uy); C3b(229uy,196uy,148uy); C3b(179uy,179uy,179uy) |]
                                }

                            ]
                    }

                let Accent =
                    {
                        Name = Sym.ofString "Accent"
                        Type = SchemeType.Qualitative
                        Palettes =
                            MapExt.ofList [
                                3, {
                                    Usage = PaletteUsage.Print ||| PaletteUsage.LCD
                                    Colors = [| C3b(127uy,201uy,127uy); C3b(190uy,174uy,212uy); C3b(253uy,192uy,134uy) |]
                                }

                                4, {
                                    Usage = PaletteUsage.Print ||| PaletteUsage.LCD
                                    Colors = [| C3b(127uy,201uy,127uy); C3b(190uy,174uy,212uy); C3b(253uy,192uy,134uy); C3b(255uy,255uy,153uy) |]
                                }

                                5, {
                                    Usage = PaletteUsage.LCD
                                    Colors = [| C3b(127uy,201uy,127uy); C3b(190uy,174uy,212uy); C3b(253uy,192uy,134uy); C3b(255uy,255uy,153uy)
                                                C3b(56uy,108uy,176uy) |]
                                }

                                6, {
                                    Usage = PaletteUsage.None
                                    Colors = [| C3b(127uy,201uy,127uy); C3b(190uy,174uy,212uy); C3b(253uy,192uy,134uy); C3b(255uy,255uy,153uy)
                                                C3b(56uy,108uy,176uy); C3b(240uy,2uy,127uy) |]
                                }

                                7, {
                                    Usage = PaletteUsage.None
                                    Colors = [| C3b(127uy,201uy,127uy); C3b(190uy,174uy,212uy); C3b(253uy,192uy,134uy); C3b(255uy,255uy,153uy)
                                                C3b(56uy,108uy,176uy); C3b(240uy,2uy,127uy); C3b(191uy,91uy,23uy) |]
                                }

                                8, {
                                    Usage = PaletteUsage.None
                                    Colors = [| C3b(127uy,201uy,127uy); C3b(190uy,174uy,212uy); C3b(253uy,192uy,134uy); C3b(255uy,255uy,153uy)
                                                C3b(56uy,108uy,176uy); C3b(240uy,2uy,127uy); C3b(191uy,91uy,23uy); C3b(102uy,102uy,102uy) |]
                                }

                            ]
                    }

                let Set1 =
                    {
                        Name = Sym.ofString "Set1"
                        Type = SchemeType.Qualitative
                        Palettes =
                            MapExt.ofList [
                                3, {
                                    Usage = PaletteUsage.Print ||| PaletteUsage.LCD
                                    Colors = [| C3b(228uy,26uy,28uy); C3b(55uy,126uy,184uy); C3b(77uy,175uy,74uy) |]
                                }

                                4, {
                                    Usage = PaletteUsage.Print ||| PaletteUsage.LCD
                                    Colors = [| C3b(228uy,26uy,28uy); C3b(55uy,126uy,184uy); C3b(77uy,175uy,74uy); C3b(152uy,78uy,163uy) |]
                                }

                                5, {
                                    Usage = PaletteUsage.Print ||| PaletteUsage.LCD
                                    Colors = [| C3b(228uy,26uy,28uy); C3b(55uy,126uy,184uy); C3b(77uy,175uy,74uy); C3b(152uy,78uy,163uy)
                                                C3b(255uy,127uy,0uy) |]
                                }

                                6, {
                                    Usage = PaletteUsage.Print ||| PaletteUsage.LCD
                                    Colors = [| C3b(228uy,26uy,28uy); C3b(55uy,126uy,184uy); C3b(77uy,175uy,74uy); C3b(152uy,78uy,163uy)
                                                C3b(255uy,127uy,0uy); C3b(255uy,255uy,51uy) |]
                                }

                                7, {
                                    Usage = PaletteUsage.Print ||| PaletteUsage.LCD
                                    Colors = [| C3b(228uy,26uy,28uy); C3b(55uy,126uy,184uy); C3b(77uy,175uy,74uy); C3b(152uy,78uy,163uy)
                                                C3b(255uy,127uy,0uy); C3b(255uy,255uy,51uy); C3b(166uy,86uy,40uy) |]
                                }

                                8, {
                                    Usage = PaletteUsage.Print ||| PaletteUsage.LCD
                                    Colors = [| C3b(228uy,26uy,28uy); C3b(55uy,126uy,184uy); C3b(77uy,175uy,74uy); C3b(152uy,78uy,163uy)
                                                C3b(255uy,127uy,0uy); C3b(255uy,255uy,51uy); C3b(166uy,86uy,40uy); C3b(247uy,129uy,191uy) |]
                                }

                                9, {
                                    Usage = PaletteUsage.Print ||| PaletteUsage.LCD
                                    Colors = [| C3b(228uy,26uy,28uy); C3b(55uy,126uy,184uy); C3b(77uy,175uy,74uy); C3b(152uy,78uy,163uy)
                                                C3b(255uy,127uy,0uy); C3b(255uy,255uy,51uy); C3b(166uy,86uy,40uy); C3b(247uy,129uy,191uy)
                                                C3b(153uy,153uy,153uy) |]
                                }

                            ]
                    }

                let Set3 =
                    {
                        Name = Sym.ofString "Set3"
                        Type = SchemeType.Qualitative
                        Palettes =
                            MapExt.ofList [
                                3, {
                                    Usage = PaletteUsage.Print ||| PaletteUsage.LCD ||| PaletteUsage.PhotoCopy
                                    Colors = [| C3b(141uy,211uy,199uy); C3b(255uy,255uy,179uy); C3b(190uy,186uy,218uy) |]
                                }

                                4, {
                                    Usage = PaletteUsage.Print ||| PaletteUsage.LCD
                                    Colors = [| C3b(141uy,211uy,199uy); C3b(255uy,255uy,179uy); C3b(190uy,186uy,218uy); C3b(251uy,128uy,114uy) |]
                                }

                                5, {
                                    Usage = PaletteUsage.Print ||| PaletteUsage.LCD
                                    Colors = [| C3b(141uy,211uy,199uy); C3b(255uy,255uy,179uy); C3b(190uy,186uy,218uy); C3b(251uy,128uy,114uy)
                                                C3b(128uy,177uy,211uy) |]
                                }

                                6, {
                                    Usage = PaletteUsage.Print
                                    Colors = [| C3b(141uy,211uy,199uy); C3b(255uy,255uy,179uy); C3b(190uy,186uy,218uy); C3b(251uy,128uy,114uy)
                                                C3b(128uy,177uy,211uy); C3b(253uy,180uy,98uy) |]
                                }

                                7, {
                                    Usage = PaletteUsage.Print
                                    Colors = [| C3b(141uy,211uy,199uy); C3b(255uy,255uy,179uy); C3b(190uy,186uy,218uy); C3b(251uy,128uy,114uy)
                                                C3b(128uy,177uy,211uy); C3b(253uy,180uy,98uy); C3b(179uy,222uy,105uy) |]
                                }

                                8, {
                                    Usage = PaletteUsage.Print
                                    Colors = [| C3b(141uy,211uy,199uy); C3b(255uy,255uy,179uy); C3b(190uy,186uy,218uy); C3b(251uy,128uy,114uy)
                                                C3b(128uy,177uy,211uy); C3b(253uy,180uy,98uy); C3b(179uy,222uy,105uy); C3b(252uy,205uy,229uy) |]
                                }

                                9, {
                                    Usage = PaletteUsage.None
                                    Colors = [| C3b(141uy,211uy,199uy); C3b(255uy,255uy,179uy); C3b(190uy,186uy,218uy); C3b(251uy,128uy,114uy)
                                                C3b(128uy,177uy,211uy); C3b(253uy,180uy,98uy); C3b(179uy,222uy,105uy); C3b(252uy,205uy,229uy)
                                                C3b(217uy,217uy,217uy) |]
                                }

                                10, {
                                    Usage = PaletteUsage.None
                                    Colors = [| C3b(141uy,211uy,199uy); C3b(255uy,255uy,179uy); C3b(190uy,186uy,218uy); C3b(251uy,128uy,114uy)
                                                C3b(128uy,177uy,211uy); C3b(253uy,180uy,98uy); C3b(179uy,222uy,105uy); C3b(252uy,205uy,229uy)
                                                C3b(217uy,217uy,217uy); C3b(188uy,128uy,189uy) |]
                                }

                                11, {
                                    Usage = PaletteUsage.None
                                    Colors = [| C3b(141uy,211uy,199uy); C3b(255uy,255uy,179uy); C3b(190uy,186uy,218uy); C3b(251uy,128uy,114uy)
                                                C3b(128uy,177uy,211uy); C3b(253uy,180uy,98uy); C3b(179uy,222uy,105uy); C3b(252uy,205uy,229uy)
                                                C3b(217uy,217uy,217uy); C3b(188uy,128uy,189uy); C3b(204uy,235uy,197uy) |]
                                }

                                12, {
                                    Usage = PaletteUsage.None
                                    Colors = [| C3b(141uy,211uy,199uy); C3b(255uy,255uy,179uy); C3b(190uy,186uy,218uy); C3b(251uy,128uy,114uy)
                                                C3b(128uy,177uy,211uy); C3b(253uy,180uy,98uy); C3b(179uy,222uy,105uy); C3b(252uy,205uy,229uy)
                                                C3b(217uy,217uy,217uy); C3b(188uy,128uy,189uy); C3b(204uy,235uy,197uy); C3b(255uy,237uy,111uy) |]
                                }

                            ]
                    }

                let Dark2 =
                    {
                        Name = Sym.ofString "Dark2"
                        Type = SchemeType.Qualitative
                        Palettes =
                            MapExt.ofList [
                                3, {
                                    Usage = PaletteUsage.ColorBlind ||| PaletteUsage.Print ||| PaletteUsage.LCD
                                    Colors = [| C3b(27uy,158uy,119uy); C3b(217uy,95uy,2uy); C3b(117uy,112uy,179uy) |]
                                }

                                4, {
                                    Usage = PaletteUsage.Print ||| PaletteUsage.LCD
                                    Colors = [| C3b(27uy,158uy,119uy); C3b(217uy,95uy,2uy); C3b(117uy,112uy,179uy); C3b(231uy,41uy,138uy) |]
                                }

                                5, {
                                    Usage = PaletteUsage.Print ||| PaletteUsage.LCD
                                    Colors = [| C3b(27uy,158uy,119uy); C3b(217uy,95uy,2uy); C3b(117uy,112uy,179uy); C3b(231uy,41uy,138uy)
                                                C3b(102uy,166uy,30uy) |]
                                }

                                6, {
                                    Usage = PaletteUsage.Print ||| PaletteUsage.LCD
                                    Colors = [| C3b(27uy,158uy,119uy); C3b(217uy,95uy,2uy); C3b(117uy,112uy,179uy); C3b(231uy,41uy,138uy)
                                                C3b(102uy,166uy,30uy); C3b(230uy,171uy,2uy) |]
                                }

                                7, {
                                    Usage = PaletteUsage.Print ||| PaletteUsage.LCD
                                    Colors = [| C3b(27uy,158uy,119uy); C3b(217uy,95uy,2uy); C3b(117uy,112uy,179uy); C3b(231uy,41uy,138uy)
                                                C3b(102uy,166uy,30uy); C3b(230uy,171uy,2uy); C3b(166uy,118uy,29uy) |]
                                }

                                8, {
                                    Usage = PaletteUsage.Print ||| PaletteUsage.LCD
                                    Colors = [| C3b(27uy,158uy,119uy); C3b(217uy,95uy,2uy); C3b(117uy,112uy,179uy); C3b(231uy,41uy,138uy)
                                                C3b(102uy,166uy,30uy); C3b(230uy,171uy,2uy); C3b(166uy,118uy,29uy); C3b(102uy,102uy,102uy) |]
                                }

                            ]
                    }

                let Paired =
                    {
                        Name = Sym.ofString "Paired"
                        Type = SchemeType.Qualitative
                        Palettes =
                            MapExt.ofList [
                                3, {
                                    Usage = PaletteUsage.ColorBlind ||| PaletteUsage.Print ||| PaletteUsage.LCD
                                    Colors = [| C3b(166uy,206uy,227uy); C3b(31uy,120uy,180uy); C3b(178uy,223uy,138uy) |]
                                }

                                4, {
                                    Usage = PaletteUsage.ColorBlind ||| PaletteUsage.Print ||| PaletteUsage.LCD
                                    Colors = [| C3b(166uy,206uy,227uy); C3b(31uy,120uy,180uy); C3b(178uy,223uy,138uy); C3b(51uy,160uy,44uy) |]
                                }

                                5, {
                                    Usage = PaletteUsage.Print ||| PaletteUsage.LCD
                                    Colors = [| C3b(166uy,206uy,227uy); C3b(31uy,120uy,180uy); C3b(178uy,223uy,138uy); C3b(51uy,160uy,44uy)
                                                C3b(251uy,154uy,153uy) |]
                                }

                                6, {
                                    Usage = PaletteUsage.Print ||| PaletteUsage.LCD
                                    Colors = [| C3b(166uy,206uy,227uy); C3b(31uy,120uy,180uy); C3b(178uy,223uy,138uy); C3b(51uy,160uy,44uy)
                                                C3b(251uy,154uy,153uy); C3b(227uy,26uy,28uy) |]
                                }

                                7, {
                                    Usage = PaletteUsage.Print ||| PaletteUsage.LCD
                                    Colors = [| C3b(166uy,206uy,227uy); C3b(31uy,120uy,180uy); C3b(178uy,223uy,138uy); C3b(51uy,160uy,44uy)
                                                C3b(251uy,154uy,153uy); C3b(227uy,26uy,28uy); C3b(253uy,191uy,111uy) |]
                                }

                                8, {
                                    Usage = PaletteUsage.LCD
                                    Colors = [| C3b(166uy,206uy,227uy); C3b(31uy,120uy,180uy); C3b(178uy,223uy,138uy); C3b(51uy,160uy,44uy)
                                                C3b(251uy,154uy,153uy); C3b(227uy,26uy,28uy); C3b(253uy,191uy,111uy); C3b(255uy,127uy,0uy) |]
                                }

                                9, {
                                    Usage = PaletteUsage.LCD
                                    Colors = [| C3b(166uy,206uy,227uy); C3b(31uy,120uy,180uy); C3b(178uy,223uy,138uy); C3b(51uy,160uy,44uy)
                                                C3b(251uy,154uy,153uy); C3b(227uy,26uy,28uy); C3b(253uy,191uy,111uy); C3b(255uy,127uy,0uy)
                                                C3b(202uy,178uy,214uy) |]
                                }

                                10, {
                                    Usage = PaletteUsage.LCD
                                    Colors = [| C3b(166uy,206uy,227uy); C3b(31uy,120uy,180uy); C3b(178uy,223uy,138uy); C3b(51uy,160uy,44uy)
                                                C3b(251uy,154uy,153uy); C3b(227uy,26uy,28uy); C3b(253uy,191uy,111uy); C3b(255uy,127uy,0uy)
                                                C3b(202uy,178uy,214uy); C3b(106uy,61uy,154uy) |]
                                }

                                11, {
                                    Usage = PaletteUsage.None
                                    Colors = [| C3b(166uy,206uy,227uy); C3b(31uy,120uy,180uy); C3b(178uy,223uy,138uy); C3b(51uy,160uy,44uy)
                                                C3b(251uy,154uy,153uy); C3b(227uy,26uy,28uy); C3b(253uy,191uy,111uy); C3b(255uy,127uy,0uy)
                                                C3b(202uy,178uy,214uy); C3b(106uy,61uy,154uy); C3b(255uy,255uy,153uy) |]
                                }

                                12, {
                                    Usage = PaletteUsage.None
                                    Colors = [| C3b(166uy,206uy,227uy); C3b(31uy,120uy,180uy); C3b(178uy,223uy,138uy); C3b(51uy,160uy,44uy)
                                                C3b(251uy,154uy,153uy); C3b(227uy,26uy,28uy); C3b(253uy,191uy,111uy); C3b(255uy,127uy,0uy)
                                                C3b(202uy,178uy,214uy); C3b(106uy,61uy,154uy); C3b(255uy,255uy,153uy); C3b(177uy,89uy,40uy) |]
                                }

                            ]
                    }

                let Pastel2 =
                    {
                        Name = Sym.ofString "Pastel2"
                        Type = SchemeType.Qualitative
                        Palettes =
                            MapExt.ofList [
                                3, {
                                    Usage = PaletteUsage.None
                                    Colors = [| C3b(179uy,226uy,205uy); C3b(253uy,205uy,172uy); C3b(203uy,213uy,232uy) |]
                                }

                                4, {
                                    Usage = PaletteUsage.None
                                    Colors = [| C3b(179uy,226uy,205uy); C3b(253uy,205uy,172uy); C3b(203uy,213uy,232uy); C3b(244uy,202uy,228uy) |]
                                }

                                5, {
                                    Usage = PaletteUsage.None
                                    Colors = [| C3b(179uy,226uy,205uy); C3b(253uy,205uy,172uy); C3b(203uy,213uy,232uy); C3b(244uy,202uy,228uy)
                                                C3b(230uy,245uy,201uy) |]
                                }

                                6, {
                                    Usage = PaletteUsage.None
                                    Colors = [| C3b(179uy,226uy,205uy); C3b(253uy,205uy,172uy); C3b(203uy,213uy,232uy); C3b(244uy,202uy,228uy)
                                                C3b(230uy,245uy,201uy); C3b(255uy,242uy,174uy) |]
                                }

                                7, {
                                    Usage = PaletteUsage.None
                                    Colors = [| C3b(179uy,226uy,205uy); C3b(253uy,205uy,172uy); C3b(203uy,213uy,232uy); C3b(244uy,202uy,228uy)
                                                C3b(230uy,245uy,201uy); C3b(255uy,242uy,174uy); C3b(241uy,226uy,204uy) |]
                                }

                                8, {
                                    Usage = PaletteUsage.None
                                    Colors = [| C3b(179uy,226uy,205uy); C3b(253uy,205uy,172uy); C3b(203uy,213uy,232uy); C3b(244uy,202uy,228uy)
                                                C3b(230uy,245uy,201uy); C3b(255uy,242uy,174uy); C3b(241uy,226uy,204uy); C3b(204uy,204uy,204uy) |]
                                }

                            ]
                    }

                let Pastel1 =
                    {
                        Name = Sym.ofString "Pastel1"
                        Type = SchemeType.Qualitative
                        Palettes =
                            MapExt.ofList [
                                3, {
                                    Usage = PaletteUsage.None
                                    Colors = [| C3b(251uy,180uy,174uy); C3b(179uy,205uy,227uy); C3b(204uy,235uy,197uy) |]
                                }

                                4, {
                                    Usage = PaletteUsage.None
                                    Colors = [| C3b(251uy,180uy,174uy); C3b(179uy,205uy,227uy); C3b(204uy,235uy,197uy); C3b(222uy,203uy,228uy) |]
                                }

                                5, {
                                    Usage = PaletteUsage.None
                                    Colors = [| C3b(251uy,180uy,174uy); C3b(179uy,205uy,227uy); C3b(204uy,235uy,197uy); C3b(222uy,203uy,228uy)
                                                C3b(254uy,217uy,166uy) |]
                                }

                                6, {
                                    Usage = PaletteUsage.None
                                    Colors = [| C3b(251uy,180uy,174uy); C3b(179uy,205uy,227uy); C3b(204uy,235uy,197uy); C3b(222uy,203uy,228uy)
                                                C3b(254uy,217uy,166uy); C3b(255uy,255uy,204uy) |]
                                }

                                7, {
                                    Usage = PaletteUsage.None
                                    Colors = [| C3b(251uy,180uy,174uy); C3b(179uy,205uy,227uy); C3b(204uy,235uy,197uy); C3b(222uy,203uy,228uy)
                                                C3b(254uy,217uy,166uy); C3b(255uy,255uy,204uy); C3b(229uy,216uy,189uy) |]
                                }

                                8, {
                                    Usage = PaletteUsage.None
                                    Colors = [| C3b(251uy,180uy,174uy); C3b(179uy,205uy,227uy); C3b(204uy,235uy,197uy); C3b(222uy,203uy,228uy)
                                                C3b(254uy,217uy,166uy); C3b(255uy,255uy,204uy); C3b(229uy,216uy,189uy); C3b(253uy,218uy,236uy) |]
                                }

                                9, {
                                    Usage = PaletteUsage.None
                                    Colors = [| C3b(251uy,180uy,174uy); C3b(179uy,205uy,227uy); C3b(204uy,235uy,197uy); C3b(222uy,203uy,228uy)
                                                C3b(254uy,217uy,166uy); C3b(255uy,255uy,204uy); C3b(229uy,216uy,189uy); C3b(253uy,218uy,236uy)
                                                C3b(242uy,242uy,242uy) |]
                                }

                            ]
                    }

            /// Sequential schemes are suited to ordered data that progress from low to high.
            /// Lightness steps dominate the look of these schemes, with light colors for low data values to dark colors for high data values.
            module Sequential =

                let OrRd =
                    {
                        Name = Sym.ofString "OrRd"
                        Type = SchemeType.Sequential
                        Palettes =
                            MapExt.ofList [
                                3, {
                                    Usage = PaletteUsage.ColorBlind ||| PaletteUsage.Print ||| PaletteUsage.LCD ||| PaletteUsage.PhotoCopy
                                    Colors = [| C3b(254uy,232uy,200uy); C3b(253uy,187uy,132uy); C3b(227uy,74uy,51uy) |]
                                }

                                4, {
                                    Usage = PaletteUsage.ColorBlind ||| PaletteUsage.Print ||| PaletteUsage.LCD ||| PaletteUsage.PhotoCopy
                                    Colors = [| C3b(254uy,240uy,217uy); C3b(253uy,204uy,138uy); C3b(252uy,141uy,89uy); C3b(215uy,48uy,31uy) |]
                                }

                                5, {
                                    Usage = PaletteUsage.ColorBlind ||| PaletteUsage.LCD
                                    Colors = [| C3b(254uy,240uy,217uy); C3b(253uy,204uy,138uy); C3b(252uy,141uy,89uy); C3b(227uy,74uy,51uy)
                                                C3b(179uy,0uy,0uy) |]
                                }

                                6, {
                                    Usage = PaletteUsage.ColorBlind
                                    Colors = [| C3b(254uy,240uy,217uy); C3b(253uy,212uy,158uy); C3b(253uy,187uy,132uy); C3b(252uy,141uy,89uy)
                                                C3b(227uy,74uy,51uy); C3b(179uy,0uy,0uy) |]
                                }

                                7, {
                                    Usage = PaletteUsage.ColorBlind
                                    Colors = [| C3b(254uy,240uy,217uy); C3b(253uy,212uy,158uy); C3b(253uy,187uy,132uy); C3b(252uy,141uy,89uy)
                                                C3b(239uy,101uy,72uy); C3b(215uy,48uy,31uy); C3b(153uy,0uy,0uy) |]
                                }

                                8, {
                                    Usage = PaletteUsage.ColorBlind
                                    Colors = [| C3b(255uy,247uy,236uy); C3b(254uy,232uy,200uy); C3b(253uy,212uy,158uy); C3b(253uy,187uy,132uy)
                                                C3b(252uy,141uy,89uy); C3b(239uy,101uy,72uy); C3b(215uy,48uy,31uy); C3b(153uy,0uy,0uy) |]
                                }

                                9, {
                                    Usage = PaletteUsage.ColorBlind
                                    Colors = [| C3b(255uy,247uy,236uy); C3b(254uy,232uy,200uy); C3b(253uy,212uy,158uy); C3b(253uy,187uy,132uy)
                                                C3b(252uy,141uy,89uy); C3b(239uy,101uy,72uy); C3b(215uy,48uy,31uy); C3b(179uy,0uy,0uy)
                                                C3b(127uy,0uy,0uy) |]
                                }

                            ]
                    }

                let PuBu =
                    {
                        Name = Sym.ofString "PuBu"
                        Type = SchemeType.Sequential
                        Palettes =
                            MapExt.ofList [
                                3, {
                                    Usage = PaletteUsage.ColorBlind ||| PaletteUsage.Print ||| PaletteUsage.LCD ||| PaletteUsage.PhotoCopy
                                    Colors = [| C3b(236uy,231uy,242uy); C3b(166uy,189uy,219uy); C3b(43uy,140uy,190uy) |]
                                }

                                4, {
                                    Usage = PaletteUsage.ColorBlind ||| PaletteUsage.LCD
                                    Colors = [| C3b(241uy,238uy,246uy); C3b(189uy,201uy,225uy); C3b(116uy,169uy,207uy); C3b(5uy,112uy,176uy) |]
                                }

                                5, {
                                    Usage = PaletteUsage.ColorBlind
                                    Colors = [| C3b(241uy,238uy,246uy); C3b(189uy,201uy,225uy); C3b(116uy,169uy,207uy); C3b(43uy,140uy,190uy)
                                                C3b(4uy,90uy,141uy) |]
                                }

                                6, {
                                    Usage = PaletteUsage.ColorBlind
                                    Colors = [| C3b(241uy,238uy,246uy); C3b(208uy,209uy,230uy); C3b(166uy,189uy,219uy); C3b(116uy,169uy,207uy)
                                                C3b(43uy,140uy,190uy); C3b(4uy,90uy,141uy) |]
                                }

                                7, {
                                    Usage = PaletteUsage.ColorBlind
                                    Colors = [| C3b(241uy,238uy,246uy); C3b(208uy,209uy,230uy); C3b(166uy,189uy,219uy); C3b(116uy,169uy,207uy)
                                                C3b(54uy,144uy,192uy); C3b(5uy,112uy,176uy); C3b(3uy,78uy,123uy) |]
                                }

                                8, {
                                    Usage = PaletteUsage.ColorBlind
                                    Colors = [| C3b(255uy,247uy,251uy); C3b(236uy,231uy,242uy); C3b(208uy,209uy,230uy); C3b(166uy,189uy,219uy)
                                                C3b(116uy,169uy,207uy); C3b(54uy,144uy,192uy); C3b(5uy,112uy,176uy); C3b(3uy,78uy,123uy) |]
                                }

                                9, {
                                    Usage = PaletteUsage.ColorBlind
                                    Colors = [| C3b(255uy,247uy,251uy); C3b(236uy,231uy,242uy); C3b(208uy,209uy,230uy); C3b(166uy,189uy,219uy)
                                                C3b(116uy,169uy,207uy); C3b(54uy,144uy,192uy); C3b(5uy,112uy,176uy); C3b(4uy,90uy,141uy)
                                                C3b(2uy,56uy,88uy) |]
                                }

                            ]
                    }

                let BuPu =
                    {
                        Name = Sym.ofString "BuPu"
                        Type = SchemeType.Sequential
                        Palettes =
                            MapExt.ofList [
                                3, {
                                    Usage = PaletteUsage.ColorBlind ||| PaletteUsage.Print ||| PaletteUsage.LCD ||| PaletteUsage.PhotoCopy
                                    Colors = [| C3b(224uy,236uy,244uy); C3b(158uy,188uy,218uy); C3b(136uy,86uy,167uy) |]
                                }

                                4, {
                                    Usage = PaletteUsage.ColorBlind ||| PaletteUsage.Print ||| PaletteUsage.LCD
                                    Colors = [| C3b(237uy,248uy,251uy); C3b(179uy,205uy,227uy); C3b(140uy,150uy,198uy); C3b(136uy,65uy,157uy) |]
                                }

                                5, {
                                    Usage = PaletteUsage.ColorBlind ||| PaletteUsage.LCD
                                    Colors = [| C3b(237uy,248uy,251uy); C3b(179uy,205uy,227uy); C3b(140uy,150uy,198uy); C3b(136uy,86uy,167uy)
                                                C3b(129uy,15uy,124uy) |]
                                }

                                6, {
                                    Usage = PaletteUsage.ColorBlind
                                    Colors = [| C3b(237uy,248uy,251uy); C3b(191uy,211uy,230uy); C3b(158uy,188uy,218uy); C3b(140uy,150uy,198uy)
                                                C3b(136uy,86uy,167uy); C3b(129uy,15uy,124uy) |]
                                }

                                7, {
                                    Usage = PaletteUsage.ColorBlind
                                    Colors = [| C3b(237uy,248uy,251uy); C3b(191uy,211uy,230uy); C3b(158uy,188uy,218uy); C3b(140uy,150uy,198uy)
                                                C3b(140uy,107uy,177uy); C3b(136uy,65uy,157uy); C3b(110uy,1uy,107uy) |]
                                }

                                8, {
                                    Usage = PaletteUsage.ColorBlind
                                    Colors = [| C3b(247uy,252uy,253uy); C3b(224uy,236uy,244uy); C3b(191uy,211uy,230uy); C3b(158uy,188uy,218uy)
                                                C3b(140uy,150uy,198uy); C3b(140uy,107uy,177uy); C3b(136uy,65uy,157uy); C3b(110uy,1uy,107uy) |]
                                }

                                9, {
                                    Usage = PaletteUsage.ColorBlind
                                    Colors = [| C3b(247uy,252uy,253uy); C3b(224uy,236uy,244uy); C3b(191uy,211uy,230uy); C3b(158uy,188uy,218uy)
                                                C3b(140uy,150uy,198uy); C3b(140uy,107uy,177uy); C3b(136uy,65uy,157uy); C3b(129uy,15uy,124uy)
                                                C3b(77uy,0uy,75uy) |]
                                }

                            ]
                    }

                let Oranges =
                    {
                        Name = Sym.ofString "Oranges"
                        Type = SchemeType.Sequential
                        Palettes =
                            MapExt.ofList [
                                3, {
                                    Usage = PaletteUsage.ColorBlind ||| PaletteUsage.Print ||| PaletteUsage.LCD ||| PaletteUsage.PhotoCopy
                                    Colors = [| C3b(254uy,230uy,206uy); C3b(253uy,174uy,107uy); C3b(230uy,85uy,13uy) |]
                                }

                                4, {
                                    Usage = PaletteUsage.ColorBlind ||| PaletteUsage.LCD
                                    Colors = [| C3b(254uy,237uy,222uy); C3b(253uy,190uy,133uy); C3b(253uy,141uy,60uy); C3b(217uy,71uy,1uy) |]
                                }

                                5, {
                                    Usage = PaletteUsage.ColorBlind ||| PaletteUsage.LCD
                                    Colors = [| C3b(254uy,237uy,222uy); C3b(253uy,190uy,133uy); C3b(253uy,141uy,60uy); C3b(230uy,85uy,13uy)
                                                C3b(166uy,54uy,3uy) |]
                                }

                                6, {
                                    Usage = PaletteUsage.ColorBlind
                                    Colors = [| C3b(254uy,237uy,222uy); C3b(253uy,208uy,162uy); C3b(253uy,174uy,107uy); C3b(253uy,141uy,60uy)
                                                C3b(230uy,85uy,13uy); C3b(166uy,54uy,3uy) |]
                                }

                                7, {
                                    Usage = PaletteUsage.ColorBlind
                                    Colors = [| C3b(254uy,237uy,222uy); C3b(253uy,208uy,162uy); C3b(253uy,174uy,107uy); C3b(253uy,141uy,60uy)
                                                C3b(241uy,105uy,19uy); C3b(217uy,72uy,1uy); C3b(140uy,45uy,4uy) |]
                                }

                                8, {
                                    Usage = PaletteUsage.ColorBlind
                                    Colors = [| C3b(255uy,245uy,235uy); C3b(254uy,230uy,206uy); C3b(253uy,208uy,162uy); C3b(253uy,174uy,107uy)
                                                C3b(253uy,141uy,60uy); C3b(241uy,105uy,19uy); C3b(217uy,72uy,1uy); C3b(140uy,45uy,4uy) |]
                                }

                                9, {
                                    Usage = PaletteUsage.ColorBlind
                                    Colors = [| C3b(255uy,245uy,235uy); C3b(254uy,230uy,206uy); C3b(253uy,208uy,162uy); C3b(253uy,174uy,107uy)
                                                C3b(253uy,141uy,60uy); C3b(241uy,105uy,19uy); C3b(217uy,72uy,1uy); C3b(166uy,54uy,3uy)
                                                C3b(127uy,39uy,4uy) |]
                                }

                            ]
                    }

                let BuGn =
                    {
                        Name = Sym.ofString "BuGn"
                        Type = SchemeType.Sequential
                        Palettes =
                            MapExt.ofList [
                                3, {
                                    Usage = PaletteUsage.ColorBlind ||| PaletteUsage.Print ||| PaletteUsage.LCD ||| PaletteUsage.PhotoCopy
                                    Colors = [| C3b(229uy,245uy,249uy); C3b(153uy,216uy,201uy); C3b(44uy,162uy,95uy) |]
                                }

                                4, {
                                    Usage = PaletteUsage.ColorBlind ||| PaletteUsage.Print
                                    Colors = [| C3b(237uy,248uy,251uy); C3b(178uy,226uy,226uy); C3b(102uy,194uy,164uy); C3b(35uy,139uy,69uy) |]
                                }

                                5, {
                                    Usage = PaletteUsage.ColorBlind
                                    Colors = [| C3b(237uy,248uy,251uy); C3b(178uy,226uy,226uy); C3b(102uy,194uy,164uy); C3b(44uy,162uy,95uy)
                                                C3b(0uy,109uy,44uy) |]
                                }

                                6, {
                                    Usage = PaletteUsage.ColorBlind
                                    Colors = [| C3b(237uy,248uy,251uy); C3b(204uy,236uy,230uy); C3b(153uy,216uy,201uy); C3b(102uy,194uy,164uy)
                                                C3b(44uy,162uy,95uy); C3b(0uy,109uy,44uy) |]
                                }

                                7, {
                                    Usage = PaletteUsage.ColorBlind
                                    Colors = [| C3b(237uy,248uy,251uy); C3b(204uy,236uy,230uy); C3b(153uy,216uy,201uy); C3b(102uy,194uy,164uy)
                                                C3b(65uy,174uy,118uy); C3b(35uy,139uy,69uy); C3b(0uy,88uy,36uy) |]
                                }

                                8, {
                                    Usage = PaletteUsage.ColorBlind
                                    Colors = [| C3b(247uy,252uy,253uy); C3b(229uy,245uy,249uy); C3b(204uy,236uy,230uy); C3b(153uy,216uy,201uy)
                                                C3b(102uy,194uy,164uy); C3b(65uy,174uy,118uy); C3b(35uy,139uy,69uy); C3b(0uy,88uy,36uy) |]
                                }

                                9, {
                                    Usage = PaletteUsage.ColorBlind
                                    Colors = [| C3b(247uy,252uy,253uy); C3b(229uy,245uy,249uy); C3b(204uy,236uy,230uy); C3b(153uy,216uy,201uy)
                                                C3b(102uy,194uy,164uy); C3b(65uy,174uy,118uy); C3b(35uy,139uy,69uy); C3b(0uy,109uy,44uy)
                                                C3b(0uy,68uy,27uy) |]
                                }

                            ]
                    }

                let YlOrBr =
                    {
                        Name = Sym.ofString "YlOrBr"
                        Type = SchemeType.Sequential
                        Palettes =
                            MapExt.ofList [
                                3, {
                                    Usage = PaletteUsage.ColorBlind ||| PaletteUsage.Print ||| PaletteUsage.LCD ||| PaletteUsage.PhotoCopy
                                    Colors = [| C3b(255uy,247uy,188uy); C3b(254uy,196uy,79uy); C3b(217uy,95uy,14uy) |]
                                }

                                4, {
                                    Usage = PaletteUsage.ColorBlind ||| PaletteUsage.Print
                                    Colors = [| C3b(255uy,255uy,212uy); C3b(254uy,217uy,142uy); C3b(254uy,153uy,41uy); C3b(204uy,76uy,2uy) |]
                                }

                                5, {
                                    Usage = PaletteUsage.ColorBlind
                                    Colors = [| C3b(255uy,255uy,212uy); C3b(254uy,217uy,142uy); C3b(254uy,153uy,41uy); C3b(217uy,95uy,14uy)
                                                C3b(153uy,52uy,4uy) |]
                                }

                                6, {
                                    Usage = PaletteUsage.ColorBlind
                                    Colors = [| C3b(255uy,255uy,212uy); C3b(254uy,227uy,145uy); C3b(254uy,196uy,79uy); C3b(254uy,153uy,41uy)
                                                C3b(217uy,95uy,14uy); C3b(153uy,52uy,4uy) |]
                                }

                                7, {
                                    Usage = PaletteUsage.ColorBlind
                                    Colors = [| C3b(255uy,255uy,212uy); C3b(254uy,227uy,145uy); C3b(254uy,196uy,79uy); C3b(254uy,153uy,41uy)
                                                C3b(236uy,112uy,20uy); C3b(204uy,76uy,2uy); C3b(140uy,45uy,4uy) |]
                                }

                                8, {
                                    Usage = PaletteUsage.ColorBlind
                                    Colors = [| C3b(255uy,255uy,229uy); C3b(255uy,247uy,188uy); C3b(254uy,227uy,145uy); C3b(254uy,196uy,79uy)
                                                C3b(254uy,153uy,41uy); C3b(236uy,112uy,20uy); C3b(204uy,76uy,2uy); C3b(140uy,45uy,4uy) |]
                                }

                                9, {
                                    Usage = PaletteUsage.ColorBlind
                                    Colors = [| C3b(255uy,255uy,229uy); C3b(255uy,247uy,188uy); C3b(254uy,227uy,145uy); C3b(254uy,196uy,79uy)
                                                C3b(254uy,153uy,41uy); C3b(236uy,112uy,20uy); C3b(204uy,76uy,2uy); C3b(153uy,52uy,4uy)
                                                C3b(102uy,37uy,6uy) |]
                                }

                            ]
                    }

                let YlGn =
                    {
                        Name = Sym.ofString "YlGn"
                        Type = SchemeType.Sequential
                        Palettes =
                            MapExt.ofList [
                                3, {
                                    Usage = PaletteUsage.ColorBlind ||| PaletteUsage.Print ||| PaletteUsage.LCD ||| PaletteUsage.PhotoCopy
                                    Colors = [| C3b(247uy,252uy,185uy); C3b(173uy,221uy,142uy); C3b(49uy,163uy,84uy) |]
                                }

                                4, {
                                    Usage = PaletteUsage.ColorBlind ||| PaletteUsage.Print ||| PaletteUsage.LCD
                                    Colors = [| C3b(255uy,255uy,204uy); C3b(194uy,230uy,153uy); C3b(120uy,198uy,121uy); C3b(35uy,132uy,67uy) |]
                                }

                                5, {
                                    Usage = PaletteUsage.ColorBlind ||| PaletteUsage.Print ||| PaletteUsage.LCD
                                    Colors = [| C3b(255uy,255uy,204uy); C3b(194uy,230uy,153uy); C3b(120uy,198uy,121uy); C3b(49uy,163uy,84uy)
                                                C3b(0uy,104uy,55uy) |]
                                }

                                6, {
                                    Usage = PaletteUsage.ColorBlind
                                    Colors = [| C3b(255uy,255uy,204uy); C3b(217uy,240uy,163uy); C3b(173uy,221uy,142uy); C3b(120uy,198uy,121uy)
                                                C3b(49uy,163uy,84uy); C3b(0uy,104uy,55uy) |]
                                }

                                7, {
                                    Usage = PaletteUsage.ColorBlind
                                    Colors = [| C3b(255uy,255uy,204uy); C3b(217uy,240uy,163uy); C3b(173uy,221uy,142uy); C3b(120uy,198uy,121uy)
                                                C3b(65uy,171uy,93uy); C3b(35uy,132uy,67uy); C3b(0uy,90uy,50uy) |]
                                }

                                8, {
                                    Usage = PaletteUsage.ColorBlind
                                    Colors = [| C3b(255uy,255uy,229uy); C3b(247uy,252uy,185uy); C3b(217uy,240uy,163uy); C3b(173uy,221uy,142uy)
                                                C3b(120uy,198uy,121uy); C3b(65uy,171uy,93uy); C3b(35uy,132uy,67uy); C3b(0uy,90uy,50uy) |]
                                }

                                9, {
                                    Usage = PaletteUsage.ColorBlind
                                    Colors = [| C3b(255uy,255uy,229uy); C3b(247uy,252uy,185uy); C3b(217uy,240uy,163uy); C3b(173uy,221uy,142uy)
                                                C3b(120uy,198uy,121uy); C3b(65uy,171uy,93uy); C3b(35uy,132uy,67uy); C3b(0uy,104uy,55uy)
                                                C3b(0uy,69uy,41uy) |]
                                }

                            ]
                    }

                let Reds =
                    {
                        Name = Sym.ofString "Reds"
                        Type = SchemeType.Sequential
                        Palettes =
                            MapExt.ofList [
                                3, {
                                    Usage = PaletteUsage.ColorBlind ||| PaletteUsage.Print ||| PaletteUsage.LCD ||| PaletteUsage.PhotoCopy
                                    Colors = [| C3b(254uy,224uy,210uy); C3b(252uy,146uy,114uy); C3b(222uy,45uy,38uy) |]
                                }

                                4, {
                                    Usage = PaletteUsage.ColorBlind
                                    Colors = [| C3b(254uy,229uy,217uy); C3b(252uy,174uy,145uy); C3b(251uy,106uy,74uy); C3b(203uy,24uy,29uy) |]
                                }

                                5, {
                                    Usage = PaletteUsage.ColorBlind
                                    Colors = [| C3b(254uy,229uy,217uy); C3b(252uy,174uy,145uy); C3b(251uy,106uy,74uy); C3b(222uy,45uy,38uy)
                                                C3b(165uy,15uy,21uy) |]
                                }

                                6, {
                                    Usage = PaletteUsage.ColorBlind
                                    Colors = [| C3b(254uy,229uy,217uy); C3b(252uy,187uy,161uy); C3b(252uy,146uy,114uy); C3b(251uy,106uy,74uy)
                                                C3b(222uy,45uy,38uy); C3b(165uy,15uy,21uy) |]
                                }

                                7, {
                                    Usage = PaletteUsage.ColorBlind
                                    Colors = [| C3b(254uy,229uy,217uy); C3b(252uy,187uy,161uy); C3b(252uy,146uy,114uy); C3b(251uy,106uy,74uy)
                                                C3b(239uy,59uy,44uy); C3b(203uy,24uy,29uy); C3b(153uy,0uy,13uy) |]
                                }

                                8, {
                                    Usage = PaletteUsage.ColorBlind
                                    Colors = [| C3b(255uy,245uy,240uy); C3b(254uy,224uy,210uy); C3b(252uy,187uy,161uy); C3b(252uy,146uy,114uy)
                                                C3b(251uy,106uy,74uy); C3b(239uy,59uy,44uy); C3b(203uy,24uy,29uy); C3b(153uy,0uy,13uy) |]
                                }

                                9, {
                                    Usage = PaletteUsage.ColorBlind
                                    Colors = [| C3b(255uy,245uy,240uy); C3b(254uy,224uy,210uy); C3b(252uy,187uy,161uy); C3b(252uy,146uy,114uy)
                                                C3b(251uy,106uy,74uy); C3b(239uy,59uy,44uy); C3b(203uy,24uy,29uy); C3b(165uy,15uy,21uy)
                                                C3b(103uy,0uy,13uy) |]
                                }

                            ]
                    }

                let RdPu =
                    {
                        Name = Sym.ofString "RdPu"
                        Type = SchemeType.Sequential
                        Palettes =
                            MapExt.ofList [
                                3, {
                                    Usage = PaletteUsage.ColorBlind ||| PaletteUsage.Print ||| PaletteUsage.LCD ||| PaletteUsage.PhotoCopy
                                    Colors = [| C3b(253uy,224uy,221uy); C3b(250uy,159uy,181uy); C3b(197uy,27uy,138uy) |]
                                }

                                4, {
                                    Usage = PaletteUsage.ColorBlind ||| PaletteUsage.Print ||| PaletteUsage.LCD
                                    Colors = [| C3b(254uy,235uy,226uy); C3b(251uy,180uy,185uy); C3b(247uy,104uy,161uy); C3b(174uy,1uy,126uy) |]
                                }

                                5, {
                                    Usage = PaletteUsage.ColorBlind ||| PaletteUsage.Print ||| PaletteUsage.LCD
                                    Colors = [| C3b(254uy,235uy,226uy); C3b(251uy,180uy,185uy); C3b(247uy,104uy,161uy); C3b(197uy,27uy,138uy)
                                                C3b(122uy,1uy,119uy) |]
                                }

                                6, {
                                    Usage = PaletteUsage.ColorBlind
                                    Colors = [| C3b(254uy,235uy,226uy); C3b(252uy,197uy,192uy); C3b(250uy,159uy,181uy); C3b(247uy,104uy,161uy)
                                                C3b(197uy,27uy,138uy); C3b(122uy,1uy,119uy) |]
                                }

                                7, {
                                    Usage = PaletteUsage.ColorBlind
                                    Colors = [| C3b(254uy,235uy,226uy); C3b(252uy,197uy,192uy); C3b(250uy,159uy,181uy); C3b(247uy,104uy,161uy)
                                                C3b(221uy,52uy,151uy); C3b(174uy,1uy,126uy); C3b(122uy,1uy,119uy) |]
                                }

                                8, {
                                    Usage = PaletteUsage.ColorBlind
                                    Colors = [| C3b(255uy,247uy,243uy); C3b(253uy,224uy,221uy); C3b(252uy,197uy,192uy); C3b(250uy,159uy,181uy)
                                                C3b(247uy,104uy,161uy); C3b(221uy,52uy,151uy); C3b(174uy,1uy,126uy); C3b(122uy,1uy,119uy) |]
                                }

                                9, {
                                    Usage = PaletteUsage.ColorBlind
                                    Colors = [| C3b(255uy,247uy,243uy); C3b(253uy,224uy,221uy); C3b(252uy,197uy,192uy); C3b(250uy,159uy,181uy)
                                                C3b(247uy,104uy,161uy); C3b(221uy,52uy,151uy); C3b(174uy,1uy,126uy); C3b(122uy,1uy,119uy)
                                                C3b(73uy,0uy,106uy) |]
                                }

                            ]
                    }

                let Greens =
                    {
                        Name = Sym.ofString "Greens"
                        Type = SchemeType.Sequential
                        Palettes =
                            MapExt.ofList [
                                3, {
                                    Usage = PaletteUsage.ColorBlind ||| PaletteUsage.Print ||| PaletteUsage.LCD ||| PaletteUsage.PhotoCopy
                                    Colors = [| C3b(229uy,245uy,224uy); C3b(161uy,217uy,155uy); C3b(49uy,163uy,84uy) |]
                                }

                                4, {
                                    Usage = PaletteUsage.ColorBlind
                                    Colors = [| C3b(237uy,248uy,233uy); C3b(186uy,228uy,179uy); C3b(116uy,196uy,118uy); C3b(35uy,139uy,69uy) |]
                                }

                                5, {
                                    Usage = PaletteUsage.ColorBlind
                                    Colors = [| C3b(237uy,248uy,233uy); C3b(186uy,228uy,179uy); C3b(116uy,196uy,118uy); C3b(49uy,163uy,84uy)
                                                C3b(0uy,109uy,44uy) |]
                                }

                                6, {
                                    Usage = PaletteUsage.ColorBlind
                                    Colors = [| C3b(237uy,248uy,233uy); C3b(199uy,233uy,192uy); C3b(161uy,217uy,155uy); C3b(116uy,196uy,118uy)
                                                C3b(49uy,163uy,84uy); C3b(0uy,109uy,44uy) |]
                                }

                                7, {
                                    Usage = PaletteUsage.ColorBlind
                                    Colors = [| C3b(237uy,248uy,233uy); C3b(199uy,233uy,192uy); C3b(161uy,217uy,155uy); C3b(116uy,196uy,118uy)
                                                C3b(65uy,171uy,93uy); C3b(35uy,139uy,69uy); C3b(0uy,90uy,50uy) |]
                                }

                                8, {
                                    Usage = PaletteUsage.ColorBlind
                                    Colors = [| C3b(247uy,252uy,245uy); C3b(229uy,245uy,224uy); C3b(199uy,233uy,192uy); C3b(161uy,217uy,155uy)
                                                C3b(116uy,196uy,118uy); C3b(65uy,171uy,93uy); C3b(35uy,139uy,69uy); C3b(0uy,90uy,50uy) |]
                                }

                                9, {
                                    Usage = PaletteUsage.ColorBlind
                                    Colors = [| C3b(247uy,252uy,245uy); C3b(229uy,245uy,224uy); C3b(199uy,233uy,192uy); C3b(161uy,217uy,155uy)
                                                C3b(116uy,196uy,118uy); C3b(65uy,171uy,93uy); C3b(35uy,139uy,69uy); C3b(0uy,109uy,44uy)
                                                C3b(0uy,68uy,27uy) |]
                                }

                            ]
                    }

                let YlGnBu =
                    {
                        Name = Sym.ofString "YlGnBu"
                        Type = SchemeType.Sequential
                        Palettes =
                            MapExt.ofList [
                                3, {
                                    Usage = PaletteUsage.ColorBlind ||| PaletteUsage.Print ||| PaletteUsage.LCD ||| PaletteUsage.PhotoCopy
                                    Colors = [| C3b(237uy,248uy,177uy); C3b(127uy,205uy,187uy); C3b(44uy,127uy,184uy) |]
                                }

                                4, {
                                    Usage = PaletteUsage.ColorBlind ||| PaletteUsage.Print ||| PaletteUsage.LCD
                                    Colors = [| C3b(255uy,255uy,204uy); C3b(161uy,218uy,180uy); C3b(65uy,182uy,196uy); C3b(34uy,94uy,168uy) |]
                                }

                                5, {
                                    Usage = PaletteUsage.ColorBlind ||| PaletteUsage.Print
                                    Colors = [| C3b(255uy,255uy,204uy); C3b(161uy,218uy,180uy); C3b(65uy,182uy,196uy); C3b(44uy,127uy,184uy)
                                                C3b(37uy,52uy,148uy) |]
                                }

                                6, {
                                    Usage = PaletteUsage.ColorBlind
                                    Colors = [| C3b(255uy,255uy,204uy); C3b(199uy,233uy,180uy); C3b(127uy,205uy,187uy); C3b(65uy,182uy,196uy)
                                                C3b(44uy,127uy,184uy); C3b(37uy,52uy,148uy) |]
                                }

                                7, {
                                    Usage = PaletteUsage.ColorBlind
                                    Colors = [| C3b(255uy,255uy,204uy); C3b(199uy,233uy,180uy); C3b(127uy,205uy,187uy); C3b(65uy,182uy,196uy)
                                                C3b(29uy,145uy,192uy); C3b(34uy,94uy,168uy); C3b(12uy,44uy,132uy) |]
                                }

                                8, {
                                    Usage = PaletteUsage.ColorBlind
                                    Colors = [| C3b(255uy,255uy,217uy); C3b(237uy,248uy,177uy); C3b(199uy,233uy,180uy); C3b(127uy,205uy,187uy)
                                                C3b(65uy,182uy,196uy); C3b(29uy,145uy,192uy); C3b(34uy,94uy,168uy); C3b(12uy,44uy,132uy) |]
                                }

                                9, {
                                    Usage = PaletteUsage.ColorBlind
                                    Colors = [| C3b(255uy,255uy,217uy); C3b(237uy,248uy,177uy); C3b(199uy,233uy,180uy); C3b(127uy,205uy,187uy)
                                                C3b(65uy,182uy,196uy); C3b(29uy,145uy,192uy); C3b(34uy,94uy,168uy); C3b(37uy,52uy,148uy)
                                                C3b(8uy,29uy,88uy) |]
                                }

                            ]
                    }

                let Purples =
                    {
                        Name = Sym.ofString "Purples"
                        Type = SchemeType.Sequential
                        Palettes =
                            MapExt.ofList [
                                3, {
                                    Usage = PaletteUsage.ColorBlind ||| PaletteUsage.Print ||| PaletteUsage.LCD ||| PaletteUsage.PhotoCopy
                                    Colors = [| C3b(239uy,237uy,245uy); C3b(188uy,189uy,220uy); C3b(117uy,107uy,177uy) |]
                                }

                                4, {
                                    Usage = PaletteUsage.ColorBlind
                                    Colors = [| C3b(242uy,240uy,247uy); C3b(203uy,201uy,226uy); C3b(158uy,154uy,200uy); C3b(106uy,81uy,163uy) |]
                                }

                                5, {
                                    Usage = PaletteUsage.ColorBlind
                                    Colors = [| C3b(242uy,240uy,247uy); C3b(203uy,201uy,226uy); C3b(158uy,154uy,200uy); C3b(117uy,107uy,177uy)
                                                C3b(84uy,39uy,143uy) |]
                                }

                                6, {
                                    Usage = PaletteUsage.ColorBlind
                                    Colors = [| C3b(242uy,240uy,247uy); C3b(218uy,218uy,235uy); C3b(188uy,189uy,220uy); C3b(158uy,154uy,200uy)
                                                C3b(117uy,107uy,177uy); C3b(84uy,39uy,143uy) |]
                                }

                                7, {
                                    Usage = PaletteUsage.ColorBlind
                                    Colors = [| C3b(242uy,240uy,247uy); C3b(218uy,218uy,235uy); C3b(188uy,189uy,220uy); C3b(158uy,154uy,200uy)
                                                C3b(128uy,125uy,186uy); C3b(106uy,81uy,163uy); C3b(74uy,20uy,134uy) |]
                                }

                                8, {
                                    Usage = PaletteUsage.ColorBlind
                                    Colors = [| C3b(252uy,251uy,253uy); C3b(239uy,237uy,245uy); C3b(218uy,218uy,235uy); C3b(188uy,189uy,220uy)
                                                C3b(158uy,154uy,200uy); C3b(128uy,125uy,186uy); C3b(106uy,81uy,163uy); C3b(74uy,20uy,134uy) |]
                                }

                                9, {
                                    Usage = PaletteUsage.ColorBlind
                                    Colors = [| C3b(252uy,251uy,253uy); C3b(239uy,237uy,245uy); C3b(218uy,218uy,235uy); C3b(188uy,189uy,220uy)
                                                C3b(158uy,154uy,200uy); C3b(128uy,125uy,186uy); C3b(106uy,81uy,163uy); C3b(84uy,39uy,143uy)
                                                C3b(63uy,0uy,125uy) |]
                                }

                            ]
                    }

                let GnBu =
                    {
                        Name = Sym.ofString "GnBu"
                        Type = SchemeType.Sequential
                        Palettes =
                            MapExt.ofList [
                                3, {
                                    Usage = PaletteUsage.ColorBlind ||| PaletteUsage.Print ||| PaletteUsage.LCD ||| PaletteUsage.PhotoCopy
                                    Colors = [| C3b(224uy,243uy,219uy); C3b(168uy,221uy,181uy); C3b(67uy,162uy,202uy) |]
                                }

                                4, {
                                    Usage = PaletteUsage.ColorBlind ||| PaletteUsage.Print ||| PaletteUsage.LCD
                                    Colors = [| C3b(240uy,249uy,232uy); C3b(186uy,228uy,188uy); C3b(123uy,204uy,196uy); C3b(43uy,140uy,190uy) |]
                                }

                                5, {
                                    Usage = PaletteUsage.ColorBlind ||| PaletteUsage.Print
                                    Colors = [| C3b(240uy,249uy,232uy); C3b(186uy,228uy,188uy); C3b(123uy,204uy,196uy); C3b(67uy,162uy,202uy)
                                                C3b(8uy,104uy,172uy) |]
                                }

                                6, {
                                    Usage = PaletteUsage.ColorBlind
                                    Colors = [| C3b(240uy,249uy,232uy); C3b(204uy,235uy,197uy); C3b(168uy,221uy,181uy); C3b(123uy,204uy,196uy)
                                                C3b(67uy,162uy,202uy); C3b(8uy,104uy,172uy) |]
                                }

                                7, {
                                    Usage = PaletteUsage.ColorBlind
                                    Colors = [| C3b(240uy,249uy,232uy); C3b(204uy,235uy,197uy); C3b(168uy,221uy,181uy); C3b(123uy,204uy,196uy)
                                                C3b(78uy,179uy,211uy); C3b(43uy,140uy,190uy); C3b(8uy,88uy,158uy) |]
                                }

                                8, {
                                    Usage = PaletteUsage.ColorBlind
                                    Colors = [| C3b(247uy,252uy,240uy); C3b(224uy,243uy,219uy); C3b(204uy,235uy,197uy); C3b(168uy,221uy,181uy)
                                                C3b(123uy,204uy,196uy); C3b(78uy,179uy,211uy); C3b(43uy,140uy,190uy); C3b(8uy,88uy,158uy) |]
                                }

                                9, {
                                    Usage = PaletteUsage.ColorBlind
                                    Colors = [| C3b(247uy,252uy,240uy); C3b(224uy,243uy,219uy); C3b(204uy,235uy,197uy); C3b(168uy,221uy,181uy)
                                                C3b(123uy,204uy,196uy); C3b(78uy,179uy,211uy); C3b(43uy,140uy,190uy); C3b(8uy,104uy,172uy)
                                                C3b(8uy,64uy,129uy) |]
                                }

                            ]
                    }

                let Greys =
                    {
                        Name = Sym.ofString "Greys"
                        Type = SchemeType.Sequential
                        Palettes =
                            MapExt.ofList [
                                3, {
                                    Usage = PaletteUsage.ColorBlind ||| PaletteUsage.Print ||| PaletteUsage.LCD ||| PaletteUsage.PhotoCopy
                                    Colors = [| C3b(240uy,240uy,240uy); C3b(189uy,189uy,189uy); C3b(99uy,99uy,99uy) |]
                                }

                                4, {
                                    Usage = PaletteUsage.ColorBlind ||| PaletteUsage.Print
                                    Colors = [| C3b(247uy,247uy,247uy); C3b(204uy,204uy,204uy); C3b(150uy,150uy,150uy); C3b(82uy,82uy,82uy) |]
                                }

                                5, {
                                    Usage = PaletteUsage.ColorBlind
                                    Colors = [| C3b(247uy,247uy,247uy); C3b(204uy,204uy,204uy); C3b(150uy,150uy,150uy); C3b(99uy,99uy,99uy)
                                                C3b(37uy,37uy,37uy) |]
                                }

                                6, {
                                    Usage = PaletteUsage.ColorBlind
                                    Colors = [| C3b(247uy,247uy,247uy); C3b(217uy,217uy,217uy); C3b(189uy,189uy,189uy); C3b(150uy,150uy,150uy)
                                                C3b(99uy,99uy,99uy); C3b(37uy,37uy,37uy) |]
                                }

                                7, {
                                    Usage = PaletteUsage.ColorBlind
                                    Colors = [| C3b(247uy,247uy,247uy); C3b(217uy,217uy,217uy); C3b(189uy,189uy,189uy); C3b(150uy,150uy,150uy)
                                                C3b(115uy,115uy,115uy); C3b(82uy,82uy,82uy); C3b(37uy,37uy,37uy) |]
                                }

                                8, {
                                    Usage = PaletteUsage.ColorBlind
                                    Colors = [| C3b(255uy,255uy,255uy); C3b(240uy,240uy,240uy); C3b(217uy,217uy,217uy); C3b(189uy,189uy,189uy)
                                                C3b(150uy,150uy,150uy); C3b(115uy,115uy,115uy); C3b(82uy,82uy,82uy); C3b(37uy,37uy,37uy) |]
                                }

                                9, {
                                    Usage = PaletteUsage.ColorBlind
                                    Colors = [| C3b(255uy,255uy,255uy); C3b(240uy,240uy,240uy); C3b(217uy,217uy,217uy); C3b(189uy,189uy,189uy)
                                                C3b(150uy,150uy,150uy); C3b(115uy,115uy,115uy); C3b(82uy,82uy,82uy); C3b(37uy,37uy,37uy)
                                                C3b(0uy,0uy,0uy) |]
                                }

                            ]
                    }

                let YlOrRd =
                    {
                        Name = Sym.ofString "YlOrRd"
                        Type = SchemeType.Sequential
                        Palettes =
                            MapExt.ofList [
                                3, {
                                    Usage = PaletteUsage.ColorBlind ||| PaletteUsage.Print ||| PaletteUsage.LCD ||| PaletteUsage.PhotoCopy
                                    Colors = [| C3b(255uy,237uy,160uy); C3b(254uy,178uy,76uy); C3b(240uy,59uy,32uy) |]
                                }

                                4, {
                                    Usage = PaletteUsage.ColorBlind ||| PaletteUsage.Print
                                    Colors = [| C3b(255uy,255uy,178uy); C3b(254uy,204uy,92uy); C3b(253uy,141uy,60uy); C3b(227uy,26uy,28uy) |]
                                }

                                5, {
                                    Usage = PaletteUsage.ColorBlind
                                    Colors = [| C3b(255uy,255uy,178uy); C3b(254uy,204uy,92uy); C3b(253uy,141uy,60uy); C3b(240uy,59uy,32uy)
                                                C3b(189uy,0uy,38uy) |]
                                }

                                6, {
                                    Usage = PaletteUsage.ColorBlind
                                    Colors = [| C3b(255uy,255uy,178uy); C3b(254uy,217uy,118uy); C3b(254uy,178uy,76uy); C3b(253uy,141uy,60uy)
                                                C3b(240uy,59uy,32uy); C3b(189uy,0uy,38uy) |]
                                }

                                7, {
                                    Usage = PaletteUsage.ColorBlind
                                    Colors = [| C3b(255uy,255uy,178uy); C3b(254uy,217uy,118uy); C3b(254uy,178uy,76uy); C3b(253uy,141uy,60uy)
                                                C3b(252uy,78uy,42uy); C3b(227uy,26uy,28uy); C3b(177uy,0uy,38uy) |]
                                }

                                8, {
                                    Usage = PaletteUsage.ColorBlind
                                    Colors = [| C3b(255uy,255uy,204uy); C3b(255uy,237uy,160uy); C3b(254uy,217uy,118uy); C3b(254uy,178uy,76uy)
                                                C3b(253uy,141uy,60uy); C3b(252uy,78uy,42uy); C3b(227uy,26uy,28uy); C3b(177uy,0uy,38uy) |]
                                }

                                9, {
                                    Usage = PaletteUsage.ColorBlind
                                    Colors = [| C3b(255uy,255uy,204uy); C3b(255uy,237uy,160uy); C3b(254uy,217uy,118uy); C3b(254uy,178uy,76uy)
                                                C3b(253uy,141uy,60uy); C3b(252uy,78uy,42uy); C3b(227uy,26uy,28uy); C3b(189uy,0uy,38uy)
                                                C3b(128uy,0uy,38uy) |]
                                }

                            ]
                    }

                let PuRd =
                    {
                        Name = Sym.ofString "PuRd"
                        Type = SchemeType.Sequential
                        Palettes =
                            MapExt.ofList [
                                3, {
                                    Usage = PaletteUsage.ColorBlind ||| PaletteUsage.Print ||| PaletteUsage.LCD ||| PaletteUsage.PhotoCopy
                                    Colors = [| C3b(231uy,225uy,239uy); C3b(201uy,148uy,199uy); C3b(221uy,28uy,119uy) |]
                                }

                                4, {
                                    Usage = PaletteUsage.ColorBlind ||| PaletteUsage.Print ||| PaletteUsage.LCD
                                    Colors = [| C3b(241uy,238uy,246uy); C3b(215uy,181uy,216uy); C3b(223uy,101uy,176uy); C3b(206uy,18uy,86uy) |]
                                }

                                5, {
                                    Usage = PaletteUsage.ColorBlind ||| PaletteUsage.Print ||| PaletteUsage.LCD
                                    Colors = [| C3b(241uy,238uy,246uy); C3b(215uy,181uy,216uy); C3b(223uy,101uy,176uy); C3b(221uy,28uy,119uy)
                                                C3b(152uy,0uy,67uy) |]
                                }

                                6, {
                                    Usage = PaletteUsage.ColorBlind
                                    Colors = [| C3b(241uy,238uy,246uy); C3b(212uy,185uy,218uy); C3b(201uy,148uy,199uy); C3b(223uy,101uy,176uy)
                                                C3b(221uy,28uy,119uy); C3b(152uy,0uy,67uy) |]
                                }

                                7, {
                                    Usage = PaletteUsage.ColorBlind
                                    Colors = [| C3b(241uy,238uy,246uy); C3b(212uy,185uy,218uy); C3b(201uy,148uy,199uy); C3b(223uy,101uy,176uy)
                                                C3b(231uy,41uy,138uy); C3b(206uy,18uy,86uy); C3b(145uy,0uy,63uy) |]
                                }

                                8, {
                                    Usage = PaletteUsage.ColorBlind
                                    Colors = [| C3b(247uy,244uy,249uy); C3b(231uy,225uy,239uy); C3b(212uy,185uy,218uy); C3b(201uy,148uy,199uy)
                                                C3b(223uy,101uy,176uy); C3b(231uy,41uy,138uy); C3b(206uy,18uy,86uy); C3b(145uy,0uy,63uy) |]
                                }

                                9, {
                                    Usage = PaletteUsage.ColorBlind
                                    Colors = [| C3b(247uy,244uy,249uy); C3b(231uy,225uy,239uy); C3b(212uy,185uy,218uy); C3b(201uy,148uy,199uy)
                                                C3b(223uy,101uy,176uy); C3b(231uy,41uy,138uy); C3b(206uy,18uy,86uy); C3b(152uy,0uy,67uy)
                                                C3b(103uy,0uy,31uy) |]
                                }

                            ]
                    }

                let Blues =
                    {
                        Name = Sym.ofString "Blues"
                        Type = SchemeType.Sequential
                        Palettes =
                            MapExt.ofList [
                                3, {
                                    Usage = PaletteUsage.ColorBlind ||| PaletteUsage.Print ||| PaletteUsage.LCD ||| PaletteUsage.PhotoCopy
                                    Colors = [| C3b(222uy,235uy,247uy); C3b(158uy,202uy,225uy); C3b(49uy,130uy,189uy) |]
                                }

                                4, {
                                    Usage = PaletteUsage.ColorBlind
                                    Colors = [| C3b(239uy,243uy,255uy); C3b(189uy,215uy,231uy); C3b(107uy,174uy,214uy); C3b(33uy,113uy,181uy) |]
                                }

                                5, {
                                    Usage = PaletteUsage.ColorBlind
                                    Colors = [| C3b(239uy,243uy,255uy); C3b(189uy,215uy,231uy); C3b(107uy,174uy,214uy); C3b(49uy,130uy,189uy)
                                                C3b(8uy,81uy,156uy) |]
                                }

                                6, {
                                    Usage = PaletteUsage.ColorBlind
                                    Colors = [| C3b(239uy,243uy,255uy); C3b(198uy,219uy,239uy); C3b(158uy,202uy,225uy); C3b(107uy,174uy,214uy)
                                                C3b(49uy,130uy,189uy); C3b(8uy,81uy,156uy) |]
                                }

                                7, {
                                    Usage = PaletteUsage.ColorBlind
                                    Colors = [| C3b(239uy,243uy,255uy); C3b(198uy,219uy,239uy); C3b(158uy,202uy,225uy); C3b(107uy,174uy,214uy)
                                                C3b(66uy,146uy,198uy); C3b(33uy,113uy,181uy); C3b(8uy,69uy,148uy) |]
                                }

                                8, {
                                    Usage = PaletteUsage.ColorBlind
                                    Colors = [| C3b(247uy,251uy,255uy); C3b(222uy,235uy,247uy); C3b(198uy,219uy,239uy); C3b(158uy,202uy,225uy)
                                                C3b(107uy,174uy,214uy); C3b(66uy,146uy,198uy); C3b(33uy,113uy,181uy); C3b(8uy,69uy,148uy) |]
                                }

                                9, {
                                    Usage = PaletteUsage.ColorBlind
                                    Colors = [| C3b(247uy,251uy,255uy); C3b(222uy,235uy,247uy); C3b(198uy,219uy,239uy); C3b(158uy,202uy,225uy)
                                                C3b(107uy,174uy,214uy); C3b(66uy,146uy,198uy); C3b(33uy,113uy,181uy); C3b(8uy,81uy,156uy)
                                                C3b(8uy,48uy,107uy) |]
                                }

                            ]
                    }

                let PuBuGn =
                    {
                        Name = Sym.ofString "PuBuGn"
                        Type = SchemeType.Sequential
                        Palettes =
                            MapExt.ofList [
                                3, {
                                    Usage = PaletteUsage.ColorBlind ||| PaletteUsage.Print ||| PaletteUsage.LCD ||| PaletteUsage.PhotoCopy
                                    Colors = [| C3b(236uy,226uy,240uy); C3b(166uy,189uy,219uy); C3b(28uy,144uy,153uy) |]
                                }

                                4, {
                                    Usage = PaletteUsage.ColorBlind ||| PaletteUsage.LCD
                                    Colors = [| C3b(246uy,239uy,247uy); C3b(189uy,201uy,225uy); C3b(103uy,169uy,207uy); C3b(2uy,129uy,138uy) |]
                                }

                                5, {
                                    Usage = PaletteUsage.ColorBlind
                                    Colors = [| C3b(246uy,239uy,247uy); C3b(189uy,201uy,225uy); C3b(103uy,169uy,207uy); C3b(28uy,144uy,153uy)
                                                C3b(1uy,108uy,89uy) |]
                                }

                                6, {
                                    Usage = PaletteUsage.ColorBlind
                                    Colors = [| C3b(246uy,239uy,247uy); C3b(208uy,209uy,230uy); C3b(166uy,189uy,219uy); C3b(103uy,169uy,207uy)
                                                C3b(28uy,144uy,153uy); C3b(1uy,108uy,89uy) |]
                                }

                                7, {
                                    Usage = PaletteUsage.ColorBlind
                                    Colors = [| C3b(246uy,239uy,247uy); C3b(208uy,209uy,230uy); C3b(166uy,189uy,219uy); C3b(103uy,169uy,207uy)
                                                C3b(54uy,144uy,192uy); C3b(2uy,129uy,138uy); C3b(1uy,100uy,80uy) |]
                                }

                                8, {
                                    Usage = PaletteUsage.ColorBlind
                                    Colors = [| C3b(255uy,247uy,251uy); C3b(236uy,226uy,240uy); C3b(208uy,209uy,230uy); C3b(166uy,189uy,219uy)
                                                C3b(103uy,169uy,207uy); C3b(54uy,144uy,192uy); C3b(2uy,129uy,138uy); C3b(1uy,100uy,80uy) |]
                                }

                                9, {
                                    Usage = PaletteUsage.ColorBlind
                                    Colors = [| C3b(255uy,247uy,251uy); C3b(236uy,226uy,240uy); C3b(208uy,209uy,230uy); C3b(166uy,189uy,219uy)
                                                C3b(103uy,169uy,207uy); C3b(54uy,144uy,192uy); C3b(2uy,129uy,138uy); C3b(1uy,108uy,89uy)
                                                C3b(1uy,70uy,54uy) |]
                                }

                            ]
                    }

            /// Array of all available color schemes.
            let All =
                [|
                    Diverging.Spectral; Diverging.RdYlGn; Diverging.RdBu; Diverging.PiYG; Diverging.PRGn; Diverging.RdYlBu
                    Diverging.BrBG; Diverging.RdGy; Diverging.PuOr; Qualitative.Set2; Qualitative.Accent; Qualitative.Set1
                    Qualitative.Set3; Qualitative.Dark2; Qualitative.Paired; Qualitative.Pastel2; Qualitative.Pastel1; Sequential.OrRd
                    Sequential.PuBu; Sequential.BuPu; Sequential.Oranges; Sequential.BuGn; Sequential.YlOrBr; Sequential.YlGn
                    Sequential.Reds; Sequential.RdPu; Sequential.Greens; Sequential.YlGnBu; Sequential.Purples; Sequential.GnBu
                    Sequential.Greys; Sequential.YlOrRd; Sequential.PuRd; Sequential.Blues; Sequential.PuBuGn
                |]
