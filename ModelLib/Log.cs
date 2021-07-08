namespace ModelLib
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Log")]
    public partial class Log : EntityBase
    {
        public Log() { }

        public Log(string message)
        {
            Message = message;
            Time = DateTime.Now;
        }
                
        [Required]
        [StringLength(200)]
        public string Message { get; set; }

        public DateTime Time { get; set; }
    }
}
