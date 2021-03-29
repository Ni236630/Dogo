using System;
using System.Collections.Generic;
namespace DogGo.Models.ViewModels
{
    public class WalkerDetailModel
    {
        public Walker Walker { get; set; }
        public Dog Dog { get; set; }
        public Owner Owner { get; set; }
        public List<Walk> Walks { get; set; }
    }
}
