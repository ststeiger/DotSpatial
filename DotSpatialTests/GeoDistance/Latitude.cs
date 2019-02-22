
namespace DotSpatialTests
{

    public struct Latitude
    {
        private double _decimalDegrees;

        public Latitude(double decimalDegrees)
        {
            _decimalDegrees = decimalDegrees;
        }

        public Radian ToRadians()
        {
            return new Radian(_decimalDegrees * Radian.RADIANS_PER_DEGREE);
        }

        /// <summary>
        /// Indicates whether the value has been normalized and is within the
        /// allowed bounds of -90° and 90°.
        /// </summary>
        public bool IsNormalized
        {
            get { return _decimalDegrees >= -90 && _decimalDegrees <= 90; }
        }


        /// <summary>
        /// Causes the value to be adjusted to between -90 and +90.
        /// </summary>
        /// <returns></returns>
        public Latitude Normalize()
        {
            // Is the value not a number, infinity, or already normalized?
            if (double.IsInfinity(_decimalDegrees)
                || double.IsNaN(_decimalDegrees)
                || IsNormalized)
                return this;

            // Calculate the number of times the degree value winds completely
            // through a hemisphere
            int hemisphereFlips = System.Convert.ToInt32(System.Math.Floor(_decimalDegrees / 180.0));

            // If the value is in the southern hemisphere, apply another flip
            if (_decimalDegrees < 0)
                hemisphereFlips++;

            // Calculate the new value
            double newValue = _decimalDegrees % 180;

            // if the value is > 90, return 180 - X
            if (newValue > 90)
                newValue = 180 - newValue;

            // If the value id < -180, return -180 - X
            else if (newValue < -90.0)
                newValue = -180.0 - newValue;

            // Account for flips around hemispheres by flipping the sign
            if (hemisphereFlips % 2 != 0)
                return new Latitude(-newValue);
            return new Latitude(newValue);
        }

    }

}
