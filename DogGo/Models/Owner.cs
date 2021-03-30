using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DogGo.Models
{
    public class Owner
    {
        public int Id { get; set; }
       
        [Required]
        public string Name { get; set; }
        [Required(ErrorMessage = "Please add a name.")]
        [MaxLength(35)]
        public string Address { get; set; }
        [Required]
        [DisplayName("Neighborhood")]
        public int NeighborhoodId { get; set; }
        [Phone]
        [DisplayName("Phone Number")]
        public string Phone { get; set;  }
        [EmailAddress]
        [Required]
        public string Email { get; set; }
        [DisplayName("Neighborhood")]
        public Neighborhood Neighborhood { get; set; }
        public List<Dog> Dogs { get; set; }
    }
}
