namespace DAL.Dto;

public class SuggestDto<T>
{
    public string Label { get; set; }
    public T Value { get; set; }
}