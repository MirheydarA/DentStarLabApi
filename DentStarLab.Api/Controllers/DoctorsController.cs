using DentStarLab.Application.DTOs.Doctors;
using DentStarLab.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DentStarLab.Api.Controllers;

[ApiController]
[Route("api/doctors")]
[Authorize(Roles = "Admin,Technician")]
public class DoctorsController : ControllerBase
{
    private readonly DoctorService _service;

    public DoctorsController(DoctorService service)
    {
        _service = service;
    }

    // =====================================================
    // CREATE DOCTOR
    // Admin + Technician
    // =====================================================

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] DoctorCreateDto dto)
    {
        var result = await _service.CreateAsync(dto);

        return CreatedAtAction(
            nameof(GetById),
            new { id = result.Id },
            result);
    }


    // =====================================================
    // GET ALL ACTIVE DOCTORS
    // Admin + Technician
    // =====================================================

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _service.GetAllAsync();

        return Ok(result);
    }


    // =====================================================
    // GET DOCTOR BY ID
    // Admin + Technician
    // =====================================================

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _service.GetByIdAsync(id);

        if (result == null)
            return NotFound(new
            {
                message = "Doctor not found."
            });

        return Ok(result);
    }


    // =====================================================
    // UPDATE DOCTOR
    // Admin + Technician
    // =====================================================

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] DoctorUpdateDto dto)
    {
        throw new Exception("Bu test xətasıdır — Telegram-a bildiriş gəlməlidir.");

        // bool result = await _service.UpdateAsync( id, dto);

        // if (!result)
        //     return NotFound(new
        //     {
        //         message = "Doctor not found."
        //     });

        // return NoContent();
    }


    // =====================================================
    // DELETE / SOFT DELETE DOCTOR
    // =====================================================

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _service.DeleteAsync(id);

        if (!result)
            return NotFound(new
            {
                message = "Doctor not found."
            });

        return NoContent();
    }

    [HttpGet("balances")]
    public async Task<IActionResult> GetBalances()
    {
        var result = await _service.GetBalancesAsync();

        return Ok(result);
    }

    [HttpGet("{id:int}/balance")]
    public async Task<IActionResult> GetBalance(int id)
    {
        var result = await _service.GetBalanceByIdAsync(id);

        if (result == null)
        {
            return NotFound(new { message = "Doctor not found." });
        }

        return Ok(result);
    }

    [HttpGet("frequent")]
    public async Task<IActionResult> GetFrequent(
    [FromQuery] int days = 90,
    [FromQuery] int top = 5)
    {
        var result = await _service.GetFrequentAsync(days, top);
        return Ok(result);
    }
}