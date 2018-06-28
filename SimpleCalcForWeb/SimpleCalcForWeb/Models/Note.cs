using System.ComponentModel.DataAnnotations;

namespace SimpleCalcForWeb.Models
{
    public class Note
    {
        [Key]
        public string Expression { get; set; }
        public string Result { get; set; }
    }
}
