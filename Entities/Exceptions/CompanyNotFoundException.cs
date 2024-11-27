namespace Entities.Exceptions;

public class CompanyNotFoundException(Guid id)
    : NotFoundException($"The company with id: {id} does not exist in the database.");