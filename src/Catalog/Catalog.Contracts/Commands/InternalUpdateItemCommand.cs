namespace Catalog.Contracts.Commands;

using MediatR;

public class InternalUpdateItemCommand : IRequest<bool>
{
    public InternalUpdateItemCommand(Guid id, UpdateItemCommand command)
    {
        Id = id;
        Name = command.Name;
        Description = command.Description;
        Price = command.Price;
        Type = command.Type;
        Brand = command.Brand;
        AvailableStock = command.AvailableStock;
    }

    public Guid Id { get; }
    public string Name { get; }
    public string Description { get; }
    public decimal Price { get; }
    public string Type { get; }
    public string Brand { get; }
    public int AvailableStock { get; }
}

public record UpdateItemCommand(
    string Name,
    string Description,
    decimal Price,
    string Type,
    string Brand,
    int AvailableStock) : IRequest<bool>;