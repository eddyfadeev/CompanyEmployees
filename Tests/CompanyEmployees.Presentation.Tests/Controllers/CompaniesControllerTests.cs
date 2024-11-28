using CompanyEmployees.Presentation.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Service.Contracts;
using Shared.DTO;

namespace CompanyEmployees.Presentation.Tests.Controllers;

public class CompaniesControllerTests
{
    private Mock<IServiceManager> _mockServiceManager;
    private Mock<ICompanyService> _mockCompanyService;
    private CompaniesController _controller;

    [SetUp]
    public void Setup()
    {
        _mockServiceManager = new Mock<IServiceManager>();
        _mockCompanyService = new Mock<ICompanyService>();

        _mockServiceManager.Setup(sm => sm.CompanyService).Returns(_mockCompanyService.Object);

        _controller = new CompaniesController(_mockServiceManager.Object);
    }

    #region Get Companies Tests
    
    [Test]
    public async Task GetCompanies_ReturnsOkResult()
    {
        var companies = new List<CompanyDto>
        {
            new CompanyDto { Id = Guid.NewGuid(), Name = "Test Company 1" },
            new CompanyDto { Id = Guid.NewGuid(), Name = "Test Company 2" }
        };

        _mockCompanyService
            .Setup(s => s.GetAllCompanies(It.IsAny<bool>()))
            .ReturnsAsync(companies);

        var result = await _controller.GetCompanies();

        Assert.That(result, Is.InstanceOf<OkObjectResult>());
    }
    
    [Test]
    public async Task GetCompanies_ReturnsOkResult_WithListOfCompanyDtos()
    {
        var companies = new List<CompanyDto>
        {
            new CompanyDto { Id = Guid.NewGuid(), Name = "Test Company 1" },
            new CompanyDto { Id = Guid.NewGuid(), Name = "Test Company 2" }
        };

        _mockCompanyService
            .Setup(s => s.GetAllCompanies(It.IsAny<bool>()))
            .ReturnsAsync(companies);

        var result = await _controller.GetCompanies() as ObjectResult;

        Assert.That(result?.Value, Is.EqualTo(companies));
    }
    
    #endregion
    
    #region Get Company Tests
    
    [Test]
    public async Task GetCompany_ReturnsOkResult()
    {
        var company = new CompanyDto { Id = Guid.NewGuid(), Name = "Test Company 1" };

        _mockCompanyService
            .Setup(s => s.GetCompany(company.Id, It.IsAny<bool>()))
            .ReturnsAsync(company);

        var result = await _controller.GetCompany(company.Id);

        Assert.That(result, Is.InstanceOf<OkObjectResult>());
    }
    
    [Test]
    public async Task GetCompany_ReturnsOkResult_WithCompanyDto()
    {
        var company = new CompanyDto { Id = Guid.NewGuid(), Name = "Test Company 1" };

        _mockCompanyService
            .Setup(s => s.GetCompany(company.Id, It.IsAny<bool>()))
            .ReturnsAsync(company);

        var result = await _controller.GetCompany(company.Id) as ObjectResult;

        Assert.That(result?.Value, Is.EqualTo(company));
    }
    
    #endregion
    
    #region Get Company Collection Tests
    
    [Test]
    public async Task GetCompanyCollection_ReturnsOkResult()
    {
        Guid[] ids = [Guid.NewGuid(), Guid.NewGuid()];
        var companies = new List<CompanyDto>
        {
            new () { Id = ids[0], Name = "Test Company 1" },
            new () { Id = ids[1], Name = "Test Company 2" }
        };

        _mockCompanyService
            .Setup(s => s.GetByIds(It.IsAny<IEnumerable<Guid>>(), It.IsAny<bool>()))
            .ReturnsAsync(companies);

        var result = await _controller.GetCompanyCollection(ids);

        Assert.That(result, Is.InstanceOf<OkObjectResult>());
    }
    
    [Test]
    public async Task GetCompanyCollection_ReturnsOkResult_WithListOfCompanyDtos()
    {
        Guid[] ids = [Guid.NewGuid(), Guid.NewGuid()];
        var companies = new List<CompanyDto>
        {
            new () { Id = ids[0], Name = "Test Company 1" },
            new () { Id = ids[1], Name = "Test Company 2" }
        };

        _mockCompanyService
            .Setup(s => s.GetByIds(It.IsAny<IEnumerable<Guid>>(), It.IsAny<bool>()))
            .ReturnsAsync(companies);

        var result = await _controller.GetCompanyCollection(ids);

        Assert.That((result as OkObjectResult)?.Value, Is.EquivalentTo(companies));
    }
    
