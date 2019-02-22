
namespace DotSpatialTests
{


    public struct Distance
    {
        private double _value;
        private DistanceUnit _units;



        private const double FEET_PER_METER = 3.2808399;
        private const double FEET_PER_CENTIMETER = 0.032808399;
        private const double FEET_PER_STATUTE_MILE = 5280;
        private const double FEET_PER_KILOMETER = 3280.8399;
        private const double FEET_PER_INCH = 0.0833333333333333;
        private const double FEET_PER_NAUTICAL_MILE = 6076.11549;
        private const double INCHES_PER_METER = 39.3700787;
        private const double INCHES_PER_CENTIMETER = 0.393700787;
        private const double INCHES_PER_STATUTE_MILE = 63360;
        private const double INCHES_PER_KILOMETER = 39370.0787;
        private const double INCHES_PER_FOOT = 12.0;
        private const double INCHES_PER_NAUTICAL_MILE = 72913.3858;
        private const double STATUTE_MILES_PER_METER = 0.000621371192;
        private const double STATUTE_MILES_PER_CENTIMETER = 0.00000621371192;
        private const double STATUTE_MILES_PER_KILOMETER = 0.621371192;
        private const double STATUTE_MILES_PER_INCH = 0.0000157828283;
        private const double STATUTE_MILES_PER_FOOT = 0.000189393939;
        private const double STATUTE_MILES_PER_NAUTICAL_MILE = 1.15077945;
        private const double NAUTICAL_MILES_PER_METER = 0.000539956803;
        private const double NAUTICAL_MILES_PER_CENTIMETER = 0.00000539956803;
        private const double NAUTICAL_MILES_PER_KILOMETER = 0.539956803;
        private const double NAUTICAL_MILES_PER_INCH = 0.0000137149028;
        private const double NAUTICAL_MILES_PER_FOOT = 0.000164578834;
        private const double NAUTICAL_MILES_PER_STATUTE_MILE = 0.868976242;
        private const double CENTIMETERS_PER_STATUTE_MILE = 160934.4;
        private const double CENTIMETERS_PER_KILOMETER = 100000;
        private const double CENTIMETERS_PER_FOOT = 30.48;
        private const double CENTIMETERS_PER_INCH = 2.54;
        private const double CENTIMETERS_PER_METER = 100;
        private const double CENTIMETERS_PER_NAUTICAL_MILE = 185200;
        private const double METERS_PER_STATUTE_MILE = 1609.344;
        private const double METERS_PER_CENTIMETER = 0.01;
        private const double METERS_PER_KILOMETER = 1000;
        private const double METERS_PER_FOOT = 0.3048;
        private const double METERS_PER_INCH = 0.0254;
        private const double METERS_PER_NAUTICAL_MILE = 1852;
        private const double KILOMETERS_PER_METER = 0.001;
        private const double KILOMETERS_PER_CENTIMETER = 0.00001;
        private const double KILOMETERS_PER_STATUTE_MILE = 1.609344;
        private const double KILOMETERS_PER_FOOT = 0.0003048;
        private const double KILOMETERS_PER_INCH = 0.0000254;
        private const double KILOMETERS_PER_NAUTICAL_MILE = 1.852;
        

        public static readonly Distance Empty = new Distance(0, DistanceUnit.Meters).ToLocalUnitType();


        public DistanceUnit Units
        {
            get
            {
                return _units;
            }
        }


        public double Value
        {
            get
            {
                return _value;
            }
        }


        public Distance(double value, DistanceUnit units)
        {
            _value = value;
            _units = units;
        }

