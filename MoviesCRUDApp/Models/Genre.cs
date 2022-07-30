using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoviesCRUDApp.Models
{
    public class Genre
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public byte GenreId { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Name is Required"), MaxLength(100, ErrorMessage = "Name Can't be more than 100 characters"), MinLength(3, ErrorMessage = "Name Can't be less than 3 characters")]
        public string Name { get; set; }
    }
}
