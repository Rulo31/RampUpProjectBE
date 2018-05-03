using RampUpProjectBE.DAL;
using RampUpProjectBE.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace RampUpProjectBE.Services {
    public class BranchService {
        private string rampConnectionString;

        public BranchService(string rampConnectionString) {
            this.rampConnectionString = rampConnectionString;
        }

        public BranchDTO GetBranch(int branchID) {
            bool lockWasTaken = false;
            BranchDTO branchDTO = null;

            using (RampUpProjectEntities dbContext = new RampUpProjectEntities(rampConnectionString)) {
                try {
                    Monitor.Enter(dbContext, ref lockWasTaken);

                    Branch branch = dbContext.Branches.Where(x => x.Branch_Id == branchID).FirstOrDefault();

                    if (branch != null) {
                        branchDTO = ConvertBranchToBranchDTO(branch);
                    }
                    if (lockWasTaken) {
                        Monitor.Exit(dbContext);
                        lockWasTaken = false;
                    }

                } catch (Exception) {
                    if (lockWasTaken) {
                        Monitor.Exit(dbContext);
                        lockWasTaken = false;
                    }

                    throw;
                }

                return branchDTO;
            }
        }

        public List<BranchDTO> GetBranchesByBusinessId(int businessID) {
            bool lockWasTaken = false;
            List<BranchDTO> branchesDTO = new List<BranchDTO>();

            using (RampUpProjectEntities dbContext = new RampUpProjectEntities(rampConnectionString)) {
                try {
                    Monitor.Enter(dbContext, ref lockWasTaken);

                    List<Branch> branches = dbContext.Branches.Where(x => x.Business_Id == businessID).ToList();

                    foreach (Branch branch in branches) {
                        branchesDTO.Add(ConvertBranchToBranchDTO(branch));
                    }

                    if (lockWasTaken) {
                        Monitor.Exit(dbContext);
                        lockWasTaken = false;
                    }

                } catch (Exception) {
                    if (lockWasTaken) {
                        Monitor.Exit(dbContext);
                        lockWasTaken = false;
                    }

                    throw;
                }

                return branchesDTO;
            }
        }

        public BranchDTO AddBranch(BranchDTO branchDTO) {
            try {
                Branch branch = ConvertBranchDTOToBranch(branchDTO);

                using (RampUpProjectEntities dbContext = new RampUpProjectEntities(rampConnectionString)) {
                    dbContext.Branches.Add(branch);
                    dbContext.SaveChanges();
                }

                return ConvertBranchToBranchDTO(branch);
            } catch (Exception) {
                throw;
            }
        }

        public List<BranchDTO> AddBranches(List<BranchDTO> branches) {
            try {

                foreach (BranchDTO branchDTO in branches) {
                    Branch branch = ConvertBranchDTOToBranch(branchDTO);

                    using (RampUpProjectEntities dbContext = new RampUpProjectEntities(rampConnectionString)) {
                        dbContext.Branches.Add(branch);
                        dbContext.SaveChanges();
                    }

                    branchDTO.Branch_Id = branch.Branch_Id;
                }

            } catch (Exception) {
                throw;
            }

            return branches;
        }

        public void UpdateBranch(BranchDTO branchDTO) {
            try {
                Branch branch = ConvertBranchDTOToBranch(branchDTO);

                using (RampUpProjectEntities dbContext = new RampUpProjectEntities(rampConnectionString)) {
                    UpdatePhones(branchDTO, branch, dbContext);
                    UpdateFields(branchDTO, branch, dbContext);

                    if (branch.Address != null) {
                        dbContext.Entry(branch.Address).State = System.Data.Entity.EntityState.Modified;
                    }

                    dbContext.Branches.Attach(branch);
                    dbContext.Entry(branch).State = System.Data.Entity.EntityState.Modified;

                    dbContext.SaveChanges();
                }
            } catch (Exception) {
                throw;
            }
        }

        private static void UpdateFields(BranchDTO branchDTO, Branch branch, RampUpProjectEntities dbContext) {
            if (branch.Fields != null) {
                foreach (Field field in branch.Fields) {
                    if (field.Field_Id != 0) {
                        dbContext.Entry(field).State = System.Data.Entity.EntityState.Modified;
                    } else {
                        dbContext.Entry(field).State = System.Data.Entity.EntityState.Added;
                    }
                }

                List<int> fields = branch.Fields.Select(y => y.Field_Id).ToList();
                List<Field> toBeDeletedFields = dbContext.Fields.Where(x => !fields.Contains(x.Field_Id) && x.Branch_Id == branchDTO.Branch_Id).ToList();

                foreach (Field field in toBeDeletedFields) {
                    dbContext.Entry(field).State = System.Data.Entity.EntityState.Deleted;
                }
            }
        }

        private static void UpdatePhones(BranchDTO branchDTO, Branch branch, RampUpProjectEntities dbContext) {
            if (branch.Phones != null) {
                foreach (Phone phone in branch.Phones) {
                    if (phone.Phone_Id != 0) {
                        dbContext.Entry(phone).State = System.Data.Entity.EntityState.Modified;
                    } else {
                        dbContext.Entry(phone).State = System.Data.Entity.EntityState.Added;
                    }
                }

                List<int> phones = branch.Phones.Select(y => y.Phone_Id).ToList();
                List<Phone> toBeDeletedPhones = dbContext.Phones.Where(x => !phones.Contains(x.Phone_Id) && x.Branch_Id == branchDTO.Branch_Id).ToList();

                foreach (Phone phone in toBeDeletedPhones) {
                    dbContext.Entry(phone).State = System.Data.Entity.EntityState.Deleted;
                }
            }
        }

        public void RemoveBranch(int branchID) {
            try {
                Branch branch = new Branch() { Branch_Id = branchID };

                using (RampUpProjectEntities dbContext = new RampUpProjectEntities(rampConnectionString)) {
                    dbContext.Branches.Attach(branch);
                    dbContext.Branches.Remove(branch);
                    dbContext.SaveChanges();
                }
            } catch (Exception) {
                throw;
            }
        }

        private static Branch ConvertBranchDTOToBranch(BranchDTO branchDTO) {
            return new Branch {
                Branch_Id = branchDTO.Branch_Id,
                Name = branchDTO.Name,
                Creation_Date = branchDTO.Creation_Date,
                Address_Id = branchDTO.Address_Id,
                Business_Id = branchDTO.Business_Id,
                Email = branchDTO.Email,
                Address = branchDTO.Address != null ? new Address() { Address_Id = branchDTO.Address_Id, Creation_Date = branchDTO.Address.Creation_Date, First_Line = branchDTO.Address.First_Line, Second_Line = branchDTO.Address.Second_Line, City = branchDTO.Address.City, State = branchDTO.Address.State, Postal_Code = branchDTO.Address.Postal_Code } : null,
                Fields = branchDTO.Fields != null ? branchDTO.Fields.Select(x => new Field() { Branch_Id = x.Branch_Id, Cost = x.Cost, Field_Id = x.Field_Id, Length = x.Length, Material = x.Material, Name = x.Name, Number = x.Number, Width = x.Width }).ToList() : null,
                Phones = branchDTO.Phones != null ? branchDTO.Phones.Select(x => new Phone() { Phone_Id = x.Phone_Id, Branch_Id = x.Branch_Id, User_Id = x.User_Id, Phone_Number = x.Phone_Number, Phone_Type = x.Phone_Type }).ToList() : null
            };
        }

        private static BranchDTO ConvertBranchToBranchDTO(Branch branch) {
            return new BranchDTO {
                Branch_Id = branch.Branch_Id,
                Name = branch.Name,
                Creation_Date = branch.Creation_Date,
                Address_Id = branch.Address_Id,
                Business_Id = branch.Business_Id,
                Email = branch.Email,
                Address = branch.Address != null ? new AddressDTO() { Address_Id = branch.Address_Id, Creation_Date = branch.Address.Creation_Date, First_Line = branch.Address.First_Line, Second_Line = branch.Address.Second_Line, City = branch.Address.City, State = branch.Address.State, Postal_Code = branch.Address.Postal_Code } : null,
                Fields = branch.Fields != null ? branch.Fields.Select(x => new FieldDTO() { Branch_Id = x.Branch_Id, Cost = x.Cost, Field_Id = x.Field_Id, Length = x.Length, Material = x.Material, Name = x.Name, Number = x.Number, Width = x.Width }).ToList() : null,
                Phones = branch.Phones != null ? branch.Phones.Select(x => new PhoneDTO() { Phone_Id = x.Phone_Id, Branch_Id = x.Branch_Id, User_Id = x.User_Id, Phone_Number = x.Phone_Number, Phone_Type = x.Phone_Type }).ToList() : null
            };
        }
    }
}