using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{
    [Table("Projects")]
    public class Project
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string PublicId { get; set; }
        public string FileName { get; set; }
        public string FileUrl { get; set; }

        public int AppUserId { get; set; }
        public AppUser AppUser { get; set; }
    }
}