using RampUpProjectBE.Filters;
using RampUpProjectBE.Models.DTOs;
using RampUpProjectBE.Services;
using System.Collections.Generic;
using System.Web.Http;

namespace RampUpProjectBE.Controllers {
    [ExceptionFilter]
    [ApiAuthenticationFilter]
    [RoutePrefix("api/Business")]
    public class BusinessController : ApiController {
        private BusinessService businessSvc;

        public BusinessController(BusinessService businessSvc) {
            this.businessSvc = businessSvc;
        }

        [HttpPost]
        [Route("AddBusiness")]
        public IHttpActionResult AddBusiness([FromBody]BusinessDTO businessDTO) {
            if (businessDTO == null) {
                return BadRequest("BusinessDTO cannot be null");
            }

            int businessID = businessSvc.AddBusiness(businessDTO);
            return Ok(businessID);
        }

        [HttpPut]
        [Route("UpdateBusiness")]
        public IHttpActionResult UpdateBusiness([FromBody]BusinessDTO businessDTO) {
            if (businessDTO == null) {
                return BadRequest("BusinessDTO cannot be null");
            }

            businessSvc.UpdateBusiness(businessDTO);
            return Ok();
        }

        [HttpGet]
        [Route("GetBusiness/{businessID}")]
        public IHttpActionResult GetBusiness(int? businessID) {
            if (businessID == null || businessID == 0) {
                return BadRequest("BusinessID cannot be null");
            }

            BusinessDTO business = businessSvc.GetBusiness((int)businessID);
            return Ok(business);
        }

        [HttpGet]
        [Route("GetMyBusinesses/{userID}")]
        public IHttpActionResult GetMyBusinesses(int? userID) {
            if (userID == null || userID == 0) {
                return BadRequest("userID cannot be null");
            }

            List<BusinessDTO> businesses = businessSvc.GetMyBusinesses((int)userID);
            return Ok(businesses);
        }

        [HttpDelete]
        [Route("RemoveBusiness/{businessID}")]
        public IHttpActionResult RemoveBusiness(int? businessID) {
            if (businessID == null || businessID == 0) {
                return BadRequest("BusinessID cannot be null");
            }

            businessSvc.RemoveBusiness((int)businessID);
            return Ok();
        }
    }
}
