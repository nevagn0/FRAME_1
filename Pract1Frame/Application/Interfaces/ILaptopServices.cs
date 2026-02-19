namespace Pract1Frame.Application.Interfaces;
using Pract1Frame.Application.Dto;
public interface ILaptopServices
{
    List<LaptopDto> GetAllLaptops();
    LaptopDto? GetLaptopById(Guid id);
    LaptopDto CreateLaptop(CreateLaptopDto dto);
}