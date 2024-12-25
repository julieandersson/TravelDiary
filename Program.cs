using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Globalization;
using System.Text.RegularExpressions;

namespace TravelDiary
{
    class Program
    {
        // Lista för att lagra alla nya resor i dagboken
        private static List<Trip> trips = new List<Trip>();
        // Filnamn för att spara resor i JSON-format
        private static string tripsFile = "travelDiary.json";


        // Lista för att lagra alla pack-listor med de saker som ska packas ned
        private static List<PackingList> packingLists = new List<PackingList>();
        // Filnamn för att spara packlistor i JSON-format
        private static string packingListsFile = "packingLists.json";


        static void Main(string[] args)
        {
            LoadTrips(); // Laddar in befintliga resor från JSON-fil
            LoadPackingLists(); // Laddar packlistor
            MainMenu();  // Visar huvudmenyn
        }

        // Huvudmeny med olika val, visas när program startas
        public static void MainMenu()
        {
            while (true)
            {
                Console.Clear();
                // Presenterar nedan menyalternativ för användaren
                Console.WriteLine("T R A V E L   D I A R Y\n"); // "Rubrik" som visas högst upp i menyn
                Console.WriteLine("Hej och välkommen till resedagboken! Här kan du samla alla dina resor du gjort för att\nåteruppleva fantastiska minnen och dokumentera dina äventyr på ett och samma ställe.\n");
                Console.WriteLine("Gör ett val nedan för att ta dig vidare i din travel diary!\n");

                // Alternativ för hantering av resor
                Console.WriteLine("H A N T E R A   B E S Ö K T A   R E S O R");
                Console.WriteLine("[1] - Visa alla resor i dagboken");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("[2] - Lägg till en resa i dagboken");
                Console.ResetColor();
                Console.WriteLine("[3] - Redigera en befintlig resa i dagboken");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("[4] - Ta bort en resa från dagboken\n");
                Console.ResetColor();
                // Alternativ för hantering av packlistor
                Console.WriteLine("H A N T E R A   P A C K L I S T O R   F Ö R   K O M M A N D E   R E S O R");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("[5] - Skapa en packlista för kommande resa");
                Console.ResetColor();
                Console.WriteLine("[6] - Visa och hantera packlistor");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("[7] - Ta bort en packlista\n");
                Console.ResetColor();
                
                Console.WriteLine("[X] - Stäng resedagboken\n");

                // Läser in användarens menyval
                char input = Console.ReadKey(true).KeyChar;

                // Hanterar användarens val med en switch-sats
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
                    case '5':
                        CreatePackingList(); // Skapa en ny packlista
                        break;
                    case '6':
                        ViewPackingLists(); // Visa och hantera packlistor
                        break;
                    case '7':
                        DeletePackingList(); // Radera en packlista
                        break;
                    case 'X':
                    case 'x':
                        ExitProgram(); // Avsluta programmet
                        break;
                    default:
                        // Felmeddelande vid ogiltigt val
                        Console.WriteLine("Ogiltigt val. Försök igen.");
                        Console.ReadKey();
                        break;
                }
            }
        }

        // Metod för att lägga till en ny resa i resedagboken
        public static void AddTrip()
        {
            Console.Clear();
            Console.WriteLine("L Ä G G   T I L L   E N   R E S A\n");
            Trip newTrip = new Trip(); // Skapar en ny instans av klassen Trip

            // Samlar in all information för den nya resan
            newTrip.Destination = PromptForInput("\nAnge resmål: ", "destination");
            newTrip.Continent = PromptForInput("\nAnge kontinent: ", "continent");
            newTrip.Duration = PromptForIntInput("\nAnge antal dagar: ");
            newTrip.StartDate = PromptForDateInput("\nAnge startdatum för resan (yyyy-mm-dd): ");
            newTrip.EndDate = PromptForDateInput("\nAnge slutdatum för resan (yyyy-mm-dd): ");
            newTrip.Cost = PromptForDecimalInput("\nAnge kostnad för resan (SEK): ");

            // Hanterar inmatning av reskompisar eller soloresa
            bool repeatQuestion = true;
            while (true)
            {
                if (repeatQuestion)
                {
                    Console.WriteLine("\nAnge reskompisar (en i taget) eller skriv 'soloresa' om du reste själv:");
                }
                
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
                    repeatQuestion = false;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Ogiltig inmatning. Namnet måste innehålla minst 2 bokstäver och endast bokstäver."); // Felmeddelande
                    Console.ResetColor();
                    repeatQuestion = true;
                }
            }

