using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
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
    public IActionResult CreateEmployeeForCompany(Guid companyId, [FromBody] EmployeeForCreationDto? employee)
    {
        if (employee is null)
        {
            return BadRequest("EmployeeForCreationDto object is null.");
        }

        if (!ModelState.IsValid)
        {
            return UnprocessableEntity(ModelState);
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

    [HttpPut("{employeeId:guid}")]
    public IActionResult UpdateEmployeeForCompany(Guid companyId, Guid employeeId, [FromBody] EmployeeForUpdateDto? employee)
    {
        if (employee is null)
        {
            return BadRequest("EmployeeForUpdateDto object is null"); 
        }
        
        if (!ModelState.IsValid)
        {
            return UnprocessableEntity(ModelState);
        }
        
        _service.EmployeeService.UpdateEmployeeForCompany(companyId, employeeId, employee, compTrackChanges: false, empTrackChanges: true);
        
        return NoContent(); 
    }

    [HttpPatch("{employeeId:guid}")]
    public IActionResult PartialUpdateEmployeeForCompany(Guid companyId, Guid employeeId,
        [FromBody] JsonPatchDocument<EmployeeForUpdateDto>? patchDoc)
    {
        if (patchDoc is null)
        {
            return BadRequest("patchDoc object from client is null");
        }

        var result = _service.EmployeeService.GetEmployeeForPatch(companyId, employeeId, 
                                                                trackCompanyChanges: false, trackEmployeeChanges: true);
        
        patchDoc.ApplyTo(result.employeeToPatch, ModelState);

        TryValidateModel(result.employeeToPatch);

        if (!ModelState.IsValid)
        {
            return UnprocessableEntity(ModelState);
        }
        
        _service.EmployeeService.SaveChangesForPatch(result.employeeToPatch, result.employeeEntity);

        return NoContent();
    }
    
}