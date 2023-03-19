using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TurboCollection.ApplicationCore.Entities
{
    public class Unread
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public bool IsUnreadPost { get; set; }
    }
}
