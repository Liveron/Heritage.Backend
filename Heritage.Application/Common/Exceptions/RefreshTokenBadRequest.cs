namespace Heritage.Application.Common.Exceptions;

public sealed class RefreshTokenBadRequest() 
    : BadRequestException("Invalid client request. The tokenDto has some invalid values.") 
{ }
