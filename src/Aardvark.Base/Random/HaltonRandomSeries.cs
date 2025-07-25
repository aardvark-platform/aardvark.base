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

/// <summary>
/// A halton series generator, that uses a normal random generator for
/// higher order series, in order to avoid the slow coverage of higher
/// order halton series.
/// </summary>
public class HaltonRandomSeries(
    int haltonCount,
    IRandomUniform randomUniform
    ) : IRandomSeries
{
    public double UniformDouble(int seriesIndex)
    {
        if (seriesIndex < m_haltonStateArray.Length)
        {
            double value = Quasi.QuasiHaltonWithIndex(
                seriesIndex, m_haltonStateArray[seriesIndex]
                );
            m_haltonStateArray[seriesIndex] = value;
            return value;
        }
        else
        {
            return m_randomUniform.UniformDouble();
        }
    }
    
    private readonly double [] m_haltonStateArray = new double[haltonCount]
        .SetByIndex(i => randomUniform.UniformDouble());

    private readonly IRandomUniform m_randomUniform = randomUniform;
}
