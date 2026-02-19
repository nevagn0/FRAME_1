using Moq;
using FluentAssertions;
using Pract1Frame.Application.Dto;
using Pract1Frame.Application.Interfaces;
using Pract1Frame.Application.Services;
using Pract1Frame.Domain;
using Pract1Frame.Domain.Interfaces;

namespace TestProject1;

public class LaptopServiceTests
{
    private Mock<ILaptop> _mockRepo;
    private ILaptopServices _service;
    private List<Laptop> _testLaptops;

    [SetUp]
    public void Setup()
    {
        _mockRepo = new Mock<ILaptop>();
        _service = new LaptopService(_mockRepo.Object);
        
        _testLaptops = new List<Laptop>
        {
            new Laptop { Id = Guid.NewGuid(), Name = "MacBook Pro", Price = 250000 },
            new Laptop { Id = Guid.NewGuid(), Name = "Dell XPS", Price = 180000 },
            new Laptop { Id = Guid.NewGuid(), Name = "Lenovo ThinkPad", Price = 150000 }
        };
    }
    

    [Test]
    public void GetAllLaptops_ShouldReturnAllLaptops()
    {
        _mockRepo.Setup(r => r.GetAllLaptops()).Returns(_testLaptops);
        
        var result = _service.GetAllLaptops();
        
        result.Should().NotBeNull();
        result.Should().HaveCount(3);
        result[0].Name.Should().Be("MacBook Pro");
    }

    [Test]
    public void GetAllLaptops_WhenRepoEmpty_ShouldReturnEmptyList()
    {
        _mockRepo.Setup(r => r.GetAllLaptops()).Returns(new List<Laptop>());
        
        var result = _service.GetAllLaptops();
        
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }
    

    [Test]
    public void GetLaptopById_WithValidId_ShouldReturnLaptop()
    {
        var expectedLaptop = _testLaptops[0];
        _mockRepo.Setup(r => r.GetById(expectedLaptop.Id)).Returns(expectedLaptop);
        
        var result = _service.GetLaptopById(expectedLaptop.Id);
        
        result.Should().NotBeNull();
        result!.Id.Should().Be(expectedLaptop.Id);
        result.Name.Should().Be(expectedLaptop.Name);
        result.Price.Should().Be(expectedLaptop.Price);
    }

    [Test]
    public void GetLaptopById_WithInvalidId_ShouldReturnNull()
    {
        var invalidId = Guid.NewGuid();
        _mockRepo.Setup(r => r.GetById(invalidId)).Returns((Laptop)null);
        
        var result = _service.GetLaptopById(invalidId);
        
        result.Should().BeNull();
    }
    

    [Test]
    public void CreateLaptop_WithValidData_ShouldCreateAndReturnLaptop()
    {
        var createDto = new CreateLaptopDto
        {
            Name = "ASUS ROG",
            Price = 220000
        };

        var expectedLaptop = new Laptop
        {
            Id = Guid.NewGuid(),
            Name = createDto.Name,
            Price = createDto.Price
        };

        _mockRepo.Setup(r => r.CreateLaptop(It.IsAny<Laptop>()))
            .Returns(expectedLaptop);
        
        var result = _service.CreateLaptop(createDto);
        
        result.Should().NotBeNull();
        result.Name.Should().Be(createDto.Name);
        result.Price.Should().Be(createDto.Price);
        
        _mockRepo.Verify(r => r.CreateLaptop(It.Is<Laptop>(l => 
            l.Name == createDto.Name && 
            l.Price == createDto.Price)), Times.Once);
    }
    

    [Test]
    public void CreateLaptop_WithEmptyName_ShouldThrowArgumentException()
    {
        var createDto = new CreateLaptopDto
        {
            Name = "",
            Price = 220000
        };
        
        var ex = Assert.Throws<ArgumentException>(() => _service.CreateLaptop(createDto));
        ex.Message.Should().Contain("Название ноутбука не может быть пустым");
    }

    [Test]
    public void CreateLaptop_WithNullName_ShouldThrowArgumentException()
    {
        var createDto = new CreateLaptopDto
        {
            Name = null,
            Price = 220000
        };
        
        var ex = Assert.Throws<ArgumentException>(() => _service.CreateLaptop(createDto));
        ex.Message.Should().Contain("Название ноутбука не может быть пустым");
    }

    [Test]
    public void CreateLaptop_WithZeroPrice_ShouldThrowArgumentException()
    {
        var createDto = new CreateLaptopDto
        {
            Name = "Test Laptop",
            Price = 0
        };
        
        var ex = Assert.Throws<ArgumentException>(() => _service.CreateLaptop(createDto));
        ex.Message.Should().Contain("Цена должна быть больше 0");
    }

    [Test]
    public void CreateLaptop_WithNegativePrice_ShouldThrowArgumentException()
    {
        var createDto = new CreateLaptopDto
        {
            Name = "Test Laptop",
            Price = -1000
        };
        
        var ex = Assert.Throws<ArgumentException>(() => _service.CreateLaptop(createDto));
        
        Assert.That(ex.Message, Does.Contain("Цена должна быть больше 0"));
    }

    [Test]
    public void CreateLaptop_WithVeryLongName_ShouldCreateSuccessfully()
    {
        var createDto = new CreateLaptopDto
        {
            Name = new string('A', 100), // 100 символов
            Price = 50000
        };

        var expectedLaptop = new Laptop
        {
            Id = Guid.NewGuid(),
            Name = createDto.Name,
            Price = createDto.Price
        };

        _mockRepo.Setup(r => r.CreateLaptop(It.IsAny<Laptop>()))
            .Returns(expectedLaptop);
        
        var result = _service.CreateLaptop(createDto);
        
        result.Should().NotBeNull();
        result.Name.Should().HaveLength(100);
    }
}