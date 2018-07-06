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
        public DateTime DateAndTime { get; set; }
        public string HostName { get; set; }
        public int ErrorCode { get; set; }
    }
}
