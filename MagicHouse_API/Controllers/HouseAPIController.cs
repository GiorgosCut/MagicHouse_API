using MagicHouse_API.Data;
using MagicHouse_API.Models;
using MagicHouse_API.Models.DTOs;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace MagicHouse_API.Controllers
{

    [Route("api/HouseAPI")]
    [ApiController]
    public class HouseAPIController : ControllerBase
    {
        private readonly ILogger<HouseAPIController> _logger;

        //On the constructor of a controller, using dependency injection,
        //inject a logger object
        public HouseAPIController(ILogger<HouseAPIController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<HouseDTO>> GetHouses()
        {
            _logger.LogInformation("Returning all houses");
            return Ok(HouseStore.houses);
        }

        [HttpGet("{id:int}", Name = "GetHouse")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<HouseDTO?> GetHouse([FromQuery] int id)
        {
            if (id == 0)
            {
                _logger.LogError("Id is 0", [id]);
                return BadRequest();
            }
            var house = HouseStore.houses.FirstOrDefault(x => x.Id == id);
            if (house == null)
            {
                return NotFound();
            }
            return Ok(house);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<HouseDTO> CreateHouse([FromBody] HouseDTO house)
        {
            //Check if house name already exists
            if (HouseStore.houses.FirstOrDefault(x => x.Name.ToLower() == house.Name.ToLower()) != null)
            {
                //Add an error to the ModelState, with a custom name and message
                ModelState.AddModelError("DuplicateNameError", "Villa name is not unique!");
                //Send a 400 bad response with the above error
                return BadRequest(ModelState);
            }

            if (house == null || house.Id > 0)
            {
                return BadRequest(house);
            }
            house.Id = HouseStore.houses.OrderByDescending(x => x.Id).First().Id + 1;
            HouseStore.houses.Add(house);

            //Instead of returning status 200 OK, we return 201 created, while also returning the API location where the object which was created can be found.
            //Here you call the GetHouse controller, passing the id of the house you just created.
            //so it will also return "api/HouseAPI/the_id_of_the_new_object"
            return CreatedAtRoute("GetHouse", new { id = house.Id }, house);
        }

        [HttpDelete("{id:int}", Name = "DeleteVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult DeleteHouse(int id) {
            if (id == 0)
            {
                return BadRequest();
            }
            var house = HouseStore.houses.FirstOrDefault(x => x.Id == id);
            if (house is not null)
            {
                return NotFound();
            }
            else
            {
                HouseStore.houses.Remove(house);
            }

            return NoContent();
        }

        [HttpPut("{id:int}", Name = "UpdateHouse")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult UpdateHouse(int id, [FromBody] HouseDTO houseDto)
        {
            if (id == 0 || id != houseDto.Id) {
                return BadRequest();
            }
            var house = HouseStore.houses.FirstOrDefault(x => x.Id == id);
            if (house is null)
            {
                return NotFound();
            }
            house.Name = houseDto.Name;
            house.Occupancy = houseDto.Occupancy;
            house.SquareFt = houseDto.SquareFt;

            return CreatedAtRoute("GetHouse", new { id = house.Id }, house);
        }

        [HttpPatch("{id:int}", Name = "UpdateHousePart")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult UpdateHousePart(int id, JsonPatchDocument<HouseDTO> patchDto)
        {
            if (patchDto is null || id == 0)
            {
                return BadRequest();
            }
            var house = HouseStore.houses.FirstOrDefault(x => x.Id == id);
            if(house is null)
            {
                return NotFound();
            }
            //Apply The change to the house
            patchDto.ApplyTo(house, ModelState);
            
            //If something went wrong return a bad request with the model state else return the updated model
            if (ModelState.IsValid)
            {
                return CreatedAtRoute("GetHouse", new { id = house.Id }, house);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
        
    }  
}
