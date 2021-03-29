using DogGo.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace DogGo.Repositories
{
    public class WalkRepository : IWalkRepository
    {
        private readonly IConfiguration _config;

        public WalkRepository(IConfiguration config)
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

        public List<Walk> GetAllWalksById(int id)
        {
            using(SqlConnection conn = Connection)
            {
                conn.Open();
                using(SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                            SELECT w.Id, w.Date, w.Duration, w.WalkerId, w.DogId, Walker.Name, Dog.Name as DogName
                            FROM Walks w
                            LEFT JOIN Walker on w.WalkerId = Walker.Id
                            LEFT JOIN Dog on Dog.id = w.DogId
                            WHERE Walker.Id = @id
                    ";
                    cmd.Parameters.AddWithValue("@id", id);

                    SqlDataReader reader = cmd.ExecuteReader();
                    List < Walk > walks = new List<Walk>();
                    while(reader.Read())
                    {
                        Walk walk = new Walk
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Date = reader.GetDateTime(reader.GetOrdinal("Date")),
                            Duration = reader.GetInt32(reader.GetOrdinal("Duration")),
                            WalkerId = reader.GetInt32(reader.GetOrdinal("WalkerId")),
                            DogId = reader.GetInt32(reader.GetOrdinal("DogId")),

                            Dog = new Dog
                            {
                                Name = reader.GetString(reader.GetOrdinal("DogName"))
                            },
                            Walker = new Walker
                            {
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                              
                            }

                        };
                        walks.Add(walk);
                        
                    }
                    reader.Close();
                    return walks;
                }
            }
        }
    }
}
