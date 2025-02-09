namespace DAL.Dto;

public class ItemListFilter : PaginatedFilter
{
    public string Code { get; set; }
    public string[] Categories { get; set; }
}