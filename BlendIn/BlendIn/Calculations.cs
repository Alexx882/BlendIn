using System;
using Xamarin.Essentials;


namespace BlendIn
{
    public class Calculations
    {
       
        public static int GetOctantBetweenTwoPoints(Location a, Location b, double compass)
        {
            //COUNTED FROM 0-7
            double degree = (DegreeBearing(a.Latitude, a.Longitude, b.Latitude, b.Longitude) + compass) % 360;
            return (int)(degree/45);
        }

        /*public static double getFinalBearing(Location a, Location b)
        {
            //DO WE NEED THIS? (Also for Longitude?)
            double lat_rad_a = DegreeToRadian(a.Latitude);
            double lat_rad_b = DegreeToRadian(b.Latitude);
            double lon_rad_a = DegreeToRadian(a.Longitude);
            double lon_rad_b = DegreeToRadian(b.Longitude);

            double y = Math.Sin(lon_rad_b - lon_rad_a) * Math.Cos(lat_rad_b);
            double x = Math.Cos(lat_rad_a) * Math.Sin(lat_rad_b) - Math.Sin(lat_rad_a) * Math.Cos(lat_rad_b) * Math.Cos(lon_rad_b - lon_rad_a);

            double brng = RadianToDegree(Math.Atan2(y, x));
            double finalBrng = (brng + 180) % 360;
            return brng;
            //Formula: 	θ = atan2( sin Δλ ⋅ cos φ2 , cos φ1 ⋅ sin φ2 − sin φ1 ⋅ cos φ2 ⋅ cos Δλ )
            //where φ1, λ1 is the start point, φ2,λ2 the end point(Δλ is the difference in longitude)

            //For final bearing, simply take the initial bearing from the end point to the start point and reverse it (using θ = (θ+180) % 360).

        }*/

        public static double GetDistance(Location a, Location b)
        {
            double r = 6371e3; // metres
            double lat_rad_a = DegreeToRadian(a.Latitude);
            double lat_rad_b = DegreeToRadian(b.Latitude);

            double delta_lat_rad = DegreeToRadian(b.Latitude - a.Latitude);
            double delta_lon_rad = DegreeToRadian(b.Longitude - a.Longitude);

            double a_loc = Math.Sin(delta_lat_rad / 2) * Math.Sin(delta_lat_rad / 2) + Math.Cos(lat_rad_a) * Math.Cos(lat_rad_b) * Math.Sin(delta_lon_rad / 2) * Math.Sin(delta_lon_rad / 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a_loc), Math.Sqrt(1 - a_loc));

            double v = r * c;
            return v;
        }

        public static double RadianToDegree(double angle)
        {
            return angle * (180.0 / Math.PI);
        }

        public static double DegreeToRadian(double angle)
        {
            return Math.PI * angle / 180.0;
        }


        public static double DegreeBearing(double lat1, double lon1, double lat2, double lon2)
        {
            var dLon = ToRad(lon2 - lon1);
            var dPhi = Math.Log(
                Math.Tan(ToRad(lat2) / 2 + Math.PI / 4) / Math.Tan(ToRad(lat1) / 2 + Math.PI / 4));
            if (Math.Abs(dLon) > Math.PI)
                dLon = dLon > 0 ? -(2 * Math.PI - dLon) : (2 * Math.PI + dLon);
            return ToBearing(Math.Atan2(dLon, dPhi));
        }

        public static double ToRad(double degrees)
        {
            return degrees * (Math.PI / 180);
        }

        public static double ToDegrees(double radians)
        {
            return radians * 180 / Math.PI;
        }

        public static double ToBearing(double radians)
        {
            // convert radians to degrees (as bearing: 0...360)
            return (ToDegrees(radians) + 360) % 360;
        }

    }
}
