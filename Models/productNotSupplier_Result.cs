//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebApplication_ChainStores.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel;

    public partial class productNotSupplier_Result
    {
        public int ProductID { get; set; }
        [DisplayName("Название товара")]
        public string ProductName { get; set; }
        [DisplayName("Цена закупки")]
        public Nullable<decimal> PurchasePrice { get; set; }
        [DisplayName("Цена продажи")]
        public Nullable<decimal> SalePrice { get; set; }
        [DisplayName("Характеристики")]
        public string DescriptionProduct { get; set; }
        [DisplayName("Место на складе")]
        public Nullable<int> Place { get; set; }
    }
}
