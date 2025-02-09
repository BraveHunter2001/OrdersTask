namespace DAL.Dto;

public abstract class PaginatedFilter
{
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
}