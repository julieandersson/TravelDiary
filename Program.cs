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
            newTrip.Companion = PromptForInput("Ange reskompis (eller skriv 'soloresa'): ");

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
                // Inväntar användarens input
                Console.Write(prompt);
                input = Console.ReadLine();

                // Om input är tomt, be användaren försöka igen 
                if (string.IsNullOrEmpty(input))
                {
                    Console.WriteLine("Fältet får inte vara tomt. Försök igen.");
                }
            } while (string.IsNullOrEmpty(input)); // Fortsätter tills att input inte är tomt

            // Returnerar input när det är giltigt
            return input;
        }

        // Metod för att se så att användaren anger ett giltigt heltal för antal dagar
        public static int PromptForIntInput(string prompt)
        {
             int value;
             do
             {
                 // Inväntar användarens input
                 Console.Write(prompt);
                 string? input = Console.ReadLine();

                // Försöker parsa input till ett heltal
                if (!int.TryParse(input, out value) || value <= 0)
                {
                     Console.WriteLine("Ogiltig inmatning. Ange ett positivt heltal."); // Felmeddelande vid felaktigt input
                }
                 else
                {
                     break; // Om inmatningen är giltig, avsluta loopen
                }
            } while (true); // Fortsätter tills giltig input skrivs in

            return value; // Returnerar heltalet när det är giltigt
        }

        // Läser in alla resor från JSON-filen
        public static void LoadTrips()
        { 
            // Kollar om filen finns
            if (File.Exists(tripsFile))
            {
               // Läser in all data från filen
                string jsonData = File.ReadAllText(tripsFile);
               // Deserialiserar JSON-datan tillbaka till en lista med alla resor
                trips = JsonSerializer.Deserialize<List<Trip>>(jsonData) ?? new List<Trip>();
            }
        }

        // Visar meddelande och väntar på att användaren ska trycka på valfri knapp
        public static void ReturnToMenu()
        {
            Console.WriteLine("\nTryck på valfri knapp för att återvända till menyn.");
            Console.ReadKey(); // Väntar på knapptryckning
        }  

        // Sparar alla resor till en JSON-fil
        public static void SaveTrips()
        { 
            // Serialiserar listan med inlägg till JSON-format
            string jsonData = JsonSerializer.Serialize(trips, new JsonSerializerOptions { WriteIndented = true});
            // Skriver ut JSON-datan till filen "travelDiary.json"
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
