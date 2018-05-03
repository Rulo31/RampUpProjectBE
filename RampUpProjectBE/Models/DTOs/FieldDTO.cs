using System;

namespace RampUpProjectBE.Models.DTOs {
    public class FieldDTO {
        public int Field_Id { get; set; }
        public int Branch_Id { get; set; }
        public Nullable<int> Number { get; set; }
        public string Name { get; set; }
        public Nullable<double> Width { get; set; }
        public Nullable<double> Length { get; set; }
        public string Material { get; set; }
        public double Cost { get; set; }
    }
}
