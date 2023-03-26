using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TurboCollection.ApplicationCore.Entities
{
    public class TurboItemTag
    {
        public TurboItemTag(int turboItemId, int tagId)
        {
            TurboItemId = turboItemId;
            TagId = tagId;
        }

        public int TurboItemId { get; set; }
        public int TagId { get; set; }
    }
}