    #endregion

    #region Create Company Tests

    [Test]
    public async Task CreateCompany_ReturnsBadRequest_WhenNullPassed()
    {
        var result = await _controller.CreateCompany(null);

        Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
    }
    
    [Test]
    public async Task CreateCompany_ReturnsUnprocessableEntity_WhenInvalidModel()
    {
        var companyForCreationDto = new CompanyForCreationDto();
        _controller.ModelState.AddModelError("Error", "Model is invalid");

        var result = await _controller.CreateCompany(companyForCreationDto);

        Assert.That(result, Is.InstanceOf<UnprocessableEntityObjectResult>());
    }
    
    [Test]
    public async Task CreateCompany_ReturnsCreatedAtRouteResult()
    {
        var companyForCreationDto = new CompanyForCreationDto
        {
            Name = "TestName",
            Address = "TestAddress",
            Country = "TestCountry"
        };

        _mockCompanyService.Setup(cs => cs.CreateCompany(companyForCreationDto))
            .ReturnsAsync(new CompanyDto
            {
                Name = companyForCreationDto.Name, 
                FullAddress = companyForCreationDto.Address, 
                Id = Guid.Empty
            });

        var result = await _controller.CreateCompany(companyForCreationDto);

        Assert.That(result, Is.InstanceOf<CreatedAtRouteResult>());
    }
    
    [Test]
    public async Task CreateCompany_ReturnsCompanyDto()
    {
        var companyForCreationDto = new CompanyForCreationDto
        {
            Name = "TestName",
            Address = "TestAddress",
            Country = "TestCountry"
        };

        _mockCompanyService.Setup(cs => cs.CreateCompany(companyForCreationDto))
            .ReturnsAsync(new CompanyDto
            {
                Name = companyForCreationDto.Name, 
                FullAddress = companyForCreationDto.Address, 
                Id = Guid.Empty
            });

        var result = await _controller.CreateCompany(companyForCreationDto);

        Assert.That((result as CreatedAtRouteResult)?.Value, Is.InstanceOf<CompanyDto>());
    }
    
    [Test]
    public async Task CreateCompany_ReturnsCorrectCompanyDto()
    {
        var companyForCreationDto = new CompanyForCreationDto
        {
            Name = "TestName",
            Address = "TestAddress",
            Country = "TestCountry"
        };

        var expected = new CompanyDto()
        {
            Id = Guid.Empty,
            Name = companyForCreationDto.Name,
            FullAddress = companyForCreationDto.Address
        };

        _mockCompanyService.Setup(cs => cs.CreateCompany(companyForCreationDto))
            .ReturnsAsync(expected);

        var result = await _controller.CreateCompany(companyForCreationDto);

        Assert.That((result as CreatedAtRouteResult)?.Value, Is.EqualTo(expected));
    }
    
    [Test]
    public async Task CreateCompany_ReturnsCorrectRouteName()
    {
        var companyForCreationDto = new CompanyForCreationDto
        {
            Name = "TestName",
            Address = "TestAddress",
            Country = "TestCountry"
        };

        var expected = "CompanyById";

        _mockCompanyService.Setup(cs => cs.CreateCompany(companyForCreationDto))
            .ReturnsAsync(new CompanyDto
            {
                Id = Guid.Empty,
                Name = companyForCreationDto.Name,
                FullAddress = companyForCreationDto.Address
            });

        var result = await _controller.CreateCompany(companyForCreationDto);

        Assert.That((result as CreatedAtRouteResult)?.RouteName, Is.EqualTo(expected));
    }

    #endregion

    #region Create Company Collection Tests
    
    [Test]
    public async Task CreateCompanyCollection_InvalidModelState_ReturnsUnprocessableEntity()
    {
        _controller.ModelState.AddModelError("Error", "Model is invalid");

        var result = await _controller.CreateCompanyCollection([]);

        Assert.That(result, Is.InstanceOf<UnprocessableEntityObjectResult>());
    }
    
    [Test]
    public async Task CreateCompanyCollection_ReturnsUnprocessableEntity_WhenPassedCollectionIsNull()
    {
        var result = await _controller.CreateCompanyCollection(null);

        Assert.That(result, Is.InstanceOf<UnprocessableEntityObjectResult>());
    }
    
