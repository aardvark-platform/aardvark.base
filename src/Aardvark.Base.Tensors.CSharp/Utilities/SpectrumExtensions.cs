using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Aardvark.Base
{
    public static class SpectrumExtensions
    {
        public static M33d CreateConversionMatrix()
        {
            // for spectral to xyz conversion see: http://www.brucelindbloom.com/index.html?Eqn_Spect_to_XYZ.html

            var specS0Vec = new Vector<double>(SpectralData.SpecS0_380_780_10nm);
            var specS1Vec = new Vector<double>(SpectralData.SpecS1_380_780_10nm);
            var specS2Vec = new Vector<double>(SpectralData.SpecS2_380_780_10nm);

            var cieXVec = new Vector<double>(SpectralData.Ciexyz31X_380_780_10nm);
            var cieYVec = new Vector<double>(SpectralData.Ciexyz31Y_380_780_10nm);
            var cieZVec = new Vector<double>(SpectralData.Ciexyz31Z_380_780_10nm);

            var specM = new M33d
            (
                cieXVec.DotProduct(specS0Vec), cieXVec.DotProduct(specS1Vec), cieXVec.DotProduct(specS2Vec),
                cieYVec.DotProduct(specS0Vec), cieYVec.DotProduct(specS1Vec), cieYVec.DotProduct(specS2Vec),
                cieZVec.DotProduct(specS0Vec), cieZVec.DotProduct(specS1Vec), cieZVec.DotProduct(specS2Vec)
            );

            var cieN = 1.0 / cieYVec.Data.Sum(); // cie response curve integral normalization

            // sun spectral radiance (S0, S1, S2) is expressed in micro-meters => 
            // convert to wavelengthes in nm (unit of color matching functions) -> scale by 1/1000 
            // the cie response functions are in 10nm steps -> scale by 10

            var specN = 1.0 / 1000 * 10;         // spectrum unit converions (1 micro meter to 10 nano meter)

            var norm = cieN * specN;             // normalization of color space conversion matrix

            return specM * norm;
        }

        /// <summary>
        /// conversion matrix from sun color spectrum in (S0, S1, S2) to CIE XYZ color space
        /// </summary>
        public static readonly M33d SecM = CreateConversionMatrix();

    }
}
