namespace Entities.Exceptions;

public class EmployeeNotFoundException(Guid employeeId)
    : Exception($"Employee with id: {employeeId} does not exist in database.");