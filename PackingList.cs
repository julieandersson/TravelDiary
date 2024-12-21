namespace TravelDiary
{
    public class PackingList
{
    public string? Destination { get; set; } // resmålet för kommande resa
    public List<PackingItem> Items { get; set; } = new List<PackingItem>(); // lista över packningsobjekt
}

}