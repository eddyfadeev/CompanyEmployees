namespace Entities.Exceptions;

public class CollectionByIdsBadRequestException() : BadRequestException("Collection does not match the given IDs.");