using RampUpProjectBE.DAL;
using RampUpProjectBE.Models.DTOs;
using RampUpProjectBE.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Threading;

namespace RampUpProjectBE.Services {
    public class UserService {
        private string rampConnectionString;
        private string encryptionKey;

        public UserService(string rampConnectionString, string encryptionKey) {
            this.rampConnectionString = rampConnectionString;
            this.encryptionKey = encryptionKey;
        }

        public UserDTO Login(string email, SecureString password) {
            bool lockWasTaken = false;
            UserDTO userDTO = null;

            using (RampUpProjectEntities dbContext = new RampUpProjectEntities(rampConnectionString)) {
                try {
                    Monitor.Enter(dbContext, ref lockWasTaken);
                    string pass = Encryption.Encrypt(password.ConvertToUnsecureString(), encryptionKey);

                    User user = dbContext.Users.Where(x => x.Email == email && x.Password == pass).FirstOrDefault();

                    if (user != null) {
                        userDTO = ConvertUserToUserDTO(user);
                        userDTO.Password = Encryption.Decrypt(userDTO.Password, encryptionKey);
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
            }

            return userDTO;
        }

        public bool IsUserRegistered(string email) {
            bool isRegistered = false;
            bool lockWasTaken = false;

            using (RampUpProjectEntities dbContext = new RampUpProjectEntities(rampConnectionString)) {
                try {
                    Monitor.Enter(dbContext, ref lockWasTaken);

                    User user = dbContext.Users.Where(x => x.Email == email).FirstOrDefault();

                    if (user != null) {
                        isRegistered = true;
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
            }

            return isRegistered;
        }

        public UserDTO GetUser(int userID) {
            bool lockWasTaken = false;
            UserDTO userDTO = null;

            using (RampUpProjectEntities dbContext = new RampUpProjectEntities(rampConnectionString)) {
                try {
                    Monitor.Enter(dbContext, ref lockWasTaken);

                    User user = dbContext.Users.Where(x => x.User_Id == userID).FirstOrDefault();

                    if (user != null) {
                        userDTO = ConvertUserToUserDTO(user);
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

                return userDTO;
            }
        }

        public UserDTO AddUser(UserDTO userDTO) {
            try {
                User user = ConvertUserDTOToUser(userDTO);
                user.Password = Encryption.Encrypt(user.Password, encryptionKey);

                using (RampUpProjectEntities dbContext = new RampUpProjectEntities(rampConnectionString)) {
                    dbContext.Users.Add(user);
                    dbContext.SaveChanges();
                }

                userDTO = ConvertUserToUserDTO(user);
                userDTO.Password = Encryption.Decrypt(userDTO.Password, encryptionKey);

                return userDTO;
            } catch (Exception) {
                throw;
            }
        }

        public void UpdateUser(UserDTO userDTO) {
            try {
                User user = ConvertUserDTOToUser(userDTO);
                user.Password = Encryption.Encrypt(user.Password, encryptionKey);

                using (RampUpProjectEntities dbContext = new RampUpProjectEntities(rampConnectionString)) {
                    if (user.Phones != null) {
                        foreach (Phone phone in user.Phones) {
                            if (phone.Phone_Id != 0) {
                                dbContext.Entry(phone).State = System.Data.Entity.EntityState.Modified;
                            } else {
                                dbContext.Entry(phone).State = System.Data.Entity.EntityState.Added;
                            }
                        }

                        List<int> phones = user.Phones.Select(y => y.Phone_Id).ToList();
                        List<Phone> toBeDeletedPhones = dbContext.Phones.Where(x => !phones.Contains(x.Phone_Id) && x.User_Id == userDTO.User_Id).ToList();

                        foreach (Phone phone in toBeDeletedPhones) {
                            dbContext.Entry(phone).State = System.Data.Entity.EntityState.Deleted;
                        }
                    }

                    if (user.Address != null) {
                        dbContext.Entry(user.Address).State = System.Data.Entity.EntityState.Modified;
                    }

                    dbContext.Users.Attach(user);
                    dbContext.Entry(user).State = System.Data.Entity.EntityState.Modified;

                    dbContext.SaveChanges();
                }
            } catch (Exception) {
                throw;
            }
        }

        private static User ConvertUserDTOToUser(UserDTO userDTO) {
            return new User {
                User_Id = userDTO.User_Id,
                Address_Id = userDTO.Address_Id,
                Creation_Date = userDTO.Creation_Date,
                Password = userDTO.Password,
                First_Name = userDTO.First_Name,
                Middle_Name = userDTO.Middle_Name,
                Last_Name = userDTO.Last_Name,
                Email = userDTO.Email,
                Phones = userDTO.Phones != null ? userDTO.Phones.Select(x => new Phone() { Phone_Id = x.Phone_Id, Branch_Id = x.Branch_Id, User_Id = x.User_Id, Phone_Number = x.Phone_Number, Phone_Type = x.Phone_Type }).ToList() : null,
                Status = userDTO.Status,
                Address = userDTO.Address != null ? new Address() { Address_Id = userDTO.Address_Id, Creation_Date = userDTO.Address.Creation_Date, First_Line = userDTO.Address.First_Line, Second_Line = userDTO.Address.Second_Line, City = userDTO.Address.City, State = userDTO.Address.State, Postal_Code = userDTO.Address.Postal_Code } : null
            };
        }

        private static UserDTO ConvertUserToUserDTO(User user) {
            return new UserDTO {
                User_Id = user.User_Id,
                Address_Id = user.Address_Id,
                Creation_Date = user.Creation_Date,
                Password = user.Password,
                First_Name = user.First_Name,
                Middle_Name = user.Middle_Name,
                Last_Name = user.Last_Name,
                Email = user.Email,
                Phones = user.Phones != null ? user.Phones.Select(x => new PhoneDTO() { Phone_Id = x.Phone_Id, Branch_Id = x.Branch_Id, User_Id = x.User_Id, Phone_Number = x.Phone_Number, Phone_Type = x.Phone_Type }).ToList() : null,
                Status = user.Status,
                Address = user.Address != null ? new AddressDTO() { Address_Id = user.Address_Id, Creation_Date = user.Address.Creation_Date, First_Line = user.Address.First_Line, Second_Line = user.Address.Second_Line, City = user.Address.City, State = user.Address.State, Postal_Code = user.Address.Postal_Code } : null
            };
        }
    }
}