
namespace DotSpatialTests
{

    public struct Position
    {
        private Latitude _latitude;
        private Longitude _longitude;

        // Accuracy is set to 1.0E-12, the smallest value allowed by a Latitude or Longitude
        /// <summary>
        ///
        /// </summary>
        private const double TARGET_ACCURACY = 1.0E-12;


        public Position(Longitude longitude, Latitude latitude)
        {
            _latitude = latitude;
            _longitude = longitude;
        }


        public Position(Latitude latitude, Longitude longitude)
        {
            _latitude = latitude;
            _longitude = longitude;
        }


        public Latitude Latitude
        {
            get { return _latitude; }
        }

        public Longitude Longitude
        {
            get { return _longitude; }
        }


        public Distance DistanceTo(Position destination)
        {
            return DistanceTo(destination, Ellipsoid.Wgs1984);
        }




        /// <summary>
        /// Returns the distance over land from the given starting point to the specified
        /// destination.
        /// </summary>
        /// <param name="destination">The ending point of a segment.</param>
        /// <param name="ellipsoid">The model of the Earth to use for the distance calculation.</param>
        /// <returns>A <strong>Distance</strong> object containing the calculated distance in
        /// kilometers.</returns>
        /// <overloads>Calculates the great circle distance between any two points on
        /// Earth using a specific model of Earth's shape.</overloads>
        /// <remarks>This method uses trigonometry to calculate the Great Circle (over Earth's curved
        /// surface) distance between any two points on Earth. The distance is returned in
        /// kilometers but can be converted to any other unit type using methods in the
        /// <see cref="Distance">Distance</see>
        /// class.</remarks>
        public Distance DistanceTo(Position destination, Ellipsoid ellipsoid)
        {
            // From: http://www.mathworks.com/matlabcentral/files/8607/vdist.m


            // If positions are equivalent, return zero
            if (Equals(destination))
                return Distance.Empty;


            double goodAlpha = 0;
            double goodSigma = 0;
            double goodCos2SigmaM = 0;

            //            % reshape inputs
            // keepsize = size(lat1);
            // lat1=lat1(:);
            // lon1=lon1(:);
            // lat2=lat2(:);
            // lon2=lon2(:);

            // ?

            //% Input check:
            // if any(abs(lat1)>90 | abs(lat2)>90)
            //    error('Input latitudes must be between -90 and 90 degrees, inclusive.')
            // end

            // The -90 to 90 check is handled by Normalize

            //% Supply WGS84 earth ellipsoid axis lengths in meters:
            // a = 6378137; % definitionally
            // b = 6356752.31424518; % computed from WGS84 earth flattening coefficient

            double a = ellipsoid.EquatorialRadiusMeters;
            double b = ellipsoid.PolarRadiusMeters;

            //% preserve true input latitudes:
            // lat1tr = lat1;
            // lat2tr = lat2;

            // double lat1tr = pLatitude.DecimalDegrees;
            // double lat2tr = destination.Latitude.DecimalDegrees;

            //% convert inputs in degrees to radians:
            // lat1 = lat1 * 0.0174532925199433;
            // lon1 = lon1 * 0.0174532925199433;
            // lat2 = lat2 * 0.0174532925199433;
            // lon2 = lon2 * 0.0174532925199433;

            // Convert inputs into radians
            double lat1 = Latitude.Normalize().ToRadians().Value;
            double lon1 = Longitude.Normalize().ToRadians().Value;
            double lat2 = destination.Latitude.Normalize().ToRadians().Value;
            double lon2 = destination.Longitude.Normalize().ToRadians().Value;

            //% correct for errors at exact poles by adjusting 0.6 millimeters:
            // kidx = abs(pi/2-abs(lat1)) < 1e-10;
            // if any(kidx);
            //    lat1(kidx) = sign(lat1(kidx))*(pi/2-(1e-10));
            // end

            // Correct for errors at exact poles by adjusting 0.6mm
            if (System.Math.Abs(System.Math.PI * 0.5 - System.Math.Abs(lat1)) < 1E-10)
            {
                lat1 = System.Math.Sign(lat1) * (System.Math.PI * 0.5 - 1E-10);
            }

            // kidx = abs(pi/2-abs(lat2)) < 1e-10;
            // if any(kidx)
            //    lat2(kidx) = sign(lat2(kidx))*(pi/2-(1e-10));
            // end

            if (System.Math.Abs(System.Math.PI * 0.5 - System.Math.Abs(lat2)) < 1E-10)
            {
                lat2 = System.Math.Sign(lat2) * (System.Math.PI * 0.5 - 1E-10);
            }

            // f = (a-b)/a;

            double f = ellipsoid.Flattening;

            // U1 = atan((1-f)*tan(lat1));

            double u1 = System.Math.Atan((1 - f) * System.Math.Tan(lat1));

            // U2 = atan((1-f)*tan(lat2));

            double u2A = System.Math.Atan((1 - f) * System.Math.Tan(lat2));

            // lon1 = mod(lon1, 2*pi);

            lon1 = lon1 % (2 * System.Math.PI);

            // lon2 = mod(lon2, 2*pi);

            lon2 = lon2 % (2 * System.Math.PI);

            // L = abs(lon2-lon1);

            double l = System.Math.Abs(lon2 - lon1);

            // kidx = L > pi;
            // if any(kidx)
            //    L(kidx) = 2*pi - L(kidx);
            // end

            if (l > System.Math.PI)
            {
                l = 2.0 * System.Math.PI - l;
            }

            // lambda = L;

            double lambda = l;

            // lambdaold = 0*lat1;

            // itercount = 0;

            int itercount = 0;

            // notdone = logical(1+0*lat1);

            bool notdone = true;

            // alpha = 0*lat1;

            // sigma = 0*lat1;

            // cos2sigmam = 0*lat1;

            // C = 0*lat1;

            // warninggiven = logical(0);

            // bool warninggiven = false;

            // while any(notdone)  % force at least one execution

            while (notdone)
            {
                //    %disp(['lambda(21752) = ' num2str(lambda(21752), 20)]);
                //    itercount = itercount+1;

                itercount++;

                //    if itercount > 50

                if (itercount > 50)
                {
                    //        if ~warninggiven

                    // if (!warninggiven)
                    //{
                    //    //            warning(['Essentially antipodal points encountered. ' ...
                    //    //                'Precision may be reduced slightly.']);

                    //    warninggiven = true;
                    //    throw new WarningException("Distance calculation accuracy may be reduced because the two endpoints are antipodal.");
                    //}

                    //        end
                    //        lambda(notdone) = pi;

                    // lambda = System.Math.PI;

                    //        break

                    break;

                    //    end
                }

                //    lambdaold(notdone) = lambda(notdone);

                double lambdaold = lambda;

                //    sinsigma(notdone) = sqrt((cos(U2(notdone)).*sin(lambda(notdone)))...
                //        .^2+(cos(U1(notdone)).*sin(U2(notdone))-sin(U1(notdone)).*...
                //        cos(U2(notdone)).*cos(lambda(notdone))).^2);

                double sinsigma = System.Math.Sqrt(System.Math.Pow((System.Math.Cos(u2A) * System.Math.Sin(lambda)), 2)
                        + System.Math.Pow((System.Math.Cos(u1) * System.Math.Sin(u2A) - System.Math.Sin(u1) *
                        System.Math.Cos(u2A) * System.Math.Cos(lambda)), 2));

                //    cossigma(notdone) = sin(U1(notdone)).*sin(U2(notdone))+...
                //        cos(U1(notdone)).*cos(U2(notdone)).*cos(lambda(notdone));

                double cossigma = System.Math.Sin(u1) * System.Math.Sin(u2A) +
                    System.Math.Cos(u1) * System.Math.Cos(u2A) * System.Math.Cos(lambda);

                //    % eliminate rare imaginary portions at limit of numerical precision:
                //    sinsigma(notdone)=real(sinsigma(notdone));
                //    cossigma(notdone)=real(cossigma(notdone));

                // Eliminate rare imaginary portions at limit of numerical precision:
                // ?

                //    sigma(notdone) = atan2(sinsigma(notdone), cossigma(notdone));

                double sigma = System.Math.Atan2(sinsigma, cossigma);

                //    alpha(notdone) = asin(cos(U1(notdone)).*cos(U2(notdone)).*...
                //        sin(lambda(notdone))./sin(sigma(notdone)));

                double alpha = System.Math.Asin(System.Math.Cos(u1) * System.Math.Cos(u2A) *
                                         System.Math.Sin(lambda) / System.Math.Sin(sigma));

                //    cos2sigmam(notdone) = cos(sigma(notdone))-2*sin(U1(notdone)).*...
                //        sin(U2(notdone))./cos(alpha(notdone)).^2;

                double cos2SigmaM = System.Math.Cos(sigma) - 2.0 * System.Math.Sin(u1) *
                                    System.Math.Sin(u2A) / System.Math.Pow(System.Math.Cos(alpha), 2);

                //    C(notdone) = f/16*cos(alpha(notdone)).^2.*(4+f*(4-3*...
                //        cos(alpha(notdone)).^2));

                double c = f / 16 * System.Math.Pow(System.Math.Cos(alpha), 2) * (4 + f * (4 - 3 *
                                                                             System.Math.Pow(System.Math.Cos(alpha), 2)));

                //    lambda(notdone) = L(notdone)+(1-C(notdone)).*f.*sin(alpha(notdone))...
                //        .*(sigma(notdone)+C(notdone).*sin(sigma(notdone)).*...
                //        (cos2sigmam(notdone)+C(notdone).*cos(sigma(notdone)).*...
                //        (-1+2.*cos2sigmam(notdone).^2)));

                lambda = l + (1 - c) * f * System.Math.Sin(alpha)
                            * (sigma + c * System.Math.Sin(sigma) *
                            (cos2SigmaM + c * System.Math.Cos(sigma) *
                            (-1 + 2 * System.Math.Pow(cos2SigmaM, 2))));

                //    %disp(['then, lambda(21752) = ' num2str(lambda(21752), 20)]);
                //    % correct for convergence failure in the case of essentially antipodal
                //    % points

                // Correct for convergence failure in the case of essentially antipodal points

                //    if any(lambda(notdone) > pi)

                if (lambda > System.Math.PI)
                {
                    //        if ~warninggiven

                    // if (!warninggiven)
                    //{
                    //    //            warning(['Essentially antipodal points encountered. ' ...
                    //    //                'Precision may be reduced slightly.']);

                    //    warninggiven = true;
                    //    throw new WarningException("Distance calculation accuracy may be reduced because the two endpoints are antipodal.");
                    //}

                    //        end

                    //        lambdaold(lambda>pi) = pi;

                    lambdaold = System.Math.PI;

                    //        lambda(lambda>pi) = pi;

                    lambda = System.Math.PI;

                    //    end
                }

                //    notdone = abs(lambda-lambdaold) > 1e-12;

                notdone = System.Math.Abs(lambda - lambdaold) > TARGET_ACCURACY;

                // end

                // notice In some cases "alpha" would return a "NaN".  If values are healthy,
                // remember them so we get a good distance calc.
                if (!double.IsNaN(alpha))
                {
                    goodAlpha = alpha;
                    goodSigma = sigma;
                    goodCos2SigmaM = cos2SigmaM;
                }

                // Allow other threads some breathing room
                System.Threading.Thread.Sleep(0);
            }

            // u2 = cos(alpha).^2.*(a^2-b^2)/b^2;

            double u2 = System.Math.Pow(System.Math.Cos(goodAlpha), 2) * (System.Math.Pow(a, 2) - System.Math.Pow(b, 2)) / System.Math.Pow(b, 2);

            // A = 1+u2./16384.*(4096+u2.*(-768+u2.*(320-175.*u2)));

            double aa = 1 + u2 / 16384 * (4096 + u2 * (-768 + u2 * (320 - 175 * u2)));

            // B = u2./1024.*(256+u2.*(-128+u2.*(74-47.*u2)));

            double bb = u2 / 1024 * (256 + u2 * (-128 + u2 * (74 - 47 * u2)));

            // deltasigma = B.*sin(sigma).*(cos2sigmam+B./4.*(cos(sigma).*(-1+2.*...
            //    cos2sigmam.^2)-B./6.*cos2sigmam.*(-3+4.*sin(sigma).^2).*(-3+4*...
            //    cos2sigmam.^2)));

            double deltasigma = bb * System.Math.Sin(goodSigma) * (goodCos2SigmaM + bb / 4 * (System.Math.Cos(goodSigma) * (-1 + 2 *
                System.Math.Pow(goodCos2SigmaM, 2)) - bb / 6 * goodCos2SigmaM * (-3 + 4 * System.Math.Pow(System.Math.Sin(goodSigma), 2)) * (-3 + 4 *
                System.Math.Pow(goodCos2SigmaM, 2))));

            // varargout{1} = reshape(b.*A.*(sigma-deltasigma), keepsize);
            double s = b * aa * (goodSigma - deltasigma);

            // Return the Distance in meters
            return new Distance(s, DistanceUnit.Meters).ToLocalUnitType();
        }


