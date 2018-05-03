using System;

namespace RampUpProjectBE.Models.DTOs {
    public class AddressDTO {
        public int Address_Id { get; set; }
        public string First_Line { get; set; }
        public string Second_Line { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public Nullable<int> Postal_Code { get; set; }
        public System.DateTime Creation_Date { get; set; }
    }
}
