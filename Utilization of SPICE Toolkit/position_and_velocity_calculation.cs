using System;
using SpiceSharp;

class Program
{
    static void Main(string[] args)
    {
        // Load the SPICE kernels
        SpiceSharp.LoadKernel("de430.bsp"); // Planetary ephemeris
        SpiceSharp.LoadKernel("naif0012.tls"); // Leapseconds
        SpiceSharp.LoadKernel("spaceship.bsp"); // Spaceship ephemeris

        // Define some constants
        const string SPACESHIP = "-1000"; // NAIF ID for spaceship
        const double AU = 149597870.691; // Astronomical unit in km
        const double KMPS = 0.210945021; // Conversion factor from km/s to AU/day

        // Define an array of planet names and NAIF IDs
        string[] planets = {"Mercury", "Venus", "Earth", "Mars", "Jupiter", "Saturn", "Uranus", "Neptune"};
        string[] planet_ids = {"199", "299", "399", "499", "599", "699", "799", "899"};

        // Convert the date to ephemeris time (ET)
        double et = SpiceSharp.Str2Et("2023-01-01");

        // Loop over the planets
        for (int i = 0; i < planets.Length; i++)
        {
            // Compute the state vector of the spaceship relative to the planet
            double[] state = SpiceSharp.Spkgeo(SPACESHIP, et, "J2000", planet_ids[i]);

            // Extract the position and velocity vectors
            double[] pos = new double[3];
            double[] vel = new double[3];
            Array.Copy(state, pos, 3);
            Array.Copy(state, 3, vel, 0, 3);

            // Compute the distance and speed in AU and AU/day
            double dist_au = SpiceSharp.Vnorm(pos) / AU;
            double speed_aud = SpiceSharp.Vnorm(vel) * KMPS;

            // Print the result
            Console.WriteLine("The position and velocity of the spaceship relative to {0} on 2023-01-01 are:", planets[i]);
            Console.WriteLine("Position: ({0:0.00}, {1:0.00}, {2:0.00}) AU", pos[0] / AU, pos[1] / AU, pos[2] / AU);
            Console.WriteLine("Velocity: ({0:0.00}, {1:0.00}, {2:0.00}) AU/day", vel[0] * KMPS, vel[1] * KMPS, vel[2] * KMPS);
            Console.WriteLine("Distance: {0:0.00} AU", dist_au);
            Console.WriteLine("Speed: {0:0.00} AU/day", speed_aud);
            Console.WriteLine();
        }
    }
}
