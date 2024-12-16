using System;
using System.Text.Json;
using System.Globalization;

namespace TravelDiary
{
    class Program
    {
        private static List<Trip> trips = new List<Trip>();


        static void Main(string[] args)
        {
            MainMenu();
        }

        // Huvudmeny med olika val, visas när program startas
        public static void MainMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("M I N   T R A V E L   D I A R Y"); // "Rubrik" som visas högst upp i menyn
                Console.WriteLine("1 - Lägg till en resa till dagboken");
                Console.WriteLine("2 - Ta bort en resa från dagboken");
                Console.WriteLine("3 - Visa alla resor i dagboken");
                Console.WriteLine("4 - Redigera en befintlig resa i dagboken");
                Console.WriteLine("X - Avsluta programmet\n");

                char input = Console.ReadKey(true).KeyChar;

                switch (input)
                {
                    case '1':
                        AddTrip();
                        break;
                    case '2':
                        DeleteTrip();
                        break;
                    case '3':
                        DisplayTrips();
                        break;
                    case '4':
                        EditTrip();
                        break;
                    case 'X':
                    case 'x':
                        ExitProgram();
                        break;
                    default:
                        Console.WriteLine("Ogiltigt val. Försök igen.");
                        Console.ReadKey();
                        break;
                }
            }
        }

        // Stubbar, implementerar senare
        public static void AddTrip()
        {
            Console.WriteLine("Lägger till en resa...");
            Console.ReadKey();
        }

        public static void DeleteTrip()
        {
            Console.WriteLine("Tar bort en resa...");
            Console.ReadKey();
        }

        public static void DisplayTrips()
        {
            Console.WriteLine("Visar alla resor...");
            Console.ReadKey();
        }

        public static void EditTrip()
        {
            Console.WriteLine("Redigerar en resa...");
            Console.ReadKey();
        }

        public static void ExitProgram()
        {
            Console.WriteLine("Avslutar programmet...");
            Environment.Exit(0);
        }
    }
}
