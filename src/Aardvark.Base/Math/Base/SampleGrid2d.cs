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
using System;

namespace Aardvark.Base;

public class SampleGrid2d
{
    private V2l m_last;
    private Box2d m_region;
    private V2d m_delta;

    public SampleGrid2d(V2l gridSize, Box2d region)
    {
        m_last = gridSize - new V2l(1, 1);
        m_region = region;
        m_delta = region.Size / (V2d)m_last;
    }

    public static void AdaptivelyFill(
        Action<long, long, long, long, double, double> xMid,
        Action<long, long, long, long, double, double> yMid,
        Action<long, long, long, long, long, long,
                double, double, double, double> xyMid,
        long x0, long x1, long y0, long y1
        )
    {
        var dx = x1 - x0;
        var dy = y1 - y0;
        if ((dx < 2) && (dy < 2)) return;
        long xm = 0, ym = 0;
        double wx0 = 0.0, wx1 = 0.0, wy0 = 0.0, wy1 = 0.0;
        if (dx > 1)
        {
            xm = (x1 + x0) / 2;
            wx1 = (xm - x0) / (double)dx;
            wx0 = (x1 - xm) / (double)dx;
            xMid(x0, x1, y0, xm, wx0, wx1);
            xMid(x0, x1, y1, xm, wx0, wx1);
            if (dy < 2)
            {
                AdaptivelyFill(xMid, yMid, xyMid, x0, xm, y0, y1);
                AdaptivelyFill(xMid, yMid, xyMid, xm, x1, y0, y1);
                return;
            }
        }
        if (dy > 1)
        {
            ym = (y1 + y0) / 2;
            wy1 = (ym - y0) / (double)dy;
            wy0 = (y1 - ym) / (double)dy;
            yMid(x0, y0, y1, ym, wy0, wy1);
            yMid(x1, y0, y1, ym, wy0, wy1);
            if (dx < 2)
            {
                AdaptivelyFill(xMid, yMid, xyMid, x0, x1, y0, ym);
                AdaptivelyFill(xMid, yMid, xyMid, x0, x1, ym, y1);
                return;
            }
        }
        if (dx > 1 && dy > 1)
        {
            xyMid(x0, x1, y0, y1, xm, ym, wx0, wx1, wy0, wy1);
            AdaptivelyFill(xMid, yMid, xyMid, x0, xm, y0, ym);
            AdaptivelyFill(xMid, yMid, xyMid, xm, x1, y0, ym);
            AdaptivelyFill(xMid, yMid, xyMid, x0, xm, ym, y1);
            AdaptivelyFill(xMid, yMid, xyMid, xm, x1, ym, y1);
        }
    }

    public static void AdaptivelyFill(
                Action<int, long, long, long, long, double, double> xMid,
                Action<int, long, long, long, long, double, double> yMid,
                Action<int, long, long, long, long, long, long,
                     double, double, double, double> xyMid,
                int depth, long x0, long x1, long y0, long y1
                )
    {
        var dx = x1 - x0;
        var dy = y1 - y0;
        if ((dx < 2) && (dy < 2)) return;
        long xm = 0, ym = 0;
        double wx0 = 0.0, wx1 = 0.0, wy0 = 0.0, wy1 = 0.0;
        if (dx > 1)
        {
            xm = (x1 + x0) / 2;
            wx1 = (xm - x0) / (double)dx;
            wx0 = (x1 - xm) / (double)dx;
            xMid(depth, x0, x1, y0, xm, wx0, wx1);
            xMid(depth, x0, x1, y1, xm, wx0, wx1);
            if (dy < 2)
            {
                AdaptivelyFill(xMid, yMid, xyMid, depth + 1, x0, xm, y0, y1);
                AdaptivelyFill(xMid, yMid, xyMid, depth + 1, xm, x1, y0, y1);
                return;
            }
        }
        if (dy > 1)
        {
            ym = (y1 + y0) / 2;
            wy1 = (ym - y0) / (double)dy;
            wy0 = (y1 - ym) / (double)dy;
            yMid(depth, x0, y0, y1, ym, wy0, wy1);
            yMid(depth, x1, y0, y1, ym, wy0, wy1);
            if (dx < 2)
            {
                AdaptivelyFill(xMid, yMid, xyMid, depth + 1, x0, x1, y0, ym);
                AdaptivelyFill(xMid, yMid, xyMid, depth + 1, x0, x1, ym, y1);
                return;
            }
        }
        if (dx > 1 && dy > 1)
        {
            xyMid(depth, x0, x1, y0, y1, xm, ym, wx0, wx1, wy0, wy1);
            AdaptivelyFill(xMid, yMid, xyMid, depth + 1, x0, xm, y0, ym);
            AdaptivelyFill(xMid, yMid, xyMid, depth + 1, xm, x1, y0, ym);
            AdaptivelyFill(xMid, yMid, xyMid, depth + 1, x0, xm, ym, y1);
            AdaptivelyFill(xMid, yMid, xyMid, depth + 1, xm, x1, ym, y1);
        }
    }

