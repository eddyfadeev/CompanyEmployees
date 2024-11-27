﻿using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DTO;

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
        var employees = _service.EmployeeService.GetEmployees(companyId, trackChanges: false).ToList(); 
        
        return Ok(employees); 
    } 
    
    [HttpGet("{employeeId:guid}", Name = "GetEmployeeForCompany")] 
    public IActionResult GetEmployeeForCompany(Guid companyId, Guid employeeId) 
    { 
        var employee = _service.EmployeeService.GetEmployee(companyId, employeeId, trackChanges: false); 
        
        return Ok(employee); 
    }

    [HttpPost]
    public IActionResult CreateEmployeeForCompany(Guid companyId, [FromBody] EmployeeForCreationDto employee)
    {
        if (employee is null)
        {
            return BadRequest("EmployeeForCreationDto object is null.");
        }

        var employeeToReturn = _service.EmployeeService.CreateEmployeeForCompany(companyId, employee, trackChanges: false);

        return CreatedAtRoute("GetEmployeeForCompany", new { companyId, employeeId = employeeToReturn.Id }, employeeToReturn); 
    }
    
    [HttpDelete("{id:guid}")]
    public IActionResult DeleteEmployeeForCompany(Guid companyId, Guid id)
    {
        _service.EmployeeService.DeleteEmployeeForCompany(companyId, id, trackChanges: false); 
        
        return NoContent(); 
    }

}