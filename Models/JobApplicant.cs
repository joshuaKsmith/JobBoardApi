namespace JobBoardApi.Models;

public class JobApplicant
{
    public int Id { get; set; }
    public int JobId { get; set; }
    public Job Job { get; set; }
    public int ApplicantId { get; set; }
    public Applicant Applicant { get; set; }
}