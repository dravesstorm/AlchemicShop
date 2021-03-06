﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AlchemicShop.DAL.Entities
{
    public class OrderProduct: BaseEntity
    {

        [Index("IX_OrderProduct", 1, IsUnique = true)]
        public int OrderId { get; set; }

        [Index("IX_OrderProduct", 2, IsUnique = true)]
        public int ProductId { get; set; }
    
        public Order Order { get; set; }
     
        public Product Product { get; set; }

        [Required]
        public int Amount { get; set; }
    }
}
