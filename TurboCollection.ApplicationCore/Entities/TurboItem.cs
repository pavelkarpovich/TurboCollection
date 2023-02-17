using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TurboCollection.ApplicationCore.Entities
{
    public sealed class TurboItem
    {
        public int Id { get; set; }
        public int Collection { get; set; }
        public int Number { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public int Speed { get; set; }
        public int EngineCapacity { get; set; }
        public int HorsePower { get; set; }
        public int Year { get; set; }
        public string Tags { get; set; }
        public string Picture { get; set; }
    }
}
