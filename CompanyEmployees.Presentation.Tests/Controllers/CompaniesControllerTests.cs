using CompanyEmployees.Presentation.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Service.Contracts;
using Shared.DTO;

namespace CompanyEmployees.Presentation.Tests.Controllers;

public class CompaniesControllerTests
{
    private Mock<IServiceManager> _service;
    private CompaniesController _controller;
    private List<CompanyDto> _companies;
    
    [SetUp]
    public void Setup()
    {
        _service = new Mock<IServiceManager>();
        _controller = new CompaniesController(_service.Object);
        
        _companies =
        [
            new
            (
                id: Guid.NewGuid(),
                name: "Test",
                address: "Test Address",
                country: "Test Country"
            ),

            new
            (
                id: Guid.NewGuid(),
                name: "Test2",
                address: "Test Address2",
                country: "Test Country2"
            )
        ];
    }

    [Test]
    public async Task GetCompanies_ReturnsOkObjectResult()
    {
        var expected = _companies;
        _service.Setup(service => 
            service.CompanyService.GetAllCompanies(false))
            .Returns(expected);

        var result = _controller.GetCompanies();

        Assert.That(result, Is.TypeOf<OkObjectResult>());
        var okResult = (OkObjectResult)result;
        Assert.That(okResult.Value, Is.EqualTo(expected));
    }

    [Test]
    public async Task GetCompany_ReturnsOkObjectResult()
    {
        var expected = _companies[0];
        _service.Setup(service =>
            service.CompanyService.GetCompany(expected.Id, false))
            .Returns(expected);

        var result = _controller.GetCompany(expected.Id);
        
        Assert.That(result, Is.TypeOf<OkObjectResult>());
        var okResult = (OkObjectResult)result;
        Assert.That(okResult.Value, Is.EqualTo(expected));
    }
}