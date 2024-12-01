using Shared.RequestFeatures;

namespace Shared.Tests.RequestFeatures;

public class EmployeeParametersTests
{
    [Test]
    public void EmployeeParameters_CreatesEntityWithCorrectDefaultPageSize_WithDefaultSettings()
    {
        const int expected = 10;
        var result = new EmployeeParameters();

        Assert.That(result.PageSize, Is.EqualTo(expected));
    }
    
    [Test]
    public void EmployeeParameters_CreatesEntityWithCorrectDefaultPageNumber_WithDefaultSettings()
    {
        const int expected = 1;
        var result = new EmployeeParameters();

        Assert.That(result.PageNumber, Is.EqualTo(expected));
    }
    
    [Test]
    public void EmployeeParameters_PageSizeDefaultsToOne_WhenCreatingWithNegativeNum()
    {
        const int expected = 1;
        var result = new EmployeeParameters
        {
            PageSize = -10
        };

        Assert.That(result.PageSize, Is.EqualTo(expected));
    }
    
    [Test]
    public void EmployeeParameters_PageSizeDefaultsToOne_WhenTryingToSetNegativeNum()
    {
        const int expected = 1;
        var result = new EmployeeParameters(); 
        result.PageSize = -20;

        Assert.That(result.PageSize, Is.EqualTo(expected));
    }
    
    [Test]
    public void EmployeeParameters_PageSizeDefaultsToOne_WhenTryingToSetZero()
    {
        const int expected = 1;
        var result = new EmployeeParameters(); 
        result.PageSize = 0;

        Assert.That(result.PageSize, Is.EqualTo(expected));
    }
    
    [Test]
    public void EmployeeParameters_PageSizeDefaultsToOne_WhenCreatingWithZero()
    {
        const int expected = 1;
        var result = new EmployeeParameters
        {
            PageSize = 0
        };

        Assert.That(result.PageSize, Is.EqualTo(expected));
    }
    
    [Test]
    public void EmployeeParameters_PageSizeIsOne_WhenCreatingWithOne()
    {
        const int expected = 1;
        var result = new EmployeeParameters
        {
            PageSize = expected
        };

        Assert.That(result.PageSize, Is.EqualTo(expected));
    }
    
    [Test]
    public void EmployeeParameters_PageSizeDefaultsTo50_WhenCreatingWithAbove50()
    {
        const int expected = 50;
        var result = new EmployeeParameters
        {
            PageSize = 51
        };

        Assert.That(result.PageSize, Is.EqualTo(expected));
    }
    
    [Test]
    public void EmployeeParameters_PageSizeIs50_WhenCreatingWith50()
    {
        const int expected = 50;
        var result = new EmployeeParameters
        {
            PageSize = expected
        };

        Assert.That(result.PageSize, Is.EqualTo(expected));
    }
    
    [Test]
    public void EmployeeParameters_PageSizeIs50_WhenTryingToSetAbove50()
    {
        const int expected = 50;
        var result = new EmployeeParameters
        {
            PageSize = expected
        };

        result.PageSize = 51;

        Assert.That(result.PageSize, Is.EqualTo(expected));
    }
    
    [Test]
    public void EmployeeParameters_PageSizeIs50_WhenTryingToSet50()
    {
        const int expected = 50;
        var result = new EmployeeParameters
        {
            PageSize = expected
        };

        result.PageSize = 50;

        Assert.That(result.PageSize, Is.EqualTo(expected));
    }
    
    [Test]
    public void EmployeeParameters_PageNumberIsOne_WhenCreatingWithOne()
    {
        const int expected = 1;
        var result = new EmployeeParameters
        {
            PageNumber = expected
        };

        Assert.That(result.PageNumber, Is.EqualTo(expected));
    }
    
    [Test]
    public void EmployeeParameters_PageNumberDefaultsToOne_WhenCreatingWithZero()
    {
        const int expected = 1;
        var result = new EmployeeParameters
        {
            PageNumber = 0
        };

        Assert.That(result.PageNumber, Is.EqualTo(expected));
    }
    
    [Test]
    public void EmployeeParameters_PageNumberDefaultsToOne_WhenCreatingWithNegativeNum()
    {
        const int expected = 1;
        var result = new EmployeeParameters
        {
            PageNumber = -1
        };

        Assert.That(result.PageNumber, Is.EqualTo(expected));
    }
    
    [Test]
    public void EmployeeParameters_PageNumberIsOne_WhenTryingToSetOne()
    {
        const int expected = 1;
        var result = new EmployeeParameters();
        result.PageNumber = expected;

        Assert.That(result.PageNumber, Is.EqualTo(expected));
    }
    
    [Test]
    public void EmployeeParameters_PageNumberDefaultsToOne_WhenTryingToSetZero()
    {
        const int expected = 1;
        var result = new EmployeeParameters();
        result.PageNumber = 0;

        Assert.That(result.PageNumber, Is.EqualTo(expected));
    }
    
    [Test]
    public void EmployeeParameters_PageNumberDefaultsToOne_WhenTryingToSetNegativeNum()
    {
        const int expected = 1;
        var result = new EmployeeParameters();
        result.PageNumber = -1;

        Assert.That(result.PageNumber, Is.EqualTo(expected));
    }
}