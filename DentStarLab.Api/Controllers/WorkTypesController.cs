using DentStarLab.Application.DTOs.WorkTypes;
using DentStarLab.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DentStarLab.Api.Controllers;

[ApiController]
[Route("api/work-types")]
[Authorize(Roles = "Admin,Technician")]
public class WorkTypesController : ControllerBase
{
    private readonly WorkTypeService _service;

    public WorkTypesController(WorkTypeService service)
    {
        _service = service;
    }


    // =====================================================
    // CREATE WORK TYPE
    // Admin only
    // =====================================================

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create(
        [FromBody] WorkTypeCreateDto dto)
    {
        var result = await _service.CreateAsync(dto);

        return CreatedAtAction(
            nameof(GetById),
            new { id = result.Id },
            result);
    }


    // =====================================================
    // GET ALL WORK TYPES
    // Admin + Technician
    // =====================================================

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _service.GetAllAsync();

        return Ok(result);
    }


    // =====================================================
    // GET WORK TYPE BY ID
    // Admin + Technician
    // =====================================================

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _service.GetByIdAsync(id);

        if (result == null)
        {
            return NotFound(new
            {
                message = "Work type not found."
            });
        }

        return Ok(result);
    }


    // =====================================================
    // UPDATE WORK TYPE
    // Admin only
    // =====================================================

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(
        int id,
        [FromBody] WorkTypeUpdateDto dto)
    {
        var result = await _service.UpdateAsync( id, dto);

        if (!result)
        {
            return NotFound(new
            {
                message = "Work type not found."
            });
        }

        return NoContent();
    }
}