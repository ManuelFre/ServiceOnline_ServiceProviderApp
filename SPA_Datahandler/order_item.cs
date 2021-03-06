//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SPA_Datahandler
{
    using System;
    using System.Collections.Generic;
    
    public partial class order_item
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public order_item()
        {
            this.order_item_report = new HashSet<order_item_report>();
        }
    
        public int Id { get; set; }
        public long order_id { get; set; }
        public int service_id { get; set; }
        public int quantity { get; set; }
        public double per_item_tax { get; set; }
        public double tax { get; set; }
        public double price { get; set; }
        public double option_price { get; set; }
        public double final_price { get; set; }
        public double final_price_with_tax { get; set; }
        public double final_price_without_tax { get; set; }
        public string is_all_inclusive { get; set; }
        public System.DateTime preferred_date_time { get; set; }
        public string is_finished { get; set; }
        public System.DateTime createdAt { get; set; }
        public Nullable<System.DateTime> deletedAt { get; set; }
        public string is_confirmed { get; set; }
        public Nullable<double> addittional_cost { get; set; }
        public string service_provider_comment { get; set; }
    
        public virtual order_header order_header { get; set; }
        public virtual service service { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<order_item_report> order_item_report { get; set; }
    }
}