    public static void AdaptivelySample(
                Func<long, long, long, long,
                     double, double, double, double, bool> similar,
                Action<long, long, double, double> sample,
                Action<long, long, long, long,
                     double, double, double, double> region,
                Action<long, long, long, long,
                     double, double, double, double> super,
                long x0, long x1, long y0, long y1,
                double xd0, double xd1, double yd0, double yd1
                )
    {
        var dx = x1 - x0;
        var dy = y1 - y0;
        if ((dx < 2) && (dy < 2))
        {
            super(x0, x1, y0, y1, xd0, xd1, yd0, yd1);
            return;
        }
        if (similar(x0, x1, y0, y1, xd0, xd1, yd0, yd1))
        {
            region(x0, x1, y0, y1, xd0, xd1, yd0, yd1);
            return;
        }
        if (dx > 1)
        {
            var xm = (x1 + x0) / 2;
            var xdm = xd0 + (xd1 - xd0) * (xm - x0) / (double)dx;
            sample(xm, y0, xdm, yd0);
            sample(xm, y1, xdm, yd1);
            if (dy > 1)
            {
                var ym = (y1 + y0) / 2;
                var ydm = yd0 + (yd1 - yd0) * (ym - y0) / (double)dy;
                sample(x0, ym, xd0, ydm);
                sample(xm, ym, xdm, ydm);
                sample(x1, ym, xd1, ydm);
                AdaptivelySample(similar, sample, region, super,
                                 x0, xm, y0, ym, xd0, xdm, yd0, ydm);
                AdaptivelySample(similar, sample, region, super,
                                 xm, x1, y0, ym, xdm, xd1, yd0, ydm);
                AdaptivelySample(similar, sample, region, super,
                                 x0, xm, ym, y1, xd0, xdm, ydm, yd1);
                AdaptivelySample(similar, sample, region, super,
                                 xm, x1, ym, y1, xdm, xd1, ydm, yd1);
            }
            else
            {
                AdaptivelySample(similar, sample, region, super,
                                 x0, xm, y0, y1, xd0, xdm, yd0, yd1);
                AdaptivelySample(similar, sample, region, super,
                                 xm, x1, y0, y1, xdm, xd1, yd0, yd1);
            }
        }
        else
        {
            var ym = (y1 + y0) / 2;
            var ydm = yd0 + (yd1 - yd0) * (ym - y0) / (double)dy;
            sample(x0, ym, xd0, ydm);
            sample(x1, ym, xd1, ydm);
            AdaptivelySample(similar, sample, region, super,
                             x0, x1, y0, ym, xd0, xd1, yd0, ydm);
            AdaptivelySample(similar, sample, region, super,
                             x0, x1, ym, y1, xd0, xd1, ydm, yd1);
        }
    }

    public void Sample(V2l count,
                        Action<long, long, double, double> sample)
    {
        V2d step = (V2d)m_last / (V2d)count;
        double yd = 0.5;
        for (int yi = 0; yi <= m_last.Y; yd += step.Y, yi = (int)yd)
        {
            double y = m_region.Min.Y + yi * m_delta.Y;
            double xd = 0.5;
            for (int xi = 0; xi <= m_last.X; xd += step.X, xi = (int)xd)
            {
                double x = m_region.Min.X + xi * m_delta.X;
                sample(xi, yi, x, y);
            }
        }
    }

    public void Sample(V2l count,
                Action<long, long, long, long,
                     double, double, double, double> region)
    {
        V2d step = (V2d)m_last / (V2d)count;
        double y = m_region.Min.Y;
        double yd = 0.5 + step.Y;
        for (int yi = 0, nyi = (int)yd; yi < m_last.Y;
             yd += step.Y, yi = nyi, nyi = (int)yd)
        {
            double ny = m_region.Min.Y + nyi * m_delta.Y;
            double x = m_region.Min.X;
            double xd = 0.5 + step.X;
            for (int xi = 0, nxi = (int)xd; xi < m_last.X;
                 xd += step.X, xi = nxi, nxi = (int)xd)
            {
                double nx = m_region.Min.X + nxi * m_delta.X;
                region(xi, nxi, yi, nyi, x, nx, y, ny);
                x = nx;
            }
            y = ny;
        }
    }
        
