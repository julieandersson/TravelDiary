using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Globalization;
using System.Text.RegularExpressions;

namespace TravelDiary
{
    class Program
    {
        private static List<Trip> trips = new List<Trip>();
        private static string tripsFile = "travelDiary.json";

        static void Main(string[] args)
        {
            LoadTrips(); // Laddar in befintliga resor från JSON-fil
            MainMenu();  // Visar huvudmenyn
        }

        // Huvudmeny med olika val, visas när program startas
        public static void MainMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("T R A V E L   D I A R Y\n"); // "Rubrik" som visas högst upp i menyn
                Console.WriteLine("Hej och välkommen till resedagboken! Här kan du samla alla dina resor du gjort för att ");
                Console.WriteLine("återuppleva fantastiska minnen och dokumentera dina äventyr på ett och samma ställe.\n");
                Console.WriteLine("Gör ett val nedan för att ta dig vidare i din travel diary!\n");
                Console.WriteLine("[1] - Visa alla resor i dagboken");
                Console.WriteLine("[2] - Lägg till en resa i dagboken");
                Console.WriteLine("[3] - Redigera en befintlig resa i dagboken");
                Console.WriteLine("[4] - Ta bort en resa från dagboken\n");
                Console.WriteLine("[X] - Stäng resedagboken\n");

                // Användarens menyval
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

            // Samlar in all information för den nya resan
            newTrip.Destination = PromptForInput("Ange resmål: ", "destination");
            newTrip.Continent = PromptForInput("Ange kontinent (Afrika, Antarktis, Asien, Europa, Nordamerika, Oceanen, Sydamerika): ", "continent");
            newTrip.Duration = PromptForIntInput("Ange antal dagar: ");
            newTrip.StartDate = PromptForDateInput("Ange startdatum för resan (yyyy-mm-dd): ");
            newTrip.EndDate = PromptForDateInput("Ange slutdatum för resan (yyyy-mm-dd): ");
            newTrip.Cost = PromptForDecimalInput("Ange kostnad för resan (SEK): ");

            // Samlar in reskompisar
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

                // Kontrollera att input endast innehåller bokstäver och är minst 2 tecken
                if (!string.IsNullOrEmpty(companion) && Regex.IsMatch(companion, @"^[a-zA-ZåäöÅÄÖ]{2,}$"))
                {
                    newTrip.Companions.Add(companion);
                    Console.WriteLine("Ange fler reskompisar eller tryck Enter för att avsluta:"); // Meddelande som visas efter varje angiven resekompis
                }
                else
                {
                    Console.WriteLine("Ogiltig inmatning. Namnet måste innehålla minst 2 bokstäver och endast bokstäver."); // Felmeddelande
                }
            }

            // Fråga om typ av resa (semester eller jobbresa)
            newTrip.Type = PromptForTripType("Ange typ av resa (Semester eller Jobbresa): ");

            // Lägg till resan i listan och spara
            trips.Add(newTrip);
            SaveTrips();

