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
    
    [Test]
    public void IEquatableEquals_WhenEmployeesAreEqual_ShouldReturnTrue()
    {
        var otherEmployee = new Employee
        {
            Id = _employee.Id,
            Name = _employee.Name,
            Age = _employee.Age,
            Position = _employee.Position,
            CompanyId = _employee.CompanyId,
            Company = _employee.Company
        };

        var result = _employee.Equals(otherEmployee);
        
        Assert.That(result, Is.True);
    }
    
    [Test]
    public void Equals_WhenEmployeesAreEqual_ShouldReturnTrue()
    {
        object otherEmployee = new Employee
        {
            Id = _employee.Id,
            Name = _employee.Name,
            Age = _employee.Age,
            Position = _employee.Position,
            CompanyId = _employee.CompanyId,
            Company = _employee.Company
        };

        var result = _employee.Equals(otherEmployee);
        
        Assert.That(result, Is.True);
    }
    
    [Test]
    public void Equals_WhenEmployeesAreNotEqual_ShouldReturnFalse()
    {
        var otherEmployee = new Employee();

        var result = _employee.Equals(otherEmployee);
        
        Assert.That(result, Is.False);
    }
    
    [Test]
    public void Equals_WhenEmployeesHaveDifferentIds_ShouldReturnFalse()
    {
        var otherEmployee = new Employee
        {
            Id = Guid.NewGuid(),
            Name = _employee.Name,
            Age = _employee.Age,
            Position = _employee.Position,
            CompanyId = _employee.CompanyId,
            Company = _employee.Company
        };

        var result = _employee.Equals(otherEmployee);
        
        Assert.That(result, Is.False);
    }
    
    [Test]
    public void Equals_WhenEmployeesHaveDifferentNames_ShouldReturnFalse()
    {
        var otherEmployee = new Employee
        {
            Id = _employee.Id,
            Name = "Different",
            Age = _employee.Age,
            Position = _employee.Position,
            CompanyId = _employee.CompanyId,
            Company = _employee.Company
        };

        var result = _employee.Equals(otherEmployee);
        
        Assert.That(result, Is.False);
    }
    
    [Test]
    public void Equals_WhenEmployeesHaveDifferentAge_ShouldReturnFalse()
    {
        var otherEmployee = new Employee
        {
            Id = _employee.Id,
            Name = _employee.Name,
            Age = -1,
            Position = _employee.Position,
            CompanyId = _employee.CompanyId,
            Company = _employee.Company
        };

        var result = _employee.Equals(otherEmployee);
        
        Assert.That(result, Is.False);
    }
    
    [Test]
    public void Equals_WhenEmployeesHaveDifferentPositions_ShouldReturnFalse()
    {
        var otherEmployee = new Employee
        {
            Id = _employee.Id,
            Name = _employee.Name,
            Age = _employee.Age,
            Position = "Different",
            CompanyId = _employee.CompanyId,
            Company = _employee.Company
        };

        var result = _employee.Equals(otherEmployee);
        
        Assert.That(result, Is.False);
    }
    
    [Test]
    public void Equals_WhenEmployeesHaveDifferentCompanyIds_ShouldReturnFalse()
    {
        var otherEmployee = new Employee
        {
            Id = _employee.Id,
            Name = _employee.Name,
            Age = _employee.Age,
            Position = _employee.Position,
            CompanyId = Guid.NewGuid(),
            Company = _employee.Company
        };

        var result = _employee.Equals(otherEmployee);
        
        Assert.That(result, Is.False);
    }
    
    [Test]
    public void Equals_WhenEmployeesHaveDifferentCompanies_ShouldReturnFalse()
    {
        var otherEmployee = new Employee
        {
            Id = _employee.Id,
            Name = _employee.Name,
            Age = _employee.Age,
            Position = _employee.Position,
            CompanyId = _employee.CompanyId,
            Company = new Company()
        };

        var result = _employee.Equals(otherEmployee);
        
        Assert.That(result, Is.False);
    }
    
    [Test]
    public void Equals_ComparingDifferentTypes_ShouldReturnFalse()
    {
        object otherEmployee = new Company();

        var result = _employee.Equals(otherEmployee);
        
        Assert.That(result, Is.False);
    }
    
    [Test]
    public void Equals_ComparingToNull_ShouldReturnFalse()
    {
        var result = _employee.Equals(null);
        
        Assert.That(result, Is.False);
    }

    [Test]
    public void GetHashCode_EqualObjects_ShouldHaveSameHashCodes()
    {
        var expected = _employee.GetHashCode();
        var testEmployee = new Employee
        {
            Id = _employee.Id,
            Name = _employee.Name,
            Age = _employee.Age,
            Position = _employee.Position,
            CompanyId = _employee.CompanyId,
            Company = _employee.Company
        };

        var result = testEmployee.GetHashCode();
        
        Assert.That(result, Is.EqualTo(expected));
    }
    
    [Test]
    public void GetHashCode_DifferentId_ShouldGenerateDifferentHashCode()
    {
        var expected = _employee.GetHashCode();
        var testEmployee = new Employee
        {
            Id = Guid.NewGuid(),
            Name = _employee.Name,
            Age = _employee.Age,
            Position = _employee.Position,
            CompanyId = _employee.CompanyId,
            Company = _employee.Company
        };

        var result = testEmployee.GetHashCode();
        
        Assert.That(result, Is.Not.EqualTo(expected));
    }
    
    [Test]
    public void GetHashCode_DifferentName_ShouldGenerateDifferentHashCode()
    {
        var expected = _employee.GetHashCode();
        var testEmployee = new Employee
        {
            Id = _employee.Id,
            Name = "Different",
            Age = _employee.Age,
            Position = _employee.Position,
            CompanyId = _employee.CompanyId,
            Company = _employee.Company
        };

        var result = testEmployee.GetHashCode();
        
        Assert.That(result, Is.Not.EqualTo(expected));
    }
    
    [Test]
    public void GetHashCode_DifferentAge_ShouldGenerateDifferentHashCode()
    {
        var expected = _employee.GetHashCode();
        var testEmployee = new Employee
        {
            Id = _employee.Id,
            Name = _employee.Name,
            Age = -1,
            Position = _employee.Position,
            CompanyId = _employee.CompanyId,
            Company = _employee.Company
        };

        var result = testEmployee.GetHashCode();
        
        Assert.That(result, Is.Not.EqualTo(expected));
    }
    
    [Test]
    public void GetHashCode_DifferentPosition_ShouldGenerateDifferentHashCode()
    {
        var expected = _employee.GetHashCode();
        var testEmployee = new Employee
        {
            Id = _employee.Id,
            Name = _employee.Name,
            Age = _employee.Age,
            Position = "Different",
            CompanyId = _employee.CompanyId,
            Company = _employee.Company
        };

        var result = testEmployee.GetHashCode();
        
        Assert.That(result, Is.Not.EqualTo(expected));
    }
    
    [Test]
    public void GetHashCode_DifferentCompanyId_ShouldGenerateDifferentHashCode()
    {
        var expected = _employee.GetHashCode();
        var testEmployee = new Employee
        {
            Id = _employee.Id,
            Name = _employee.Name,
            Age = _employee.Age,
            Position = _employee.Position,
            CompanyId = Guid.NewGuid(),
            Company = _employee.Company
        };

        var result = testEmployee.GetHashCode();
        
        Assert.That(result, Is.Not.EqualTo(expected));
    }
    
    [Test]
    public void GetHashCode_DifferentCompany_ShouldGenerateSameHashCode()
    {
        var expected = _employee.GetHashCode();
        var testEmployee = new Employee
        {
            Id = _employee.Id,
            Name = _employee.Name,
            Age = _employee.Age,
            Position = _employee.Position,
            CompanyId = _employee.CompanyId,
            Company = new Company()
        };

        var result = testEmployee.GetHashCode();
        
        Assert.That(result, Is.EqualTo(expected));
    }
}