using CompanyEmployees.Presentation.ModelBinders;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Moq;

namespace CompanyEmployees.Presentation.Tests.ModelBinders;

public class ArrayModelBinderTests
{
    private Mock<IValueProvider> _valueProviderMock;
    private ModelBindingContext _bindingContext;
    private ArrayModelBinder _binder;

    [SetUp]
    public void SetUp()
    {
        _valueProviderMock = new Mock<IValueProvider>();

        _bindingContext = new DefaultModelBindingContext
        {
            ModelMetadata = new EmptyModelMetadataProvider().GetMetadataForType(typeof(IEnumerable<string>)),
            ModelName = "test",
            ValueProvider = _valueProviderMock.Object
        };

        _binder = new ArrayModelBinder();
    }

    [Test]
    public async Task BindModelAsync_ModelIsNotEnumerable_FailsBinding()
    {
        _bindingContext.ModelMetadata = new EmptyModelMetadataProvider().GetMetadataForType(typeof(string));

        await _binder.BindModelAsync(_bindingContext);

        Assert.That(_bindingContext.Result, Is.EqualTo(ModelBindingResult.Failed()));
    }

    [Test]
    public async Task BindModelAsync_ProvidedValueIsNull_SucceedsWithNullModel()
    {
        _valueProviderMock.Setup(v => v.GetValue("test")).Returns(ValueProviderResult.None);
        
        await _binder.BindModelAsync(_bindingContext);
        
        Assert.That(_bindingContext.Model, Is.Null);
    }
    
    [Test]
    public async Task BindModelAsync_ProvidedValueIsEmpty_SucceedsWithNullModel()
    {
        _valueProviderMock.Setup(v => v.GetValue("test")).Returns(new ValueProviderResult(string.Empty));
        
        await _binder.BindModelAsync(_bindingContext);
        
        Assert.That(_bindingContext.Model, Is.Null);
    }
    
    [Test]
    public async Task BindModelAsync_ValidCommaSeparatedString_BindsToCorrectArray()
    {
        _bindingContext.ModelMetadata = new EmptyModelMetadataProvider().GetMetadataForType(typeof(IEnumerable<Guid>));
        _valueProviderMock.Setup(v => v.GetValue("test"))
            .Returns(new ValueProviderResult("d85b1401-7d7a-4d4b-bbde-4c8bd1ff91c5, 03f0a5b3-7295-41a4-9bbd-6f7aef2c1bfb"));

        await _binder.BindModelAsync(_bindingContext);

        var expectedArray = new[]
        {
            Guid.Parse("d85b1401-7d7a-4d4b-bbde-4c8bd1ff91c5"),
            Guid.Parse("03f0a5b3-7295-41a4-9bbd-6f7aef2c1bfb")
        };

        Assert.That(_bindingContext.Model, Is.EqualTo(expectedArray));
    }
}