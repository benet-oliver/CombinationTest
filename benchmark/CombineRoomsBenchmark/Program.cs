using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using CombinationRooms;
using CombineRooms.CombineHelpers;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace RoomDedupeCombinationBenchmark
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            _ = BenchmarkRunner.Run<Combination>();
        }
    }

    [SimpleJob(RuntimeMoniker.NetCoreApp31, launchCount: 1, warmupCount: 3, targetCount: 4)]
    [RPlotExporter]
    [MemoryDiagnoser]
    public class Combination
    {
        private ProviderResponse providerResponse;

        private readonly Consumer consumer = new Consumer();

        [Params(1, 2, 3, 4, 5)]
        public int N;

        [GlobalSetup]
        public void Setup()
        {
            using StreamReader sr = new StreamReader("response.json");
            providerResponse = JsonConvert.DeserializeObject<ProviderResponse>(sr.ReadToEnd());
        }

        //[Benchmark]
        //public void RoomDedupeCombinationAll()
        //{
        //    Combine(new RoomDedupeCombinationAll<Rate>()).Consume(consumer);
        //}

        //[Benchmark]
        //public void RoomDedupeCombinationRoomType()
        //{
        //    Combine(new RoomDedupeCombinationRoomType<Rate>()).Consume(consumer);
        //}

        //[Benchmark]
        //public void RoomCombinationAll()
        //{
        //    Combine(new RoomCombinationAll<Rate>()).Consume(consumer);
        //}

        [Benchmark]
        public void RoomCombinationHotelbedsAlternative()
        {
            Combine(new RoomCombinationHotelbedsAlternative()).Consume(consumer);
        }

        [Benchmark]
        public void RoomCombinationHotelbeds()
        {
            Combine(new RoomCombinationHotelbeds()).Consume(consumer);
        }

        private IEnumerable<IEnumerable<RoomData<Rate>>> Combine(IRoomDedupeCombination<Rate> roomDedupeCombination)
        {
            foreach (var hotel in providerResponse.hotels.hotels)
            {
                foreach (var room in hotel.rooms)
                {
                    foreach (var rate in room.rates)
                    {
                        for (int i = 1; i <= N; i++)
                        {
                            var roomData = new RoomData<Rate>(hotel.code.ToString(), rate.boardCode, i, room.code, rate, rate.net, new RoomHash(true));
                            roomDedupeCombination.AddRoom(roomData);
                        }
                    }
                }
            }

            return roomDedupeCombination.GetDedupedRoomCombinations();
        }
    }
}
