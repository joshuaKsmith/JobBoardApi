using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace JobBoardApi.Models.DTOs;

public class UserProfileDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Location { get; set; }
    public string Email { get; set; }
    public string UserName { get; set; }
    public List<string> Roles { get; set; }
    public string IdentityUserId { get; set; }
    public IdentityUser IdentityUser { get; set; }
    public int IndustryId { get; set; }
    public IndustryDTO Industry { get; set; }
}