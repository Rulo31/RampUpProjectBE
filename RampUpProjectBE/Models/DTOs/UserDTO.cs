using System.Collections.Generic;

namespace RampUpProjectBE.Models.DTOs {
    public class UserDTO {
        public int User_Id { get; set; }
        public int Address_Id { get; set; }
        public string First_Name { get; set; }
        public string Middle_Name { get; set; }
        public string Last_Name { get; set; }
        public string Email { get; set; }
        public System.DateTime Creation_Date { get; set; }
        public string Password { get; set; }
        public string Status { get; set; }
        public AddressDTO Address { get; set; }
        public IEnumerable<PhoneDTO> Phones { get; set; }
    }
}
