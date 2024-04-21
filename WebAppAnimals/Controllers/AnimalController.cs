using System.Reflection.Metadata.Ecma335;
using Microsoft.AspNetCore.Mvc;
using WebAppAnimals.Animals;
using WebAppAnimals.Services;

namespace WebAppAnimals.Controllers;
[ApiController]
[Route("api/animals")]
public class AnimalController : ControllerBase
{
    private IAnimalService _service;
    public AnimalController(IAnimalService _service)
    {
        this._service = _service;
    }
    [HttpGet("")]
    public IActionResult GetAllAnimals([FromQuery]string orderBy)
    {
        var animals = _service.GetAllAnimals(orderBy);
        return Ok(animals);
    }

    [HttpPost]
    public IActionResult PostAnimal([FromBody] CreateAnimalDto dto)
    {
        var result = _service.AddAnimal(dto);
        return result ? StatusCode(StatusCodes.Status201Created) : Conflict();
    }

    [HttpPut]
    public IActionResult UpdateAnimal([FromBody] UpdateAnimalDto dto)
    {
        var result = _service.UpdateAnimal(dto);
        return result ? Ok() : NotFound();
    }

    [HttpDelete("id:int")]
    public IActionResult DeleteAnimal([FromRoute]int id)
    {
        var result = _service.DeleteAnimal(id);
        return result ? StatusCode(StatusCodes.Status200OK) : StatusCode((StatusCodes.Status404NotFound));
    }
    
}