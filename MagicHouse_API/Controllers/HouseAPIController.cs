using MagicHouse_API.Models;
using Microsoft.AspNetCore.Mvc;

namespace MagicHouse_API.Controllers
{

    [Route("api/HouseAPI")]
    [ApiController]
    public class HouseAPIController : ControllerBase
    {   
        [HttpGet]
        public IEnumerable<House> GetHouses()
        {
            return new List<House>()
            {
                new House { Id = 1, Name = "House with garden" },
                new House { Id = 2, Name = "House with pool" }
            };
        }
    }   
}
