using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebChat.Models
{
    public class DropboxAuthentication
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public string Secret { get; set; }
    }
}