        /// <summary>
        /// Returns whether the value is zero.
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                return _value == 0;
            }
        }


        public Distance ToCentimeters()
        {
            switch (_units)
            {
                case DistanceUnit.Centimeters:
                    return this;
                case DistanceUnit.Meters:
                    return new Distance(_value * CENTIMETERS_PER_METER, DistanceUnit.Centimeters);
                case DistanceUnit.Feet:
                    return new Distance(_value * CENTIMETERS_PER_FOOT, DistanceUnit.Centimeters);
                case DistanceUnit.Inches:
                    return new Distance(_value * CENTIMETERS_PER_INCH, DistanceUnit.Centimeters);
                case DistanceUnit.Kilometers:
                    return new Distance(_value * CENTIMETERS_PER_KILOMETER, DistanceUnit.Centimeters);
                case DistanceUnit.StatuteMiles:
                    return new Distance(_value * CENTIMETERS_PER_STATUTE_MILE, DistanceUnit.Centimeters);
                case DistanceUnit.NauticalMiles:
                    return new Distance(_value * CENTIMETERS_PER_NAUTICAL_MILE, DistanceUnit.Centimeters);
                default:
                    return Empty;
            }
        }


        /// <summary>
        /// Attempts to adjust the unit type to keep the value above 1 and uses the local region measurement system.
        /// </summary>
        /// <returns>A <strong>Distance</strong> converted to the chosen unit type.</returns>
        /// <remarks>When a distance becomes smaller, it may make more sense to the
        /// user to be expressed in a smaller unit type.  For example, a distance of
        /// 0.001 kilometers might be better expressed as 1 meter.  This method will
        /// determine the smallest metric unit type.</remarks>
        public Distance ToMetricUnitType()
        {
            // Yes. Start with the largest possible unit
            Distance temp = ToKilometers();

            // If the value is less than one, bump down
            if (System.Math.Abs(temp.Value) < 1.0)
                temp = temp.ToMeters();

            // And so on until we find the right unit
            if (System.Math.Abs(temp.Value) < 1.0)
                temp = temp.ToCentimeters();

            return temp;
        }


        public Distance ToNauticalMiles()
        {
            switch (_units)
            {
                case DistanceUnit.Meters:
                    return new Distance(_value * NAUTICAL_MILES_PER_METER, DistanceUnit.NauticalMiles);
                case DistanceUnit.Centimeters:
                    return new Distance(_value * NAUTICAL_MILES_PER_CENTIMETER, DistanceUnit.NauticalMiles);
                case DistanceUnit.Feet:
                    return new Distance(_value * NAUTICAL_MILES_PER_FOOT, DistanceUnit.NauticalMiles);
                case DistanceUnit.Inches:
                    return new Distance(_value * NAUTICAL_MILES_PER_INCH, DistanceUnit.NauticalMiles);
                case DistanceUnit.Kilometers:
                    return new Distance(_value * NAUTICAL_MILES_PER_KILOMETER, DistanceUnit.NauticalMiles);
                case DistanceUnit.StatuteMiles:
                    return new Distance(_value * NAUTICAL_MILES_PER_STATUTE_MILE, DistanceUnit.NauticalMiles);
                case DistanceUnit.NauticalMiles:
                    return this;
                default:
                    return Empty;
            }
        }


        public Distance ToStatuteMiles()
        {
            switch (_units)
            {
                case DistanceUnit.Meters:
                    return new Distance(_value * STATUTE_MILES_PER_METER, DistanceUnit.StatuteMiles);
                case DistanceUnit.Centimeters:
                    return new Distance(_value * STATUTE_MILES_PER_CENTIMETER, DistanceUnit.StatuteMiles);
                case DistanceUnit.Feet:
                    return new Distance(_value * STATUTE_MILES_PER_FOOT, DistanceUnit.StatuteMiles);
                case DistanceUnit.Inches:
                    return new Distance(_value * STATUTE_MILES_PER_INCH, DistanceUnit.StatuteMiles);
                case DistanceUnit.Kilometers:
                    return new Distance(_value * STATUTE_MILES_PER_KILOMETER, DistanceUnit.StatuteMiles);
                case DistanceUnit.StatuteMiles:
                    return this;
                case DistanceUnit.NauticalMiles:
                    return new Distance(_value * STATUTE_MILES_PER_NAUTICAL_MILE, DistanceUnit.StatuteMiles);
                default:
                    return Empty;
            }
        }



        public Distance ToFeet()
        {
            switch (_units)
            {
                case DistanceUnit.Meters:
                    return new Distance(_value * FEET_PER_METER, DistanceUnit.Feet);
                case DistanceUnit.Centimeters:
                    return new Distance(_value * FEET_PER_CENTIMETER, DistanceUnit.Feet);
                case DistanceUnit.Feet:
                    return this;
                case DistanceUnit.Inches:
                    return new Distance(_value * FEET_PER_INCH, DistanceUnit.Feet);
                case DistanceUnit.Kilometers:
                    return new Distance(_value * FEET_PER_KILOMETER, DistanceUnit.Feet);
                case DistanceUnit.StatuteMiles:
                    return new Distance(_value * FEET_PER_STATUTE_MILE, DistanceUnit.Feet);
                case DistanceUnit.NauticalMiles:
                    return new Distance(_value * FEET_PER_NAUTICAL_MILE, DistanceUnit.Feet);
                default:
                    return Empty;
            }
        }


        public Distance ToInches()
        {
            switch (_units)
            {
                case DistanceUnit.Meters:
                    return new Distance(_value * INCHES_PER_METER, DistanceUnit.Inches);
                case DistanceUnit.Centimeters:
                    return new Distance(_value * INCHES_PER_CENTIMETER, DistanceUnit.Inches);
                case DistanceUnit.Feet:
                    return new Distance(_value * INCHES_PER_FOOT, DistanceUnit.Inches);
                case DistanceUnit.Inches:
                    return this;
                case DistanceUnit.Kilometers:
                    return new Distance(_value * INCHES_PER_KILOMETER, DistanceUnit.Inches);
                case DistanceUnit.StatuteMiles:
                    return new Distance(_value * INCHES_PER_STATUTE_MILE, DistanceUnit.Inches);
                case DistanceUnit.NauticalMiles:
                    return new Distance(_value * INCHES_PER_NAUTICAL_MILE, DistanceUnit.Inches);
                default:
                    return Empty;
            }
        }


        /// <summary>
        /// Attempts to adjust the unit type to keep the value above 1 and uses the local region measurement system.
        /// </summary>
        /// <returns>A <strong>Distance</strong> converted to the chosen unit type.</returns>
        /// <remarks>When a distance becomes smaller, it may make more sense to the
        /// user to be expressed in a smaller unit type.  For example, a distance of
        /// 0.001 kilometers might be better expressed as 1 meter.  This method will
        /// determine the smallest Imperial unit type.</remarks>
        public Distance ToImperialUnitType()
        {
            // Start with the largest possible unit
            Distance temp = ToStatuteMiles();
            // If the value is less than one, bump down
            if (System.Math.Abs(temp.Value) < 1.0)
                temp = temp.ToFeet();
            if (System.Math.Abs(temp.Value) < 1.0)
                temp = temp.ToInches();
            if (System.Math.Abs(temp.Value) < 1.0)
                temp = temp.ToCentimeters();
            return temp;
        }

        public Distance ToLocalUnitType()
        {
            // Find the largest possible units in the local region's system
            if (System.Globalization.RegionInfo.CurrentRegion.IsMetric)
                return ToMetricUnitType();

            return ToImperialUnitType();
        }


        /// <summary>
        /// Froms the meters.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static Distance FromMeters(double value)
        {
            return new Distance(value, DistanceUnit.Meters);
        }


        public Distance ToMeters()
        {
            switch (_units)
            {
                case DistanceUnit.Meters:
                    return this;
                case DistanceUnit.Centimeters:
                    return new Distance(_value * METERS_PER_CENTIMETER, DistanceUnit.Meters);
                case DistanceUnit.Feet:
                    return new Distance(_value * METERS_PER_FOOT, DistanceUnit.Meters);
                case DistanceUnit.Inches:
                    return new Distance(_value * METERS_PER_INCH, DistanceUnit.Meters);
                case DistanceUnit.Kilometers:
                    return new Distance(_value * METERS_PER_KILOMETER, DistanceUnit.Meters);
                case DistanceUnit.StatuteMiles:
                    return new Distance(_value * METERS_PER_STATUTE_MILE, DistanceUnit.Meters);
                case DistanceUnit.NauticalMiles:
                    return new Distance(_value * METERS_PER_NAUTICAL_MILE, DistanceUnit.Meters);
                default:
                    return Empty;
            }
        }




        public Speed ToSpeed(System.TimeSpan time)
        {
            return new Speed(ToMeters().Value / (time.TotalMilliseconds / 1000.0), SpeedUnit.MetersPerSecond).ToLocalUnitType();
        }


        public Distance ToKilometers()
        {
            switch (_units)
            {
                case DistanceUnit.Meters:
                    return new Distance(_value * KILOMETERS_PER_METER, DistanceUnit.Kilometers);
                case DistanceUnit.Centimeters:
                    return new Distance(_value * KILOMETERS_PER_CENTIMETER, DistanceUnit.Kilometers);
                case DistanceUnit.Feet:
                    return new Distance(_value * KILOMETERS_PER_FOOT, DistanceUnit.Kilometers);
                case DistanceUnit.Inches:
                    return new Distance(_value * KILOMETERS_PER_INCH, DistanceUnit.Kilometers);
                case DistanceUnit.Kilometers:
                    return this;
                case DistanceUnit.StatuteMiles:
                    return new Distance(_value * KILOMETERS_PER_STATUTE_MILE, DistanceUnit.Kilometers);
                case DistanceUnit.NauticalMiles:
                    return new Distance(_value * KILOMETERS_PER_NAUTICAL_MILE, DistanceUnit.Kilometers);
                default:
                    return Empty;
            }
        }

    }
    

}
