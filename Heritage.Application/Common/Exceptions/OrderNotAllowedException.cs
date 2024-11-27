namespace Heritage.Application.Common.Exceptions;

public class OrderNotAllowedException(string username)
    : NotAllowedException($"Order is not allowed for user {username}")
{ }