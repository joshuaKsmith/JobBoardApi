namespace JobBoardApi.Models.DTOs;

public class JobApplicantDTO
{
    public int Id { get; set; }
    public JobDTO Job { get; set; }
    public ApplicantDTO Applicant { get; set; }
}