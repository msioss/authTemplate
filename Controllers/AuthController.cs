using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StaffTracking.Data.DbContext;
using StaffTracking.Data.Entities;
using StaffTracking.Models;

namespace StaffTracking.Controllers;

[Route("auth")]
public class AuthController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext _context;

    public AuthController(ILogger<HomeController> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    [HttpGet]
    [Route("authorized")]
    public async Task<AuthResponseDto> Authorized([FromQuery] string rf_id_code)
    {
        var employee = await _context.Employees.Include(x => x.AccessCards)
            .SingleOrDefaultAsync(x => x.AccessCards.Any(y => y.Value.Trim() == rf_id_code.Trim()));

        if (employee != null)
        {
            _context.AccessLogs.Add(new AccessLog
            {
                LogInTime = DateTime.UtcNow,
                UserId = employee.Id
            });

            await _context.SaveChangesAsync();

            return new AuthResponseDto
            {
                IsAuthorized = "true"
            };
        }

        return new AuthResponseDto
        {
            IsAuthorized = "false"
        };
    }
}