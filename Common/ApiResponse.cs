namespace ProductService.Common;
public class ApiResponse<T>
{
    public T Data { get; set; }
    public string Message { get; set; } = string.Empty;
    public DateTime date { get; set; } = DateTime.Now;
    public bool Success { get; set; } = true;
    public int StatusCode { get; set; }
}
