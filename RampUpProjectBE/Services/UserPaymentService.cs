using RampUpProjectBE.DAL;
using RampUpProjectBE.Models.DTOs;
using System;
using System.Linq;
using System.Threading;

namespace RampUpProjectBE.Services {
    public class UserPaymentService {
        private string rampConnectionString;

        public UserPaymentService(string rampConnectionString) {
            this.rampConnectionString = rampConnectionString;
        }

        public User_PaymentDTO GetUser_Payment(int user_paymentID) {
            bool lockWasTaken = false;
            User_PaymentDTO user_paymentDTO = null;

            using (RampUpProjectEntities dbContext = new RampUpProjectEntities(rampConnectionString)) {
                try {
                    Monitor.Enter(dbContext, ref lockWasTaken);

                    User_Payment user_payment = dbContext.User_Payment.Where(x => x.User_Payment_Id == user_paymentID).FirstOrDefault();

                    if (user_payment != null) {
                        user_paymentDTO = ConvertUser_PaymentToUser_PaymentDTO(user_payment);
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

                return user_paymentDTO;
            }
        }

        public int AddUser_Payment(User_PaymentDTO user_paymentDTO) {
            try {
                User_Payment user_payment = ConvertUser_PaymentDTOToUser_Payment(user_paymentDTO);

                using (RampUpProjectEntities dbContext = new RampUpProjectEntities(rampConnectionString)) {
                    dbContext.User_Payment.Add(user_payment);
                    dbContext.SaveChanges();
                }

                return user_payment.User_Payment_Id;
            } catch (Exception) {
                throw;
            }
        }

        public void RemoveUser_Payment(int user_paymentID) {
            try {
                User_Payment user_payment = new User_Payment() { User_Payment_Id = user_paymentID };

                using (RampUpProjectEntities dbContext = new RampUpProjectEntities(rampConnectionString)) {
                    dbContext.User_Payment.Attach(user_payment);
                    dbContext.User_Payment.Remove(user_payment);
                    dbContext.SaveChanges();
                }
            } catch (Exception) {
                throw;
            }
        }

        private static User_Payment ConvertUser_PaymentDTOToUser_Payment(User_PaymentDTO user_paymentDTO) {
            return new User_Payment {
                User_Payment_Id = user_paymentDTO.User_Payment_Id,
                Card_Id = user_paymentDTO.Card_Id,
                Creation_Date = user_paymentDTO.Creation_Date,
                Payment_Method_Ref_Id = user_paymentDTO.Payment_Method_Ref_Id,
                PayPal_Id = user_paymentDTO.PayPal_Id,
                Card = user_paymentDTO.Card != null ? new Card() { Card_Id = user_paymentDTO.Card.Card_Id, Card_Number = user_paymentDTO.Card.Card_Number, Creation_Date = user_paymentDTO.Card.Creation_Date, CVC = user_paymentDTO.Card.CVC, Expiration_Date = user_paymentDTO.Card.Expiration_Date, User_Id = user_paymentDTO.Card.User_Id } : null,
                Payment_Method_Ref = user_paymentDTO.Payment_Method_Ref != null ? new Payment_Method_Ref() { Payment_Method_Ref_Id = user_paymentDTO.Payment_Method_Ref.Payment_Method_Ref_Id, Payment_Method = user_paymentDTO.Payment_Method_Ref.Payment_Method } : null
            };
        }

        private static User_PaymentDTO ConvertUser_PaymentToUser_PaymentDTO(User_Payment user_payment) {
            return new User_PaymentDTO {
                User_Payment_Id = user_payment.User_Payment_Id,
                Card_Id = user_payment.Card_Id,
                Creation_Date = user_payment.Creation_Date,
                Payment_Method_Ref_Id = user_payment.Payment_Method_Ref_Id,
                PayPal_Id = user_payment.PayPal_Id,
                Card = user_payment.Card != null ? new CardDTO() { Card_Id = user_payment.Card.Card_Id, Card_Number = user_payment.Card.Card_Number, Creation_Date = user_payment.Card.Creation_Date, CVC = user_payment.Card.CVC, Expiration_Date = user_payment.Card.Expiration_Date, User_Id = user_payment.Card.User_Id } : null,
                Payment_Method_Ref = user_payment.Payment_Method_Ref != null ? new Payment_Method_RefDTO() { Payment_Method_Ref_Id = user_payment.Payment_Method_Ref.Payment_Method_Ref_Id, Payment_Method = user_payment.Payment_Method_Ref.Payment_Method } : null
            };
        }
    }
}