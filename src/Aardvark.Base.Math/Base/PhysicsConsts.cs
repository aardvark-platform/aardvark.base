using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aardvark.Base
{
    public static class PhysicsConstant
    {
        /// <summary>
        /// Age of universe in years.
        /// </summary>
        public const double AgeOfUniverse = 13.73e9;
        public const double AgeOfUniverseRelativeStandardUncertainty = 8.8e-3;

        /// <summary>
        /// Angular velocity of the earth in radians/s.
        /// </summary>
        public const double AngularVelocityOfEarth = 7.2921159e-5;

        /// <summary>
        /// Astronomical unit in m.
        /// </summary>
        public const double AstronomicalUnit = 1.4959787066e11;
        public const double AstronomicalUnitRelativeStandardUncertainty = 2.0e-11;

        /// <summary>
        /// Astronomical unit in m.
        /// </summary>
        public const double au = AstronomicalUnit;

        /// <summary>
        /// Atomic mass unit in kg.
        /// </summary>
        public const double AtomicMassUnit = 1.66053886e-27;
        public const double AtomicMassUnitRelativeStandardUncertainty = 1.7e-7;

        /// <summary>
        /// Avogadro constant in mol^-1.
        /// </summary>
        public const double AvogadroConstant = 6.0221415e23;
        public const double AvogadroConstantRelativeStandardUncertainty = 1.7e-7;

        /// <summary>
        /// Bohr magneton in J/T.
        /// </summary>
        public const double BohrMagneton = 927.400915e-26;
        public const double BohrMagnetonRelativeStandardUncertainty = 2.5e-8;

        /// <summary>
        /// Bohr radius in m.
        /// </summary>
        public const double BohrRadius = 5.291772108e-11;
        public const double BohrRadiusRelativeStandardUncertainty = 3.3e-9;

        /// <summary>
        /// Boltzmann constant k in J/K.
        /// </summary>
        public const double BoltzmannConstant = 1.3806504e-23;
        public const double BoltzmannConstantRelativeStandardUncertainty = 1.8e-6;

        /// <summary>
        /// Boltzmann constant k in J/K.
        /// </summary>
        public const double k = BoltzmannConstant;

        /// <summary>
        /// Classical electron radius r_e in m.
        /// </summary>
        public const double ClassicalElectronRadius = 2.8179402894e-15;
        public const double ClassicalElectronRadiusRelativeStandardUncertainty = 2.1e-9;

        /// <summary>
        /// Conductance quantum in S.
        /// </summary>
        public const double ConductanceQuantum = 7.7480917004e-5;
        public const double ConductanceQuantumRelativeStandardUncertainty = 6.8e-10;

        /// <summary>
        /// Coulomb's constant in N * m^2 * C^-2. This is a defined value
        /// with no uncertainty.
        /// </summary>
        public const double CoulombsConstant = 1.0 / (4.0 * Constant.Pi * epsilon0);

        /// <summary>
        /// Earth mass in kg.
        /// </summary>
        public const double EarthMass = 5.9737076e24;

        /// <summary>
        /// Earth equatorial radius in m.
        /// </summary>
        public const double EarthRadiusEquatorial = 6.378140e6;

        /// <summary>
        /// Electron rest mass in kg.
        /// </summary>
        public const double ElectronRestMass = 9.10938215e-31;
        public const double ElectronRestMassRelativeStandardUncertainty = 5.0e-8;

        /// <summary>
        /// Elementary charge e in coulomb.
        /// </summary>
        public const double ElementaryCharge = 1.602176487e-19;
        public const double ElementaryChargeRelativeStandardUncertainty = 2.5e-8;

        /// <summary>
        /// Faraday constant in C/mol.
        /// </summary>
        public const double FaradayConstant = 96485.3383;
        public const double FaradayConstantRelativeStandardUncertainty = 8.6e-8;

        /// <summary>
        /// Fine structure constant (dimensionless).
        /// </summary>
        public const double FineStructureConstant = 7.2973525376e-3;
        public const double FineStructureConstantRelativeStandardUncertainty = 6.8e-10;

        /// <summary>
        ///  Newtonian constant of gravitation G in m^3/(kg s^2).
        /// </summary>
        public const double GravitationalConstant = 6.67428e-11;
        public const double GravitationalConstantRelativeStandardUncertainty = 1e-4;

        /// <summary>
        ///  Newtonian constant of gravitation G in m^3/(kg s^2).
        /// </summary>
        public const double G = GravitationalConstant;

        /// <summary>
        /// Ideal (molar) gas constant in J/(kg*mol).
        /// </summary>
        public const double IdealGasConstant = 8.314472;
        public const double IdealGasConstantRelativeStandardUncertainty = 1.7e-6;

        /// <summary>
        /// Neutron rest mass in kg.
        /// </summary>
        public const double NeutronRestMass = 1.6749286e-27;

        /// <summary>
        /// Planck constant h in Js.
        /// </summary>
        public const double PlanckConstant = 6.62606896e-34;
        public const double PlanckConstantRelativeStandardUncertainty = 5e-8;
        public const double ReducedPlanckConstant = PlanckConstant / Constant.PiTimesTwo;

        /// <summary>
        /// Planck constant h in Js.
        /// </summary>
        public const double h = PlanckConstant;

        /// <summary>
        /// Planck mass in kg.
        /// </summary>
        public const double PlanckMass = 2.17644e-8;
        public const double PlanckMassRelativeStandardUncertainty = 5.0e-5;

        /// <summary>
        /// Proton rest mass in kg.
        /// </summary>
        public const double ProtonRestMass = 1.672621637e-27;
        public const double ProtonRestMassRelativeStandardUncertainty = 5.0e-8;

        /// <summary>
        /// Rydberg constat in m^-1.
        /// </summary>
        public const double RydbergConstant = 10973731.568525;
        public const double RydbergConstantRelativeStandardUncertainty = 6.6e-12;

        /// <summary>
        /// Rydberg energy in eV.
        /// </summary>
        public const double RydbergEnergy = 13.605698140;

        /// <summary>
        /// Specific gas constant of dry air in J/(kg*K).
        /// </summary>
        public const double SpecificGasConstantOfDryAir = 287.05;

        /// <summary>
        /// Velocity of light in vacuum c in m/s. This is a defined value
        /// with no uncertainty.
        /// </summary>
        public const double SpeedOfLight = 299792458;

        /// <summary>
        /// Velocity of light in vacuum c in m/s. This is a defined value
        /// with no uncertainty.
        /// </summary>
        public const double c = SpeedOfLight;

        /// <summary>
        /// Standard acceleration of gravity g in m/s^2. This is a defined
        /// value with no uncertainty.
        /// </summary>
        public const double StandardAccelerationOfGravity = 9.80665;

        /// <summary>
        /// Standard acceleration of gravity g in m/s^2. This is a defined
        /// value with no uncertainty.
        /// </summary>
        public const double g = StandardAccelerationOfGravity;

        /// <summary>
        /// Standard atmosphere in Pa. This is a defined value with no
        /// uncertainty.
        /// </summary>
        public const double StandardAtmosphere = 101325;

        /// <summary>
        /// Standard atmosphere in Pa. This is a defined value with no
        /// uncertainty.
        /// </summary>
        public const double atm = StandardAtmosphere;

        /// <summary>
        /// Tropospheric temperature lapse rate in K/m.
        /// Source: http://www.iupac.org/goldbook/T06266.pdf
        /// </summary>
        public const double TroposphericTemperatureLapseRate = 0.0065;

        /// <summary>
        /// The characteristic impedance of vacuum in Ohm. This is a
        /// defined value with no uncertainty.
        /// </summary>
        public const double VacuumImpedance = my0 * c;

        /// <summary>
        /// The characteristic impedance of vacuum in Ohm. This is a
        /// defined value with no uncertainty.
        /// </summary>
        public const double Z0 = VacuumImpedance;

        /// <summary>
        /// Vacuum Permittivity in (A^2 s^4)/(kg m^3). This is a defined
        /// value with no uncertainty.
        /// </summary>
        public const double VacuumPermittivity = 1.0 / (my0 * c * c);

        /// <summary>
        /// Vacuum Permittivity in (A^2 s^4)/(kg m^3). This is a defined
        /// value with no uncertainty.
        /// </summary>
        public const double epsilon0 = VacuumPermittivity;

        /// <summary>
        /// Vacuum Permeability in N/A^2. This is a defined value with no
        /// uncertainty.
        /// </summary>
        public const double VacuumPermeability = 4.0E-7 * Constant.Pi;

        /// <summary>
        /// Vacuum Permeability in N/A^2. This is a defined value with no
        /// uncertainty.
        /// </summary>
        public const double my0 = VacuumPermeability;

        /// <summary>
        /// Molar mass of dry air in kg/mol.
        /// Source: http://en.wikipedia.org/wiki/Density_of_air
        /// </summary>
        public const double MolarMassOfDryAir = 0.0289644;

        /// <summary>
        /// Day, mean sidereal.
        /// </summary>
        public static readonly TimeSpan DayMeanSidereal = new TimeSpan(0, 23, 56, 4, 090);

        /// <summary>
        /// Light year in m.
        /// </summary>
        public const double LightYear = 9460730472580800.0;

        /// <summary>
        /// Parsec (au/arcsec) in m.
        /// </summary>
        public const double Parsec = 3.08567758074e16;

        /// <summary>
        /// Parsec (au/arcsec) in m.
        /// </summary>
        public const double pc = Parsec;

        /// <summary>
        /// Schwarzschild radius of Sun in m.
        /// </summary>
        public const double SchwarzschildRadiusOfSun = 2953.25008;

        /// <summary>
        /// Strong coupling constant.
        /// </summary>
        public const double StrongCouplingConstant = 0.1183;

        /// <summary>
        /// Sun distance from Core in m.
        /// </summary>
        public const double SunDistanceFromCore = 8.05 * Parsec;

        /// <summary>
        /// Sun luminosity in W.
        /// </summary>
        public const double SunLuminosity = 3.846e26;

        /// <summary>
        /// Sun mass in kg.
        /// </summary>
        public const double SunMass = 1.9889225e30;

        /// <summary>
        /// Sun radius (equatorial) in m.
        /// </summary>
        public const double SunRadiusEquatorial = 6.96e8;

        /// <summary>
        /// Sun speed around Core in m/s.
        /// </summary>
        public const double SunSpeedAroundCore = 22020000;

        /// <summary>
        /// Sun speed to cosmic background in m/s.
        /// </summary>
        public const double SunSpeedToCosmicBackground = 369500;

        /// <summary>
        /// Year, sideral (fixed star to fixed star, 1994) in s.
        /// </summary>
        public const double YearSideral = 31558149.8;

        /// <summary>
        /// Year, tropical (equinox to equinox, 1994) in s.
        /// </summary>
        public const double YearTropical = 31556925.2;

        /// <summary>
        /// Angstrom in m.
        /// </summary>
        public const double Angstrom = 10e-10;

        /// <summary>
        /// Electronvolt eV in J.
        /// </summary>
        public const double ElectronvoltInJoule = 1.60217733e-19;

        /// <summary>
        /// Nautical mile in m.
        /// </summary>
        public const double NauticalMile = 1852;

        /// <summary>
        /// Minute in seconds.
        /// </summary>
        public const double Minute = 60;

        /// <summary>
        /// Hour in seconds.
        /// </summary>
        public const double Hour = 60 * Minute;

        /// <summary>
        /// Day in seconds.
        /// </summary>
        public const double Day = 24 * Hour;

        /// <summary>
        /// Knot (1 nautical mile per hour) in m/s.
        /// </summary>
        public const double Knot = NauticalMile / Hour;

        /// <summary>
        /// Are in m^2.
        /// </summary>
        public const double Are = 100;

        /// <summary>
        /// Hectare in m^2.
        /// </summary>
        public const double Hectare = 10000;

        /// <summary>
        /// Barn in m^2.
        /// </summary>
        public const double Barn = 1e-28;

        /// <summary>
        /// Bar in Pa.
        /// </summary>
        public const double Bar = 10e5;

        /// <summary>
        /// Gal in m/s^2.
        /// </summary>
        public const double Gal = 10e-2;

        /// <summary>
        /// Curie Ci in Bq.
        /// </summary>
        public const double Curie = 3.7e10;

        /// <summary>
        /// Roentgen R in C/kg.
        /// </summary>
        public const double Roentgen = 2.58e-4;
    }
}
