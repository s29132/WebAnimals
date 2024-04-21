using WebAppAnimals.Animals;
using WebAppAnimals.Repositories;

namespace WebAppAnimals.Services;

public interface IAnimalService
{
    public IEnumerable<Animal> GetAllAnimals(string orderBy);
    public bool AddAnimal(CreateAnimalDto dto);
    public bool UpdateAnimal(UpdateAnimalDto dto);
    public bool DeleteAnimal(int id);
}

public class AnimalService : IAnimalService
{
    private IAnimalRepository _animalRepository;
    public AnimalService(IAnimalRepository _animalRepository)
    {
        this._animalRepository = _animalRepository;
    }
    
    public IEnumerable<Animal> GetAllAnimals(string orderBy)
    {
        return _animalRepository.FetchAllAnimals(orderBy);
    }

    public bool AddAnimal(CreateAnimalDto dto)
    {
        return _animalRepository.PutAnimal(dto.Name, dto.Description, dto.Category, dto.Area);
    }

    public bool UpdateAnimal(UpdateAnimalDto dto)
    {
        var animal = _animalRepository.GetAnimal(dto.IdAnimal);
        if (animal != null)
        {
            if (dto.Name != null)
            {
                animal.Name = dto.Name;
            }
            if (dto.Description != null)
            {
                animal.Description = dto.Description;
            }
            if (dto.Category != null)
            {
                animal.Category = dto.Category;
            }
            if (dto.Area != null)
            {
                animal.Area = dto.Area;
            }
        }
        else
        {
            return false;
        }

        return _animalRepository.UpdateAnimal(animal.Id, animal.Name, animal.Description, animal.Category, animal.Area);

    }

    public bool DeleteAnimal(int id)
    {
        return _animalRepository.DeleteAnimal(id);
    }
}