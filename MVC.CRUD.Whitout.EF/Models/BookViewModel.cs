using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MVC.CRUD.Whitout.EF.Models
{
    public class BookViewModel
    {
        // EntityFramework need a Key Id to work ok.
        [Key]
        public int BookId { get; set; }

        [Required]
        public string Title { get; set; }
        [Required]
        public string Author { get; set; }
        [Range(1, int.MaxValue, ErrorMessage ="Should be greated than or equal to 1")]
        public int Price { get; set; }
    }
}
