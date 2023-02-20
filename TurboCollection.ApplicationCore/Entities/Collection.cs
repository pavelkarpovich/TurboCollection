using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TurboCollection.ApplicationCore.Entities
{
    public sealed class Collection
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Collection(string name)
        {
            Name = name;
        }
    }
}
