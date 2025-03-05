namespace JobBoardApi.Models.DTOs;

public class CompanyJobDTO
{
    public int Id { get; set; }
    public UserProfileDTO Company { get; set; }
    public JobDTO Job { get; set; }
}