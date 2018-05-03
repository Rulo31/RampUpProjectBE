namespace RampUpProjectBE.Models.DTOs {
    public class CardDTO {
        public int Card_Id { get; set; }
        public int User_Id { get; set; }
        public string Card_Number { get; set; }
        public int CVC { get; set; }
        public System.DateTime Expiration_Date { get; set; }
        public System.DateTime Creation_Date { get; set; }
    }
}
