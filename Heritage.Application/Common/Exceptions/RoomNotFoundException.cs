using Heritage.Application.Models;

namespace Heritage.Application.Common.Exceptions;

public class RoomNotFoundException(int id) :
    NotFoundException($"The room with id: {id} doesn't exist int the database.")
{ }