        public Distance DistanceTo(Position destination, Ellipsoid ellipsoid, bool isApproximated)
        {
            //// Make sure the destination isn't null
            // if (destination == null)
            //    throw new ArgumentNullException("destination", "The Position.DistanceTo method requires a non-null destination parameter.");

            // If they want the high-speed formula, use it
            if (!isApproximated)
                return DistanceTo(destination);

            // The ellipsoid cannot be null
            if (ellipsoid == null)
                throw new System.ArgumentNullException("ellipsoid", "Resources.Position_DistanceTo_Null_Ellipsoid");

            // Dim AdjustedDestination As Position = destination.ToDatum(Datum)
            // USING THE FORMULA FROM:
            //$lat1 = deg2rad(28.5333);
            double lat1 = Latitude.ToRadians().Value;
            //$lat2 = deg2rad(31.1000);
            double lat2 = destination.Latitude.ToRadians().Value;
            //$long1 = deg2rad(-81.3667);
            double long1 = Longitude.ToRadians().Value;
            //$long2 = deg2rad(121.3667);
            double long2 = destination.Longitude.ToRadians().Value;
            //$dlat = abs($lat2 - $lat1);
            double dlat = System.Math.Abs(lat2 - lat1);
            //$dlong = abs($long2 - $long1);
            double dlong = System.Math.Abs(long2 - long1);
            //$l = ($lat1 + $lat2) / 2;
            double l = (lat1 + lat2) * 0.5;
            //$a = 6378;
            double a = ellipsoid.EquatorialRadius.ToKilometers().Value;
            //$b = 6357;
            double b = ellipsoid.PolarRadius.ToKilometers().Value;
            //$e = sqrt(1 - ($b * $b)/($a * $a));
            double e = System.Math.Sqrt(1 - (b * b) / (a * a));
            //$r1 = ($a * (1 - ($e * $e))) / pow((1 - ($e * $e) * (sin($l) * sin($l))), 3/2);
            double r1 = (a * (1 - (e * e))) / System.Math.Pow((1 - (e * e) * (System.Math.Sin(l) * System.Math.Sin(l))), 3 * 0.5);
            //$r2 = $a / sqrt(1 - ($e * $e) * (sin($l) * sin($l)));
            double r2 = a / System.Math.Sqrt(1 - (e * e) * (System.Math.Sin(l) * System.Math.Sin(l)));
            //$ravg = ($r1 * ($dlat / ($dlat + $dlong))) + ($r2 * ($dlong / ($dlat + $dlong)));
            double ravg = (r1 * (dlat / (dlat + dlong))) + (r2 * (dlong / (dlat + dlong)));
            //$sinlat = sin($dlat / 2);
            double sinlat = System.Math.Sin(dlat * 0.5);
            //$sinlon = sin($dlong / 2);
            double sinlon = System.Math.Sin(dlong * 0.5);
            //$a = pow($sinlat, 2) + cos($lat1) * cos($lat2) * pow($sinlon, 2);
            a = System.Math.Pow(sinlat, 2) + System.Math.Cos(lat1) * System.Math.Cos(lat2) * System.Math.Pow(sinlon, 2);
            //$c = 2 * asin(min(1, sqrt($a)));
            double c = 2 * System.Math.Asin(System.Math.Min(1, System.Math.Sqrt(a)));
            //$d = $ravg * $c;
            double d = ravg * c;
            // If it's NaN, return zero
            if (double.IsNaN(d))
            {
                d = 0.0;
            }
            // Return a new distance
            return new Distance(d, DistanceUnit.Kilometers).ToLocalUnitType();
        }

    }


}
