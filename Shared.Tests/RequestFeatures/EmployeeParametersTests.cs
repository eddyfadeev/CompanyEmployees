using Shared.RequestFeatures;

namespace Shared.Tests.RequestFeatures;

public class EmployeeParametersTests
{
    [Test]
    public void RequestParameters_PageSizeDefaultsToOne_WhenSettingNegativeNum()
    {
        const int expected = 1;
        var result = new EmployeeParameters
        {
            PageSize = -10
        };

        Assert.That(result.PageSize, Is.EqualTo(expected));
    }
    
    [Test]
    public void RequestParameters_PageSizeDefaultsToOne_WhenSettingToZero()
    {
        const int expected = 1;
        var result = new EmployeeParameters
        {
            PageSize = 0
        };

        Assert.That(result.PageSize, Is.EqualTo(expected));
    }
    
    [Test]
    public void RequestParameters_PageSizeIsOne_WhenSettingToOne()
    {
        const int expected = 1;
        var result = new EmployeeParameters
        {
            PageSize = expected
        };

        Assert.That(result.PageSize, Is.EqualTo(expected));
    }
    
    [Test]
    public void RequestParameters_PageSizeDefaultsTo50_WhenSettingAbove50()
    {
        const int expected = 50;
        var result = new EmployeeParameters
        {
            PageSize = 51
        };

        Assert.That(result.PageSize, Is.EqualTo(expected));
    }
    
    [Test]
    public void RequestParameters_PageSizeIs50_WhenSetting50()
    {
        const int expected = 50;
        var result = new EmployeeParameters
        {
            PageSize = expected
        };

        Assert.That(result.PageSize, Is.EqualTo(expected));
    }
}