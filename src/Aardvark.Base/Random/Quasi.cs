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
namespace Aardvark.Base;

public static class Quasi
{
    /// <summary>
    /// Generates a new number in the halton sequence
    /// with the given inverse base, and the previous
    /// number in the sequence.
    /// 
    /// This is the incremental version to generate
    /// the halton squence of quasi-random numbers of
    /// a given base.  It has been taken from:
    ///
    /// A. Keller: Instant Radiosity,
    /// In Computer Graphics (SIGGRAPH 97 Conference Proceedings),
    /// pp. 49--56, August 1997.
    /// 
    /// As a small optimization, the inverse of the base is used.
    /// </summary>
    public static double QuasiHalton(
            double inverse_base, double value)
    {
        double r = 1.0 - value - 1e-10;
        if (inverse_base < r)
        {
            value += inverse_base;
        }
        else
        {
            double h = inverse_base * inverse_base;
            double hh = inverse_base;
            while (h >= r)
            {
                hh = h;
                h *= inverse_base;
            }
            value += hh + h - 1.0;
        }
        return value;
    }
    
    /// <summary>
    /// Generates a new number in the halton sequence
    /// with the 'index'th prime as base.
    /// </summary>
    public static double QuasiHaltonWithIndex(
            int index, double value)
    {
        return QuasiHalton(Prime.InverseWithIndex(index), value);
    }
}
