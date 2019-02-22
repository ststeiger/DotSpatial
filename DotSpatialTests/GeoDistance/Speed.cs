
namespace DotSpatialTests
{

    public struct Speed
    {
        private double _value;
        private SpeedUnit _units;


        private const double STATUTE_MPH_PER_KNOT = 0.8689762;
        private const double KPH_PER_KNOT = 0.5399568;
        private const double FPS_PER_KNOT = 0.5924838;
        private const double MPS_PER_KNOT = 1.943845;
        private const double KPS_PER_KNOT = 1943.845;
        private const double KNOTS_PER_STATUTE_MPH = 1.150779;
        private const double KPH_PER_STATUTE_MPH = 0.6213712;
        private const double FPS_PER_STATUTE_MPH = 0.6818182;
        private const double MPS_PER_STATUTE_MPH = 2.236936;
        private const double KPS_PER_STATUTE_MPH = 2236.936;
        private const double KNOTS_PER_KPH = 1.852;
        private const double STATUTE_MPH_PER_KPH = 1.609344;
        private const double FPS_PER_KPH = 1.09728;
        private const double MPS_PER_KPH = 3.6;
        private const double KPS_PER_KPH = 3600;
        private const double KNOTS_PER_KPS = 0.0005144444;
        private const double STATUTE_MPH_PER_KPS = 0.000447;
        private const double FPS_PER_KPS = 0.0003048;
        private const double MPS_PER_KPS = 0.001;
        private const double KPH_PER_KPS = 0.0002777778;
        private const double KNOTS_PER_FPS = 1.68781;
        private const double STATUTE_MPH_PER_FPS = 1.466667;
        private const double KPH_PER_FPS = 0.9113444;
        private const double MPS_PER_FPS = 3.28084;
        private const double KPS_PER_FPS = 3280.84;
        private const double KNOTS_PER_MPS = 0.5144444;
        private const double STATUTE_MPH_PER_MPS = 0.447;
        private const double FPS_PER_MPS = 0.3048;
        private const double KPH_PER_MPS = 0.2777778;
        private const double KPS_PER_MPS = 1000;


        public static readonly Speed Empty = new Speed(0, SpeedUnit.MetersPerSecond);
        public static readonly Speed AtRest = new Speed(0, SpeedUnit.MetersPerSecond);
        public static readonly Speed SpeedOfLight = new Speed(299792458, SpeedUnit.MetersPerSecond);
        public static readonly Speed Maximum = new Speed(double.MaxValue, SpeedUnit.KilometersPerSecond).ToLocalUnitType();
        public static readonly Speed Minimum = new Speed(double.MinValue, SpeedUnit.KilometersPerSecond).ToLocalUnitType();
        public static readonly Speed SpeedOfSoundAtSeaLevel = new Speed(340.29, SpeedUnit.MetersPerSecond);
        public static readonly Speed Infinity = new Speed(double.PositiveInfinity, SpeedUnit.MetersPerSecond);
        public static readonly Speed Invalid = new Speed(double.NaN, SpeedUnit.KilometersPerSecond);




        public double Value
        {
            get
            {
                return _value;
            }
        }

        public SpeedUnit Units
        {
            get
            {
                return _units;
            }
        }


        public bool IsEmpty
        {
            get
            {
                return _value == 0;
            }
        }

        public bool IsMetric
        {
            get
            {
                return _units == SpeedUnit.KilometersPerHour
                    || _units == SpeedUnit.KilometersPerSecond
                    || _units == SpeedUnit.MetersPerSecond;
            }
        }

        public bool IsInvalid
        {
            get { return double.IsNaN(_value); }
        }

        public Speed(double value, SpeedUnit units)
        {
            _value = value;
            _units = units;
        }


