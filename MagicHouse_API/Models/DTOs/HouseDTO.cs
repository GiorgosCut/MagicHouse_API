using System.ComponentModel.DataAnnotations;

namespace MagicHouse_API.Models.DTOs
{
    public class HouseDTO
    {
        
        public int Id { get; set; }

        [Required]
        [MaxLength(30)]
        [MinLength(1)]
        public string Name { get; set; }
        public int SquareFt {  get; set; }
        public int Occupancy { get; set; }
    }
}
