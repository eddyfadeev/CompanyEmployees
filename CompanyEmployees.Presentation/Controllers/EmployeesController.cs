using CompanyEmployees.Presentation.ActionFilters;
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
    public async Task<IActionResult> GetEmployeesForCompany(Guid companyId) 
    { 
        var employees = await _service.EmployeeService.GetEmployees(companyId, trackChanges: false); 
        
        return Ok(employees); 
    } 
    
    [HttpGet("{employeeId:guid}", Name = "GetEmployeeForCompany")] 
    public async Task<IActionResult> GetEmployeeForCompany(Guid companyId, Guid employeeId) 
    { 
        var employee = await _service.EmployeeService.GetEmployee(companyId, employeeId, trackChanges: false); 
        
        return Ok(employee); 
    }

    [HttpPost]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    public async Task<IActionResult> CreateEmployeeForCompany(Guid companyId, [FromBody] EmployeeForCreationDto? employee)
    {
        var employeeToReturn = await _service.EmployeeService.CreateEmployeeForCompany(companyId, employee, trackChanges: false);

        return CreatedAtRoute("GetEmployeeForCompany", new { companyId, employeeId = employeeToReturn.Id }, employeeToReturn); 
    }
    
    [HttpDelete("{employeeId:guid}")]
    public async Task<IActionResult> DeleteEmployeeForCompany(Guid companyId, Guid employeeId)
    {
        await _service.EmployeeService.DeleteEmployeeForCompany(companyId, employeeId, trackChanges: false); 
        
        return NoContent(); 
    }

    [HttpPut("{employeeId:guid}")]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    public async Task<IActionResult> UpdateEmployeeForCompany(Guid companyId, Guid employeeId, [FromBody] EmployeeForUpdateDto? employee)
    {
        await _service.EmployeeService.UpdateEmployeeForCompany(companyId, employeeId, employee, trackChanges: true);
        
        return NoContent(); 
    }

    [HttpPatch("{employeeId:guid}")]
    public async Task<IActionResult> PartialUpdateEmployeeForCompany(Guid companyId, Guid employeeId,
        [FromBody] JsonPatchDocument<EmployeeForUpdateDto>? patchDoc)
    {
        if (patchDoc is null)
        {
            return BadRequest("patchDoc object from client is null");
        }

        var result = await _service.EmployeeService.GetEmployeeForPatch(companyId, employeeId,
            trackChanges: true);
        
        patchDoc.ApplyTo(result.employeeToPatch, ModelState);

        TryValidateModel(result.employeeToPatch);

        if (!ModelState.IsValid)
        {
            return UnprocessableEntity(ModelState);
        }
        
        await _service.EmployeeService.SaveChangesForPatch(result.employeeToPatch, result.employeeEntity);

        return NoContent();
    }
}