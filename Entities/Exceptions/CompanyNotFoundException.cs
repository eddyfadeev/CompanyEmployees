namespace Entities.Exceptions;

public class CompanyNotFoundException : NotFoundException 
{
    public CompanyNotFoundException(Guid id) 
        : base($"The company with id: { id } does not exist in the database.")
    {
    }
}