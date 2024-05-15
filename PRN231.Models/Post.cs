namespace PRN231.Models;

public partial class Post
{
    public int Id { get; set; }

    public int TutorId { get; set; }

    public string Title { get; set; }

    public string Content { get; set; }

    public string Thumbnail { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime UpdatedDate { get; set; }

    public bool Status { get; set; }

    public User Tutor { get; set; }
}
