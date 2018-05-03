using RampUpProjectBE.DAL;
using RampUpProjectBE.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace RampUpProjectBE.Services {
    public class PaymentMethodService {
        private string rampConnectionString;

        public PaymentMethodService(string rampConnectionString) {
            this.rampConnectionString = rampConnectionString;
        }

        public List<Payment_Method_RefDTO> GetPaymentMethods() {
            bool lockWasTaken = false;
            List<Payment_Method_RefDTO> paymentMethodsDTO = null;

            using (RampUpProjectEntities dbContext = new RampUpProjectEntities(rampConnectionString)) {
                try {
                    Monitor.Enter(dbContext, ref lockWasTaken);

                    paymentMethodsDTO = dbContext.Payment_Method_Ref.Select(x => new Payment_Method_RefDTO() { Payment_Method_Ref_Id = x.Payment_Method_Ref_Id, Payment_Method = x.Payment_Method }).ToList();

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

                return paymentMethodsDTO;
            }
        }
    }
}