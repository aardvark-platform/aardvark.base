/*
    Copyright 2006-2025. The Aardvark Platform Team.

        https://aardvark.graphics

    Licensed under the Apache License, Version 2.0 (the "License");
    you may not use this file except in compliance with the License.
    You may obtain a copy of the License at

        http://www.apache.org/licenses/LICENSE-2.0

    Unless required by applicable law or agreed to in writing, software
    distributed under the License is distributed on an "AS IS" BASIS,
    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
    See the License for the specific language governing permissions and
    limitations under the License.
*/
using static System.Math;

namespace Aardvark.Base;

/// <summary>
/// Various coordinate transformations.
/// 
/// Abbreviations:
/// ll = lower left, lr = lower right, ul = upper left, ur = upper right
/// 
/// 2D:
/// Normalized Image Pos:
/// Our coord-exchange format; Independent of reolution.
/// (0,0)=ul edge of sensor = ul edge of ul pixel, (1,1) = lr edge of sensor = lr edge of lr pixel.
/// 
/// Pixel Center:
/// (0,0)=center of ul pixel, (w-1, h-1)=center of lr pixel.
/// 
/// Pixel Edge:
/// (0,0)=ul edge of ul pixel, (w-1, h-1)= lr edge of lr pixel.
/// </summary>
public static class CoordTransforms
{
    public static readonly V2d V2dHalf = new(0.5, 0.5);

    /// <summary>
    /// Convert from pixel-center position to normalized image position [0,1][0,1].
    /// (The inverse of toPixelCenter.)
    /// </summary>
    /// <param name="pos">The pixel location defined in pixel space: (0,0)=center of upper left pixel, (w-1,h-1)=center of lower right pixel.</param>
    /// <param name="imgSizeInPixel">The size of the image (as V2d to safe lots of conversions).</param>
    /// <returns>A normalized image position in [0,1][0,1].</returns>
    public static V2d PixelCenterToNormalizedImgPos(V2d pos, V2d imgSizeInPixel)
        => (pos + V2dHalf) / imgSizeInPixel;

    public static V2d PixelCenterToNormalizedImgPos(V2i pos, V2d imgSizeInPixel)
        => PixelCenterToNormalizedImgPos((V2d)pos, imgSizeInPixel);

    public static V2d PixelCenterToNormalizedImgPos(int x, int y, V2d imgSizeInPixel)
        => PixelCenterToNormalizedImgPos(new V2d(x, y), imgSizeInPixel);

    /// <summary>
    /// Returns a Matrix to 
    /// convert from pixel-center position to normalized image position [0,1][0,1].
    /// (The inverse of toPixelCenter.)
    /// </summary>
    public static M33d PixelCenterToNormalizedImgMat(V2d imgSizeInPixel)
        => M33d.Scale(V2d.II / imgSizeInPixel) * M33d.Translation(V2dHalf);

    /// <summary>
    /// Convert from normalized image position [0,1][0,1] to pixel-center position
    /// (The inverse of toNormalizedImgPos.)
    /// </summary>
    /// <param name="pos">The pixel location defined in pixel space: (0,0)=center of upper left pixel, (w-1,h-1)=center of lower right pixel.</param>
    /// <param name="imgSizeInPixel">The size of the image (as V2d to safe lots of conversions).</param>
    /// <returns>A image position in [-0.5,imgSizeInPixel.X-0.5][-0.5,imgSizeInPixel.Y-0.5].</returns>
    public static V2d NormalizedImagePosToPixelCenter(V2d pos, V2i imgSizeInPixel)
        => new V2d(pos.X * imgSizeInPixel.X, pos.Y * imgSizeInPixel.Y) - V2dHalf;

    /// <summary>
    /// Convert from normalized image position [0,1][0,1] to pixel-center position
    /// (The inverse of toNormalizedImgPos.)
    /// </summary>
    /// <param name="pos">The pixel location defined in pixel space: (0,0)=center of upper left pixel, (w-1,h-1)=center of lower right pixel.</param>
    /// <param name="imgSizeInPixel">The size of the image (as V2d to safe lots of conversions).</param>
    /// <returns>A image position in [-0.5,imgSizeInPixel.X-0.5][-0.5,imgSizeInPixel.Y-0.5].</returns>
    public static V2d NormalizedImagePosToPixelCenter(V2d pos, V2l imgSizeInPixel)
        => new V2d(pos.X * imgSizeInPixel.X, pos.Y * imgSizeInPixel.Y) - V2dHalf;

    /// <summary>
    /// Convert from normalized image position [0,1][0,1] to already rounded pixel-center position.
    /// </summary>
    /// <param name="pos">The pixel location defined in pixel space: (0,0)=center of upper left pixel, (w-1,h-1)=center of lower right pixel.</param>
    /// <param name="imgSizeInPixel">The size of the image (as V2d to safe lots of conversions).</param>
    /// <returns>A normalized image position in [0, imgSizeInPixel.X-1][0, imgSizeInPixel.Y-1].</returns>
    public static V2i NormalizedImagePosToPixelCenterRound(V2d pos, V2i imgSizeInPixel)
        => (V2i)NormalizedImagePosToPixelCenter(pos, imgSizeInPixel).Copy(v => Round(v));

    /// <summary>
    /// Convert from normalized image position [0,1][0,1] to already rounded pixel-center position.
    /// </summary>
    /// <param name="pos">The pixel location defined in pixel space: (0,0)=center of upper left pixel, (w-1,h-1)=center of lower right pixel.</param>
    /// <param name="imgSizeInPixel">The size of the image (as V2d to safe lots of conversions).</param>
    /// <returns>A normalized image position in [0, imgSizeInPixel.X-1][0, imgSizeInPixel.Y-1].</returns>
    public static V2l NormalizedImagePosToPixelCenterRound(V2d pos, V2l imgSizeInPixel)
        => (V2l)NormalizedImagePosToPixelCenter(pos, imgSizeInPixel).Copy(v => Round(v));

    /// <summary>
    /// Returns a Matrix to 
    /// convert from normalized image position [0,1][0,1] to pixel-center position
    /// (The inverse of toNormalizedImgPos.)
    /// </summary>
    public static M33d NormalizedImagePosToPixelCenterMat(V2d imgSizeInPixel)
        => M33d.Translation(-V2dHalf) * M33d.Scale(imgSizeInPixel);

    //[ISSUE 20090819 andi] add docu
    /////////////////////////
    public static V2d PixelEdgeToNormalizedImgPos(V2d pos, V2d imgSizeInPixel)
        => pos / (imgSizeInPixel-1);

    public static V2d PixelEdgeToNormalizedImgPos(V2i pos, V2d imgSizeInPixel)
        => PixelEdgeToNormalizedImgPos((V2d)pos, imgSizeInPixel);

    public static M33d PixelEdgeToNormalizedImgMat(V2d imgSizeInPixel)
        => M33d.Scale(V2d.II / (imgSizeInPixel-1));

    public static V2d NormalizedImagePosToPixelEdge(V2d pos, V2d imgSizeInPixel)
        => pos * (imgSizeInPixel-1);

    public static V2i NormalizedImagePosToPixelEdgeRound(V2d pos, V2d imgSizeInPixel)
        => (V2i)NormalizedImagePosToPixelEdge(pos, imgSizeInPixel).Copy(v => Round(v));

    public static M33d NormalizedImagePosToPixelEdgeMat(V2d imgSizeInPixel)
        => M33d.Scale(imgSizeInPixel-1);
}
