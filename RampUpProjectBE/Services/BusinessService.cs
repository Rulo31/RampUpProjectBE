using RampUpProjectBE.DAL;
using RampUpProjectBE.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace RampUpProjectBE.Services {
    public class BusinessService {
        private string rampConnectionString;

        public BusinessService(string rampConnectionString) {
            this.rampConnectionString = rampConnectionString;
        }

        public List<BusinessDTO> GetMyBusinesses(int userID) {
            bool lockWasTaken = false;
            List<BusinessDTO> businesses = new List<BusinessDTO>();

            using (RampUpProjectEntities dbContext = new RampUpProjectEntities(rampConnectionString)) {
                try {
                    Monitor.Enter(dbContext, ref lockWasTaken);

                    businesses = dbContext.Businesses.Where(x => x.User_Id == userID).Select(x => new BusinessDTO() { Business_Id = x.Business_Id, Creation_Date = x.Creation_Date, Name = x.Name, User_Id = x.User_Id }).ToList();

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

                return businesses;
            }
        }

        public BusinessDTO GetBusiness(int businessID) {
            bool lockWasTaken = false;
            BusinessDTO businessDTO = null;

            using (RampUpProjectEntities dbContext = new RampUpProjectEntities(rampConnectionString)) {
                try {
                    Monitor.Enter(dbContext, ref lockWasTaken);

                    Business business = dbContext.Businesses.Where(x => x.Business_Id == businessID).FirstOrDefault();

                    if (business != null) {
                        businessDTO = ConvertBusinessToBusinessDTO(business);
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

                return businessDTO;
            }
        }

        public int AddBusiness(BusinessDTO businessDTO) {
            try {
                Business business = ConvertBusinessDTOToBusiness(businessDTO);

                using (RampUpProjectEntities dbContext = new RampUpProjectEntities(rampConnectionString)) {
                    dbContext.Businesses.Add(business);
                    dbContext.SaveChanges();
                }

                return business.Business_Id;
            } catch (Exception) {
                throw;
            }
        }

        public void UpdateBusiness(BusinessDTO businessDTO) {
            try {
                Business business = ConvertBusinessDTOToBusiness(businessDTO);

                using (RampUpProjectEntities dbContext = new RampUpProjectEntities(rampConnectionString)) {
                    dbContext.Businesses.Attach(business);
                    dbContext.Entry(business).State = System.Data.Entity.EntityState.Modified;
                    dbContext.SaveChanges();
                }
            } catch (Exception) {
                throw;
            }
        }

        public void RemoveBusiness(int businessID) {
            try {
                Business business = new Business() { Business_Id = businessID };

                using (RampUpProjectEntities dbContext = new RampUpProjectEntities(rampConnectionString)) {
                    dbContext.Businesses.Attach(business);
                    dbContext.Businesses.Remove(business);
                    dbContext.SaveChanges();
                }
            } catch (Exception) {
                throw;
            }
        }

        private static Business ConvertBusinessDTOToBusiness(BusinessDTO businessDTO) {
            return new Business {
                Business_Id = businessDTO.Business_Id,
                Name = businessDTO.Name,
                Creation_Date = businessDTO.Creation_Date,
                User_Id = businessDTO.User_Id
            };
        }

        private static BusinessDTO ConvertBusinessToBusinessDTO(Business business) {
            return new BusinessDTO {
                Business_Id = business.Business_Id,
                Name = business.Name,
                Creation_Date = business.Creation_Date,
                User_Id = business.User_Id
            };
        }
    }
}