            // Fråga om typ av resa (semester eller jobbresa)
            newTrip.Type = PromptForTripType("\nAnge typ av resa (Semester eller Jobbresa): ");

            // Lägg till resan i listan och spara
            trips.Add(newTrip);
            SaveTrips();

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nResan har sparats!"); // bekräftelse
            ReturnToMenu();
            Console.ResetColor();
        }

        // Metod för string-input (för destination och kontinent) med validering
        public static string PromptForInput(string prompt, string validationType = "", string? defaultValue = null)
        {
            // Lista över tillåtna kontinenter
            string[] validContinents = { "Afrika", "Antarktis", "Asien", "Europa", "Nordamerika", "Oceanen", "Sydamerika" };

            while (true)
            {
                // Visa prompten med nuvarande värde om det finns något
                if (defaultValue != null)
                {
                    Console.Write($"{prompt} (nuvarande: {defaultValue}): ");
                }
                else
                {
                    Console.Write($"{prompt} ");
                }

                // Läser in användarens input
                string? input = Console.ReadLine()?.Trim();

                // Om ett standardvärde finns och användaren trycker enter (input är tom), behåll nuvarande värde (gäller vid redigering av resa)
                if (!string.IsNullOrEmpty(defaultValue) && string.IsNullOrEmpty(input))
                {
                    return defaultValue; // Behåll det nuvarande värdet
                }

                // Validering för "destination" (minst 3 tecken och endast bokstäver)
                if (validationType.Equals("destination", StringComparison.OrdinalIgnoreCase))
                {
                    if (!string.IsNullOrEmpty(input) && Regex.IsMatch(input, @"^[a-zA-ZåäöÅÄÖ\s]{3,}$"))
                    {
                        return input; // om input är giltig
                    }
                    // Felmeddelande vid ogiltig input
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Ogiltig inmatning. Resmålet måste vara minst 3 bokstäver och får endast innehålla bokstäver.");
                    Console.ResetColor();
                }
                // Validering för "continent" (måste matcha de fördefinierade kontinenterna)
                else if (validationType.Equals("continent", StringComparison.OrdinalIgnoreCase))
                {
                    if (!string.IsNullOrEmpty(input) && validContinents.Contains(input, StringComparer.OrdinalIgnoreCase))
                    {
                        return input; // om input är giltig
                    }
                    // Felmeddelande vid ogiltig input
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Ogiltig inmatning. Ange Afrika, Antarktis, Asien, Europa, Nordamerika, Oceanen eller Sydamerika.");
                    Console.ResetColor();
                }
                // Generell validering för andra fall
                else if (!string.IsNullOrEmpty(input))
                {
                    return input;
                }
                else
                {
                    // Felmeddelande om input är tomt
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Fältet får inte vara tomt. Försök igen.");
                    Console.ResetColor();
                }
            }
        }

        // Metod för heltalsinput så att användaren anger ett giltigt heltal för antal dagar (med eller utan standardvärde)
        public static int PromptForIntInput(string prompt, int? defaultValue = null)
        {
            while (true)
            {
                // Visa prompten och inkludera nuvarande värde om det finns något
                if (defaultValue.HasValue)
                {
                    Console.Write($"{prompt} (nuvarande: {defaultValue}): ");
                }
                else
                {
                    Console.Write(prompt);
                }

                // Läser in användarens input
                string? input = Console.ReadLine()?.Trim();

                // Om ett standardvärde finns och användaren trycker enter (input är tom), behåll nuvarande värde (gäller vid redigering av resa)
                if (defaultValue.HasValue && string.IsNullOrEmpty(input))
                {
                    return defaultValue.Value;
                }

                // Försök att tolka input som ett positivt heltal
                if (int.TryParse(input, out int value) && value > 0)
                {
                    return value; // returnera giltigt värde
                }

                // Felmeddelande för ogiltig input
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Ogiltig inmatning. Ange ett positivt heltal.");
                Console.ResetColor();
            }
        }

        // Metod för datum-input med validering (med eller utan standardvärde)
        public static DateTime PromptForDateInput(string prompt, DateTime? defaultValue = null)
        {
            while (true)
            {
                // Visa prompt med nuvarande värde om det finns något
                if (defaultValue.HasValue)
                {
                    Console.Write($"{prompt} (nuvarande: {defaultValue:yyyy-MM-dd}): ");
                }
                else
                {
                    // annars visa bara prompten
                    Console.Write(prompt);
                }

                // Läser input från användaren
                string? input = Console.ReadLine()?.Trim();

                // Om ett standardvärde finns och användaren trycker enter (input är tom), behåll nuvarande värde (gäller vid redigering av resa)
                if (defaultValue.HasValue && string.IsNullOrEmpty(input))
                {
                    return defaultValue.Value; // behåll det nuvarande värdet
                }

                // Försök att parsa input till ett giltigt datum
                if (DateTime.TryParseExact(input, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateValue))
                {
                    // Kontrollera så datumet inte är i framtiden
                    if (dateValue >= DateTime.Today)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Datumet kan inte vara i framtiden. Försök igen.");
                        Console.ResetColor();
                        continue; // be användaren om nytt input
                    }

                    return dateValue; // returnera det giltiga datumet
                }

                // Felmeddelande för ogiltig input
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Ogiltigt datumformat. Ange datum i formatet yyyy-MM-dd.");
                Console.ResetColor();
            }
        }



        // Metod för decimal-input för kostnad av resan med validering (med eller utan standardvärde)
        public static decimal PromptForDecimalInput(string prompt, decimal? defaultValue = null)
        {
            while (true)
            {
                // Visa prompt med nuvarande värde om det finns något
                if (defaultValue.HasValue)
                {
                    Console.Write($"{prompt} (nuvarande: {defaultValue}): ");
                }
                else
                {
                    // annars visa bara prompten
                    Console.Write(prompt);
                }

                // läser in input från användaren
                string? input = Console.ReadLine()?.Trim();

                // Om ett standardvärde finns och användaren trycker enter (input är tom), behåll nuvarande värde (gäller vid redigering av resa)
                if (defaultValue.HasValue && string.IsNullOrEmpty(input))
                {
                    return defaultValue.Value; // behåll det nuvarande värdet
                }

                // Försök att tolka inmatningen som ett giltigt decimaltal
                if (decimal.TryParse(input, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out decimal value) && value >= 0)
                {
                    return value; // returnera giltigt decimaltal
                }

                // Felmeddelande för ogiltig input
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Ogiltig inmatning. Ange endast positiva siffror (exempel: 20000).");
                Console.ResetColor();
            }
        }


        // Metod för att låta användaren välja typ av resa
        // Tar ett strängvärde som prompt som visas för användaren
        public static TripType PromptForTripType(string prompt)
        {
            var validChoices = new Dictionary<string, TripType>(StringComparer.OrdinalIgnoreCase)
            {
                { "semester", TripType.Vacation }, // "semester" mappas till TripType.Vacation
                { "jobbresa", TripType.Business } // "jobbresa" mappas till TripType.Business
            };

            // Startar en loop för att få giltigt input från användaren
            while (true)
            {
                // Visar prompten för användaren
                Console.WriteLine(prompt);
                Console.Write("Skriv ditt val: ");

                // Läser in användarens input
                string? input = Console.ReadLine();

                // Kontrollera om input är giltig genom att kolla om det matchar validChoises
                if (input != null && validChoices.TryGetValue(input, out TripType tripType))
                {
                    // Om giltigt, returnera motsvarande TripType (vacation eller business)
                    return tripType;
                }
                else
                {
                    // Vid ogiltig input, visa ett felmeddelande och be användaren försöka igen
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Ogiltigt val. Vänligen ange 'semester' eller 'jobbresa'.");
                    Console.ResetColor();
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

        // metod för att visa alla resor i dagboken
        public static void DisplayTrips()
       {
            while (true) // Loop för att låta användaren navigera tills de går tillbaka till huvudmenyn
            {
                Console.Clear();
                Console.WriteLine("A L L A   D I N A   R E S O R\n"); // rubrik för listan

                // Kontroll om det inte finns några resor i listan
                if (trips.Count == 0) // Om listan "trips" är tom
                {
                    // Meddelande om det inte finns några resor
                    Console.WriteLine("Inga resor har lagts till i dagboken ännu.\n"); // Meddelade om listan är tom
                    ReturnToMenu(); // Går tillbaka till huvudmenyn
                    return;
                }

                // Visa alla destinationer med index
                Console.WriteLine("Ange den resan du vill läsa mer om.");
                Console.WriteLine("Ange X för att gå tillbaka till huvudmenyn.\n");

                // loopar genom alla resor och skriver ut varje resemål med ett index
                for (int i = 0; i < trips.Count; i++)
                {
                    Console.WriteLine($"[{i}] {trips[i].Destination}"); // Exempel: [0] Paris
                }

                // Låter användaren göra ett val
                Console.Write("\nSkriv ditt val: ");
                string? input = Console.ReadLine(); // läser in användarens val

                // Gå tillbaka till huvudmenyn om användaren skriver "X" eller "x"
                if (input?.Equals("X", StringComparison.OrdinalIgnoreCase) == true)
                {
                    return;
                }

                // Försök att parsa input till ett giltigt index
                if (int.TryParse(input, out int index) && index >= 0 && index < trips.Count)
                {
                    // om input är ett giltigt index, visa detaljer för den resan
                    ShowTripDetails(index);
                }
                else
                {
                    // Felmeddelande vid ogiltigt val
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Ogiltigt val.");
                    Console.WriteLine("Tryck på valfri tangent för att försöka igen...");
                    Console.ResetColor();
                    Console.ReadKey();
                }
            }
        }

        // Metod för att visa detaljerad information om en specifik resa
        public static void ShowTripDetails(int index)
        {
            Console.Clear();
            Trip trip = trips[index]; // Hämtar den valda resan

            Console.WriteLine("R E S E D E T A L J E R\n"); // Rubrik för detaljer
            Console.WriteLine($"Resmål: {trip.Destination}"); // resans destination
            Console.WriteLine($"Kontinent: {trip.Continent}"); // kontinenten där destinationen ligger
            Console.WriteLine($"Längd på resan: {trip.Duration} dagar"); // hur många dagar resan varade
            Console.WriteLine($"Startdatum: {trip.StartDate:yyyy-MM-dd}"); // startdatum på resan
            Console.WriteLine($"Slutdatum: {trip.EndDate:yyyy-MM-dd}"); // slutdatum för resan
            Console.WriteLine($"Totalkostnad: {trip.Cost} kr"); // kostnaden för resan i kronor

            // sammanfogar alla reskompisar eller visar "soloresa" om användaren reste ensam
            string companions = string.Join(", ", trip.Companions);
            Console.WriteLine($"Reskompis(ar) eller soloresa: {companions}");

            // visar typen av resa: semester eller jobbresa
            Console.WriteLine($"Typ av resa: {(trip.Type == TripType.Vacation ? "Semester" : "Jobbresa")}");

            Console.WriteLine("\nAnge X för att gå tillbaka till alla resor.");
            while (true) // loop för att vänta på korrekt input
            {
                string? input = Console.ReadLine(); // läser in användarens input
                if (input?.Equals("X", StringComparison.OrdinalIgnoreCase) == true)
                {
                break; // Gå tillbaka till listan med resor
                }
                else
                {
                // felmeddelande om användaren skriver något annat än "X"
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Ogiltigt val. Ange X för att gå tillbaka.");
                Console.ResetColor();
                }
            }
        }

        // Metod för att redigera en resa med samma validering som för addTrip
        public static void EditTrip()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("R E D I G E R A   E N   R E S A\n");

                // Kontrollera om det finns några resor att redigera
                if (trips.Count == 0)
                {
                    Console.WriteLine("Det finns inga resor att redigera.\n"); // meddelande som visas om det inte finns några resor
                    ReturnToMenu();
                    return;
                }

                // Om resorr finns, visa alla resor med index
                for (int i = 0; i < trips.Count; i++)
                {
                    Console.WriteLine($"[{i}] {trips[i].Destination}");
                }

                // Ber användaren välja en resa att redigera, eller gå tillbaka till huvudmenyn
                Console.Write("\nAnge numret på resan du vill redigera.\n");
                Console.Write("Tryck X för att gå tillbaka till huvudmenyn.\n");
                string? input = Console.ReadLine();

                // Gå tillbaka till huvudmenyn om användaren skriver x
                if (input?.Equals("X", StringComparison.OrdinalIgnoreCase) == true) return;

                // Kontrollera så att användaren anger ett giltigt index
                if (int.TryParse(input, out int index) && index >= 0 && index < trips.Count)
                {
                    // Redigerar vald resa
                    Trip selectedTrip = trips[index];

                    Console.Clear();
                    Console.WriteLine($"Redigerar resan: {selectedTrip.Destination}\n");

                    // Skapar ny information om resan, eller behåll befintlig info
                    selectedTrip.Destination = PromptForInput("\nAnge nytt resmål eller tryck enter för att behålla nuvarande", "destination", selectedTrip.Destination ?? "");
                    selectedTrip.Continent = PromptForInput("\nAnge ny kontinent eller tryck enter för att behålla nuvarande", "continent", selectedTrip.Continent ?? "");
                    selectedTrip.Duration = PromptForIntInput($"\nAnge nya antal dagar eller tryck enter för att behålla nuvarande", selectedTrip.Duration);
                    selectedTrip.StartDate = PromptForDateInput($"\nAnge nytt startdatum eller tryck enter för att behålla nuvarande", selectedTrip.StartDate);
                    selectedTrip.EndDate = PromptForDateInput($"\nAnge nytt slutdatum eller tryck enter för att behålla nuvarande", selectedTrip.EndDate);
                    selectedTrip.Cost = PromptForDecimalInput($"\nAnge ny kostnad eller tryck enter för att behålla nuvarande", selectedTrip.Cost);

                    // Spara den uppdaterade resan och visa meddelande
                    SaveTrips();
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("\nResan har uppdaterats!");
                    ReturnToMenu();
                    Console.ResetColor();
                    return;
                }
                else
                {
                    // Felmeddelande som visas vid ogiltigt val
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Ogiltigt val.");
                    Console.WriteLine("Tryck på valfri tangent för att försöka igen...");
                    Console.ResetColor();
                    Console.ReadKey();
                }
            }

        }

        // Metod för att ta bort en resa
        public static void DeleteTrip()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("T A   B O R T   E N   R E S A\n");

                // Kontrollera om det finns några resor att ta bort
                if (trips.Count == 0)
                {
                    Console.WriteLine("Det finns inga resor att ta bort."); // Meddelande som visas om det inte finns några resor
                    ReturnToMenu();
                    return;
                }

                // Visa alla resor med index, användaren kan trycka x för att gå tillbaka
                Console.WriteLine("Ange numret på resan du vill ta bort.");
                Console.WriteLine("Tryck X för att återgå till huvudmenyn.\n");
                for (int i = 0; i < trips.Count; i++)
                {
                    Console.WriteLine($"[{i}] {trips[i].Destination}");
                }

                // Be användaren ange ett val
                Console.Write("\nSkriv ditt val: ");
                string? input = Console.ReadLine()?.Trim();

                // Hantera X för att återgå till huvudmenyn
                if (input?.Equals("X", StringComparison.OrdinalIgnoreCase) == true)
                {
                    ReturnToMenu();
                    return;
                }

                // Kontrollera att input är en giltig siffra
                if (int.TryParse(input, out int index))
                {
                    if (index >= 0 && index < trips.Count) // Kontrollera att index finns i listan
                    {
                        // en extra bekräftelse (loop) för att ta bort resan
                        while (true)
                        {
                            Console.Clear();
                            Console.WriteLine($"Är du säker på att du vill ta bort resan till \"{trips[index].Destination}\"? (Ja/Nej)");
                            string confirmation = Console.ReadLine()?.Trim().ToUpper() ?? "";

                            // Om användaren skriver Ja, ta bort resan
                            if (confirmation == "JA")
                            {
                                trips.RemoveAt(index);
                                SaveTrips();
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine("\nResan har tagits bort!");
                                Console.ResetColor();
                                ReturnToMenu();
                                return;
                            }
                            
                            // Om användaren skriver Nej, avbryt och ta inte bort
                            else if (confirmation == "NEJ")
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("\nÅtgärden avbröts. Ingen resa har tagits bort.");
                                Console.ResetColor();
                                ReturnToMenu();
                                return;
                            }
                            else
                            {
                                // Felmeddelande för ogiltig input, användaren kan endast skriva in Ja eller Nej
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("Ogiltig inmatning. Ange antingen Ja för att ta bort eller Nej för att avbryta.");
                                Console.WriteLine("Tryck på valfri tangent för att försöka igen...");
                                Console.ResetColor();
                                Console.ReadKey();
                            }
                        }
                    }
                    else
                    {
                        // Felmeddelande om index som användaren anger inte finns i listan
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"Numret {index} finns inte i listan.");
                        Console.WriteLine("Tryck på valfri tangent för att försöka igen...");
                        Console.ResetColor();
                        Console.ReadKey();
                    }
                }
                else
                {
                        // Felmeddelande för ogiltig input (om användaren inte anger en siffra)
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Ogiltig inmatning. Ange ett giltigt nummer från listan.");
                        Console.WriteLine("Tryck på valfri tangent för att försöka igen...");
                        Console.ResetColor();
                        Console.ReadKey();
                }
            }
        }


        /* METODER FÖR PACKLISTA */

        // Skapar en ny packlista 
        public static void CreatePackingList()
       {
            Console.Clear();
            Console.WriteLine("S K A P A   P A C K L I S T A\n"); // rubrik för att visa att en ny packlista ska skapas

            // Frågar användaren efter destination för den nya packlistan
            // Använder validering för att säkerställa korrekt inmatning
            string destination = PromptForInput("Ange resmålet för din kommande resa: ", "destination");

            // Skapar en ny packlista av klassen PackingList och sätter destinationen
            PackingList newList = new PackingList { Destination = destination };

            // Lägger till den nya packlistan i listan med packlistor
            packingLists.Add(newList);
            SavePackingLists(); // Sparar direkt till JSON-filen

            // meddelande till användaren om att packlistan har skapats
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Packlista för {destination} skapad! Tryck på valfri tangent för att lägga till objekt...");
            Console.ResetColor();
            Console.ReadKey();

           // Lägger till nytt objekt/föremål i den nya packlistan
           AddPackingItems(newList);
       }

       // Metod för att lägga till till objekt i en specifik packlista
       public static void AddPackingItems(PackingList packingList)
       {
            while (true) // loop för att fortsätta lägga till objekt tills att användaren avslutar
            {
                Console.Clear();
                Console.WriteLine($"Packlista för {packingList.Destination}\n"); // visar vilken packlista som redigeras

                // Visar befintliga objekt i packlistan om det finns några
                if (packingList.Items.Count == 0)
                {
                    Console.WriteLine("Packlistan är tom.");
                }
                else
                {
                    // loopa igenom och visa alla föremål som finns i packlistan
                    for (int i = 0; i < packingList.Items.Count; i++)
                    {
                        // visar varje föremål med dess index och markerar om det är packat eller inte
                        Console.WriteLine($"[{i}] {(packingList.Items[i].IsPacked ? "[X]" : "[ ]")} {packingList.Items[i].Item}");
                    }
                }
   
                // Låter användaren lägga till ett nytt objekt
                Console.WriteLine("\nAnge ett objekt att lägga till (eller tryck Enter för att avsluta):");
                string? input = Console.ReadLine()?.Trim();

                // om användaren trycker enter utan att skriva något avslutas loopen
                if (string.IsNullOrEmpty(input)) break;

                // Lägger till det nya föremålet i packlistan som opackad från början 
                packingList.Items.Add(new PackingItem { Item = input, IsPacked = false });
                SavePackingLists(); // Sparar efter varje ändring
            }

            // Meddelande till användaren om att packlistan har uppdaterats
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Packlista uppdaterad! Tryck på valfri tangent för att återvända till menyn...");
            Console.ResetColor();
            Console.ReadKey();
        }

        // Visa packlistor
        public static void ViewPackingLists()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("P A C K L I S T O R   F Ö R   K O M M A N D E   R E S O R\n");

                if (packingLists.Count == 0)
                {
                    Console.WriteLine("Du har inga packlistor sparade.\n");
                    Console.WriteLine("Tryck på valfri tangent för att återvända till huvudmenyn...");
                    Console.ReadKey();
                    return;
                }

                // Visa alla packlistor
                for (int i = 0; i < packingLists.Count; i++)
                {
                    Console.WriteLine($"[{i}] {packingLists[i].Destination}");
                }

                Console.WriteLine("\nAnge numret på packlistan du vill hantera, eller X för att återvända:");
                string? input = Console.ReadLine();

                if (input?.Equals("X", StringComparison.OrdinalIgnoreCase) == true)
                    return;

                if (int.TryParse(input, out int index) && index >= 0 && index < packingLists.Count)
                {
                    HandlePackingList(packingLists[index]);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Ogiltigt val. Tryck på valfri tangent för att försöka igen...");
                    Console.ResetColor();
                    Console.ReadKey();
                }
            }
        }

        // Metod för att hantera en specifik packlista
        public static void HandlePackingList(PackingList packingList)
        {
            while (true) // loop för att hantera packlistan tills att användaren väljer att gå tillbaka
            {
                Console.Clear();
                Console.WriteLine($"P A C K L I S T A   F Ö R   {packingList.Destination}\n"); // Rubrik som visar vilekn packlista som hanteras 

                // Visa alla objekt i packlistan eller meddela om listan är tom
                if (packingList.Items.Count == 0) // kontroll om listan är tom
                {
                    Console.WriteLine("Packlistan är tom."); // meddelande som visas om det inte finns några objekt
                }
                else
                {
                    // loopa igenom och visa alla objekt i packlistan
                    for (int i = 0; i < packingList.Items.Count; i++)
                    {
                        // visar varje objekt med index och markerar om det är packat eller ej 
                        Console.WriteLine($"[{i}] {(packingList.Items[i].IsPacked ? "[X]" : "[ ]")} {packingList.Items[i].Item}");
                    }
                }

                // Visar menyalternativen för att hantera packlistan
                Console.WriteLine("\nVälj ett alternativ:");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("[1] - Lägg till ett objekt"); // val för att lägga till nya objekt i listan
                Console.ResetColor();
                Console.WriteLine("[2] - Markera som packad"); // val för att markera objekt som packat
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("[3] - Ta bort ett objekt"); // val för att ta bort ett befintligt objekt
                Console.ResetColor();
                Console.WriteLine("[X] - Gå tillbaka\n"); // val för att återvända till menyn

                // Läser in användarens val
                string? choice = Console.ReadLine();

                // hanterar användarens val baserat på menyval
                switch (choice)
                {
                    case "1":
                        AddPackingItems(packingList); // anropar metod för att lägga till nya objekt
                        break;
                    case "2":
                        MarkItemPacked(packingList); // anropar metod för att markera objekt som packat
                        break;
                    case "3":
                        RemovePackingItem(packingList); // anropar metod för att ta bort ett objekt
                        break;
                    case "X":
                    case "x":
                        return; // avslutar metoden och går tillbaka till föregående meny
                    default:
                        // felmeddelande för ogiltigt val
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Ogiltigt val. Tryck på valfri tangent för att försöka igen...");
                        Console.ResetColor();
                        Console.ReadKey();
                        break;
                }
            }
        }

        // Metod för att markera objekt som packat
        public static void MarkItemPacked(PackingList packingList)
        {
            Console.Write("Ange numret på objektet att markera som packat: "); // ber användaren ange index med det objekt som ska markeras
            string? input = Console.ReadLine();

            // kontrollerar om input är en giltig siffra och om indexet finns i listan
            if (int.TryParse(input, out int index) && index >= 0 && index < packingList.Items.Count)
            {
                // markerar objektet som packat
                packingList.Items[index].IsPacked = true;
                // sparar ändringarna
                SavePackingLists();
                // bekräftelsemeddelande
                Console.WriteLine("Objekt markerat som packat! Tryck på valfri tangent för att fortsätta...");
            }
            else
            {
            // felmeddelande vid ogiltig input
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Ogiltigt val. Tryck på valfri tangent för att försöka igen...");
            Console.ResetColor();
            }
            Console.ReadKey();

        }

        // Metod för att ta bort objekt i packlistan
        public static void RemovePackingItem(PackingList packingList)
        {
            Console.Write("Ange numret på objektet att ta bort: "); // ber användaren att ange index för objekt som ska tas bort
            string? input = Console.ReadLine();

            // kontrollerar om input är en giltig siffra och om index finns i listan
            if (int.TryParse(input, out int index) && index >= 0 && index < packingList.Items.Count)
            {
                var itemToRemove = packingList.Items[index]; // hämtar objektet som ska tas bort
        
                while (true) // loop för att bekräfta borttagningen
                {
                    Console.Clear();
                    Console.WriteLine($"Är du säker på att du vill ta bort objektet \"{itemToRemove.Item}\"? (Ja/Nej)"); // bekräftelsefråga
                    string? confirmation = Console.ReadLine()?.Trim().ToUpper();

                    if (confirmation == "JA") // om användaren bekräftar med ett "ja"
                    {
                        packingList.Items.RemoveAt(index); // tar bort objektet
                        SavePackingLists(); // sparar till jSON-filen
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("\nObjektet har tagits bort!\nTryck på valfri tangent för att fortsätta...");
                        Console.ResetColor();
                        Console.ReadKey();
                        return; // avslutar metoden
                    }
                    else if (confirmation == "NEJ") // om användaren ångrar sig och skriver "nej"
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\nÅtgärden avbröts. Inget objekt har tagits bort.\nTryck på valfri tangent för att fortsätta...");
                        Console.ResetColor();
                        Console.ReadKey();
                        return; // avslutar metoden
                    }
                    else
                    {
                        // felmeddelande vid giltigt svar
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Ogiltig inmatning. Ange 'Ja' för att ta bort eller 'Nej' för att avbryta.");
                        Console.WriteLine("Tryck på valfri tangent för att försöka igen...");
                        Console.ResetColor();
                        Console.ReadKey();
                    }
                }
            }
            else
            {
                // Felmeddelande vid ogiltigt val
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Ogiltigt val. Ange ett giltigt nummer från listan.\nTryck på valfri tangent för att försöka igen...");
                Console.ResetColor();
                Console.ReadKey();
            }
        }

        // Metod för att ta bort en hel packlista
        public static void DeletePackingList()
        {
            while (true) // Loop för att hantera packlistan tills användaren väljer att gå tillbaka
            {
                Console.Clear();
                Console.WriteLine("T A   B O R T   E N   P A C K L I S T A\n");

                // Kontrollera om det finns några packlistor
                if (packingLists.Count == 0) // Om listan är tom
                {
                    Console.WriteLine("Det finns inga packlistor att ta bort."); // Meddelande om listan är tom
                    ReturnToMenu(); // Återgår till huvudmenyn
                    return; // Avslutar metoden
                }

                // Visa alla packlistor med index
                Console.WriteLine("Ange numret på packlistan du vill ta bort.");
                Console.WriteLine("Tryck X för att återgå till huvudmenyn.\n");

                for (int i = 0; i < packingLists.Count; i++)
                {
                    Console.WriteLine($"[{i}] {packingLists[i].Destination}"); // Visar packlistor med destination
                }

                // Be användaren ange sitt val
                Console.Write("\nSkriv ditt val: ");
                string? input = Console.ReadLine()?.Trim();

                // Tryck X för att återgå till huvudmenyn
                if (input?.Equals("X", StringComparison.OrdinalIgnoreCase) == true)
                {
                    ReturnToMenu();
                    return;
                }

                // Kontrollera att input är en giltig siffra och att index finns
                if (int.TryParse(input, out int index) && index >= 0 && index < packingLists.Count)
                {
                    // En extra bekräftelse för att ta bort packlistan
                    while (true)
                    {
                        Console.Clear();
                        Console.WriteLine($"Är du säker på att du vill ta bort packlistan för \"{packingLists[index].Destination}\"? (Ja/Nej)");
                        string confirmation = Console.ReadLine()?.Trim().ToUpper() ?? "";

                        // Om användaren skriver Ja, ta bort packlistan
                        if (confirmation == "JA")
                        {
                            packingLists.RemoveAt(index); // tar bort packlistan från listan
                            SavePackingLists(); // sparar ändringarna till JSON-filen
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("\nPacklistan har tagits bort!");
                            Console.ResetColor();
                            ReturnToMenu(); // återgår till huvudmenyn
                            return; // avslutar metoden
                        }
                        // Om användaren ångrar sig och skriver Nej, avbryt
                        else if (confirmation == "NEJ")
                        {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\nÅtgärden avbröts. Ingen packlista har tagits bort.");
                        Console.ResetColor();
                        ReturnToMenu(); // återgår till huvudmenyn
                        return; // avslutar metoden
                        }
                        else
                        {
                            // Felmeddelande för ogiltig input
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Ogiltig inmatning. Ange antingen 'Ja' för att ta bort eller 'Nej' för att avbryta.");
                            Console.WriteLine("Tryck på valfri tangent för att försöka igen...");
                            Console.ResetColor();
                            Console.ReadKey();
                        }
                    }
                }
                else
                {
                    // Felmeddelande för ogiltig input
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Ogiltig inmatning. Ange ett giltigt nummer från listan.");
                    Console.WriteLine("Tryck på valfri tangent för att försöka igen...");
                    Console.ResetColor();
                    Console.ReadKey();
                }
            }
        }
    
        // Sparar packlistor till JSON-fil
        public static void SavePackingLists()
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string jsonData = JsonSerializer.Serialize(packingLists, options);
            File.WriteAllText(packingListsFile, jsonData);
        }

        // Laddar in packlistor från JSON-fil
        public static void LoadPackingLists()
        {
            if (File.Exists(packingListsFile))
            {
                string jsonData = File.ReadAllText(packingListsFile);
                packingLists = JsonSerializer.Deserialize<List<PackingList>>(jsonData) ?? new List<PackingList>();
            }
        }

        // Metod för att avsluta programmet
        public static void ExitProgram()
        {
            Console.Clear();
            Console.WriteLine("Stänger ner...\n");
            Console.WriteLine("Tack för att du använde resedagboken! Välkommen åter!\n");
            Environment.Exit(0);
        }
    }
}
