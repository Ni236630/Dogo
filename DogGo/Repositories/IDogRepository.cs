using DogGo.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DogGo.Repositories
{
   public interface IDogRepository
    {
        List<Dog> GetAllDogs();
        Dog GetDogById(int id);
        void AddDog(Dog dog);
        void DeleteDog(int id);
        void UpdateDog(Dog dog);
        
    }
}
