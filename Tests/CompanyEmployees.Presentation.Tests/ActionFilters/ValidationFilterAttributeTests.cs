using CompanyEmployees.Presentation.ActionFilters;
using CompanyEmployees.Presentation.Controllers;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;
using Moq;
using Service.Contracts;
using Shared.DTO;

namespace CompanyEmployees.Presentation.Tests.ActionFilters;

public class ValidationFilterAttributeTests
{
    private RouteData _defaultRouteData;
    private Dictionary<string, object> _defaultActionArguments;
    private ValidationFilterAttribute _validationFilter;
    
    [SetUp]
    public void Setup()
    {
        _defaultRouteData = new RouteData();
        _defaultRouteData.Values.Add("action", nameof(CompaniesController.UpdateCompany)); 
        _defaultRouteData.Values.Add("controller", typeof(CompaniesController)); 
        
        _defaultActionArguments = new ()
        {
            { "id", Guid.NewGuid() },
            { "company", new CompanyForCreationDto() }
        };

        _validationFilter = new ValidationFilterAttribute();
    }

    [Test]
    public void OnActionExecuting_ContextResultIsNull_WhenAllCorrect()
    {
        var actionContext = new ActionContext(
            Mock.Of<HttpContext>(),
            _defaultRouteData,
            Mock.Of<ActionDescriptor>(),
            Mock.Of<ModelStateDictionary>()
        );
        
        var actionExecutingContext = new ActionExecutingContext(
            actionContext,
            new List<IFilterMetadata>(),
            _defaultActionArguments!,
            new Mock<CompaniesController>(Mock.Of<IServiceManager>())
        );
        
        _validationFilter.OnActionExecuting(actionExecutingContext);
        
        Assert.That(actionExecutingContext.Result, Is.Null);
    }
    
    [Test]
    public void OnActionExecuting_ContextResultIsNull_WhenModelErrorsExist()
    {
        var validationFilter = new ValidationFilterAttribute();
        var modelState = new ModelStateDictionary();
        modelState.AddModelError("Test", "Error");
    
        var actionContext = new ActionContext(
            Mock.Of<HttpContext>(),
            Mock.Of<RouteData>(),
            Mock.Of<ActionDescriptor>(),
            modelState
            );
    
        var actionExecutingContext = new ActionExecutingContext(
            actionContext,
            new List<IFilterMetadata>(),
            _defaultActionArguments!,
            new Mock<CompaniesController>(Mock.Of<IServiceManager>())
            );
        
        
        validationFilter.OnActionExecuting(actionExecutingContext);
        
        Assert.That(actionExecutingContext.Result, Is.InstanceOf<UnprocessableEntityObjectResult>());
    }
    
    [Test]
    public void OnActionExecuting_ContextResultIsNull_WhenOtherThanDtoInActionArguments()
    {
        var validationFilter = new ValidationFilterAttribute();
        
        var actionContext = new ActionContext(
            Mock.Of<HttpContext>(),
            Mock.Of<RouteData>(),
            Mock.Of<ActionDescriptor>(),
            Mock.Of<ModelStateDictionary>());

        _defaultActionArguments["company"] = new Company();
        
        var actionExecutingContext = new ActionExecutingContext(
            actionContext,
            new List<IFilterMetadata>(),
            _defaultActionArguments!,
            new Mock<CompaniesController>(Mock.Of<IServiceManager>())
        );
        
        validationFilter.OnActionExecuting(actionExecutingContext);
        
        Assert.That(actionExecutingContext.Result, Is.InstanceOf<BadRequestObjectResult>());
    }
    
    [Test]
    public void OnActionExecuting_ContextResultIsNull_WhenActionArgumentsIsEmpty()
    {
        var validationFilter = new ValidationFilterAttribute();
        
        var actionContext = new ActionContext(
            Mock.Of<HttpContext>(),
            Mock.Of<RouteData>(),
            Mock.Of<ActionDescriptor>(),
            Mock.Of<ModelStateDictionary>());

        _defaultActionArguments["company"] = new Company();
        
        var actionExecutingContext = new ActionExecutingContext(
            actionContext,
            new List<IFilterMetadata>(),
            new Dictionary<string, object?>(),
            new Mock<CompaniesController>(Mock.Of<IServiceManager>())
        );
        
        validationFilter.OnActionExecuting(actionExecutingContext);
        
        Assert.That(actionExecutingContext.Result, Is.InstanceOf<BadRequestObjectResult>());
    }
}