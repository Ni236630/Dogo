using DogGo.Models;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;


namespace DogGo.Repositories
{
    public class DogRepository : IDogRepository
    {
        private readonly IConfiguration _config;

        public DogRepository(IConfiguration config)
        {
            _config = config;
        }

        public SqlConnection Connection
        {
            get
            {
                return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            }
        }

        public List<Dog> GetAllDogs()
        {
            using(SqlConnection conn = Connection)
            {
                conn.Open();
                using(SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                                SELECT d.Id, d.[Name], d.Breed, d.OwnerId, d.Notes, d.ImageUrl, o.[Name] as [Owner]
                                FROM Dog d
                                LEFT Join [Owner] o ON o.Id = d.OwnerId  
                    ";
                    SqlDataReader reader = cmd.ExecuteReader();
                    List<Dog> dogs = new List<Dog>();
                    while(reader.Read())
                    {
                        Dog dog = new Dog
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            Breed = reader.GetString(reader.GetOrdinal("Breed")),
                            Notes = reader.IsDBNull(reader.GetOrdinal("Notes"))
                                ? "No Notes Here!"
                                : reader.GetString(reader.GetOrdinal("Notes")),
                            ImageUrl = reader.IsDBNull(reader.GetOrdinal("ImageUrl"))
                                ? "No Image Found"
                                : reader.GetString(reader.GetOrdinal("ImageUrl")),
                            Owner = new Owner
                            {
                                Name = reader.GetString(reader.GetOrdinal("Owner"))
                            }

                        };
                        dogs.Add(dog);
                    }
                    reader.Close();
                    return dogs;
                }

            }
        }

        public Dog GetDogById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT d.Id, d.[Name], d.Breed, d.OwnerId, d.Notes, d.ImageUrl, o.[Name] as [Owner]
                                FROM Dog d
                                LEFT Join [Owner] o ON o.Id = d.OwnerId
                                WHERE d.Id = @id
                    ";
                    cmd.Parameters.AddWithValue("@id", id);

                    SqlDataReader reader = cmd.ExecuteReader();
                    
                    if(reader.Read())
                    {
                        Dog dog = new Dog
                        {
                        
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            Breed = reader.GetString(reader.GetOrdinal("Breed")),
                            Notes = reader.IsDBNull(reader.GetOrdinal("Notes"))
                                ? "No Notes Here!"
                                : reader.GetString(reader.GetOrdinal("Notes")),
                            ImageUrl = reader.IsDBNull(reader.GetOrdinal("ImageUrl"))
                                ? "No Image Found"
                                : reader.GetString(reader.GetOrdinal("ImageUrl")),
                            Owner = new Owner
                            {
                                Name = reader.GetString(reader.GetOrdinal("Owner")),
                                Id = reader.GetInt32(reader.GetOrdinal("OwnerId"))
                            }
                        };
                        reader.Close();
                        return dog;
                    }
                    else
                    {
                        reader.Close();
                        return null;
                    }

                }
            }
        }

        public void AddDog(Dog dog)
        {
            using(SqlConnection conn = Connection)
            {
                conn.Open();
                using(SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        INSERT INTO Dog ([Name], Breed, OwnerId, Notes, ImageUrl)
                        OUTPUT INSERTED.ID
                        VALUES (@name, @breed, @ownerId, @notes, @imageUrl)
                    ";

                    cmd.Parameters.AddWithValue("@name", dog.Name);
                    cmd.Parameters.AddWithValue("@breed", dog.Breed);
                    cmd.Parameters.AddWithValue("@ownerId", dog.OwnerId);
                    cmd.Parameters.AddWithValue("@imageUrl", dog.ImageUrl);
                    cmd.Parameters.AddWithValue("@notes", dog.Notes);

                    int id = (int)cmd.ExecuteScalar();

                    dog.Id = id;
                }
            }
        }

        public void UpdateDog(Dog dog)
        {
            using(SqlConnection conn = Connection)
            {
                conn.Open();

                using(SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                            UPDATE Dog
                            SET
                               [Name] = @name,
                                Breed = @breed,
                                OwnerId = @ownerId,
                                ImageUrl = @imageUrl,
                                Notes = @notes
                            WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@name", dog.Name);
                    cmd.Parameters.AddWithValue("@breed", dog.Breed);
                    cmd.Parameters.AddWithValue("@ownerId", dog.OwnerId);
                    cmd.Parameters.AddWithValue("@imageUrl", dog.ImageUrl);
                    cmd.Parameters.AddWithValue("@notes", dog.Notes);
                    cmd.Parameters.AddWithValue("@id", dog.Id);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void DeleteDog(int dogId)
        {
            using(SqlConnection conn = Connection)
            {
                conn.Open();
                using(SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                            DELETE FROM Dog
                            WHERE Id = @id
                    ";
                    cmd.Parameters.AddWithValue("@id", dogId);

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
