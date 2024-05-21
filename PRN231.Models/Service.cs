namespace PRN231.Models;

public partial class Service
{
    public int Id { get; set; }

    public int TutorId { get; set; }

    public int SubjectId { get; set; }

    public string Name { get; set; }

    public string Title { get; set; }

    public string Thumbnail { get; set; }

    public string Content { get; set; }

    public string Duration { get; set; }

    public decimal Price { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime UpdatedDate { get; set; }

    public bool Status { get; set; }

    public User Tutor { get; set; }

    public Subject Subject { get; set; }

    public List<Schedule> Schedules { get; set; }
}
