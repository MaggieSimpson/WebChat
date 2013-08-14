using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebChat.Models
{
    public class User
    {
        public int UserId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        [Required]
        [MinLength(4)]
        [MaxLength(40)]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        public string Email { get; set; }

        public string Sessionkey { get; set; }

        public string ProfilePicture { get; set; }

        public virtual ICollection<MessageBase> Messages { get; set; }

        public User()
        {
            this.Messages = new HashSet<MessageBase>();
        }
    }
}
