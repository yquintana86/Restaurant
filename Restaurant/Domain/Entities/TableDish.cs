namespace Domain.Entities
{
    public class TableDish
    {
        #region Keys

        public int TableId { get; set; }
        public int TableRoomId { get; set; }
        public int DishId { get; set; }
        public int ReservationId { get; set; }        

        #endregion

        #region  Join Table Payload

        public int OrderedQty { get; set; }       //How many dishes were ordered
        public decimal Amount { get; set; }

        #endregion

        #region Navigation References

        public RoomTable Table { get; set; } = null!;
        public Dish Dish { get; set; } = null!;
        public Reservation Reservation { get; set; } = null!;

        #endregion
    }
}
