using DentStarLab.Application.DTOs.DoctorPortal;
using DentStarLab.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DentStarLab.Api.Controllers;

[ApiController]
[Route("api/doctor-portal")]
public class DoctorPortalController : ControllerBase
{
    private readonly DoctorPortalService _doctorPortalService;

    public DoctorPortalController(
        DoctorPortalService doctorPortalService)
    {
        _doctorPortalService =
            doctorPortalService;
    }

    // =========================================================
    // SUMMARY
    // =========================================================

    [AllowAnonymous]
    [HttpGet("{accessToken:guid}/summary")]
    public async Task<IActionResult> GetSummary(
        Guid accessToken)
    {
        var result =
            await _doctorPortalService
                .GetSummaryAsync(accessToken);

        if (result == null)
        {
            return NotFound(new
            {
                message =
                    "Hesabat linki etibarsızdır."
            });
        }

        return Ok(result);
    }

    // =========================================================
    // WORKS
    // =========================================================

    [AllowAnonymous]
    [HttpGet("{accessToken:guid}/works")]
    public async Task<IActionResult> GetWorks(
        Guid accessToken,
        [FromQuery]
        DoctorPortalWorkFilterDto filter)
    {
        var result =
            await _doctorPortalService
                .GetWorksAsync(
                    accessToken,
                    filter);

        if (result == null)
        {
            return NotFound(new
            {
                message =
                    "Hesabat linki etibarsızdır."
            });
        }

        return Ok(result);
    }

    // =========================================================
    // PAYMENTS
    // =========================================================

    [AllowAnonymous]
    [HttpGet("{accessToken:guid}/payments")]
    public async Task<IActionResult> GetPayments(
        Guid accessToken,
        [FromQuery]
        DoctorPortalPaymentFilterDto filter)
    {
        var result =
            await _doctorPortalService
                .GetPaymentsAsync(
                    accessToken,
                    filter);

        if (result == null)
        {
            return NotFound(new
            {
                message =
                    "Hesabat linki etibarsızdır."
            });
        }

        return Ok(result);
    }
}