        public Speed ToKilometersPerSecond() //'Implements ISpeed.ToKilometersPerSecond
        {
            switch (Units)
            {
                case SpeedUnit.StatuteMilesPerHour:
                    return new Speed(Value * STATUTE_MPH_PER_KPS, SpeedUnit.KilometersPerSecond);
                case SpeedUnit.KilometersPerHour:
                    return new Speed(Value * KPH_PER_KPS, SpeedUnit.KilometersPerSecond);
                case SpeedUnit.KilometersPerSecond:
                    return this;
                case SpeedUnit.FeetPerSecond:
                    return new Speed(Value * FPS_PER_KPS, SpeedUnit.KilometersPerSecond);
                case SpeedUnit.MetersPerSecond:
                    return new Speed(Value * MPS_PER_KPS, SpeedUnit.KilometersPerSecond);
                case SpeedUnit.Knots:
                    return new Speed(Value * KNOTS_PER_KPS, SpeedUnit.KilometersPerSecond);
                default:
                    return Empty;
            }
        }


        public Speed ToMetersPerSecond() //'Implements ISpeed.ToMetersPerSecond
        {
            switch (Units)
            {
                case SpeedUnit.StatuteMilesPerHour:
                    return new Speed(Value * STATUTE_MPH_PER_MPS, SpeedUnit.MetersPerSecond);
                case SpeedUnit.KilometersPerHour:
                    return new Speed(Value * KPH_PER_MPS, SpeedUnit.MetersPerSecond);
                case SpeedUnit.KilometersPerSecond:
                    return new Speed(Value * KPS_PER_MPS, SpeedUnit.MetersPerSecond);
                case SpeedUnit.FeetPerSecond:
                    return new Speed(Value * FPS_PER_MPS, SpeedUnit.MetersPerSecond);
                case SpeedUnit.MetersPerSecond:
                    return this;
                case SpeedUnit.Knots:
                    return new Speed(Value * KNOTS_PER_MPS, SpeedUnit.MetersPerSecond);
                default:
                    return Empty;
            }
        }


        public Speed ToMetricUnitType()
        {
            // Start with the largest possible unit
            Speed temp = ToKilometersPerHour();
            // If the value is less than one, bump down
            if (System.Math.Abs(temp.Value) < 1.0)
                temp = temp.ToMetersPerSecond();
            // And so on until we find the right unit
            if (System.Math.Abs(temp.Value) < 1.0)
                temp = temp.ToKilometersPerSecond();
            return temp;
        }

        public Speed ToImperialUnitType()
        {
            // Start with the largest possible unit
            Speed temp = ToStatuteMilesPerHour();
            // If the value is less than one, bump down
            if (System.Math.Abs(temp.Value) < 1.0)
                temp = temp.ToFeetPerSecond();
            return temp;
        }

        public Speed ToLocalUnitType()
        {
            // Find the largest possible units in the local region's system
            if (System.Globalization.RegionInfo.CurrentRegion.IsMetric)
                return ToMetricUnitType();
            return ToImperialUnitType();
        }


        public Speed ToUnitType(SpeedUnit value)
        {
            switch (value)
            {
                case SpeedUnit.FeetPerSecond:
                    return ToFeetPerSecond();
                case SpeedUnit.KilometersPerHour:
                    return ToKilometersPerHour();
                case SpeedUnit.KilometersPerSecond:
                    return ToKilometersPerSecond();
                case SpeedUnit.Knots:
                    return ToKnots();
                case SpeedUnit.MetersPerSecond:
                    return ToMetersPerSecond();
                case SpeedUnit.StatuteMilesPerHour:
                    return ToStatuteMilesPerHour();
                default:
                    return Empty;
            }
        }


        public Speed ToKnots() //'Implements ISpeed.ToKnots
        {
            switch (Units)
            {
                case SpeedUnit.StatuteMilesPerHour:
                    return new Speed(Value * STATUTE_MPH_PER_KNOT, SpeedUnit.Knots);
                case SpeedUnit.KilometersPerHour:
                    return new Speed(Value * KPH_PER_KNOT, SpeedUnit.Knots);
                case SpeedUnit.KilometersPerSecond:
                    return new Speed(Value * KPS_PER_KNOT, SpeedUnit.Knots);
                case SpeedUnit.FeetPerSecond:
                    return new Speed(Value * FPS_PER_KNOT, SpeedUnit.Knots);
                case SpeedUnit.MetersPerSecond:
                    return new Speed(Value * MPS_PER_KNOT, SpeedUnit.Knots);
                case SpeedUnit.Knots:
                    return this;
                default:
                    return Empty;
            }
        }


