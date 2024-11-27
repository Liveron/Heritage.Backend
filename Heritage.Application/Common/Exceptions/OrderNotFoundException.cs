using Heritage.Application.Models;

namespace Heritage.Application.Common.Exceptions;

public class OrderNotFoundException(Guid id) :
    NotFoundException($"The order with id: {id} doesn't exist int the database.")
{ }
