using RampUpProjectBE.Models.DTOs;
using RampUpProjectBE.Services;
using System.Web.Http;
using System.Web.Http.Filters;

namespace RampUpProjectBE.Controllers {
    [RoutePrefix("api/User_Payment")]
    public class UserPaymentController : ApiController {
        private UserPaymentService reservationSvc;

        public UserPaymentController(UserPaymentService reservationSvc) {
            this.reservationSvc = reservationSvc;
        }

        [HttpPost]
        [Route("AddUser_Payment")]
        public IHttpActionResult AddUser_Payment([FromBody]User_PaymentDTO reservationDTO) {
            if (reservationDTO == null) {
                return BadRequest("User_PaymentDTO cannot be null");
            }

            int reservationID = reservationSvc.AddUser_Payment(reservationDTO);
            return Ok(reservationID);
        }

        [HttpGet]
        [Route("GetUser_Payment/{reservationID}")]
        public IHttpActionResult GetUser_Payment(int? reservationID) {
            if (reservationID == null || reservationID == 0) {
                return BadRequest("User_PaymentID cannot be null");
            }

            User_PaymentDTO reservation = reservationSvc.GetUser_Payment((int)reservationID);
            return Ok(reservation);
        }

        [HttpDelete]
        [Route("RemoveUser_Payment/{reservationID}")]
        public IHttpActionResult RemoveUser_Payment(int? reservationID) {
            if (reservationID == null || reservationID == 0) {
                return BadRequest("User_PaymentID cannot be null");
            }

            reservationSvc.RemoveUser_Payment((int)reservationID);
            return Ok();
        }
    }
}
