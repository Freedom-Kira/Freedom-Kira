namespace api_snake_case.Models;

public class ProductDto
{
    public string? SupplierId { get; set; }

    public ContactDto? Contact { get; set; }

    public List<string>? Tags { get; set; }
}

public class ContactDto
{
    public string? FullName { get; set; }
    public string? PhoneNumber { get; set; }
}

