namespace Basket.Contracts.Models;

public class Card
{
    public string Number { get; set; }
    public string HolderName { get; set; }
    public DateTime Expiration { get; set; }
    public string SecurityNumber { get; set; }
    public int TypeId { get; set; }
}
