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
            List<JobDTO> jobs = _dbContext.Jobs
                .Include(j => j.UserProfile)
                    .ThenInclude(up => up.Industry)
                .Select(j => new JobDTO
                {
                    Id = j.Id,
                    Title = j.Title,
                    Description = j.Description,
                    PostedDate = j.PostedDate,
                    ClosesDate = j.ClosesDate,
                    Company = new UserProfileDTO
                    {
                        Id = j.UserProfile.Id,
                        Name = j.UserProfile.Name,
                        Location = j.UserProfile.Location,
                        Industry = new IndustryDTO
                        {
                            Id = j.UserProfile.Industry.Id,
                            Name = j.UserProfile.Industry.Name
                        }
                    }
                })
                .ToList();
        
            return Ok(jobs);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred while retrieving jobs: {ex.Message}");
        }
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        try
        {
            JobDTO job = _dbContext.Jobs
                .Where(j => j.Id == id)
                .Select(j => new JobDTO
                {
                    Id = j.Id,
                    Title = j.Title,
                    Description = j.Description,
                    PostedDate = j.PostedDate,
                    ClosesDate = j.ClosesDate
                })
                .FirstOrDefault();

            if (job == null)
            {
                return NotFound();
            }
            return Ok(job);
        }
        catch
        {
            return StatusCode(500, "An error occurred");
        }
    }

    [HttpPut("{id}")]
    public IActionResult EditJob(int id, Job job)
    {
        try
        {
            if (id != job.Id)
            {
                return BadRequest("JobId mismatch");
            }
            Job jobToUpdate = _dbContext.Jobs
                .SingleOrDefault((j) => j.Id == id);
            if (jobToUpdate == null)
            {
                return NotFound($"Job with Id = {id} not found");
            }
            jobToUpdate.Title = job.Title;
            jobToUpdate.Description = job.Description;
            jobToUpdate.ClosesDate = job.ClosesDate;
            _dbContext.SaveChanges();
            return NoContent();

        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
            "Error updating data");
        }
    }

    [HttpGet("my")]
    public IActionResult GetMyJobs()
    {
        try
        {
            string identityUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            
            UserProfile userProfile = _dbContext.UserProfiles
                .SingleOrDefault(up => up.IdentityUserId == identityUserId);

            if (userProfile == null)
            {
                return NotFound("User profile not found");
            }

        List<JobDTO> jobs = _dbContext.Jobs
            .Include(j => j.UserProfile)
                .ThenInclude(up => up.Industry)
            .Where(j => j.UserProfile.Id == userProfile.Id)
            .Select(j => new JobDTO
            {
                Id = j.Id,
                Title = j.Title,
                Description = j.Description,
                PostedDate = j.PostedDate,
                ClosesDate = j.ClosesDate,
                Company = new UserProfileDTO
                {
                    Id = j.UserProfile.Id,
                    Name = j.UserProfile.Name,
                    Location = j.UserProfile.Location,
                    Industry = new IndustryDTO
                    {
                        Id = j.UserProfile.Industry.Id,
                        Name = j.UserProfile.Industry.Name
                    }
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

    [HttpPost]
    public IActionResult CreateJob(Job job)
    {
        job.PostedDate = DateTime.Now;        

        _dbContext.Jobs.Add(job);
        _dbContext.SaveChanges();
        return Created($"/api/job/{job.Id}", job);
    }
}