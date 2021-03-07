using CombinationRooms;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RoomDedupeCombinationBenchmark
{
    public static class HotelbedsCombination
    {

        public static List<List<Rate>> GetBookableOptions(Hotel providerHotel, int numberOfRooms)
        {

            Dictionary<int, List<Rate>> rates = new Dictionary<int, List<Rate>>();

            foreach (var room in providerHotel.rooms)
            {
                foreach (var rate in room.rates)
                {
                    for (int i = 1; i <= numberOfRooms; i++)
                    {
                        if (!rates.TryGetValue(i, out var list))
                        {
                            list = new List<Rate>();
                            rates.Add(i, list);
                        }
                        list.Add(rate);
                    }
                }
            }

            List<List<Rate>> ac = new List<List<Rate>>();

            ForEachAvailableCombination(
                rates.Values,
                board => board,
                c => AcumulateRates(c, ac));

            return ac;
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

        private static void AcumulateRates(IReadOnlyList<Rate> rates, List<List<Rate>> ac)
        {
            if (!TryGetCombinationCommonInfo(rates))
            {
                return;
            }

            ac.Add(rates.ToList());
        }

        private static bool TryGetCombinationCommonInfo(IReadOnlyList<Rate> combination)
        {
            var boardCode = combination[0].boardCode;

            for (var i = 1; i < combination.Count; i += 1)
            {
                if (combination[i].boardCode != boardCode)
                {
                    return false;
                }
            }
            return true;
        }

    }
}
