using System;
using System.ComponentModel.DataAnnotations;

namespace ModelLib
{
    public class EntityBase
    {
        [Key]
        public int Id { get; set; }

        [Timestamp]
        public byte[] Timestamp { get; set; }
    }
}
