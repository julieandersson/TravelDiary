namespace TravelDiary
{
    public class Trip
    {
        public string? Destination { get; set; }  // Resmål
        public string? Continent { get; set; }   // Kontinent
        public int Duration { get; set; }        // Antal dagar
        public DateTime StartDate { get; set; }  // Startdatum
        public DateTime EndDate { get; set; }    // Slutdatum
        public decimal Cost { get; set; }        // Kostnad för resan
        public List<string> Companions { get; set; } = new List<string>(); // Lista med reskompis(ar) (eller soloresa)
        public TripType Type { get; set; }       // Typ av resa (semester eller jobbresa)
    }

        // Enum för typ av resa
    public enum TripType
    {
        Vacation,
        Business
    }
}