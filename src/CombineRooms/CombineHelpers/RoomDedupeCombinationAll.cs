using System.Collections.Generic;

namespace CombineRooms.CombineHelpers
{
    public class RoomDedupeCombinationAll<TRoom> : BaseRoomDedupeCombination<TRoom>
        where TRoom : class
    {
        //Hotel-Board-RoomCandidateID-RoomType-RoomHash
        private readonly Dictionary<string, Dictionary<string, SortedDictionary<int, Dictionary<string, Dictionary<RoomHash, RoomData<TRoom>>>>>> data;

        public RoomDedupeCombinationAll()
        {
            data = new Dictionary<string, Dictionary<string, SortedDictionary<int, Dictionary<string, Dictionary<RoomHash, RoomData<TRoom>>>>>>();
        }

        public override void AddRoom(RoomData<TRoom> actualRoom)
        {
            if (!data.TryGetValue(actualRoom.hotel, out var roomsPerHotel))
            {
                roomsPerHotel = new Dictionary<string, SortedDictionary<int, Dictionary<string, Dictionary<RoomHash, RoomData<TRoom>>>>>();
                data.Add(actualRoom.hotel, roomsPerHotel);
            }

            if (!roomsPerHotel.TryGetValue(actualRoom.board, out var roomsPerBoard))
            {
                roomsPerBoard = new SortedDictionary<int, Dictionary<string, Dictionary<RoomHash, RoomData<TRoom>>>>();
                roomsPerHotel.Add(actualRoom.board, roomsPerBoard);
            }

            if (!roomsPerBoard.TryGetValue(actualRoom.roomCandidateID, out var roomsPerRoomCandidateID))
            {
                roomsPerRoomCandidateID = new Dictionary<string, Dictionary<RoomHash, RoomData<TRoom>>>();
                roomsPerBoard.Add(actualRoom.roomCandidateID, roomsPerRoomCandidateID);
            }

            if (!roomsPerRoomCandidateID.TryGetValue(actualRoom.roomType, out var roomsPerType))
            {
                roomsPerType = new Dictionary<RoomHash, RoomData<TRoom>>();
                roomsPerRoomCandidateID.Add(actualRoom.roomType, roomsPerType);
            }

            if (!roomsPerType.TryGetValue(actualRoom.roomHash, out var roomsPerHash))
            {
                roomsPerType.Add(actualRoom.roomHash, actualRoom);
            }
            else if (ReplaceRoom(roomsPerHash, actualRoom))
            {
                roomsPerType[actualRoom.roomHash] = actualRoom;
            }
        }

        public override IEnumerable<IEnumerable<RoomData<TRoom>>> GetDedupedRoomCombinations()
        {
            SortedDictionary<int, List<RoomData<TRoom>>> roomsPerCandidateID;

            List<IEnumerable<RoomData<TRoom>>> combinations = new List<IEnumerable<RoomData<TRoom>>>();

            foreach (var hotel in data)
            {
                foreach (var board in hotel.Value)
                {
                    roomsPerCandidateID = new SortedDictionary<int, List<RoomData<TRoom>>>();

                    foreach (var roomCandidate in board.Value)
                    {
                        List<RoomData<TRoom>> roomCandidateIDList = new List<RoomData<TRoom>>();

                        roomsPerCandidateID.Add(roomCandidate.Key, roomCandidateIDList);

                        foreach (var roomType in roomCandidate.Value)
                        {
                            foreach (var roomHash in roomType.Value)
                            {
                                roomCandidateIDList.Add(roomHash.Value);
                            }
                        }
                    }

                    combinations.AddRange(roomsPerCandidateID.CartesianProduct());
                }
            }

            return combinations;
        }
    }
}
