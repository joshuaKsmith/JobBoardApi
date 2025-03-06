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
using Microsoft.EntityFrameworkCore;

namespace JobBoardApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ApplicationController : ControllerBase
{
    private JobBoardApiDbContext _dbContext;

    public ApplicationController(JobBoardApiDbContext context)
    {
        _dbContext = context;
    }

    [HttpGet("{jobId}")]
    public IActionResult GetApplicationsByJobId()
    {
        try
        {
            List<JobApplicantDTO> applications = _dbContext.JobApplicants
                .Include(ja => ja.Job)
                    .ThenInclude(j => j.UserProfile)
                    .ThenInclude(up => up.Industry)
                .Include(ja => ja.Applicant)
                .Select(ja => new JobApplicantDTO
                {
                    Id = ja.Id,
                    Job = new JobDTO
                    {
                        Id = ja.Job.Id,
                        Title = ja.Job.Title,
                        Description = ja.Job.Description,
                        PostedDate = ja.Job.PostedDate,
                        ClosesDate = ja.Job.ClosesDate,
                        Company = new UserProfileDTO
                        {
                            Id = ja.Job.UserProfile.Id,
                            Name = ja.Job.UserProfile.Name,
                            Location = ja.Job.UserProfile.Location,
                            Industry = new IndustryDTO
                            {
                                Id = ja.Job.UserProfile.Industry.Id,
                                Name = ja.Job.UserProfile.Industry.Name
                            }
                        }
                    },
                    Applicant = new ApplicantDTO
                    {
                        Id = ja.Applicant.Id,
                        FirstName = ja.Applicant.FirstName,
                        LastName = ja.Applicant.LastName,
                        Address = ja.Applicant.Address
                    }
                })
                .ToList();


            if (applications == null)
            {
                return NotFound();
            }    
            return Ok(applications);
        }
        catch
        {
            return StatusCode(500, "An error occurred");
        }
    }
    
}