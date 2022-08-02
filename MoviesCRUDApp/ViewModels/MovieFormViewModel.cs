using MoviesCRUDApp.Models;
using System.ComponentModel.DataAnnotations;

namespace MoviesCRUDApp.ViewModels
{
    public class MovieFormViewModel
    {
        public int MovieId { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Title is Required"), StringLength(250, ErrorMessage = "Title Can't be less than 3 characters and can't be more than 250 characters",MinimumLength = 3)]
        public string Title { get; set; }
        [Range(1,10,ErrorMessage ="Rate can't be less than 1 and more than 10")]
        public double Rate { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "StoryLine is Required"), StringLength(2500, ErrorMessage = "StoryLine Can't be less than 10 characters and can't be more than 2500 characters",MinimumLength =10)]
        public string StoryLine { get; set; }
        public int Year { get; set; }
        [Display(Name = "Select Poster...")]

        public byte[]? Poster { get; set; }
        [Display(Name ="Genre")]
        public byte GenreId { get; set; }
        public IEnumerable<Genre>? Genres { get; set; }
    }
}
