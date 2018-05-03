using RampUpProjectBE.Filters;
using RampUpProjectBE.Models.DTOs;
using RampUpProjectBE.Services;
using System.Collections.Generic;
using System.Web.Http;

namespace RampUpProjectBE.Controllers {
    [ExceptionFilter]
    [ApiAuthenticationFilter]
    [RoutePrefix("api/Field")]
    public class FieldController : ApiController {
        private FieldService fieldSvc;

        public FieldController(FieldService fieldSvc) {
            this.fieldSvc = fieldSvc;
        }

        [HttpPost]
        [Route("AddField")]
        public IHttpActionResult AddField([FromBody]FieldDTO fieldDTO) {
            if (fieldDTO == null) {
                return BadRequest("FieldDTO cannot be null");
            }

            fieldDTO = fieldSvc.AddField(fieldDTO);
            return Ok(fieldDTO);
        }

        [HttpPut]
        [Route("UpdateField")]
        public IHttpActionResult UpdateField([FromBody]FieldDTO fieldDTO) {
            if (fieldDTO == null) {
                return BadRequest("FieldDTO cannot be null");
            }

            fieldSvc.UpdateField(fieldDTO);
            return Ok();
        }

        [HttpGet]
        [Route("GetFieldsByBranchId/{branchID}")]
        public IHttpActionResult GetFieldsByBranchId(int? branchID) {
            if (branchID == null || branchID == 0) {
                return BadRequest("branchID cannot be null");
            }

            List<FieldDTO> field = fieldSvc.GetFieldsByBranchId((int)branchID);
            return Ok(field);
        }

        [HttpGet]
        [Route("GetAllFields/")]
        public IHttpActionResult GetAllFields() {
            List<FieldResultDTO> field = fieldSvc.GetAllFields();
            return Ok(field);
        }

        [HttpDelete]
        [Route("RemoveField/{fieldID}")]
        public IHttpActionResult RemoveField(int? fieldID) {
            if (fieldID == null || fieldID == 0) {
                return BadRequest("FieldID cannot be null");
            }

            fieldSvc.RemoveField((int)fieldID);
            return Ok();
        }
    }
}
