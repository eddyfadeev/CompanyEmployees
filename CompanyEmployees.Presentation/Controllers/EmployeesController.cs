using Microsoft.AspNetCore.Mvc;
using Service.Contracts;

namespace CompanyEmployees.Presentation.Controllers;

[Route("api/companies/{companyId:guid}/employees")]
[ApiController]
public class EmployeesController : ControllerBase
{
    private readonly IServiceManager _service;
    
    public EmployeesController(IServiceManager service) => 
        _service = service;

    [HttpGet] 
    public IActionResult GetEmployeesForCompany(Guid companyId) 
    { 
        var employees = _service.EmployeeService.GetEmployees(companyId, trackChanges: false); 
        
        return Ok(employees); 
    } 
    
    [HttpGet("{employeeId:guid}" )] 
    public IActionResult GetEmployeeForCompany(Guid companyId, Guid employeeId) 
    { 
        var employee = _service.EmployeeService.GetEmployee(companyId, employeeId, trackChanges: false); 
        
        return Ok(employee); 
    } 
}