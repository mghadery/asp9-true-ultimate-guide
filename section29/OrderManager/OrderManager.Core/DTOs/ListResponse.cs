namespace OrderManager.Core.DTOs;

public class ListResponse<T>
{
    public int TotalCount { get; set; }
    public int PageSize { get; set; }
    public int PageNumber { get; set; }
    public int Count { get; set; }
    public List<T> Result { get; set; }
}
