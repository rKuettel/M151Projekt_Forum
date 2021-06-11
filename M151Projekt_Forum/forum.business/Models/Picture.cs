namespace forum.business.Models
{
    public class Picture
    {
        public int Id { get; set; }
        public int DiscussionId { get; set; }
        public Discussion Discussion { get; set; }
        public string PicturePath { get; set; }
    }
}