namespace Pract1Frame.Domain;

public sealed class Laptop
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; }
    public decimal Price { get; set; }
}