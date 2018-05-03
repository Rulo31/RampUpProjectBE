using System;

namespace RampUpProjectBE.Models.DTOs {
    public class PhoneDTO {
        public int Phone_Id { get; set; }
        public Nullable<int> Branch_Id { get; set; }
        public Nullable<int> User_Id { get; set; }
        public string Phone_Number { get; set; }
        public string Phone_Type { get; set; }
    }
}