    [Test]
    public async Task CreateCompanyCollection_ReturnsUnprocessableEntity_WhenPassedCollectionIsEmpty()
    {
        var result = await _controller.CreateCompanyCollection([]);

        Assert.That(result, Is.InstanceOf<UnprocessableEntityObjectResult>());
    }
    
    [Test]
    public async Task CreateCompanyCollection_ReturnsCreatedAtRoute_WhenPassedCollectionHasTwoEntries()
    {
        var result = await _controller.CreateCompanyCollection([new CompanyForCreationDto(), new CompanyForCreationDto()]);

        Assert.That(result, Is.InstanceOf<CreatedAtRouteResult>());
    }
    
    [Test]
    public async Task CreateCompanyCollection_ReturnsListOfCompanyDtos()
    {
        var companyCollection = new List<CompanyForCreationDto>
        {
            new () { Name = "Company1", Address = "Address1" },
            new () { Name = "Company2", Address = "Address2" }
        };

        List<CompanyDto> expectedCollection = [
            new () { Id = Guid.NewGuid(), Name = companyCollection[0].Name, FullAddress = companyCollection[0].Address},
            new () { Id = Guid.NewGuid(), Name = companyCollection[1].Name, FullAddress = companyCollection[1].Address}
        ];
        (IEnumerable<CompanyDto> companies, string ids) expected = 
            (expectedCollection, string.Join(',', expectedCollection[0].Id, expectedCollection[1].Id));

        _mockCompanyService.Setup(service => service.CreateCompanyCollection(It.IsAny<IEnumerable<CompanyForCreationDto>>()))
            .ReturnsAsync(expected);

        var result = await _controller.CreateCompanyCollection(companyCollection) as CreatedAtRouteResult;

        Assert.That(result?.Value, Is.InstanceOf<List<CompanyDto>>());
    }
    
    [Test]
    public async Task CreateCompanyCollection_ReturnsNotEmptyListOfCompanyDtos()
    {
        var companyCollection = new List<CompanyForCreationDto>
        {
            new () { Name = "Company1", Address = "Address1" },
            new () { Name = "Company2", Address = "Address2" }
        };

        List<CompanyDto> expectedCollection = [
            new () { Id = Guid.NewGuid(), Name = companyCollection[0].Name, FullAddress = companyCollection[0].Address},
            new () { Id = Guid.NewGuid(), Name = companyCollection[1].Name, FullAddress = companyCollection[1].Address}
        ];
        (IEnumerable<CompanyDto> companies, string ids) expected = 
            (expectedCollection, string.Join(',', expectedCollection[0].Id, expectedCollection[1].Id));

        _mockCompanyService.Setup(service => service.CreateCompanyCollection(It.IsAny<IEnumerable<CompanyForCreationDto>>()))
            .ReturnsAsync(expected);

        var result = await _controller.CreateCompanyCollection(companyCollection) as CreatedAtRouteResult;

        Assert.That(result?.Value, Is.Not.Empty);
    }
    
    [Test]
    public async Task CreateCompanyCollection_ReturnsCorrectListOfCompanyDtos()
    {
        var companyCollection = new List<CompanyForCreationDto>
        {
            new () { Name = "Company1", Address = "Address1" },
            new () { Name = "Company2", Address = "Address2" }
        };

        List<CompanyDto> expectedCollection = [
            new () { Id = Guid.NewGuid(), Name = companyCollection[0].Name, FullAddress = companyCollection[0].Address},
            new () { Id = Guid.NewGuid(), Name = companyCollection[1].Name, FullAddress = companyCollection[1].Address}
        ];
        (IEnumerable<CompanyDto> companies, string ids) expected = 
            (expectedCollection, string.Join(',', expectedCollection[0].Id, expectedCollection[1].Id));

        _mockCompanyService.Setup(service => service.CreateCompanyCollection(It.IsAny<IEnumerable<CompanyForCreationDto>>()))
            .ReturnsAsync(expected);

        var result = await _controller.CreateCompanyCollection(companyCollection) as CreatedAtRouteResult;

        Assert.That(result?.Value, Is.EquivalentTo(expected.companies));
    }
    
