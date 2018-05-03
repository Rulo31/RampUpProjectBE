using RampUpProjectBE.Models.DTOs;
using RampUpProjectBE.Services;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Filters;

namespace RampUpProjectBE.Controllers {
    [RoutePrefix("api/PaymentMethod")]
    public class PaymentMethodController : ApiController {
        private PaymentMethodService paymentMethodSvc;

        public PaymentMethodController(PaymentMethodService paymentMethodSvc) {
            this.paymentMethodSvc = paymentMethodSvc;
        }

        [HttpGet]
        [Route("GetPaymentMethods")]
        public IHttpActionResult GetPaymentMethods() {
            List<Payment_Method_RefDTO> paymentMethodsDTO = paymentMethodSvc.GetPaymentMethods();
            return Ok(paymentMethodsDTO);
        }
    }
}
