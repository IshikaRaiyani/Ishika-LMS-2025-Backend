using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.Models
{
    public class Studentwishlist
    {
        [Key]
       public int WishlistId { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int BookId { get; set; }

        public Book Book { get; set; }

        public DateOnly AddedOn { get; set; }
    }
}
