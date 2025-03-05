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

public class EmployerController : ControllerBase
{
    private JobBoardApiDbContext _dbContext;
    
    private EmployerController(JobBoardApiDbContext context)
    {
        _dbContext = context;
    }

}