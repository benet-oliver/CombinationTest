using System.Collections.Generic;

namespace CombineRooms.CombineHelpers
{
    public abstract class BaseRoomDedupeCombination<TRoom> :
        IRoomDedupeCombination<TRoom>
        where TRoom : class
    {
        public abstract void AddRoom(RoomData<TRoom> actualRoom);
        public abstract IEnumerable<IEnumerable<RoomData<TRoom>>> GetDedupedRoomCobinations();

        protected virtual bool ReplaceRoom(RoomData<TRoom> actualRoom, RoomData<TRoom> newRoom) => actualRoom.price > newRoom.price;
    }
}
