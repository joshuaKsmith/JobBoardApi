namespace JobBoardApi.Models.DTOs;

public class RegistrationDTO
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string UserName { get; set; }
    public string Name { get; set; }
    public string Location { get; set; }
    public int IndustryId { get; set; }
}