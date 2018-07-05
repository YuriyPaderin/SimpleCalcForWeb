using System;
using System.ComponentModel.DataAnnotations;

namespace SimpleCalcForWeb.Models
{
    public class Note
    {
        [Key]
        public Guid Id { get; set; }
        public string Expression { get; set; }
        public double? Result { get; set; }
        public DateTime Date { get; set; }
        public string Host { get; set; }
        public int CodeError { get; set; }
    }
}
