using Api.Dtos.Dependent;
using Api.Models;
using Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class DependentsController : ControllerBase
{
    private readonly IDependentService _dependentService;
    public DependentsController(IDependentService dependentService)
    {
        _dependentService = dependentService;
    }

    /// <summary>
    /// Returns dependent information given an id
    /// </summary>
    /// <param name="id">int</param>
    /// <returns>GetDependentDto</returns>
    [SwaggerOperation(Summary = "Get dependent by id")]
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<GetDependentDto>>> Get(int id)
    {
        try
        {
            var dependent = await _dependentService.GetDependentAsync(id);
            if (dependent is null)
            {
                return NotFound(new ApiResponse<GetDependentDto>
                {
                    Success = false,
                    Message = "Dependent not found",
                    Error = "Dependent not found"
                });
            }

            return Ok(new ApiResponse<GetDependentDto>
            {
                Data = dependent,
                Success = true
            });
        }
        catch (Exception)
        {
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                new ApiResponse<GetDependentDto>
                {
                    Success = false,
                    Message = "An Unexpected error has occured",
                    Error = "An Unexpected error has occured"
                });
        }
    }

    /// <summary>
    /// Returns all dependents
    /// </summary>
    /// <returns>List of GetDependentDto</returns>
    [SwaggerOperation(Summary = "Get all dependents")]
    [HttpGet("")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<List<GetDependentDto>>>> GetAll()
    {
        try
        {
            var dependentList = await _dependentService.GetAllDependentsAsync();
            return Ok(new ApiResponse<List<GetDependentDto>>
            {
                Data = dependentList,
                Success = true
            });
        }
        catch (Exception)
        {
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                new ApiResponse<List<GetDependentDto>>
                {
                    Success = false,
                    Message = "An Unexpected error has occured",
                    Error = "An Unexpected error has occured"
                });
        }
    }
}
