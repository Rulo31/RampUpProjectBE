using System;

namespace RampUpProjectBE.Models.DTOs {
    public class AuditLogDTO {
        public string Field_Name { get; set; }
        public string Previous_Value { get; set; }
        public string New_Value { get; set; }
        public DateTime Timestamp { get; set; }
        public int User_Id { get; set; }
        public string User_Name { get; set; }
        public string Object_Type { get; set; }
        public int Object_Id { get; set; }
    }
}