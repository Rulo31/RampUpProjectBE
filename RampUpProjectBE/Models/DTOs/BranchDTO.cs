using System;
using System.Collections.Generic;

namespace RampUpProjectBE.Models.DTOs {
    public class BranchDTO {
        public int Branch_Id { get; set; }
        public int Business_Id { get; set; }
        public int Address_Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public Nullable<System.DateTime> Creation_Date { get; set; }
        public AddressDTO Address { get; set; }
        public IEnumerable<FieldDTO> Fields { get; set; }
        public IEnumerable<PhoneDTO> Phones { get; set; }
    }
}
