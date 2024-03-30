using Api.Dtos.Dependent;
using Api.Dtos.Employee;
using Api.Models;
using Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class EmployeesController : ControllerBase
{
    private readonly IEmployeeService _employeeService;

    public EmployeesController(IEmployeeService employeeService)
    {
        _employeeService = employeeService;
    }

    /// <summary>
    /// Returns employee information given an id
    /// </summary>
    /// <param name="id">The employee id</param>
    /// <returns>GetEmployeeDto</returns>
    [SwaggerOperation(Summary = "Get employee by id")]
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<GetEmployeeDto>>> Get(int id)
    {
        try
        {
            var employee = await _employeeService.GetEmployeeAsync(id);
            if (employee is null)
            {
                return NotFound(new ApiResponse<GetEmployeeDto>
                {
                    Success = false,
                    Message = "Employee not found",
                    Error = "Employee not found"
                });
            }

            return Ok(new ApiResponse<GetEmployeeDto>
            {
                Data = employee,
                Success = true
            });
        }
        catch (Exception)
        {
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                new ApiResponse<GetEmployeeDto>
                {
                    Success = false,
                    Message = "An Unexpected error has occured",
                    Error = "An Unexpected error has occured"
                });
        }
    }

    /// <summary>
    /// Returns all employees
    /// </summary>
    /// <returns>List of GetEmployeDto's</returns>
    [SwaggerOperation(Summary = "Get all employees")]
    [HttpGet("")]
    public async Task<ActionResult<ApiResponse<List<GetEmployeeDto>>>> GetAll()
    {
        try
        {
            var employeeList = await _employeeService.GetAllEmployeesAsync();
            return Ok(new ApiResponse<List<GetEmployeeDto>>
            {
                Data = employeeList,
                Success = true
            });
        }
        catch (Exception)
        {
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                new ApiResponse<List<GetEmployeeDto>>
                {
                    Success = false,
                    Message = "An Unexpected error has occured",
                    Error = "An Unexpected error has occured"
                });
        }
    }
}
