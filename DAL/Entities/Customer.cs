namespace DAL.Entities;

// note: как правильно хранить customer
// относительно пользователя
public class Customer : User
{
    public string Name { get; set; }
    public string Code { get; set; }
    public string Address { get; set; }
    public decimal? Discount { get; set; }

    public Customer(string login, string password, UserRole role, string name, string address, decimal? discount) : 
        base(login, password, role)
    {
        Name = name;
        Address = address;
        Discount = discount;
    }
}