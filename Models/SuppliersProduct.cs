using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace WebApplication_ChainStores.Models
{
    public class SuppliersProduct
    {
        [DisplayName("ID")]
        public int ProductID { get; set; }
        [DisplayName("Название продукта")]
        public string ProductName { get; set; }
        [DisplayName("Закупочная цена")]
        public Nullable<decimal> SalePrice { get; set; }
        [DisplayName("Наличие товара у поставщика")]
        public int Quantity { get; set; }

    }
}