using MagicHouse_API.Data;
using MagicHouse_API.Models;
using MagicHouse_API.Models.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace MagicHouse_API.Controllers
{

    [Route("api/HouseAPI")]
    [ApiController]
    public class HouseAPIController : ControllerBase
    {   
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<HouseDTO>> GetHouses()
        {
            return Ok(HouseStore.houses);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<HouseDTO?> GetHouse(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            var house = HouseStore.houses.FirstOrDefault(x => x.Id == id) ;
            if(house == null)
            {
                return NotFound();
            }
            return Ok(house);    
        }

    }   
}
