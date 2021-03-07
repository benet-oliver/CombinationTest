namespace CombinationRooms.Combinations
{
    public class RoomHash
    {
        protected readonly bool nrf;
        public RoomHash(bool nrf)
        {
            this.nrf = nrf;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return nrf.GetHashCode();
            }
        }

        public override bool Equals(object obj)
        {
            return obj != null && obj is RoomHash roomHash && roomHash.nrf == nrf;
        }


    }

    public class RoomHashWithCancelPenalties<TCancelPenalties> : RoomHash
    {

        protected readonly TCancelPenalties cancelPenalties;
        public RoomHashWithCancelPenalties(bool nrf, TCancelPenalties cancelPenalties):base(nrf)
        {
            this.cancelPenalties = cancelPenalties;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

}
