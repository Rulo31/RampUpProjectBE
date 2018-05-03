using System;

namespace RampUpProjectBE.Models.DTOs {
    public class FieldResultDTO {
        public int Field_Id { get; set; }
        public int Branch_Id { get; set; }
        public string Branch_Name { get; set; }
        public string Business_Name { get; set; }
        public Nullable<int> Number { get; set; }
        public string Name { get; set; }
        public Nullable<double> Width { get; set; }
        public Nullable<double> Length { get; set; }
        public string Material { get; set; }
        public double Cost { get; set; }
    }
}
