﻿namespace DAL.Entities;

public class Item
{
    public Guid Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string Category { get; set; }

    public Item()
    {
        
    }
    public Item(string name, decimal price, string category)
    {
        Name = name;
        Price = price;
        Category = category;
    }
}