using Heritage.Application.Models;
using Heritage.Application.RequestFeatures;
using Heritage.Domain;

namespace Heritage.Application.Interfaces;

public interface IRoomRepository
{
    public Task<int> CreateRoomAsync(Room room);
    public Task UpdateRoomAsync(Room room);
    public Task DeleteRoomAsync(int id);
    public Task<PagedList<Room>> GetAllRoomsAsync(RoomParameters parameters);
    public Task<Room> GetRoom(int id);
    public Task SaveChangesAsync();
}
