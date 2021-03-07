using System.Collections.Generic;

namespace CombinationRooms.Combinations
{
    public abstract class BaseRoomDedupeCombination<TRoom> :
        IRoomDedupeCombination<TRoom>
        where TRoom : class
    {
        public abstract void AddRoom(RoomData<TRoom> actualRoom);
        public abstract IEnumerable<IEnumerable<RoomData<TRoom>>> GetDedupedRoomCobinations();

        protected virtual bool ReplaceRoom(RoomData<TRoom> actualRoom, RoomData<TRoom> newRoom)
        {
            return actualRoom.price > newRoom.price;
        }
    }
}
