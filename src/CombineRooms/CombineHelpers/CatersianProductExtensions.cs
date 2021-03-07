using System.Collections.Generic;
using System.Linq;

namespace CombineRooms.CombineHelpers
{
    public static class CatersianProductExtensions
    {
        public static IEnumerable<IEnumerable<T>> CartesianProduct<T>(this Dictionary<int, List<T>> sequences)
        {
            IEnumerable<IEnumerable<T>> emptyProduct =
              new[] { Enumerable.Empty<T>() };
            return sequences.Values.Aggregate(
              emptyProduct,
              (accumulator, sequence) =>
                from accseq in accumulator
                from item in sequence
                select accseq.Concat(new[] { item }));
        }
    }
}
