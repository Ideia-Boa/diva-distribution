using System;
using System.IO;
using System.Collections.Generic;

namespace MetaverseInk.Configuration
{
    public class Configure
    {
        private static string worldName = "My World";
        private static string dbPasswd = "secret";
        private static string masterPasswd = "secret";
        private static string ipAddress = "127.0.0.1";

        public static void Main(string[] args)
        {
            GetUserInput();
            ConfigureRegions();
            ConfigureMyWorld();
            DisplayInfo();
        }

        private static void GetUserInput()
        {
            Console.Write("Name of your world: ");
            worldName = Console.ReadLine();
            if (worldName == string.Empty)
                worldName = "My World";

            Console.Write("MySql database password for opensim account: ");
            dbPasswd = Console.ReadLine();

            Console.Write("Master Avatar password (default " + dbPasswd + "): ");
            masterPasswd = Console.ReadLine();
            if (masterPasswd == string.Empty)
                masterPasswd = dbPasswd;

            Console.Write("Your IP address or domain name (default 127.0.0.1 not reacheable from outside): ");
            ipAddress = Console.ReadLine();
            if (ipAddress == string.Empty)
                ipAddress = "127.0.0.1";
        }

        private static void ConfigureMyWorld()
        {
            try
            {
                using (TextReader tr = new StreamReader("config-include/MyWorld.ini.example"))
                {
                    using (TextWriter tw = new StreamWriter("config-include/MyWorld.ini"))
                    {
                        string line;
                        while ((line = tr.ReadLine()) != null)
                        {
                            if (line.Contains("Password"))
                                line = line.Replace("***", dbPasswd);
                            if (line.Contains("127.0.0.1"))
                                line = line.Replace("127.0.0.1", ipAddress);
                            tw.WriteLine(line);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error configuring MyWorld " + e.Message);
                return;
            }
            Console.WriteLine("Your World has been successfully configured");
        }

        private static void ConfigureRegions()
        {
            try
            {
                using (TextReader tr = new StreamReader("Regions/RegionConfig.ini.example"))
                {
                    using (TextWriter tw = new StreamWriter("Regions/RegionConfig.ini"))
                    {
                        string line;
                        while ((line = tr.ReadLine()) != null)
                        {
                            if (line.Contains("My World"))
                                line = line.Replace("My World", worldName);
                            if (line.Contains("Password"))
                                line = line.Replace("***", masterPasswd);
                            if (line.Contains("SYSTEMIP"))
                                line = line.Replace("SYSTEMIP", ipAddress);
                            tw.WriteLine(line);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error configuring RegionConfig " + e.Message);
                return;
            }
            Console.WriteLine("Your regions have been successfully configured");
        }

        private static void DisplayInfo()
        {
            Console.WriteLine("\n***************************************************");
            Console.WriteLine("Your world is " + worldName);
            Console.WriteLine("The owner/god account is Master Avatar with password " + masterPasswd);
            Console.WriteLine("Your loginuri is http://" + ipAddress + ":9000");
            Console.WriteLine("***************************************************\n");
            Console.Write("<Press enter to exit>");
            Console.ReadLine();
        }
    }
}
