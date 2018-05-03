using System;
using System.Collections.Generic;

namespace RampUpProjectBE.Models.DTOs {
    public class BusinessDTO {
        public Nullable<int> User_Id { get; set; }
        public int Business_Id { get; set; }
        public string Name { get; set; }
        public Nullable<System.DateTime> Creation_Date { get; set; }
        public IEnumerable<BranchDTO> Branches { get; set; }
    }
}
