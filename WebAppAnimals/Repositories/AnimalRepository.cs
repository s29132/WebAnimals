using System.Data.SqlClient;
using System.Runtime.CompilerServices;
using WebAppAnimals.Animals;

namespace WebAppAnimals.Repositories;

public interface IAnimalRepository
{
    public IEnumerable<Animal> FetchAllAnimals(string orderBy);
    public bool PutAnimal(string name, string description, string category, string area);
    public Animal? GetAnimal(int id);
    public bool UpdateAnimal(int id, string name, string description, string category, string area);
    public bool DeleteAnimal(int id);
}

public class AnimalRepository : IAnimalRepository
{
    private readonly IConfiguration _configuration;

    public AnimalRepository(IConfiguration configuration)
    {
        this._configuration = configuration;
    }
    
    public IEnumerable<Animal> FetchAllAnimals(string orderBy)
    {
        using var con = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
        con.Open();
        var safeOrderBy = new string[] { "Name, Description, CATEGORY, AREA" }.Contains(orderBy) ? orderBy : "Name";
        using var cmd = new SqlCommand("SELECT * FROM Animal ORDER BY "+safeOrderBy+" ASC", con);
        var reader = cmd.ExecuteReader();
        var animals = new List<Animal>();
        while (reader.Read())
        {
            var animal = new Animal()
            {
                Id = (int)reader["IdAnimal"],
                Name = reader["Name"].ToString()!,
                Description = reader["Description"] != DBNull.Value ? reader["Description"].ToString()! : "",
                Category = reader["CATEGORY"].ToString()!,
                Area = reader["AREA"].ToString()!
            };
            animals.Add(animal);
        }

        return animals;
    }
    
    public Animal? GetAnimal(int id)
    {
        using var con = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
        con.Open();
        using var cmd = new SqlCommand("SELECT * FROM Animal WHERE IdAnimal = @id", con);
        cmd.Parameters.AddWithValue("@id", id);
        var reader = cmd.ExecuteReader();
        var animals = new List<Animal>();

        if (reader.Read())
        {
            return new Animal()
            {
                Id = (int)reader["IdAnimal"],
                Name = reader["Name"].ToString()!,
                Description = reader["Description"] != DBNull.Value ? reader["Description"].ToString()! : "",
                Category = reader["CATEGORY"].ToString()!,
                Area = reader["AREA"].ToString()!
            };
        }

        return null;
    }

    public bool UpdateAnimal(int id, string name, string description, string category, string area)
    {
        using var con = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
        con.Open();
        using var cmd =
            new SqlCommand(
                "UPDATE Animal SET Name = @name, Description = @description, CATEGORY = @category, AREA = @area WHERE IdAnimal = @id", con);
        cmd.Parameters.AddWithValue("@name", name);
        cmd.Parameters.AddWithValue("@description", description);
        cmd.Parameters.AddWithValue("@category", category);
        cmd.Parameters.AddWithValue("@area", area);
        cmd.Parameters.AddWithValue("@id", id);

        var rowsAffected = cmd.ExecuteNonQuery();
        return rowsAffected == 1;
    }

    public bool PutAnimal(string name, string description, string category, string area)
    {
        using var con = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
        con.Open();
        using var cmd =
            new SqlCommand(
                "INSERT INTO Animal (Name, Description, CATEGORY, AREA) VALUES(@name, @description, @category, @area)", con);
        cmd.Parameters.AddWithValue("@name", name);
        cmd.Parameters.AddWithValue("@description", description);
        cmd.Parameters.AddWithValue("@category", category);
        cmd.Parameters.AddWithValue("@area", area);

        var rowsAffected = cmd.ExecuteNonQuery();
        return rowsAffected == 1;
    }

    public bool DeleteAnimal(int id)
    {
        using var con = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
        con.Open();
        using var cmd = new SqlCommand("DELETE FROM Animal WHERE IdAnimal = @id", con);
        cmd.Parameters.AddWithValue("@id", id);
        var result = cmd.ExecuteNonQuery();
        return result == 1;
    }
    
    
}