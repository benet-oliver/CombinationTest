using System.Collections.Generic;

namespace CombineRooms.CombineHelpers
{
    public class RoomDedupeCombinationRoomType<TRoom> : BaseRoomDedupeCombination<TRoom>
        where TRoom : class
    {
        //Hotel-Board-RoomType-RoomCandidateID-RoomHash
        private readonly Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<int, Dictionary<RoomHash, RoomData<TRoom>>>>>> data;

        public RoomDedupeCombinationRoomType()
        {
            data = new Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<int, Dictionary<RoomHash, RoomData<TRoom>>>>>>();
        }

        public override void AddRoom(RoomData<TRoom> actualRoom)
        {
            if (!data.TryGetValue(actualRoom.hotel, out var roomsPerHotel))
            {
                roomsPerHotel = new Dictionary<string, Dictionary<string, Dictionary<int, Dictionary<RoomHash, RoomData<TRoom>>>>>();
                data.Add(actualRoom.hotel, roomsPerHotel);
            }

            if (!roomsPerHotel.TryGetValue(actualRoom.board, out var roomsPerBoard))
            {
                roomsPerBoard = new Dictionary<string, Dictionary<int, Dictionary<RoomHash, RoomData<TRoom>>>>();
                roomsPerHotel.Add(actualRoom.board, roomsPerBoard);
            }

            if (!roomsPerBoard.TryGetValue(actualRoom.roomType, out var roomsPerType))
            {
                roomsPerType = new Dictionary<int, Dictionary<RoomHash, RoomData<TRoom>>>();
                roomsPerBoard.Add(actualRoom.roomType, roomsPerType);
            }

            if (!roomsPerType.TryGetValue(actualRoom.roomCandidateID, out var roomsPerRoomCandidateID))
            {
                roomsPerRoomCandidateID = new Dictionary<RoomHash, RoomData<TRoom>>();
                roomsPerType.Add(actualRoom.roomCandidateID, roomsPerRoomCandidateID);
            }

            if (!roomsPerRoomCandidateID.TryGetValue(actualRoom.roomHash, out var roomsPerHash))
            {
                roomsPerRoomCandidateID.Add(actualRoom.roomHash, actualRoom);
            }
            else if (ReplaceRoom(roomsPerHash, actualRoom))
            {
                roomsPerRoomCandidateID[actualRoom.roomHash] = actualRoom;
            }
        }

        public override IEnumerable<IEnumerable<RoomData<TRoom>>> GetDedupedRoomCobinations()
        {
            Dictionary<int, List<RoomData<TRoom>>> roomsPerCandidateID;

            List<IEnumerable<RoomData<TRoom>>> combinations = new List<IEnumerable<RoomData<TRoom>>>();

            foreach (var hotel in data)
            {
                foreach (var board in hotel.Value)
                {
                    foreach (var roomType in board.Value)
                    {
                        roomsPerCandidateID = new Dictionary<int, List<RoomData<TRoom>>>();

                        foreach (var roomCandidate in roomType.Value)
                        {
                            List<RoomData<TRoom>> roomCandidateIDList = new List<RoomData<TRoom>>();

                            roomsPerCandidateID.Add(roomCandidate.Key, roomCandidateIDList);

                            foreach (var roomHash in roomCandidate.Value)
                            {
                                roomCandidateIDList.Add(roomHash.Value);
                            }
                        }

                        combinations.AddRange(roomsPerCandidateID.CartesianProduct());
                    }
                }
            }

            return combinations;
        }
    }
}
