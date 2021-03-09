using CombinationRooms;
using System.Collections.Generic;
using System.Linq;

namespace CombineRooms.CombineHelpers
{
    public class RoomCombinationHotelbeds : IRoomDedupeCombination<Rate>
    {
        private readonly SortedDictionary<int, List<RoomData<Rate>>> _data;

        public RoomCombinationHotelbeds() => this._data = new SortedDictionary<int, List<RoomData<Rate>>>();

        public void AddRoom(RoomData<Rate> actualRoom)
        {

            if (!this._data.TryGetValue(actualRoom.roomCandidateID, out var roomDataList))
            {
                roomDataList = new List<RoomData<Rate>>();

                this._data.Add(actualRoom.roomCandidateID, roomDataList);
            }

            roomDataList.Add(actualRoom);
        }
        public IEnumerable<IEnumerable<RoomData<Rate>>> GetDedupedRoomCombinations()
        {
            var acumulator = new List<IEnumerable<RoomData<Rate>>>();

            foreach (var rates in this._data.CartesianProduct())
            {
                if (!TryGetCombinationCommonInfo(rates))
                {
                    continue;
                }

                acumulator.Add(rates);
            }

            return acumulator;
        }

        private static bool TryGetCombinationCommonInfo(IEnumerable<RoomData<Rate>> combination)
        {
            var boardCode = combination.ElementAt(0).board;

            for (var i = 1; i < combination.Count(); ++i)
            {
                if (combination.ElementAt(i).board != boardCode)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