    /// <summary>
    /// Perform the supplied action on grid elements that are separated
    /// by a supplied step size. Only near the borders, the separation
    /// may be smaller. The (possibly) smaller border separation is
    /// distributed to all four borders.
    /// </summary>
    public void SampleRegular(V2l step, Action<long, long, double, double> sample)
    {
        V2l regularLast = (m_last / step) * step;
        V2l offset = (m_last - regularLast) / 2;
        regularLast += offset;

        if (offset.Y > 0)
        {
            if (offset.X > 0)
                sample(0, 0, m_region.Min.X, m_region.Min.Y);
            for (long xi = offset.X; xi <= regularLast.X; xi += step.X)
                sample(xi, 0, m_region.Min.X + xi * m_delta.X, m_region.Min.Y);
            if (regularLast.X < m_last.X)
                sample(m_last.X, 0, m_region.Max.X, m_region.Min.Y);
        }
        for (long yi = offset.Y; yi <= regularLast.Y; yi += step.Y)
        {
            double y = m_region.Min.Y + yi * m_delta.Y;
            if (offset.X > 0)
                sample(0, yi, m_region.Min.X, y);
            for (long xi = offset.X; xi <= regularLast.X; xi += step.X)
                sample(xi, yi, m_region.Min.X + xi * m_delta.X, y);
            if (regularLast.X < m_last.X)
                sample(m_last.X, yi, m_region.Max.X, y);
        }
        if (regularLast.Y < m_last.Y)
        {
            if (offset.X > 0)
                sample(0, m_last.Y, m_region.Min.X, m_region.Max.Y);
            for (long xi = offset.X; xi <= regularLast.X; xi += step.X)
                sample(xi, m_last.Y, m_region.Min.X + xi * m_delta.X, m_region.Max.Y);
            if (regularLast.X < m_last.X)
                sample(m_last.X, m_last.Y, m_region.Max.X, m_region.Max.Y);
        }
    }

    /// <summary>
    /// Perform the supplied action on grid regions of a supplied step
    /// size. Only near the borders, the regions may be smaller. The
    /// (possibly) smaller border region size is distributed to all four
    /// borders.
    /// </summary>
    public void SampleRegular(V2l step,
                Action<long, long, long, long,
                     double, double, double, double> region)
    {
        V2l regularLast = (m_last / step) * step;
        V2l offset = (m_last - regularLast) / 2;
        regularLast += offset;

        if (offset.Y > 0)
        {
            double maxY = m_region.Min.Y + offset.Y * m_delta.Y;
            if (offset.X > 0)
                region(0, offset.X, 0, offset.Y,
                    m_region.Min.X,
                    m_region.Min.X + offset.X * m_delta.X,
                    m_region.Min.Y, maxY);
            for (long xi = offset.X, nxi = xi + step.X;
                 xi < regularLast.X; xi = nxi, nxi += step.X)
                region(xi, nxi, 0, offset.Y,
                    m_region.Min.X + xi * m_delta.X,
                    m_region.Min.X + nxi * m_delta.X,
                    m_region.Min.Y, maxY);
            if (regularLast.X < m_last.X)
                region(regularLast.X, m_last.X, 0, offset.Y,
                    m_region.Min.X + regularLast.X * m_delta.X,
                    m_region.Min.X + m_last.X * m_delta.X,
                    m_region.Min.Y, maxY);
        }

        for (long yi = offset.Y, nyi = yi + step.Y;
             yi < regularLast.Y; yi = nyi, nyi += step.Y)
        {
            double minY = m_region.Min.Y + yi * m_delta.Y;
            double maxY = minY + m_delta.Y;
            if (offset.X > 0)
                region(0, offset.X, yi, nyi,
                    m_region.Min.X,
                    m_region.Min.X + offset.X * m_delta.X,
                    minY, maxY);
            for (long xi = offset.X, nxi = xi + step.X;
                 xi < regularLast.X; xi = nxi, nxi += step.X)
            {
                region(xi, nxi, yi, nyi,
                    m_region.Min.X + xi * m_delta.X,
                    m_region.Min.X + nxi * m_delta.X,
                    minY, maxY);
            }
            if (regularLast.X < m_last.X)
                region(regularLast.X, m_last.X, yi, nyi,
                    m_region.Min.X + regularLast.X * m_delta.X,
                    m_region.Min.X + m_last.X * m_delta.X,
                    minY, maxY);
        }

        if (regularLast.Y < m_last.Y)
        {
            double minY = m_region.Min.Y + regularLast.Y * m_delta.Y;
            if (offset.X > 0)
                region(0, offset.X, regularLast.Y, m_last.Y,
                    m_region.Min.X,
                    m_region.Min.X + offset.X * m_delta.X,
                    minY, m_region.Max.Y);
            for (long xi = offset.X, nxi = xi + step.X;
                 xi < regularLast.X; xi = nxi, nxi += step.X)
                region(xi, xi + step.X, regularLast.Y, m_last.Y,
                    m_region.Min.X + xi * m_delta.X,
                    m_region.Min.X + nxi * m_delta.X,
                    minY, m_region.Max.Y);
            if (regularLast.X < m_last.X)
                region(regularLast.X, m_last.X, regularLast.Y, m_last.Y,
                    m_region.Min.X + regularLast.X * m_delta.X,
                    m_region.Min.X + m_last.X * m_delta.X,
                    minY, m_region.Max.Y);
        }
    }
}
