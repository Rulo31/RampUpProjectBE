namespace RampUpProjectBE.Models.DTOs {
    public class ReservationDTO {
        public int Reservation_Id { get; set; }
        public int User_Id { get; set; }
        public int Field_Id { get; set; }
        public System.DateTime Date { get; set; }
        public System.TimeSpan Start_Time { get; set; }
        public System.TimeSpan End_Time { get; set; }
        public System.DateTime Creation_Date { get; set; }
        public string Branch_Name { get; set; }
        public int? Field_Number { get; set; }
    }
}
