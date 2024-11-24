using MagicHouse_API.Models;
using MagicHouse_API.Models.DTOs;

namespace MagicHouse_API.Data
{
    public static class HouseStore
    {
        public static List<HouseDTO> houses = new List<HouseDTO>
        {
            new HouseDTO { Id = 1, Name = "House with garden", SquareFt = 200, Occupancy = 4},
            new HouseDTO { Id = 2, Name = "House with pool", SquareFt = 100, Occupancy = 2 }
        };
    }
}
