namespace SoccerStore.Api.Models;

// This class represents the body of a sell request.
// When the frontend wants to sell an item, it sends how many units to sell.
public class SellRequest
{
    public int Quantity { get; set; }
}
