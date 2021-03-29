using System;
using System.Collections.Generic;
using DogGo.Models;
using DogGo.Repositories;

namespace DogGo.Repositories
{
    public interface IWalkRepository
    {
        List<Walk> GetAllWalksById(int id);

    }
}
