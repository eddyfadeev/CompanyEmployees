using System.ComponentModel.DataAnnotations;
using Entities.Models;

namespace Entities.Tests.Models;

public class CompanyTests
{
    private Company _company;
    
    [SetUp]
    public void Setup()
    {
        _company = new()
        {
            Id = Guid.NewGuid(),
            Name = "TestCompany",
            Address = "TestAddress",
            Country = "TestCountry",
            Employees = 
            [ 
                new Employee {Name = "Employee One", Age = 1, Position = "TestPosition"}, 
                new Employee {Name = "Employee Two", Age = 99, Position = "TestPosition"}
            ]
        };
    }
    
    [Test]
    public void Validate_NameIsRequired_ShouldReturnValidationError()
    {
        _company.Name = null;

        var results = ValidationHelper.ValidateObject(_company);

        Assert.That(results, Has.One.Matches<ValidationResult>(r => 
            r.ErrorMessage == "Company name is required."));
    }

    [Test]
    public void Validate_AddressIsRequired_ShouldReturnValidationError()
    {
        _company.Address = null;

        var result = ValidationHelper.ValidateObject(_company);
        
        Assert.That(result, Has.One.Matches<ValidationResult>(r =>
            r.ErrorMessage == "Company address is required."));
    }
    
    [Test]
    public void Validate_NameMaxLengthIs60_ShouldReturnValidationError()
    {
        _company.Name = new string('a', 61);
        
        var result = ValidationHelper.ValidateObject(_company);
        
        Assert.That(result, Has.One.Matches<ValidationResult>(r =>
            r.ErrorMessage == "Maximum length for the name is 60 characters."));
    }
    
    [Test]
    public void Validate_AddressMaxLengthIs200_ShouldReturnValidationError()
    {
        _company.Address = new string('a', 201);
        
        var result = ValidationHelper.ValidateObject(_company);
        
        Assert.That(result, Has.One.Matches<ValidationResult>(r =>
            r.ErrorMessage == "Maximum length is 200 characters."));
    }
    
    [Test]
    public void Validate_NameMaxLengthIs60_ShouldNotReturnValidationError()
    {
        _company.Name = new string('a', 60);
        
        var result = ValidationHelper.ValidateObject(_company);
    
        Assert.That(result, Is.Empty, "Validation should not return any errors for a valid name length.");
    }
    
    [Test]
    public void Validate_AddressMaxLengthIs200_ShouldNotReturnValidationError()
    {
        _company.Address = new string('a', 200);
        
        var result = ValidationHelper.ValidateObject(_company);
    
        Assert.That(result, Is.Empty, "Validation should not return any errors for a valid address length.");
    }

    [Test]
    public void Validate_WhenCountryIsNull_ShouldNotReturnValidationError()
    {
        _company.Country = null;

        var result = ValidationHelper.ValidateObject(_company);
        
        Assert.That(result, Is.Empty, "Validation should not return any errors for a country that is null.");
    }
    
    [Test]
    public void Validate_WhenIdIsEmptyGuid_ShouldNotReturnValidationError()
    {
        _company.Id = Guid.Empty;

        var result = ValidationHelper.ValidateObject(_company);
        
        Assert.That(result, Is.Empty, "Validation should not return any errors for a country that is null.");
    }
    
    [Test]
    public void Validate_WhenEmployeesCollectionIsNull_ShouldNotReturnValidationError()
    {
        _company.Employees = null;

        var result = ValidationHelper.ValidateObject(_company);
        
        Assert.That(result, Is.Empty, "Validation should not return any errors for a country that is null.");
    }
    
    [Test]
    public void Validate_WhenEmployeesCollectionIsEmpty_ShouldNotReturnValidationError()
    {
        _company.Employees = [];

        var result = ValidationHelper.ValidateObject(_company);
        
        Assert.That(result, Is.Empty, "Validation should not return any errors for a country that is null.");
    }
    
    [Test]
    public void Validate_WhenEmployeesCollectionIsEmpty_ShouldReturnEmptyCollection()
    {
        var expectedResult = new Company
        {
            Id = _company.Id,
            Name = _company.Name,
            Address = _company.Address,
            Country = _company.Country,
            Employees = []
        };
        
        Assert.That(expectedResult.Employees, Is.Empty);
    }
    
    [Test]
    public void Validate_WhenEmployeesCollectionIsNull_ShouldReturnEmptyCollection()
    {
        var expectedResult = new Company
        {
            Id = _company.Id,
            Name = _company.Name,
            Address = _company.Address,
            Country = _company.Country,
            Employees = null
        };
        
        Assert.That(expectedResult.Employees, Is.Empty);
    }
    
    [Test]
    public void Validate_WhenEmployeesCollectionIsNotEmpty_ShouldReturnNotEmptyCollection()
    {
        var expectedResult = new Company
        {
            Id = _company.Id,
            Name = _company.Name,
            Address = _company.Address,
            Country = _company.Country,
            Employees = _company.Employees
        };
        
        Assert.That(expectedResult.Employees, Is.Not.Empty);
    }
}