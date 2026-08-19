using DentStarLab.Application.DTOs.Works;
using DentStarLab.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DentStarLab.Api.Controllers;

[ApiController]
[Route("api/works")]
// [Authorize(Roles = "Admin,Technician")] TODO:
public class WorksController : ControllerBase
{
    private readonly WorkService _service;

    public WorksController(WorkService service)
    {
        _service = service;
    }


    // =====================================================
    // CREATE WORK
    // Admin + Technician
    // =====================================================

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] WorkCreateDto dto)
    {
        var result = await _service.CreateAsync(dto);

        return CreatedAtAction(
            nameof(GetById),
            new { id = result.Id },
            result);
    }


    // =====================================================
    // GET ALL WORKS
    // Admin + Technician
    // =====================================================

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] WorkQueryDto query)
    {
        var result = await _service.GetPagedAsync(query);

        return Ok(result);
    }


    // =====================================================
    // GET WORK BY ID
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
                message = "Work not found."
            });
        }

        return Ok(result);
    }


    // =====================================================
    // UPDATE WORK
    // Admin only
    // =====================================================

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(
        int id,
        [FromBody] WorkUpdateDto dto)
    {
        var result = await _service.UpdateAsync(
            id,
            dto);

        if (!result)
        {
            return NotFound(new
            {
                message = "Work not found."
            });
        }

        return NoContent();
    }


    // =====================================================
    // DELETE WORK
    // Admin + Technician
    // =====================================================

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _service.DeleteAsync(id);

        if (!result)
        {
            return NotFound(new
            {
                message = "Work not found."
            });
        }

        return NoContent();
    }
}