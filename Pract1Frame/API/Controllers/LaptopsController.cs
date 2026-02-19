using Microsoft.AspNetCore.Mvc;
using Pract1Frame.Application.Dto;
using Pract1Frame.Application.Interfaces;

namespace Pract1Frame.Controllers;

[ApiController]
[Route("api/laptops")]
public class LaptopsController : ControllerBase
{
    private readonly ILaptopServices _laptopServices;
    private readonly ILogger<LaptopsController> _logger;

    public LaptopsController(
        ILaptopServices laptopServices, 
        ILogger<LaptopsController> logger)
    {
        _laptopServices = laptopServices;
        _logger = logger;
    }

    [HttpGet]
    public IActionResult GetAllLaptops()
    {
        var requestId = HttpContext.Items["RequestId"]?.ToString();
        _logger.LogInformation("[{RequestId}] Получение всех ноутбуков", requestId);
        
        var laptops = _laptopServices.GetAllLaptops();
        return Ok(laptops);
    }

    [HttpGet("{id}")]
    public IActionResult GetById(Guid id)
    {
        var requestId = HttpContext.Items["RequestId"]?.ToString();
        _logger.LogInformation("[{RequestId}] Поиск ноутбука с ID: {Id}", requestId, id);
        
        var laptop = _laptopServices.GetLaptopById(id);
        if (laptop == null)
        {
            throw new KeyNotFoundException($"Ноутбук с id {id} не найден");
        }
        
        _logger.LogInformation("[{RequestId}] Ноутбук найден: {Name}", requestId, laptop.Name);
        return Ok(laptop);
    }

    [HttpPost]
    public IActionResult CreateLaptop([FromBody] CreateLaptopDto dto)
    {
        var requestId = HttpContext.Items["RequestId"]?.ToString();
        _logger.LogInformation("[{RequestId}] Создание ноутбука: {Name}", requestId, dto.Name);
        
        try
        {
            var laptop = _laptopServices.CreateLaptop(dto);
            return CreatedAtAction(nameof(GetById), new { id = laptop.Id }, laptop);
        }
        catch (ArgumentException ex)
        {
            throw new ArgumentException(ex.Message);
        }
    }
}