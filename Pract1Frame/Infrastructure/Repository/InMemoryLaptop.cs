using Pract1Frame.Domain;
using Pract1Frame.Domain.Interfaces;

namespace Pract1Frame.Infrastructure.Repository;

public class InMemoryLaptop : ILaptop
{
    private readonly List<Laptop> _laptops;
    
    public InMemoryLaptop()
    {
        _laptops = new List<Laptop>(); 
    }

    public Laptop CreateLaptop(Laptop laptop)
    {
        _laptops.Add(laptop);
        return laptop;
    }

    public Laptop? GetById(Guid id) 
    {
        return _laptops.FirstOrDefault(l => l.Id == id);
    }

    public List<Laptop> GetAllLaptops()
    {
        return _laptops.ToList(); 
    }
}