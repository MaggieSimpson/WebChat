using System.ComponentModel.DataAnnotations.Schema;

namespace WebChat.Models
{
    public class TextMessage : MessageBase
    {
        [Column(TypeName = "ntext")]
        public string Content { get; set; }
    }
}