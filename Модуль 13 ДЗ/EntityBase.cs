using System;
using System.ComponentModel.DataAnnotations;

namespace Модуль_13_ДЗ
{
    public class EntityBase
    {
        [Key]
        public int Id { get; set; }

        [Timestamp]
        public byte[] Timestamp { get; set; }
    }
}
