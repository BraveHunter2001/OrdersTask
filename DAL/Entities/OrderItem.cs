﻿namespace DAL.Entities;

public class OrderItem
{
    public Guid Id { get; set; }

    public Guid OrderId { get; set; }
    public Order Order { get; set; }

    public Guid ItemId { get; set; }
    public Item Item { get; set; }
    public int ItemsCount { get; set; }
    public decimal ItemPrice { get; set; }
}