namespace CombinationRooms.Combinations
{
    public static class RoomDedupeCombinationFactory
    {
        public static IRoomDedupeCombination<TRoom> GetRoomDedupeCombination<TRoom>(ApiVersion apiVersion, int numberOfRooms)
            where TRoom : class
        {
            if (apiVersion == ApiVersion.V1 || numberOfRooms > 3)
            {
                return new RoomDedupeCombinationRoomType<TRoom>();
            }

            return new RoomDedupeCombinationAll<TRoom>();
        }

        public static IRoomDedupeCombination<TRoom> GetRoomCombination<TRoom>()
            where TRoom : class
        {
            return new RoomCombinationAll<TRoom>();
        }
    }

    public enum ApiVersion
    {
        V1,
        V2,
        V21
    }
}
