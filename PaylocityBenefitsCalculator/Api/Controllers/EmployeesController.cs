using Api.Dtos.Dependent;
using Api.Dtos.Employee;
using Api.Dtos.Paycheck;
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
    private readonly IPaycheckService _paycheckService;

    public EmployeesController(
        IEmployeeService employeeService,
        IPaycheckService paycheckService)
    {
        _employeeService = employeeService;
        _paycheckService = paycheckService;
    }

    /// <summary>
    /// Returns employee information given an id
    /// </summary>
    /// <param name="id">The employee id</param>
    /// <returns>GetEmployeeDto</returns>
    [SwaggerOperation(Summary = "Get employee by id")]
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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

    [SwaggerOperation(Summary = "Get paycheck information for an employee")]
    [HttpGet("{id}/paycheck")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<EmployeePaycheckDto>>> CalculatePaycheck(int id)
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

            var paycheck = await _paycheckService.CalculatePaycheckAsync(employee);
            if (paycheck is null)
            {
                return BadRequest(new ApiResponse<EmployeePaycheckDto>
                {
                    Success = false,
                    Message = "Error while calculating paycheck",
                    Error = "Error while calculating paycheck"
                });
            }

            return Ok(new ApiResponse<EmployeePaycheckDto>
            {
                Data = paycheck,
                Success = true
            });
        }
        catch (Exception)
        {
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                new ApiResponse<List<EmployeePaycheckDto>>
                {
                    Success = false,
                    Message = "An Unexpected error has occured",
                    Error = "An Unexpected error has occured"
                });
        }
    }
}
