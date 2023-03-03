using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TurboCollection.ApplicationCore.Entities
{
    public sealed class ExtraTurboItem
    {
        public ExtraTurboItem(int collectionId, int number, string userId, string pictureUrl)
        {
            UserId = userId;
            CollectionId = collectionId;
            Number = number;
            PictureUrl = pictureUrl;
        }

        public long Id { get; set; }
        public int CollectionId { get; set; }
        public int Number { get; set; }
        public string PictureUrl { get; set; }
        public string UserId { get; set; }
    }
}
