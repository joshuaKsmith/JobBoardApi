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

public class ProfileController : ControllerBase
{
    private JobBoardApiDbContext _dbContext;
    
    public ProfileController(JobBoardApiDbContext context)
    {
        _dbContext = context;
    }


    [HttpGet("employer/{id}")]
    public IActionResult GetEmployerProfileById(int id)
    {
        try
        {
            UserProfileDTO profile = _dbContext.UserProfiles
                .Where(up => up.Id == id)
                .Select(up => new UserProfileDTO
                {
                    Id = up.Id,
                    Name = up.Name,
                    Location = up.Location
                })
                .FirstOrDefault();

            if (profile == null)
            {
                return NotFound();
            }
            return Ok(profile);
        }
        catch
        {
            return StatusCode(500, "An error occurred");
        }
    }

    [HttpPut("employer/{id}")]
    public IActionResult UpdateEmployerProfile(int id, UserProfile profile)
    {
        try
        {
            if (id != profile.Id)
            {
                return BadRequest("Profile Id Mismatch");
            }
            UserProfile profileToUpdate = _dbContext.UserProfiles
                .SingleOrDefault((up) => up.Id == id);
            if (profileToUpdate == null)
            {
                return NotFound($"Employer Profile with Id = {id} not found");
            }
            profileToUpdate.Name = profile.Name;
            profileToUpdate.Location = profile.Location;
            _dbContext.SaveChanges();
            return NoContent();
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
            "Error updating data");
        }
    }

    [HttpGet("applicant/{id}")]
    public IActionResult GetApplicantProfileById(int id)
    {
        try
        {
            ApplicantDTO profile = _dbContext.Applicants
                .Where(a => a.Id == id)
                .Select(a => new ApplicantDTO
                {
                    Id = a.Id,
                    FirstName = a.FirstName,
                    LastName = a.LastName,
                    Email = a.Email
                })
                .FirstOrDefault();

            if (profile == null)
            {
                return NotFound();
            }
            return Ok(profile);
        }
        catch
        {
            return StatusCode(500, "An error occurred");
        }
    }

    [HttpPut("applicant/{id}")]
    public IActionResult UpdateApplicantProfile(int id, Applicant profile)
    {
        try
        {
            if (id != profile.Id)
            {
                return BadRequest("Profile Id Mismatch");
            }
            Applicant profileToUpdate = _dbContext.Applicants
                .SingleOrDefault((a) => a.Id == id);
            if (profileToUpdate == null)
            {
                return NotFound($"Applicant Profile with Id = {id} not found");
            }
            profileToUpdate.FirstName = profile.FirstName;
            profileToUpdate.LastName = profile.LastName;
            _dbContext.SaveChanges();
            return NoContent();
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
            "Error updating data");
        }
    }
}