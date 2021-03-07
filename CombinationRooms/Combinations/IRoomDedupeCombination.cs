using CombinationRooms.Combinations;
using System.Collections.Generic;

namespace CombinationRooms
{
    public interface IRoomDedupeCombination<TRoom>
        where TRoom : class
    {
        void AddRoom(RoomData<TRoom> actualRoom);

        IEnumerable<IEnumerable<RoomData<TRoom>>> GetDedupedRoomCobinations();
    }


}
