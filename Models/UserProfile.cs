using Microsoft.AspNetCore.Identity;

namespace JobBoardApi.Models;

public class UserProfile
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Location { get; set; }
    public string IdentityUserId { get; set; }
    public IdentityUser IdentityUser { get; set; }
    public int IndustryId { get; set; }
}