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

    [Test]
    public void IEquatableEquals_WhenCompaniesAreEqual_ShouldReturnTrue()
    {
        var company = _company;
        var otherCompany = new Company
        {
            Id = _company.Id,
            Name = _company.Name,
            Address = _company.Address,
            Country = _company.Country,
            Employees = _company.Employees
        };

        var result = company.Equals(otherCompany);
        
        Assert.That(result, Is.True);
    }
    
    [Test]
    public void Equals_WhenCompaniesAreEqual_ShouldReturnTrue()
    {
        var company = _company;
        object otherCompany = new Company
        {
            Id = _company.Id,
            Name = _company.Name,
            Address = _company.Address,
            Country = _company.Country,
            Employees = _company.Employees
        };

        var result = company.Equals(otherCompany);
        
        Assert.That(result, Is.True);
    }
    
    [Test]
    public void Equals_WhenCompaniesAreNotEqual_ShouldReturnFalse()
    {
        var company = _company;
        var otherCompany = new Company();

        var result = company.Equals(otherCompany);
        
        Assert.That(result, Is.False);
    }
    
    [Test]
    public void Equals_WhenCompaniesHaveDifferentIds_ShouldReturnFalse()
    {
        var company = _company;
        var otherCompany = new Company
        {
            Id = Guid.Empty,
            Name = _company.Name,
            Address = _company.Address,
            Country = _company.Country,
            Employees = _company.Employees
        };

        var result = company.Equals(otherCompany);
        
        Assert.That(result, Is.False);
    }
    
    [Test]
    public void Equals_WhenCompaniesHaveDifferentNames_ShouldReturnFalse()
    {
        var company = _company;
        var otherCompany = new Company
        {
            Id = _company.Id,
            Name = "Different",
            Address = _company.Address,
            Country = _company.Country,
            Employees = _company.Employees
        };

        var result = company.Equals(otherCompany);
        
        Assert.That(result, Is.False);
    }
    
    [Test]
    public void Equals_WhenCompaniesHaveDifferentAddresses_ShouldReturnFalse()
    {
        var company = _company;
        var otherCompany = new Company
        {
            Id = _company.Id,
            Name = _company.Name,
            Address = "Different",
            Country = _company.Country,
            Employees = _company.Employees
        };

        var result = company.Equals(otherCompany);
        
        Assert.That(result, Is.False);
    }
    
    [Test]
    public void Equals_WhenCompaniesHaveDifferentCountries_ShouldReturnFalse()
    {
        var company = _company;
        var otherCompany = new Company
        {
            Id = _company.Id,
            Name = _company.Name,
            Address = _company.Address,
            Country = "Different",
            Employees = _company.Employees
        };

        var result = company.Equals(otherCompany);
        
        Assert.That(result, Is.False);
    }
    
    [Test]
    public void Equals_WhenCompaniesHaveDifferentEmployees_ShouldReturnFalse()
    {
        var company = _company;
        var otherCompany = new Company
        {
            Id = _company.Id,
            Name = _company.Name,
            Address = _company.Address,
            Country = _company.Country,
            Employees = []
        };

        var result = company.Equals(otherCompany);
        
        Assert.That(result, Is.False);
    }
    
    [Test]
    public void Equals_ComparingDifferentTypes_ShouldReturnFalse()
    {
        var company = _company;
        object otherCompany = new Employee();

        var result = company.Equals(otherCompany);
        
        Assert.That(result, Is.False);
    }
    
    [Test]
    public void Equals_ComparingToNull_ShouldReturnFalse()
    {
        var result = _company.Equals(null);
        
        Assert.That(result, Is.False);
    }

    [Test]
    public void GetHashCode_EqualObjects_ShouldHaveSameHashCodes()
    {
        var expected = _company.GetHashCode();
        var testCompany = new Company
        {
            Id = _company.Id,
            Name = _company.Name,
            Address = _company.Address,
            Country = _company.Country,
            Employees = _company.Employees
        };

        var result = testCompany.GetHashCode();
        
        Assert.That(result, Is.EqualTo(expected));
    }
    
    [Test]
    public void GetHashCode_DifferentId_ShouldGenerateDifferentHashCode()
    {
        var expected = _company.GetHashCode();
        var testCompany = new Company
        {
            Id = Guid.NewGuid(),
            Name = _company.Name,
            Address = _company.Address,
            Country = _company.Country,
            Employees = _company.Employees
        };

        var result = testCompany.GetHashCode();
        
        Assert.That(result, Is.Not.EqualTo(expected));
    }
    
    [Test]
    public void GetHashCode_DifferentName_ShouldGenerateDifferentHashCode()
    {
        var expected = _company.GetHashCode();
        var testCompany = new Company
        {
            Id = _company.Id,
            Name = "Different",
            Address = _company.Address,
            Country = _company.Country,
            Employees = _company.Employees
        };

        var result = testCompany.GetHashCode();
        
        Assert.That(result, Is.Not.EqualTo(expected));
    }
    
    [Test]
    public void GetHashCode_DifferentAddress_ShouldGenerateDifferentHashCode()
    {
        var expected = _company.GetHashCode();
        var testCompany = new Company
        {
            Id = _company.Id,
            Name = _company.Name,
            Address = "Different",
            Country = _company.Country,
            Employees = _company.Employees
        };

        var result = testCompany.GetHashCode();
        
        Assert.That(result, Is.Not.EqualTo(expected));
    }
    
    [Test]
    public void GetHashCode_DifferentCountry_ShouldGenerateDifferentHashCode()
    {
        var expected = _company.GetHashCode();
        var testCompany = new Company
        {
            Id = _company.Id,
            Name = _company.Name,
            Address = _company.Address,
            Country = "Different",
            Employees = _company.Employees
        };

        var result = testCompany.GetHashCode();
        
        Assert.That(result, Is.Not.EqualTo(expected));
    }
    
    [Test]
    public void GetHashCode_DifferentEmployees_ShouldGenerateSameHashCode()
    {
        var expected = _company.GetHashCode();
        var testCompany = new Company
        {
            Id = _company.Id,
            Name = _company.Name,
            Address = _company.Address,
            Country = _company.Country,
            Employees = []
        };

        var result = testCompany.GetHashCode();
        
        Assert.That(result, Is.EqualTo(expected));
    }
}