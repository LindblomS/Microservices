namespace Ordering.Application.Services;

using System;

public interface IValidationService
{
    bool OrderExists(Guid id);
}
