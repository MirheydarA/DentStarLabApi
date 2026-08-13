using DentStarLab.Application.DTOs.Works;
using DentStarLab.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace DentStarLab.Api.Controllers;

[ApiController]
[Route("api/works")]
public class WorksController : ControllerBase
{
    private readonly WorkService _service;

    public WorksController(WorkService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        WorkCreateDto dto)
    {
        var result = await _service.CreateAsync(dto);

        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _service.GetAllAsync();

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _service.GetByIdAsync(id);

        if (result == null)
            return NotFound();

        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(
        int id,
        WorkUpdateDto dto)
    {
        var result = await _service.UpdateAsync(id, dto);

        if (!result)
            return NotFound();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _service.DeleteAsync(id);

        if (!result)
            return NotFound();

        return NoContent();
    }
}