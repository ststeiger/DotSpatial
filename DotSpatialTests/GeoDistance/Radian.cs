
namespace DotSpatialTests
{
    public struct Radian
    {
        public const double RADIANS_PER_DEGREE = System.Math.PI / 180.0;
        public const double DEGREES_PER_RADIAN = 180.0 / System.Math.PI;

        private double _value;

        public Radian(double value)
        {
            _value = value;
        }

        public double Value
        {
            get
            {
                return _value;
            }
        }

    }
}
