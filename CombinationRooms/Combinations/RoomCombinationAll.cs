using System.Collections.Generic;

namespace CombinationRooms.Combinations
{
    public class RoomCombinationAll<TRoom> : IRoomDedupeCombination<TRoom>
        where TRoom : class
    {

        //Hotel-Board-RoomCandidateID-RoomType-RoomHash
        private readonly Dictionary<string, Dictionary<string, Dictionary<int, List<RoomData<TRoom>>>>> data;


        public RoomCombinationAll()
        {
            data = new Dictionary<string, Dictionary<string, Dictionary<int, List<RoomData<TRoom>>>>>();
        }

        public void AddRoom(RoomData<TRoom> actualRoom)
        {
            if (!data.TryGetValue(actualRoom.hotel, out var roomsPerHotel))
            {
                roomsPerHotel = new Dictionary<string, Dictionary<int, List<RoomData<TRoom>>>>();
                data.Add(actualRoom.hotel, roomsPerHotel);
            }

            if (!roomsPerHotel.TryGetValue(actualRoom.board, out var roomsPerBoard))
            {
                roomsPerBoard = new Dictionary<int, List<RoomData<TRoom>>>();
                roomsPerHotel.Add(actualRoom.board, roomsPerBoard);
            }

            if (!roomsPerBoard.TryGetValue(actualRoom.roomCandidateID, out var roomsPerRoomCandidateID))
            {
                roomsPerRoomCandidateID = new List<RoomData<TRoom>>();
                roomsPerBoard.Add(actualRoom.roomCandidateID, roomsPerRoomCandidateID);
            }

            roomsPerRoomCandidateID.Add(actualRoom);

        }

        public IEnumerable<IEnumerable<RoomData<TRoom>>> GetDedupedRoomCobinations()
        {
            List<IEnumerable<RoomData<TRoom>>> combinations = new List<IEnumerable<RoomData<TRoom>>>();

            foreach (var hotel in data)
            {
                foreach (var board in hotel.Value)
                {
                    combinations.AddRange(board.Value.CartesianProduct());
                }
            }

            return combinations;
        }
    }
}
