using System;

namespace RampUpProjectBE.Models.DTOs {
    public class ChallengeDTO {
        public int Challenge_Id { get; set; }
        public Nullable<int> Reservation_Id { get; set; }
        public Nullable<int> First_Team_Id { get; set; }
        public Nullable<int> Second_Team_Id { get; set; }
        public System.DateTime Creation_Date { get; set; }
    }
}
