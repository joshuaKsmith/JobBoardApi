namespace JobBoardApi.Models;

public class CompanyJob
{
    public int Id { get; set; }
    public int CompanyId { get; set; }
    public UserProfile Company { get; set; }
    public int JobId { get; set; }
    public Job Job { get; set; }
}