using System;
using SpiceSharp;

class Program
{
    static void Main(string[] args)
    {
        // Load the SPICE kernels
        SpiceSharp.LoadKernel("de430.bsp"); // Planetary ephemeris
        SpiceSharp.LoadKernel("naif0012.tls"); // Leapseconds

        // Define some constants
        const string EARTH = "399"; // NAIF ID for Earth
        const string MARS = "499"; // NAIF ID for Mars
        const double AU = 149597870.691; // Astronomical unit in km

        // Convert the date to ephemeris time (ET)
        double et = SpiceSharp.Str2Et("2023-01-01");

        // Compute the state vector of Mars relative to Earth
        double[] state = SpiceSharp.Spkgeo(MARS, et, "J2000", EARTH);

        // Extract the position vector
        double[] pos = new double[3];
        Array.Copy(state, pos, 3);

        // Compute the distance in km and AU
        double dist_km = SpiceSharp.Vnorm(pos);
        double dist_au = dist_km / AU;

        // Print the result
        Console.WriteLine("The distance between Earth and Mars on 2023-01-01 is:");
        Console.WriteLine("{0:0.00} km", dist_km);
        Console.WriteLine("{0:0.00} AU", dist_au);
    }
}
