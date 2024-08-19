namespace Aardvark.Base

open System
open System.ComponentModel

[<AutoOpen>]
module FSharpPixImageErrorMetricExtensions =

    [<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
    module PixImage =

        [<EditorBrowsable(EditorBrowsableState.Never)>]
        module ErrorMetricHelpers =

            let maxValues =
                LookupTable.lookup [
                    typeof<uint8>,   255.0
                    typeof<int8>,    255.0
                    typeof<uint16>,  65535.0
                    typeof<int16>,   65535.0
                    typeof<int32>,   4294967295.0
                    typeof<uint32>,  4294967295.0
                    typeof<float16>, 1.0
                    typeof<float32>, 1.0
                    typeof<float>,   1.0
                ]

        open ErrorMetricHelpers

        /// Computes the mean squared error between two images.
        /// The images may have different channel counts, excess channels are ignored.
        let inline meanSquaredError (input : PixImage<'T>) (output : PixImage<'T>) =
            if input.Size <> output.Size then
                raise <| ArgumentException("Image sizes do not match.")

            let mutable error = KahanSum()
            let channels = min input.ChannelCount output.ChannelCount
            let count = output.Size.X * output.Size.Y * channels

            for x in 0 .. output.Size.X - 1 do
                for y in 0 .. output.Size.Y - 1 do
                    for c in 0 .. channels - 1 do
                        let inputData = input.GetChannel(int64 c)
                        let outputData = output.GetChannel(int64 c)

                        let diff = float inputData.[x, y] - float outputData.[x, y]
                        error.Add ((diff * diff) / float count)

            error.Value

        /// Computes the peak signal-to-noise ratio between two images.
        /// The images may have different channel counts, excess channels are ignored.
        let inline peakSignalToNoiseRatio' (maxValue : float) (input : PixImage<'T>) (output : PixImage<'T>) =
            20.0 * (log10 maxValue) - 10.0 * log10 (meanSquaredError input output)

        /// Computes the peak signal-to-noise ratio between two images.
        /// The images may have different channel counts, excess channels are ignored.
        let inline peakSignalToNoiseRatio (input : PixImage<'T>) (output : PixImage<'T>) =
            let maxValue = maxValues typeof<'T>
            peakSignalToNoiseRatio' maxValue input output