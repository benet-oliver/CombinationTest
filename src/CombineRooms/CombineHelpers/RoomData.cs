namespace CombineRooms.CombineHelpers
{
    public class RoomData<TRoom>
    {
        public readonly string hotel;

        public readonly string board;

        public readonly int roomCandidateID;

        public readonly string roomType;

        public readonly TRoom room;

        public readonly decimal price;

        public readonly RoomHash roomHash;

        public RoomData(string hotel, string board, int roomCandidateID, string roomType, TRoom room, decimal price, RoomHash roomHash)
        {
            this.hotel = hotel;
            this.board = board;
            this.roomCandidateID = roomCandidateID;
            this.roomType = roomType;
            this.room = room;
            this.price = price;
            this.roomHash = roomHash;
        }
    }
}
