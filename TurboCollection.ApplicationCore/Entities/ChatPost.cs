using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TurboCollection.ApplicationCore.Entities
{
    public class ChatPost
    {
        public ChatPost(string text, DateTime dateTime, string fromUserId, string toUserId, bool isRead)
        {
            Text = text;
            DateTime = dateTime;
            FromUserId = fromUserId;
            ToUserId = toUserId;
            IsRead = isRead;
        }

        public long Id { get; set; }
        public string Text { get; set; }
        public DateTime DateTime { get; set; }
        public string FromUserId { get; set; }
        public string ToUserId { get; set; }
        public bool IsRead { get; set; }
    }
}
