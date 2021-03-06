﻿using System.Threading.Tasks;

namespace AlchemicShop.BLL.Interfaces
{
    public interface IShoppingCartService
    {
        Task<int> GetMaxOrderIdAsync();

        Task UpdateProductAmount(int amount, int id);

        Task<bool> IsEnoughProduct(int? id, int? amount);
    }
}
