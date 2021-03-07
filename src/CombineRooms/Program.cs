using CombineRooms.CombineHelpers;
using Newtonsoft.Json;
using System.IO;

namespace CombinationRooms
{
    public static class Program
    {
        private const int numberOfRooms = 3;
        public static void Main(string[] args)
        {
            using StreamReader sr = new StreamReader("response.json");
            var providerResponse = JsonConvert.DeserializeObject<ProviderResponse>(sr.ReadToEnd());

            var roomDedupeCombination1 = RoomDedupeCombinationFactory.GetRoomDedupeCombination<Rate>(ApiVersion.V1, numberOfRooms);

            var roomDedupeCombination2 = RoomDedupeCombinationFactory.GetRoomDedupeCombination<Rate>(ApiVersion.V2, numberOfRooms);

            var roomDedupeCombination3 = RoomDedupeCombinationFactory.GetRoomCombination<Rate>();

            foreach (var hotel in providerResponse.hotels.hotels)
            {
                foreach (var room in hotel.rooms)
                {
                    foreach (var rate in room.rates)
                    {
                        for (int i = 1; i <= numberOfRooms; i++)
                        {
                            var roomData = new RoomData<Rate>(hotel.code.ToString(), rate.boardCode, i, room.code, rate, rate.net, new RoomHash(true));

                            roomDedupeCombination1.AddRoom(roomData);
                            roomDedupeCombination2.AddRoom(roomData);
                            roomDedupeCombination3.AddRoom(roomData);
                        }
                    }
                }
            }

            var combinations1 = roomDedupeCombination1.GetDedupedRoomCombinations();
            var combinations2 = roomDedupeCombination2.GetDedupedRoomCombinations();
            var combinations3 = roomDedupeCombination3.GetDedupedRoomCombinations();
        }
    }
}
