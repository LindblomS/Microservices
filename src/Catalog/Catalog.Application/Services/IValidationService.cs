namespace Catalog.Application.Services;

public interface IValidationService
{
    bool BrandExists(string brand);
    bool TypeExists(string type);
    bool ItemExists(Guid id);
}
