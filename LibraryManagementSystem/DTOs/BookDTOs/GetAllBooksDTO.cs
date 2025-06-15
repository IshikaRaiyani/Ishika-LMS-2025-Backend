using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.DTOs.BookDTOs
{
    public class GetAllBooksDTO
    {
        [Key]
        public int BookId { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        [StringLength(100, ErrorMessage = "Title cannot exceed 100 characters")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Author is required.")]
        [StringLength(100, ErrorMessage = "Author cannot exceed 100 characters")]
        public string Author { get; set; }


        [Required(ErrorMessage = "Genre is required.")]
        [StringLength(50, ErrorMessage = "Genre cannot exceed 100 characters")]
        public string Genre { get; set; }

        [Required(ErrorMessage = "TotalCopies is required.")]

        public int TotalCopies { get; set; }

        [Required(ErrorMessage = "AvailableCopies is required.")]

        public int AvailableCopies { get; set; }

       
        public string ImageUrl { get; set; }
        public string Description { get; set; }

        public bool IsBestSelling { get; set; }

        public DateOnly AddedOn { get; set; }
    }
}
