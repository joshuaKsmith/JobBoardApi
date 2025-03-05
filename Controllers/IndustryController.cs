using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text;
using JobBoardApi.Models;
using JobBoardApi.Models.DTOs;
using JobBoardApi.Data;

namespace JobBoardApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class IndustryController : ControllerBase
{
    private JobBoardApiDbContext _dbContext;

    public IndustryController(JobBoardApiDbContext context)
    {
        _dbContext = context;
    }

    [HttpGet]
    public IActionResult Get()
    {
        return Ok(_dbContext
            .Industries
            .Select(i => new IndustryDTO
            {
                Id = i.Id,
                Name = i.Name
            })
            .ToList());
    }
}