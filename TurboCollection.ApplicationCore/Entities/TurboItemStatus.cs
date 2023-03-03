using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TurboCollection.ApplicationCore.Entities
{
    public sealed class TurboItemStatus
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public TurboItemStatus(string name)
        {
            Name = name;
        }
    }
}
