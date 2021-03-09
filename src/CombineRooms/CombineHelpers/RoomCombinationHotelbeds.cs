using CombinationRooms;
using System;
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
            List<List<RoomData<Rate>>> acumulator = new List<List<RoomData<Rate>>>();

            ForEachAvailableCombination(
                this._data.Values,
                board => board,
                c => AcumulateRates(c, acumulator));

            return acumulator;
        }

        private void AcumulateRates(IReadOnlyList<RoomData<Rate>> rates, List<List<RoomData<Rate>>> acumulator)
        {
            if (!TryGetCombinationCommonInfo(rates))
            {
                return;
            }

            acumulator.Add(rates.ToList());
        }

        private static void ForEachAvailableCombination<TSource, TElement>(
            IEnumerable<TSource> enumerable,
            Func<TSource, IEnumerable<TElement>> getElements,
            Action<IReadOnlyList<TElement>> consumeCombination)
        {
            var list = enumerable.ToList();
            var currentCombination = new List<TElement>();

            ForEachAvailableCombinationAux(0);

            void ForEachAvailableCombinationAux(int currentIndex)
            {
                Action<int> action;
                if (currentIndex < list.Count - 1)
                {
                    action = ForEachAvailableCombinationAux;
                }
                else
                {
                    action = ConsumeCurrentCombination;
                }

                var currentElements = getElements(list[currentIndex]);

                foreach (var element in currentElements)
                {
                    currentCombination.Add(element);
                    action(currentIndex + 1);
                    currentCombination.RemoveAt(currentCombination.Count - 1);
                }

                void ConsumeCurrentCombination(int _) => consumeCombination(currentCombination);
            }
        }

        private static bool TryGetCombinationCommonInfo(IReadOnlyList<RoomData<Rate>> combination)
        {
            var boardCode = combination[0].board;

            for (var i = 1; i < combination.Count; ++i)
            {
                if (combination[i].board != boardCode)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
