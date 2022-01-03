namespace Basket.API.Models;

public class BasketCheckout
{
    public string BuyerId { get; set; }
    public string Username { get; set; }
    public Address Address { get; set; }
    public Card Card { get; set; }
}
