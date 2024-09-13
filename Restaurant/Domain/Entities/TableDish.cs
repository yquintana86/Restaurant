namespace Domain.Entities
{
    public class TableDish
    {
        #region Keys
        public int TableId { get; set; }
        public int RoomId { get; set; }
        public int DishId { get; set; }
        public int ShiftId { get; set; }
        public DateTime Date { get; set; }

        #endregion

        #region  Join Table Payload
        public int OrderQty { get; set; }       //How many dishes were ordered
        public decimal Amount { get; set; }

        #endregion

        #region Navigation References
        public Table Table { get; set; } = null!;
        public Dish Dish { get; set; } = null!;
        public Shift Shift { get; set; } = null!;

        #endregion
    }
}
