using System.Collections.Generic;

namespace CombineRooms.CombineHelpers
{
    public interface IRoomDedupeCombination<TRoom>
        where TRoom : class
    {
        void AddRoom(RoomData<TRoom> actualRoom);

        IEnumerable<IEnumerable<RoomData<TRoom>>> GetDedupedRoomCombinations();
    }
}
