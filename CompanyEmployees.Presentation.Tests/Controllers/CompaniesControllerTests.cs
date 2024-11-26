using CompanyEmployees.Presentation.Controllers;
using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Service.Contracts;
using Shared.Extensions;

namespace CompanyEmployees.Presentation.Tests.Controllers;

public class CompaniesControllerTests
{
    private Mock<IServiceManager> _service;
    private CompaniesController _controller;
    private List<Company> _companies;
    
    [SetUp]
    public void Setup()
    {
        _service = new Mock<IServiceManager>();
        _controller = new CompaniesController(_service.Object);
        
        _companies =
        [
            new ()
            {
                Id = Guid.NewGuid(),
                Name = "Test",
                Address = "Test Address",
                Country = "Test Country"
            },
            new ()
            {
                Id = Guid.NewGuid(),
                Name = "Test2",
                Address = "Test Address2",
                Country = "Test Country2"
            }
        ];
    }

    [Test]
    public async Task GetCompanies_ReturnsOkObjectResult()
    {
        var expected = _companies.Select(c => c.MapToCompanyDto());
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
        var expected = _companies[0].MapToCompanyDto();
        _service.Setup(service =>
            service.CompanyService.GetCompany(expected.Id, false))
            .Returns(expected);

        var result = _controller.GetCompany(expected.Id);
        
        Assert.That(result, Is.TypeOf<OkObjectResult>());
        var okResult = (OkObjectResult)result;
        Assert.That(okResult.Value, Is.EqualTo(expected));
    }
}