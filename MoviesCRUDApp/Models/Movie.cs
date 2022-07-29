using System.ComponentModel.DataAnnotations;

namespace MoviesCRUDApp.Models
{
    public class Movie
    {
        [Key]
        public int MovieId { get; set; }
        [Required(AllowEmptyStrings =false,ErrorMessage ="Title is Required"),MaxLength(250,ErrorMessage ="Title Can't be more than 250 characters"),MinLength(3,ErrorMessage ="Title Can't be less than 3 characters")]
        public string Title { get; set; }
        public double Rate { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "StoryLine is Required"), MaxLength(2500, ErrorMessage = "StoryLine Can't be more than 2500 characters"), MinLength(10, ErrorMessage = "StoryLine Can't be less than 10 characters")]
        public string StoryLine { get; set; }
        public int Year { get; set; }
        [Required]
        public byte[] Poster { get; set; }
        public Genre Genre { get; set; }
        public byte GenreId { get; set; }
    }
}
