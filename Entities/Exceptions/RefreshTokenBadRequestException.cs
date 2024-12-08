namespace Entities.Exceptions;

public class RefreshTokenBadRequestException() 
            : BadRequestException("Invalid client request. The tokenDto has some invalid values.");