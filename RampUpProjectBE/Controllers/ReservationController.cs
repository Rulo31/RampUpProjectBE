using RampUpProjectBE.Filters;
using RampUpProjectBE.Models.DTOs;
using RampUpProjectBE.Services;
using System.Collections.Generic;
using System.Web.Http;

namespace RampUpProjectBE.Controllers {
    [ExceptionFilter]
    [ApiAuthenticationFilter]
    [RoutePrefix("api/Reservation")]
    public class ReservationController : ApiController {
        private ReservationService reservationSvc;

        public ReservationController(ReservationService reservationSvc) {
            this.reservationSvc = reservationSvc;
        }

        [HttpPost]
        [Route("AddReservation")]
        public IHttpActionResult AddReservation([FromBody]ReservationDTO reservationDTO) {
            if (reservationDTO == null) {
                return BadRequest("ReservationDTO cannot be null");
            }

            int reservationID = reservationSvc.AddReservation(reservationDTO);
            return Ok(reservationID);
        }

        [HttpGet]
        [Route("GetReservation/{reservationID}")]
        public IHttpActionResult GetReservation(int? reservationID) {
            if (reservationID == null || reservationID == 0) {
                return BadRequest("ReservationID cannot be null");
            }

            ReservationDTO reservation = reservationSvc.GetReservation((int)reservationID);
            return Ok(reservation);
        }

        [HttpGet]
        [Route("GetReservationsPerDay/{date}/{fieldID}")]
        public IHttpActionResult GetReservationsPerDay(string date, int? fieldID) {
            if (fieldID == null || fieldID == 0) {
                return BadRequest("fieldID cannot be null");
            }

            if (string.IsNullOrWhiteSpace(date)) {
                return BadRequest("date cannot be null");
            }

            List<ReservationDTO> reservations = reservationSvc.GetReservationsPerDay(date, (int)fieldID);
            return Ok(reservations);
        }

        [HttpGet]
        [Route("GetMyReservations/{userID}")]
        public IHttpActionResult GetMyReservations(int? userID) {
            if (userID == null || userID == 0) {
                return BadRequest("userID cannot be null");
            }

            List<ReservationDTO> reservations = reservationSvc.GetMyReservations((int)userID);
            return Ok(reservations);
        }

        [HttpDelete]
        [Route("RemoveReservation/{reservationID}")]
        public IHttpActionResult RemoveReservation(int? reservationID) {
            if (reservationID == null || reservationID == 0) {
                return BadRequest("ReservationID cannot be null");
            }

            reservationSvc.RemoveReservation((int)reservationID);
            return Ok();
        }
    }
}
