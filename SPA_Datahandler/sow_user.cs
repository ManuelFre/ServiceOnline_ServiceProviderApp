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
    
    public partial class sow_user
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public sow_user()
        {
            this.order_header = new HashSet<order_header>();
            this.sow_user_delivery_address = new HashSet<sow_user_delivery_address>();
        }
    
        public int Id { get; set; }
        public string name { get; set; }
        public string username { get; set; }
        public string email { get; set; }
        public System.DateTime registeredAt { get; set; }
        public Nullable<System.DateTime> deletedAt { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<order_header> order_header { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<sow_user_delivery_address> sow_user_delivery_address { get; set; }
    }
}
