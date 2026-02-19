namespace Pract1Frame.Application.Dto;

public class LaptopDto
{
    public Guid Id { get; set; } 
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
}

public class CreateLaptopDto
{
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    
    
}