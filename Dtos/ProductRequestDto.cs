namespace ProductService.Dtos;
public class ProductRequestDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public decimal Price { get; set; }
}