    [Test]
    public async Task CreateCompanyCollection_ReturnsIdsValuesTypeOfString()
    {
        var companyCollection = new List<CompanyForCreationDto>
        {
            new () { Name = "Company1", Address = "Address1" },
            new () { Name = "Company2", Address = "Address2" }
        };

        List<CompanyDto> expectedCollection = [
            new () { Id = Guid.NewGuid(), Name = companyCollection[0].Name, FullAddress = companyCollection[0].Address},
            new () { Id = Guid.NewGuid(), Name = companyCollection[1].Name, FullAddress = companyCollection[1].Address}
        ];
        (IEnumerable<CompanyDto> companies, string ids) expected = 
            (expectedCollection, string.Join(',', expectedCollection[0].Id, expectedCollection[1].Id));

        _mockCompanyService.Setup(service => service.CreateCompanyCollection(It.IsAny<IEnumerable<CompanyForCreationDto>>()))
            .ReturnsAsync(expected);

        var result = await _controller.CreateCompanyCollection(companyCollection) as CreatedAtRouteResult;

        Assert.That(result?.RouteValues?["ids"], Is.InstanceOf<string>());
    }
    
    [Test]
    public async Task CreateCompanyCollection_ReturnsCorrectIds()
    {
        var companyCollection = new List<CompanyForCreationDto>
        {
            new () { Name = "Company1", Address = "Address1" },
            new () { Name = "Company2", Address = "Address2" }
        };

        List<CompanyDto> expectedCollection = [
            new () { Id = Guid.NewGuid(), Name = companyCollection[0].Name, FullAddress = companyCollection[0].Address},
            new () { Id = Guid.NewGuid(), Name = companyCollection[1].Name, FullAddress = companyCollection[1].Address}
        ];
        (IEnumerable<CompanyDto> companies, string ids) expected = 
            (expectedCollection, string.Join(',', expectedCollection[0].Id, expectedCollection[1].Id));

        _mockCompanyService.Setup(service => service.CreateCompanyCollection(It.IsAny<IEnumerable<CompanyForCreationDto>>()))
            .ReturnsAsync(expected);

        var result = await _controller.CreateCompanyCollection(companyCollection) as CreatedAtRouteResult;

        Assert.That(result?.RouteValues?["ids"], Is.EqualTo(expected.ids));
    }
    
    [Test]
    public async Task CreateCompanyCollection_ReturnsCorrectRouteName()
    {
        var companyCollection = new List<CompanyForCreationDto>
        {
            new () { Name = "Company1", Address = "Address1" },
            new () { Name = "Company2", Address = "Address2" }
        };

        List<CompanyDto> expectedCollection = [
            new () { Id = Guid.NewGuid(), Name = companyCollection[0].Name, FullAddress = companyCollection[0].Address},
            new () { Id = Guid.NewGuid(), Name = companyCollection[1].Name, FullAddress = companyCollection[1].Address}
        ];
        (IEnumerable<CompanyDto> companies, string ids) expected = 
            (expectedCollection, string.Join(',', expectedCollection[0].Id, expectedCollection[1].Id));

        var expectedRouteName = "CompanyCollection";

        _mockCompanyService.Setup(service => service.CreateCompanyCollection(It.IsAny<IEnumerable<CompanyForCreationDto>>()))
            .ReturnsAsync(expected);

        var result = await _controller.CreateCompanyCollection(companyCollection) as CreatedAtRouteResult;

        Assert.That(result?.RouteName, Is.EqualTo(expectedRouteName));
    }
    
    #endregion
    
    #region Delete Company Tests

    [Test]
    public async Task DeleteCompany_ReturnsNoContent()
    {
        var id = Guid.NewGuid();
        
        var result = await _controller.DeleteCompany(id);

        Assert.That(result, Is.InstanceOf<NoContentResult>());
    }

    #endregion

    #region Update Company Tests

    [Test]
    public async Task UpdateCompany_ReturnsBadRequest_WhenPassedCompanyIsNull()
    {
        var result = await _controller.UpdateCompany(id: Guid.NewGuid(), null);
        
        Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
    }

    [Test]
    public async Task UpdateCompany_ReturnsUnprocessableEntity_WhenModelStateIsInvalid()
    {
        _controller.ModelState.AddModelError("Error", "Model is invalid");
        
        var result = await _controller.UpdateCompany(id: Guid.NewGuid(), new CompanyForUpdateDto());
        
        Assert.That(result, Is.InstanceOf<UnprocessableEntityObjectResult>());
    }

    [Test]
    public async Task UpdateCompany_ReturnsNoContent_WhenSuccess()
    {
        var result = await _controller.UpdateCompany(id: Guid.NewGuid(), new CompanyForUpdateDto());
        
        Assert.That(result, Is.InstanceOf<NoContentResult>());
    }

    #endregion
}