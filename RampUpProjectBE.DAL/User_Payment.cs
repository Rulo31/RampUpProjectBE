//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RampUpProjectBE.DAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class User_Payment
    {
        public int User_Payment_Id { get; set; }
        public int Payment_Method_Ref_Id { get; set; }
        public Nullable<int> Card_Id { get; set; }
        public Nullable<int> PayPal_Id { get; set; }
        public System.DateTime Creation_Date { get; set; }
    
        public virtual Card Card { get; set; }
        public virtual Payment_Method_Ref Payment_Method_Ref { get; set; }
        public virtual PayPal PayPal { get; set; }
    }
}
