using RampUpProjectBE.DAL;
using RampUpProjectBE.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace RampUpProjectBE.Services {
    public class FieldService {
        private string rampConnectionString;

        public FieldService(string rampConnectionString) {
            this.rampConnectionString = rampConnectionString;
        }

        public List<FieldDTO> GetFieldsByBranchId(int branchId) {
            bool lockWasTaken = false;
            List<FieldDTO> fieldsDTO = new List<FieldDTO>();

            using (RampUpProjectEntities dbContext = new RampUpProjectEntities(rampConnectionString)) {
                try {
                    Monitor.Enter(dbContext, ref lockWasTaken);
                    List<Field> fields = dbContext.Fields.Where(x => x.Branch_Id == branchId).ToList();

                    foreach (Field field in fields) {
                        fieldsDTO.Add(ConvertFieldToFieldDTO(field));
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

                return fieldsDTO;
            }
        }

        public List<FieldResultDTO> GetAllFields() {
            bool lockWasTaken = false;
            List<FieldResultDTO> fieldsDTO = new List<FieldResultDTO>();

            using (RampUpProjectEntities dbContext = new RampUpProjectEntities(rampConnectionString)) {
                try {
                    Monitor.Enter(dbContext, ref lockWasTaken);
                    List<Field> fields = dbContext.Fields.ToList();

                    foreach (Field field in fields) {
                        fieldsDTO.Add(ConvertFieldToFieldResultDTO(field));
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

                return fieldsDTO;
            }
        }

        public FieldDTO AddField(FieldDTO fieldDTO) {
            try {
                Field field = ConvertFieldDTOToField(fieldDTO);

                using (RampUpProjectEntities dbContext = new RampUpProjectEntities(rampConnectionString)) {
                    dbContext.Fields.Add(field);
                    dbContext.SaveChanges();
                }

                return ConvertFieldToFieldDTO(field);
            } catch (Exception) {
                throw;
            }
        }

        public void UpdateField(FieldDTO fieldDTO) {
            try {
                Field field = ConvertFieldDTOToField(fieldDTO);

                using (RampUpProjectEntities dbContext = new RampUpProjectEntities(rampConnectionString)) {
                    dbContext.Fields.Attach(field);
                    dbContext.Entry(field).State = System.Data.Entity.EntityState.Modified;
                    dbContext.SaveChanges();
                }
            } catch (Exception) {
                throw;
            }
        }

        public void RemoveField(int fieldID) {
            try {
                Field field = new Field() { Field_Id = fieldID };

                using (RampUpProjectEntities dbContext = new RampUpProjectEntities(rampConnectionString)) {
                    dbContext.Fields.Attach(field);
                    dbContext.Fields.Remove(field);
                    dbContext.SaveChanges();
                }
            } catch (Exception) {
                throw;
            }
        }

        private static Field ConvertFieldDTOToField(FieldDTO fieldDTO) {
            return new Field {
                Field_Id = fieldDTO.Field_Id,
                Name = fieldDTO.Name,
                Branch_Id = fieldDTO.Branch_Id,
                Cost = fieldDTO.Cost,
                Length = fieldDTO.Length,
                Material = fieldDTO.Material,
                Number = fieldDTO.Number,
                Width = fieldDTO.Width
            };
        }

        private static FieldDTO ConvertFieldToFieldDTO(Field field) {
            return new FieldDTO {
                Field_Id = field.Field_Id,
                Name = field.Name,
                Branch_Id = field.Branch_Id,
                Cost = field.Cost,
                Length = field.Length,
                Material = field.Material,
                Number = field.Number,
                Width = field.Width
            };
        }

        private static FieldResultDTO ConvertFieldToFieldResultDTO(Field field) {
            return new FieldResultDTO {
                Field_Id = field.Field_Id,
                Name = field.Name,
                Branch_Id = field.Branch_Id,
                Cost = field.Cost,
                Length = field.Length,
                Material = field.Material,
                Number = field.Number,
                Width = field.Width,
                Branch_Name = field.Branch.Name,
                Business_Name = field.Branch.Business.Name
            };
        }
    }
}