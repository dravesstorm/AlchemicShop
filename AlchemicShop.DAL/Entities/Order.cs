﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AlchemicShop.DAL.Entities
{
    public class Order: BaseEntity
    {
       
        public int UserId { get; set; }

        public User User { get; set; }

        [Required]
        public DateTime SheduledDate { get; set; }
        
        public DateTime? ClosedDate { get; set; }

        [Required]
        public Status Status { get; set; }
       
        public virtual ICollection<OrderProduct> OrderProducts { get; set; }
    }
}
