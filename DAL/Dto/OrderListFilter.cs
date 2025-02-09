using DAL.Entities;

namespace DAL.Dto;

public class OrderListFilter : PaginatedFilter
{
    public OrderStatus? Status { get; set; }
}

