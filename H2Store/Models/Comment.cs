using System.ComponentModel.DataAnnotations;

namespace H2Store.Models
{
    public class Comment
    {
        [Key]
        public int CommentID { get; set; }
        public string? Message { get; set; }
        public string? ProductID { get; set; }
        public string? FullName { get; set; }
        public string? UserID { get; set; }
        public int? ParentID { get; set; }
        public int? Rate { get; set; }
        public DateTime? CommentDate { get; set; }
        public string? Email { get; set; }
        public Product? Product { get; set; }
        public User? User { get; set; }
    }
}
