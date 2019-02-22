
namespace DotSpatialTests
{


    class Program
    {

        public static double SpatialDistanceBetweenPlaces(
              double lat1
            , double lng1
            , double lat2
            , double lng2)
        {
           
            var fablat = new Latitude(lat1);
            var fablng = new Longitude(lng1);
            
            var sglat = new Latitude(lat2);
            var sglng = new Longitude(lng2);

            var fab = new Position(fablat, fablng);
            var sg = new Position(sglat, sglng);

            Distance dist = fab.DistanceTo(sg);
            
            return dist.ToMeters().Value;
        } // End Function SpatialDistanceBetweenPlaces


        public static double SpatialDistanceBetweenPlacesDotSpatial(
              double lat1
            , double lng1
            , double lat2
            , double lng2)
        {
            var fablat = new DotSpatial.Positioning.Latitude(lat1);
            var fablng = new DotSpatial.Positioning.Longitude(lng1);

            var sglat = new DotSpatial.Positioning.Latitude(lat2);
            var sglng = new DotSpatial.Positioning.Longitude(lng2);

            var fab = new DotSpatial.Positioning.Position(fablat, fablng);
            var sg = new DotSpatial.Positioning.Position(sglat, sglng);

            DotSpatial.Positioning.Distance dist = fab.DistanceTo(sg);

            return dist.ToMeters().Value;
        } // End Function SpatialDistanceBetweenPlacesDotSpatial


        static void Main(string[] args)
        {
            double lat1 = 47.552063;
            double lng1 = 9.226081;
            double lat2 = 47.374487;
            double lng2 = 9.556946;


            double distance1 = SpatialDistanceBetweenPlaces(lat1, lng1, lat2, lng2);
            double distance2 = SpatialDistanceBetweenPlacesDotSpatial(lat1, lng1, lat2, lng2);
            bool areEqual = distance1 == distance2;
            System.Diagnostics.Debug.Assert(areEqual);

            System.Console.WriteLine(distance1);
            System.Console.WriteLine(distance2);
            System.Console.WriteLine(areEqual);

            System.Console.WriteLine(System.Environment.NewLine);
            System.Console.WriteLine(" --- Press any key to continue --- ");
            System.Console.ReadKey();
        }


    }


}
