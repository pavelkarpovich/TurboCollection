using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TurboCollection.ApplicationCore.Entities
{
    public sealed class TurboItem
    {
        public TurboItem(int collectionId, int number, string picture, string? brand = null, string? model = null, int? speed = null,
            int? engineCapacity = null, int? horsePower = null, int? year = null, string? tags = null)
        {
            CollectionId = collectionId;
            Number = number;
            Picture = picture;
            Brand = brand;
            Model = model;
            Speed = speed;
            EngineCapacity = engineCapacity;
            HorsePower = horsePower;
            Year = year;
            Tags = tags;
        }
        public int Id { get; set; }
        public int CollectionId { get; set; }
        public int Number { get; set; }
        public string Picture { get; set; }
        public string? Brand { get; set; }
        public string? Model { get; set; }
        public int? Speed { get; set; }
        public int? EngineCapacity { get; set; }
        public int? HorsePower { get; set; }
        public int? Year { get; set; }
        public string? Tags { get; set; }
    }
}
