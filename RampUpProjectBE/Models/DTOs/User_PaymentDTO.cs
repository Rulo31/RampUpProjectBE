using System;

namespace RampUpProjectBE.Models.DTOs {
    public class User_PaymentDTO {
        public PayPalDTO PayPal { get; set; }
        public CardDTO Card { get; set; }
        public Payment_Method_RefDTO Payment_Method_Ref { get; set; }
        public int User_Payment_Id { get; set; }
        public int Payment_Method_Ref_Id { get; set; }
        public Nullable<int> Card_Id { get; set; }
        public Nullable<int> PayPal_Id { get; set; }
        public System.DateTime Creation_Date { get; set; }
    }
}
