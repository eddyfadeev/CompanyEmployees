using System.ComponentModel.DataAnnotations;
using Entities.Models;

namespace Entities.Tests.Models;

public class EmployeeTests
{
    private Employee _employee;
    
    [SetUp]
    public void Setup()
    {
        var company = new Company
        {
            Id = Guid.NewGuid(),
            Name = "TestCompany",
            Address = "TestAddress",
            Country = "TestCountry"
        };
        
        _employee = new()
        {
            Id = Guid.NewGuid(),
            Name = "TestName",
            Age = 30,
            Position = "TestPosition",
            CompanyId = company.Id,
            Company = company
        };
    }
    
    [Test]
    public void Validate_NameIsRequired_ShouldReturnValidationError()
    {
        _employee.Name = null;

        var results = ValidationHelper.ValidateObject(_employee);

        Assert.That(results, Has.One.Matches<ValidationResult>(r => 
            r.ErrorMessage == "Employee name is required."));
    }
    
    [Test]
    public void Validate_NameMaxLengthIs60_ShouldReturnValidationError()
    {
        _employee.Name = new string('a', 61);
        
        var result = ValidationHelper.ValidateObject(_employee);
        
        Assert.That(result, Has.One.Matches<ValidationResult>(r =>
            r.ErrorMessage == "Maximum length for the name is 60 characters."));
    }
    
    [Test]
    public void Validate_NameMaxLengthIs60_ShouldNotReturnValidationError()
    {
        _employee.Name = new string('a', 60);
        
        var result = ValidationHelper.ValidateObject(_employee);
        
        Assert.That(result, Is.Empty);
    }
    
    [Test]
    public void Validate_MaxAgeIs199_ShouldReturnValidationError()
    {
        _employee.Age = 200;
        
        var result = ValidationHelper.ValidateObject(_employee);
        
        Assert.That(result, Has.One.Matches<ValidationResult>(r =>
            r.ErrorMessage == "Age must be between 1 and 199."));
    }
    
    [Test]
    public void Validate_MaxAgeIs199_ShouldNotReturnValidationError()
    {
        _employee.Age = 199;
        
        var result = ValidationHelper.ValidateObject(_employee);
        
        Assert.That(result, Is.Empty, "Validation should not return any errors with age < 199");
    }
    
    [Test]
    public void Validate_ZeroAge_ShouldReturnValidationError()
    {
        _employee.Age = 0;
        
        var result = ValidationHelper.ValidateObject(_employee);
        
        Assert.That(result, Has.One.Matches<ValidationResult>(r =>
            r.ErrorMessage == "Age must be between 1 and 199."));
    }
    
    [Test]
    public void Validate_NegativeAge_ShouldReturnValidationError()
    {
        _employee.Age = -1;
        
        var result = ValidationHelper.ValidateObject(_employee);
        
        Assert.That(result, Has.One.Matches<ValidationResult>(r =>
            r.ErrorMessage == "Age must be between 1 and 199."));
    }
    
    [Test]
    public void Validate_PositionIsRequired_ShouldReturnValidationError()
    {
        _employee.Position = null;
        
        var result = ValidationHelper.ValidateObject(_employee);
        
        Assert.That(result, Has.One.Matches<ValidationResult>(r =>
            r.ErrorMessage == "Position is required."));
    }
    
    [Test]
    public void Validate_PositionMaxLengthIs20_ShouldReturnValidationError()
    {
        _employee.Position = new string('a', 21);
        
        var result = ValidationHelper.ValidateObject(_employee);
        
        Assert.That(result, Has.One.Matches<ValidationResult>(r =>
            r.ErrorMessage == "Maximum length for the position is 20 characters."));
    }
    
    [Test]
    public void Validate_PositionMaxLengthIs20_ShouldNotReturnValidationError()
    {
        _employee.Position = new string('a', 20);
        
        var result = ValidationHelper.ValidateObject(_employee);
        
        Assert.That(result, Is.Empty);
    }
    
    [Test]
    public void Validate_CompanyIsNull_ShouldNotReturnValidationError()
    {
        _employee.Company = null;
        
        var result = ValidationHelper.ValidateObject(_employee);
        
        Assert.That(result, Is.Empty);
    }
    
    [Test]
    public void ReturnsCorrectCompany_WhenCompanyIsAssigned()
    {
        var expected = new Company { Id = Guid.NewGuid() };

        var result = new Employee
        {
            Company = expected
        };
        
        Assert.That(result.Company, Is.EqualTo(expected));
    }

    [Test]
    public void ThrowsNullException_WhenTryToGetCompanyWithoutAssigning()
    {
        var result = new Employee();

        Assert.Throws<NullReferenceException>(() =>
            result.Company.ToString());
    }
}