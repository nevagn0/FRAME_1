namespace Pract1Frame.Domain.Interfaces;

public interface ILaptop
{
    Laptop CreateLaptop(Laptop laptop);
    Laptop GetById(Guid id);
    List<Laptop> GetAllLaptops();
}