            Console.WriteLine("Resan har sparats!");
            ReturnToMenu();
        }

        // Metod för string-input (för destination och kontinent) med validering
        public static string PromptForInput(string prompt, string validationType = "", bool allowEmpty = false)
        { 
              // Lista över tillåtna kontinenter
              string[] validContinents = { "Afrika", "Antarktis", "Asien", "Europa", "Nordamerika", "Oceanen", "Sydamerika" };

              while (true)
              {
                Console.Write(prompt);
                string? input = Console.ReadLine()?.Trim();

                // Tillåt tom input om allowEmpty är true
                if (allowEmpty && string.IsNullOrEmpty(input))
                {
                    return input ?? "";
                }

                // Validering för "destination" (minst 3 tecken och endast bokstäver)
                if (validationType.Equals("destination", StringComparison.OrdinalIgnoreCase))
                {
                    if (!string.IsNullOrEmpty(input) && Regex.IsMatch(input, @"^[a-zA-ZåäöÅÄÖ\s]{3,}$"))
                    {
                        return input; // Input är giltig
                    }
                    Console.WriteLine("Ogiltig inmatning. Resmålet måste vara minst 3 bokstäver och får endast innehålla bokstäver.");
                }
                // Validering för "continent" (måste matcha fördefinierade kontinenter)
                else if (validationType.Equals("continent", StringComparison.OrdinalIgnoreCase))
                {
                   if (!string.IsNullOrEmpty(input) && validContinents.Contains(input, StringComparer.OrdinalIgnoreCase))
                   {
                       return input; // Input är giltig
                   }
                   Console.WriteLine("Ogiltig inmatning.");
                }
                // Generell validering för andra fall
                else if (!string.IsNullOrEmpty(input))
                {
                   return input; // Returnera input om ingen specifik validering krävs
                }
                else
                {
                   Console.WriteLine("Fältet får inte vara tomt. Försök igen.");
                }
             }
        }

        // Metod för heltalsinput så att användaren anger ett giltgt heltal (för antal dagar)
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

        // Metod för giltigt datum (för start och slutdatum av resa)
        public static DateTime PromptForDateInput(string prompt)
        {
            DateTime dateValue;
            do
            {
                Console.Write(prompt);
                string? input = Console.ReadLine();

                // Försök att parsa input till ett giltigt datum
                if (!DateTime.TryParseExact(input, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateValue))
                {
                    Console.WriteLine("Ogiltigt datumformat. Ange datum i formatet yyyy-MM-dd.");
                }
                else if (dateValue >= DateTime.Today) // Kontrollera om datumet är i framtiden
                {
                    Console.WriteLine("Datumet kan inte vara i framtiden. Försök igen.");
                }
                else
                {
                    break; // Om input är giltigt, avsluta loopen
                }
            } while (true);

            return dateValue; // Returnera det giltiga datumet
        }

        // Metod för decimalvärde (för kostnad av resan)
        public static decimal PromptForDecimalInput(string prompt)
        {
            decimal value;
            do
            {
                Console.Write(prompt);
                string? input = Console.ReadLine();

                // Kontrollera att input endast innehåller siffror och kan konverteras till ett decimaltal
                if (string.IsNullOrEmpty(input) || !decimal.TryParse(input, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out value) || value < 0)
                {
                   Console.WriteLine("Ogiltig inmatning. Ange endast positiva siffror (exempel: 20000). Försök igen."); // Felmeddelande vid ogiltig input
                }
                else
                {
                   break; // Om input är giltig, avsluta loopen
                }
            } while (true);

            return value; // Returnerar det giltiga decimaltalet
        }

        // Metod för att låta användaren välja typ av resa
        public static TripType PromptForTripType(string prompt)
        {
            var validChoices = new Dictionary<string, TripType>(StringComparer.OrdinalIgnoreCase)
            {
                { "semester", TripType.Vacation },
                { "jobbresa", TripType.Business }
            };

            while (true)
            {
                Console.WriteLine(prompt);
                Console.WriteLine("- semester");
                Console.WriteLine("- jobbresa");
                Console.Write("Ditt val: ");
                string? input = Console.ReadLine();

                if (input != null && validChoices.TryGetValue(input, out TripType tripType))
                {
                    return tripType;
                }
                else
                {
                    Console.WriteLine("Ogiltigt val. Vänligen ange 'semester' eller 'jobbresa'.");
                }
            }
        }

        // Laddar in alla resor från jsonfilen
        public static void LoadTrips()
        {
            if (File.Exists(tripsFile))
            {
                var options = new JsonSerializerOptions { Converters = { new JsonStringEnumConverter() } };
                string jsonData = File.ReadAllText(tripsFile);
                trips = JsonSerializer.Deserialize<List<Trip>>(jsonData, options) ?? new List<Trip>();
            }
        }

        // Spara resor till fil
        public static void SaveTrips()
        {
            var options = new JsonSerializerOptions { WriteIndented = true, Converters = { new JsonStringEnumConverter() } };
            string jsonData = JsonSerializer.Serialize(trips, options);
            File.WriteAllText(tripsFile, jsonData);
        }

        // Visar meddelande och väntar på knapptryckning
        public static void ReturnToMenu()
        {
            Console.WriteLine("\nTryck på valfri knapp för att återvända till menyn.");
            Console.ReadKey();
        }

        public static void DisplayTrips()
       {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("A L L A   D I N A   R E S O R\n");

                // Kontroll om det inte finns några resor i listan
                if (trips.Count == 0)
                {
                    Console.WriteLine("Inga resor har lagts till i dagboken ännu.\n"); // Meddelade om listan är tom
                    ReturnToMenu();
                    return;
                }

                // Visa alla destinationer med index
                Console.WriteLine("Ange den resan du vill läsa mer om.");
                Console.WriteLine("Ange X för att gå tillbaka till huvudmenyn.\n");

                for (int i = 0; i < trips.Count; i++)
                {
                    Console.WriteLine($"[{i}] {trips[i].Destination}");
                }

                // Användarens input
                Console.Write("\nDitt val: ");
                string? input = Console.ReadLine();

                // Gå tillbaka till huvudmenyn om användaren skriver "X" eller "x"
                if (input?.Equals("X", StringComparison.OrdinalIgnoreCase) == true)
                {
                    return;
                }

                // Försök att parsa input till ett giltigt index
                if (int.TryParse(input, out int index) && index >= 0 && index < trips.Count)
                {
                    ShowTripDetails(index);
                }
                else
                {
                    Console.WriteLine("Ogiltigt val. Försök igen.");
                    Console.ReadKey();
                }
            }
        }

        // Metod för att visa detaljerad information om en specifik resa
        public static void ShowTripDetails(int index)
        {
            Console.Clear();
            Trip trip = trips[index]; // Hämtar den valda resan

            Console.WriteLine("R E S E D E T A L J E R\n");
            Console.WriteLine($"Resmål: {trip.Destination}");
            Console.WriteLine($"Kontinent: {trip.Continent}");
            Console.WriteLine($"Längd på resan: {trip.Duration} dagar");
            Console.WriteLine($"Startdatum: {trip.StartDate:yyyy-MM-dd}");
            Console.WriteLine($"Slutdatum: {trip.EndDate:yyyy-MM-dd}");
            Console.WriteLine($"Totalkostnad: {trip.Cost} kr");

            string companions = string.Join(", ", trip.Companions);

            Console.WriteLine($"Reskompis(ar) eller soloresa: {companions}");
            Console.WriteLine($"Typ av resa: {(trip.Type == TripType.Vacation ? "Semester" : "Jobbresa")}");

            Console.WriteLine("\nAnge X för att gå tillbaka till alla resor.");
            while (true)
            {
                string? input = Console.ReadLine();
                if (input?.Equals("X", StringComparison.OrdinalIgnoreCase) == true)
                {
                break; // Gå tillbaka till listan med resor
                }
                else
                {
                Console.WriteLine("Ogiltigt val. Ange X för att gå tillbaka.");
                }
            }
        }

        public static void EditTrip()
        {
            Console.WriteLine("Redigerar en resa...");
            Console.ReadKey();
        }

        public static void DeleteTrip()
        {
            Console.WriteLine("Tar bort en resa...");
            Console.ReadKey();
        }

        public static void ExitProgram()
        {
            Console.WriteLine("Stänger ner...");
            Console.WriteLine("Tack för att du använde resedagboken! Välkommen tillbaka!");
            Environment.Exit(0);
        }
    }
}
