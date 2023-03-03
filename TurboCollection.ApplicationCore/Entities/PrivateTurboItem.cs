using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TurboCollection.ApplicationCore.Entities
{
    public sealed class PrivateTurboItem
    {
        public PrivateTurboItem(int collectionId, int number, int statusId, string userId)
        {
            UserId = userId;
            CollectionId = collectionId;
            Number = number;
            StatusId = statusId;
        }

        public long Id { get; set; }
        public int CollectionId { get; set; }
        public int Number { get; set; }
        public int StatusId { get; set; }
        public string UserId { get; set; }
    }
}
