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
[Authorize]
public class JobController : ControllerBase
{
    private JobBoardApiDbContext _dbContext;

    public JobController(JobBoardApiDbContext context)
    {
        _dbContext = context;
    }
    
    [HttpGet]
    public IActionResult Get()
    {
        try
        {
            List<CompanyJobDTO> jobs = _dbContext.CompanyJobs
                .Include(cj => cj.Job)
                .Include(cj => cj.Company)
                    .ThenInclude(c => c.Industry)
                .Select(cj => new CompanyJobDTO
                {
                    Id = cj.Id,
                    Company = new UserProfileDTO
                    {
                        Id = cj.Company.Id,
                        Name = cj.Company.Name,
                        Location = cj.Company.Location,
                        Industry = new IndustryDTO
                        {
                            Id = cj.Company.Industry.Id,
                            Name = cj.Company.Industry.Name
                        }
                    },
                    Job = new JobDTO
                    {
                        Id = cj.Job.Id,
                        Title = cj.Job.Title,
                        Description = cj.Job.Description,
                        PostedDate = cj.Job.PostedDate,
                        ClosesDate = cj.Job.ClosesDate
                    }
                })
                .ToList();
            
            return Ok(jobs);
        }
        catch
        {
            return StatusCode(500, "An error occurred while retrieving jobs");
        }
    }


    [HttpGet("{employerId}")]
    public IActionResult GetByEmployerId(int employerId)
    {
        try
        {
            List<CompanyJobDTO> jobs = _dbContext.CompanyJobs
                .Include(cj => cj.Company)
                    .ThenInclude(c => c.Industry)
                .Include(cj => cj.Job)
                .Where(cj => cj.Company.Id == employerId)
                .Select(cj => new CompanyJobDTO
                {
                    Id = cj.Id,
                    Company = new UserProfileDTO
                    {
                        Id = cj.Company.Id,
                        Name = cj.Company.Name,
                        Location = cj.Company.Location,
                        Industry = new IndustryDTO
                        {
                            Id = cj.Company.Industry.Id,
                            Name = cj.Company.Industry.Name
                        }
                    },
                    Job = new JobDTO
                    {
                        Id = cj.Job.Id,
                        Title = cj.Job.Title,
                        Description = cj.Job.Description,
                        PostedDate = cj.Job.PostedDate,
                        ClosesDate = cj.Job.ClosesDate
                    }
                })
                .ToList();
            if (jobs == null)
            {
                return NotFound();
            }
            return Ok(jobs);
        }
        catch
        {
            return StatusCode(500, "An error occurred");
        }
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteJob(int id)
    {
        Job jobToDelete = _dbContext.Jobs
            .SingleOrDefault((j) => j.Id == id);
        if (jobToDelete == null)
        {
            return NotFound($"Job with Id = {id} not found");
        }
        _dbContext.Jobs.Remove(jobToDelete);
        _dbContext.SaveChanges();
        return NoContent();
    }
}