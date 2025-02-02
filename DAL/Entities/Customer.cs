namespace DAL.Entities;

// note: как правильно хранить customer
// относительно пользователя
public class Customer : User
{
    public string Name { get; set; }
    public string Code { get; set; }
    public string Address { get; set; }
    public decimal Discount { get; set; }
}