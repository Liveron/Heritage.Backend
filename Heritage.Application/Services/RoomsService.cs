using Heritage.Application.DataTransferObjects;
using Heritage.Application.Interfaces;
using Heritage.Application.RequestFeatures;
using Heritage.Domain;
using Mapster;

namespace Heritage.Application.Services;

public interface IRoomsService
{
    public Task<int> CreateRoom(CreateRoomDto createRoomDto);
    public Task<RoomDto> GetRoom(int id);
    public Task<(List<RoomDto> rooms, MetaData metaData)> GetAllRooms(RoomParameters parameters);
    public Task DeleteRoom(int id);
    public Task UpdateRoom(int id, UpdateRoomDto updateRoomDto);
}

public class RoomsService(IRoomRepository roomRepository) : IRoomsService
{
    private readonly IRoomRepository _roomRepository = roomRepository;

    public async Task<int> CreateRoom(CreateRoomDto roomDto)
    {
        Room room = roomDto.Adapt<Room>();
        int id = await _roomRepository.CreateRoomAsync(room);
        await _roomRepository.SaveChangesAsync();
        return id;
    }

    public async Task DeleteRoom(int id)
    {
        await _roomRepository.DeleteRoomAsync(id);
        await _roomRepository.SaveChangesAsync();
    }

    public async Task<RoomDto> GetRoom(int id)
    {
        Room room = await _roomRepository.GetRoom(id);
        return room.Adapt<RoomDto>();
    }

    public async Task<(List<RoomDto> rooms, MetaData metaData)> GetAllRooms(RoomParameters parameters)
    {
        PagedList<Room> employeesWithMetaData = await _roomRepository.GetAllRoomsAsync(parameters);
        List<RoomDto> rooms = employeesWithMetaData.Adapt<List<RoomDto>>();
        return (rooms, metaData: employeesWithMetaData.MetaData);
    }

    public async Task UpdateRoom(int id, UpdateRoomDto updateRoomDto)
    {
        Room room = updateRoomDto.BuildAdapter()
            .AddParameters("id", id)
            .AdaptToType<Room>();

        await _roomRepository.UpdateRoomAsync(room);
        await _roomRepository.SaveChangesAsync();
    }
}