        public Speed ToStatuteMilesPerHour()
        {
            switch (Units)
            {
                case SpeedUnit.StatuteMilesPerHour:
                    return this;
                case SpeedUnit.KilometersPerHour:
                    return new Speed(Value * KPH_PER_STATUTE_MPH, SpeedUnit.StatuteMilesPerHour);
                case SpeedUnit.KilometersPerSecond:
                    return new Speed(Value * KPS_PER_STATUTE_MPH, SpeedUnit.StatuteMilesPerHour);
                case SpeedUnit.FeetPerSecond:
                    return new Speed(Value * FPS_PER_STATUTE_MPH, SpeedUnit.StatuteMilesPerHour);
                case SpeedUnit.MetersPerSecond:
                    return new Speed(Value * MPS_PER_STATUTE_MPH, SpeedUnit.StatuteMilesPerHour);
                case SpeedUnit.Knots:
                    return new Speed(Value * KNOTS_PER_STATUTE_MPH, SpeedUnit.StatuteMilesPerHour);
                default:
                    return Empty;
            }
        }


        /// <summary>
        /// Returns the current instance converted to feet per second.
        /// </summary>
        /// <returns></returns>
        /// <remarks>The measurement is converted regardless of its current unit type.</remarks>
        public Speed ToFeetPerSecond() //'Implements ISpeed.ToFeetPerSecond
        {
            switch (Units)
            {
                case SpeedUnit.StatuteMilesPerHour:
                    return new Speed(Value * STATUTE_MPH_PER_FPS, SpeedUnit.FeetPerSecond);
                case SpeedUnit.KilometersPerHour:
                    return new Speed(Value * KPH_PER_FPS, SpeedUnit.FeetPerSecond);
                case SpeedUnit.KilometersPerSecond:
                    return new Speed(Value * KPS_PER_FPS, SpeedUnit.FeetPerSecond);
                case SpeedUnit.FeetPerSecond:
                    return this;
                case SpeedUnit.MetersPerSecond:
                    return new Speed(Value * MPS_PER_FPS, SpeedUnit.FeetPerSecond);
                case SpeedUnit.Knots:
                    return new Speed(Value * KNOTS_PER_FPS, SpeedUnit.FeetPerSecond);
                default:
                    return Empty;
            }
        }

        /// <summary>
        /// Converts the current measurement into kilometers per hour.
        /// </summary>
        /// <returns></returns>
        /// <remarks>The measurement is converted regardless of its current unit type.</remarks>
        public Speed ToKilometersPerHour() //'Implements ISpeed.ToKilometersPerHour
        {
            switch (Units)
            {
                case SpeedUnit.StatuteMilesPerHour:
                    return new Speed(Value * STATUTE_MPH_PER_KPH, SpeedUnit.KilometersPerHour);
                case SpeedUnit.KilometersPerHour:
                    return this;
                case SpeedUnit.FeetPerSecond:
                    return new Speed(Value * FPS_PER_KPH, SpeedUnit.KilometersPerHour);
                case SpeedUnit.MetersPerSecond:
                    return new Speed(Value * MPS_PER_KPH, SpeedUnit.KilometersPerHour);
                case SpeedUnit.Knots:
                    return new Speed(Value * KNOTS_PER_KPH, SpeedUnit.KilometersPerHour);
                case SpeedUnit.KilometersPerSecond:
                    return new Speed(Value * KPS_PER_KPH, SpeedUnit.KilometersPerHour);
                default:
                    return Empty;
            }
        }

    }


}
