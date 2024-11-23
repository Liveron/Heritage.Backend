namespace Heritage.Application.Common.Exceptions;

public class UserNotFoundByUsernameException(string userName) :
    NotFoundException($"User with {userName} UserName isn't found.")
{ }
