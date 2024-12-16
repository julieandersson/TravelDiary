using System;
using System.Text.Json;
using System.Globalization;

namespace TravelDiary
{
    class Program
    {
        private static List<Trip> trips = new List<Trip>();
        private static string tripsFile = "travelDiary.json";

        static void Main(string[] args)
        {
            LoadTrips();
            MainMenu();
        }

        // Huvudmeny med olika val, visas när program startas
        public static void MainMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("M I N   T R A V E L   D I A R Y"); // "Rubrik" som visas högst upp i menyn
                Console.WriteLine("1 - Visa alla resor i dagboken");
                Console.WriteLine("2 - Lägg till en resa i dagboken");
                Console.WriteLine("3 - Redigera en befintlig resa i dagboken");
                Console.WriteLine("4 - Ta bort en resa från dagboken");
                Console.WriteLine("X - Avsluta programmet\n");

                char input = Console.ReadKey(true).KeyChar;

                switch (input)
                {
                    case '1':
                        DisplayTrips(); // Visa alla resor
                        break;
                    case '2':
                        AddTrip(); // Lägg till en ny resa
                        break;
                    case '3':
                        EditTrip(); // Redigera en resa
                        break;
                    case '4':
                        DeleteTrip(); // Radera en resa
                        break;
                    case 'X':
                    case 'x':
                        ExitProgram(); // Avsluta programmet
                        break;
                    default:
                        Console.WriteLine("Ogiltigt val. Försök igen.");
                        Console.ReadKey();
                        break;
                }
            }
        }

        // Lägg till en ny resa i resedagboken
        public static void AddTrip()
        {
            Console.Clear();
            Trip newTrip = new Trip();

            // Exempel på att samla in data för en resa
            newTrip.Destination = PromptForInput("Ange resmål: ");
            newTrip.Continent = PromptForInput("Ange kontinent (t.ex. Europa, Asien): ");
            newTrip.Duration = PromptForIntInput("Ange antal dagar: ");

            Console.WriteLine("Ange reskompisar (en i taget) eller skriv 'soloresa' om du reste själv:");
            while (true)
            {
                string? companion = Console.ReadLine();

                // Om användaren trycker Enter utan input, avsluta insamling av data
                if (string.IsNullOrEmpty(companion) && newTrip.Companions.Count > 0)
                {
                    break; // Avsluta loopen
                }

                // Om användaren skriver "soloresa", lägg till det som enda värde och avsluta
                if (!string.IsNullOrEmpty(companion) && companion.Equals("soloresa", StringComparison.OrdinalIgnoreCase))
                {
                    newTrip.Companions.Clear(); // Säkerställ att listan är tom
                    newTrip.Companions.Add("Soloresa");
                    break; // Avsluta loopen
                }

                // Kontrollera att input inte är tomt
                if (!string.IsNullOrEmpty(companion))
                {
                    newTrip.Companions.Add(companion);
                    Console.WriteLine("Ange fler reskompisar eller tryck Enter för att avsluta:"); // Meddelande som visas efter varje angiven reskompis
                }
                else
                {
                    Console.WriteLine("Ogiltig inmatning. Ange minst en reskompis eller skriv 'soloresa'."); // Felmeddelande
                }
            }

            trips.Add(newTrip);
            SaveTrips();

            Console.WriteLine("Resan har sparats!");
            ReturnToMenu();
        }

        // Metod för att se så att inget fält är tomt
        public static string PromptForInput(string prompt)
        {
            string? input;
            do
            {
                Console.Write(prompt);
                input = Console.ReadLine();

                if (string.IsNullOrEmpty(input))
                {
                    Console.WriteLine("Fältet får inte vara tomt. Försök igen.");
                }
            } while (string.IsNullOrEmpty(input));

            return input;
        }

        // Metod för att se så att användaren anger ett giltigt heltal för antal dagar
        public static int PromptForIntInput(string prompt)
        {
            int value;
            do
            {
                Console.Write(prompt);
                string? input = Console.ReadLine();

                if (!int.TryParse(input, out value) || value <= 0)
                {
                    Console.WriteLine("Ogiltig inmatning. Ange ett positivt heltal.");
                }
                else
                {
                    break;
                }
            } while (true);

            return value;
        }

        // Läser in alla resor från JSON-filen
        public static void LoadTrips()
        {
            if (File.Exists(tripsFile))
            {
                string jsonData = File.ReadAllText(tripsFile);
                trips = JsonSerializer.Deserialize<List<Trip>>(jsonData) ?? new List<Trip>();
            }
        }

        // Visar meddelande och väntar på att användaren ska trycka på valfri knapp
        public static void ReturnToMenu()
        {
            Console.WriteLine("\nTryck på valfri knapp för att återvända till menyn.");
            Console.ReadKey();
        }

        // Sparar alla resor till en JSON-fil
        public static void SaveTrips()
        {
            string jsonData = JsonSerializer.Serialize(trips, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(tripsFile, jsonData);
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
