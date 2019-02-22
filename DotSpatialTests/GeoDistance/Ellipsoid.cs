
namespace DotSpatialTests
{

    public class Ellipsoid
    {

        private Distance _equatorialRadius;
        private Distance _polarRadius;
        private double _equatorialRadiusMeters;
        private double _polarRadiusMeters;
        private double _flattening;
        private double _inverseFlattening;
        private double _eccentricity;
        private double _eccentricitySquared;



        private string _name;
        private int _epsgNumber = 32767;

        /// <summary>
        /// Represents the World Geodetic System ellipsoid of 1984.
        /// </summary>
        public static readonly Ellipsoid Wgs1984 = new Ellipsoid(7030, 6378137, 298.2572236, 0, "WGS 84");



        /// <summary>
        /// Calculates the common ellipsoid properties. Called from the constructor
        /// </summary>
        private void Calculate()
        {
            double a = _equatorialRadius.ToMeters().Value;
            double b = _polarRadius.ToMeters().Value;
            double invf = _inverseFlattening;

            // Check the input. If a minor axis wasn't supplied, calculate it.
            if (b == 0) b = -(((1.0 / invf) * a) - a);

            _polarRadius = Distance.FromMeters(b);

            _flattening = (_equatorialRadius.ToMeters().Value - _polarRadius.ToMeters().Value) / _equatorialRadius.ToMeters().Value;
            _inverseFlattening = 1.0 / _flattening;
            _eccentricity = System.Math.Sqrt((System.Math.Pow(_equatorialRadius.Value, 2) - System.Math.Pow(_polarRadius.Value, 2)) / System.Math.Pow(_equatorialRadius.Value, 2));
            _eccentricitySquared = System.Math.Pow(Eccentricity, 2);

            // This is used very frequently by calculations.  Since ellipsoids do not change, there's
            // no need to call .ToMeters() thousands of times.
            _equatorialRadiusMeters = _equatorialRadius.ToMeters().Value;
            _polarRadiusMeters = _polarRadius.ToMeters().Value;
        }

        /// <summary>
        /// Validates the ellipsoid. Called in the constructor.
        /// </summary>
        private void SanityCheck()
        {
            if ((_equatorialRadius.IsEmpty && _inverseFlattening == 0) || (_equatorialRadius.IsEmpty && _polarRadius.IsEmpty))
                throw new System.ArgumentException("The radii and inverse flattening of an allipsoid cannot be zero.   Please specify either the equatorial and polar radius, or the equatorial radius and the inverse flattening for this ellipsoid.");
        }

        /// <summary>
        /// Internal constructor for static list generation
        /// </summary>
        /// <param name="epsgNumber">The epsg number.</param>
        /// <param name="a">A.</param>
        /// <param name="invf">The invf.</param>
        /// <param name="b">The b.</param>
        /// <param name="name">The name.</param>
        internal Ellipsoid(int epsgNumber, double a, double invf, double b, string name)
        {
            _name = name;
            _epsgNumber = epsgNumber;
            _equatorialRadius = Distance.FromMeters(a);
            _polarRadius = Distance.FromMeters(b);
            _inverseFlattening = invf;
            Calculate();

            SanityCheck();

            // _epsgEllipsoids.Add(this);
        }


        /// <summary>
        /// Returns the rate of flattening of the ellipsoid.
        /// </summary>
        /// <value>A <strong>Double</strong> measuring how elongated the ellipsoid is.</value>
        /// <remarks>The eccentricity is a positive number less than 1, or 0 in the case of a circle.
        /// The greater the eccentricity is, the larger the ratio of the equatorial radius to the
        /// polar radius is, and therefore the more elongated the ellipse is.</remarks>
        public double Eccentricity
        {
            get
            {
                return _eccentricity;
            }
        }


        /// <summary>
        /// Represents the distance from Earth's center to the equator.
        /// </summary>
        /// <value>A <strong>Distance</strong> object.</value>
        /// <seealso cref="PolarRadius">PolarRadius Property</seealso>
        /// <remarks>This property defines the radius of the Earth from its center to the equator.
        /// This property is used in conjunction with the <strong>PolarRadius</strong> property
        /// to define an ellipsoidal shape. This property returns the same value as the
        /// <strong>SemiMajorAxis</strong> property.</remarks>
        public Distance EquatorialRadius
        {
            get { return _equatorialRadius; }
        }


        /// <summary>
        /// Represents the distance from Earth's center to the North or South pole.
        /// </summary>
        /// <value>A <strong>Distance</strong> object.</value>
        /// <seealso cref="EquatorialRadius">EquatorialRadius Property</seealso>
        /// <remarks>This property defines the radius of the Earth from its center to the equator.
        /// This property is used in conjunction with the <strong>EquatorialRadius</strong>
        /// property to define an ellipsoidal shape. This property returns the same value as
        /// the <strong>SemiMinorAxis</strong> property.</remarks>
        public Distance PolarRadius
        {
            get { return _polarRadius; }
        }

        /// <summary>
        /// Gets the polar radius meters.
        /// </summary>
        internal double PolarRadiusMeters
        {
            get
            {
                return _polarRadiusMeters;
            }
        }

        /// <summary>
        /// Gets the equatorial radius meters.
        /// </summary>
        internal double EquatorialRadiusMeters
        {
            get
            {
                return _equatorialRadiusMeters;
            }
        }

        /// <summary>
        /// Indicates the shape of the ellipsoid relative to a sphere.
        /// </summary>
        /// <value>A <strong>Double</strong> containing the ellipsoid's flattening.</value>
        /// <seealso cref="EquatorialRadius">EquatorialRadius Property</seealso>
        /// <remarks>This property compares the equatorial radius with the polar radius to measure the
        /// amount that the ellipsoid is "squished" vertically.</remarks>
        public double Flattening
        {
            get
            {
                return _flattening;
            }
        }

    }


}
