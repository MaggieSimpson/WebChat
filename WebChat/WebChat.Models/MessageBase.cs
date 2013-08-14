using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace WebChat.Models
{
    public abstract class MessageBase
    {
        [Key]
        public int MessageId { get; set; }

        public int SenderId { get; set; }
        public virtual User Sender { get; set; }

        public int RecieverId { get; set; }
        public virtual User Reciever { get; set; }

        public DateTime Date { get; set; }
        public bool State { get; set; }
    }
}
