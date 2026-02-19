using Pract1Frame.Application.Dto;
using Pract1Frame.Application.Interfaces;
using Pract1Frame.Domain;
using Pract1Frame.Domain.Interfaces;

namespace Pract1Frame.Application.Services;

public class LaptopService : ILaptopServices
{
    private readonly ILaptop _laptopRepository;

    public LaptopService(ILaptop laptopRepository)
    {
        _laptopRepository = laptopRepository;
    }
    
    public List<LaptopDto> GetAllLaptops()
    {
        var laptops = _laptopRepository.GetAllLaptops();
        
        return laptops.Select(l => new LaptopDto
        {
            Id = l.Id,
            Name = l.Name,
            Price = l.Price
        }).ToList();
    }
    
    public LaptopDto? GetLaptopById(Guid id)
    {
        var laptop = _laptopRepository.GetById(id);
        
        if (laptop == null)
            return null;
        
        return new LaptopDto
        {
            Id = laptop.Id,
            Name = laptop.Name,
            Price = laptop.Price
        };
    }
    
    public LaptopDto CreateLaptop(CreateLaptopDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name))
            throw new ArgumentException("Название ноутбука не может быть пустым");
        
        if (dto.Price <= 0)
            throw new ArgumentException("Цена должна быть меньше 0");
        
        if (dto.Price > 1_000_000)
            throw new ArgumentException("Цена не может быть больше 1 000 000");
        
        var laptop = new Laptop
        {
            Name = dto.Name.Trim(),
            Price = dto.Price
        };
        
        var created = _laptopRepository.CreateLaptop(laptop);
        
        return new LaptopDto
        {
            Id = created.Id,
            Name = created.Name,
            Price = created.Price
        };
    }
}