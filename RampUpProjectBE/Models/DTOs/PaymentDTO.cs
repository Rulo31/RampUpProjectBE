using System;

namespace RampUpProjectBE.Models.DTOs {
    public class PaymentDTO {
        public int Payment_Id { get; set; }
        public int Payment_Status_Ref_Id { get; set; }
        public string Confirmation_Number { get; set; }
        public Nullable<double> Amount { get; set; }
        public Nullable<System.DateTime> Payment_Date { get; set; }
    }
}
