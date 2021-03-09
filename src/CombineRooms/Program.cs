using CombineRooms.CombineHelpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace CombinationRooms
{
    public static class Program
    {
        private static readonly List<int> _numberOfRoomsList = new List<int>() { 1, 2, 3, 4, 5, 6, 7 };

        public static void Main(string[] args)
        {
            foreach(var room in _numberOfRoomsList)
            {
                GenerateCombinations(room);
            }
        }

        private static void GenerateCombinations(int numberOfRooms)
        {
            Stopwatch sw = new Stopwatch();

            Console.WriteLine($"Starting combination for {numberOfRooms} rooms");

            List<IRoomDedupeCombination<Rate>> roomDedupeList = new List<IRoomDedupeCombination<Rate>>();
            roomDedupeList.Add(new RoomDedupeCombinationRoomType<Rate>());
            roomDedupeList.Add(new RoomDedupeCombinationAll<Rate>());
            roomDedupeList.Add(new RoomCombinationAll<Rate>());
            roomDedupeList.Add(new RoomCombinationHotelbeds());
            //roomDedupeList.Add(new RoomCombinationHotelbedsAlternative());

            AddRoomsToRoomDedupeCombinationList(GetFakeResponse(), roomDedupeList, numberOfRooms);

            foreach (var roomDedupe in roomDedupeList)
            {
                sw.Restart();
                PrintOptions(roomDedupe, roomDedupe.GetDedupedRoomCombinations().Count(), sw.ElapsedMilliseconds);
            }

            Console.WriteLine($"Finish combination for {numberOfRooms} rooms");

        }

        private static ProviderResponse GetFakeResponse()
        {
            using StreamReader sr = new StreamReader("Provider/response.json");
            return JsonConvert.DeserializeObject<ProviderResponse>(sr.ReadToEnd());
        }

        private static void AddRoomsToRoomDedupeCombinationList(
            ProviderResponse providerResponse,
            List<IRoomDedupeCombination<Rate>> roomDedupeList,
            int numberOfRooms)
        {
            foreach (var (hotel, room, rate) in from hotel in providerResponse.hotels.hotels
                                                from room in hotel.rooms
                                                from rate in room.rates
                                                select (hotel, room, rate))
            {
                for (int i = 1; i <= numberOfRooms; i++)
                {
                    var roomData = new RoomData<Rate>(hotel.code.ToString(), rate.boardCode, i, room.code, rate, rate.net, new RoomHash(true));

                    foreach (var roomDedupe in roomDedupeList)
                    {
                        roomDedupe.AddRoom(roomData);
                    }
                }
            }
        }

        private static void PrintOptions(IRoomDedupeCombination<Rate> roomDedupeCombination, int count, long elapsedMs)
        {
            Console.WriteLine($"Implementation {roomDedupeCombination.GetType().Name} return {count} options, elapsed {elapsedMs} ms");
        }
    }
}
