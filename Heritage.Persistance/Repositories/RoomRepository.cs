using Heritage.Application.Common.Exceptions;
using Heritage.Application.Interfaces;
using Heritage.Application.Models;
using Heritage.Application.RequestFeatures;
using Heritage.Domain;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Heritage.Persistance.Repositories;

public class RoomRepository(HeritageDbContext dbContext) : IRoomRepository
{
    private readonly HeritageDbContext _dbContext = dbContext;

    public async Task<int> CreateRoomAsync(Room room)
    {
        await _dbContext.AddAsync(room);
        return room.Id;
    }

    public async Task DeleteRoomAsync(int id)
    {
        Room room = await _dbContext.Rooms.AsNoTracking()
            .FirstOrDefaultAsync(room => room.Id == id) ??
            throw new RoomNotFoundException(id);

        _dbContext.Rooms.Remove(room);
    }

    public async Task<PagedList<Room>> GetAllRoomsAsync(RoomParameters parameters)
    {
        List<Room> rooms = await _dbContext.Rooms.AsNoTracking()
            .Skip((parameters.PageNumber - 1) * parameters.PageSize)
            .Take(parameters.PageSize)
            .ToListAsync();

        int count = await _dbContext.Rooms.AsNoTracking()
            .CountAsync();

        return new PagedList<Room>(rooms, count, parameters.PageNumber, parameters.PageSize);
    }

    public async Task<Room> GetRoom(int id)
    {
        Room room = await _dbContext.Rooms.FindAsync(id) ??
            throw new RoomNotFoundException(id);

        return room;
    }

    public async Task UpdateRoomAsync(Room room)
    {
        _ = await _dbContext.Rooms.AsNoTracking()
            .FirstOrDefaultAsync(r => r.Id == room.Id) ??
            throw new RoomNotFoundException(room.Id);

        _dbContext.Entry(room).State = EntityState.Modified;
    }
    public async Task SaveChangesAsync()
    {
        await _dbContext.SaveChangesAsync();
    }
}
