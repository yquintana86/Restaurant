﻿using SharedLib;

namespace Domain.Entities;

public class RoomTable
{
    public int Id { get; set; }
    public RoomTableStatusType Status { get; set; }
    public int TotalQty { get; set; }


    #region Navigation Properties

    //Room-RoomTable required relationship FK and reference navigation

    public int RoomId { get; set; }     //This is part of the composite key(Id,RoomId) for this entity
    public Room Room { get; set; } = null!;


    //RoomTable-Waiter required relationship FK and reference navigation

    public int? WaiterId { get; set; }
    public Waiter? Waiter { get; set; } = null!;


    //Table-Reservation
    public ICollection<Reservation> Reservations { get; init; } = new List<Reservation>();

    //Table-Table_Dish
    public ICollection<Dish> Dishes { get; init; } = new List<Dish>();
    public ICollection<TableDish> TablesDishes { get; init; } = new List<TableDish>();


    #endregion
}
