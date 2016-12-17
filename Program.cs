using System;
using System.Reflection;

namespace Game
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Globalization.CultureInfo customCulture =
                (System.Globalization.CultureInfo) System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
            customCulture.NumberFormat.NumberDecimalSeparator = ".";

            System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;

            Torque3D.Initializer.InitializeTypeDictionaries(Assembly.GetExecutingAssembly().GetTypes());
            Torque3D.Torque3D.Libraries libraries = new Torque3D.Torque3D.Libraries
            {
                Windows32bit = "Empty_DEBUG.dll",
                Windows64bit = "Empty_DEBUG.dll"
            };

            Torque3D.Torque3D.Initialize(args, libraries);
        }
    }
}