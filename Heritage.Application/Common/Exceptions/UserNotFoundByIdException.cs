namespace Heritage.Application.Common.Exceptions;

public class UserNotFoundByIdException(Guid id) 
    : NotFoundException($"User with id: {id} isn't found.")
{ }
