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
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel;
    public partial class Suppliers
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Suppliers()
        {
            this.Orders_Suppliers = new HashSet<Orders_Suppliers>();
            this.ProductAvailability_Suppliers = new HashSet<ProductAvailability_Suppliers>();
        }
    
        public int SupplierID { get; set; }
        [DisplayName("Имя поставщика")]
        public string SupplierName { get; set; }
        [DisplayName("Страна")]
        public string Country { get; set; }
        [DisplayName("Город")]
        public string City { get; set; }
        [DisplayName("Улица")]
        public string Street { get; set; }
        [DisplayName("Дом")]
        public string Building { get; set; }
        [DisplayName("Телефон")]
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Orders_Suppliers> Orders_Suppliers { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductAvailability_Suppliers> ProductAvailability_Suppliers { get; set; }
    }
}
