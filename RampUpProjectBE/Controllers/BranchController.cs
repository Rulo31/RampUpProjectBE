using RampUpProjectBE.Filters;
using RampUpProjectBE.Models.DTOs;
using RampUpProjectBE.Services;
using System.Collections.Generic;
using System.Web.Http;

namespace RampUpProjectBE.Controllers {
    [ExceptionFilter]
    [ApiAuthenticationFilter]
    [RoutePrefix("api/Branch")]
    public class BranchController : ApiController {
        private BranchService branchSvc;

        public BranchController(BranchService branchSvc) {
            this.branchSvc = branchSvc;
        }

        [HttpPost]
        [Route("AddBranch")]
        public IHttpActionResult AddBranch([FromBody]BranchDTO branchDTO) {
            if (branchDTO == null) {
                return BadRequest("BranchDTO cannot be null");
            }

            branchDTO = branchSvc.AddBranch(branchDTO);
            return Ok(branchDTO);
        }

        [HttpPost]
        [Route("AddBranches")]
        public IHttpActionResult AddBranches([FromBody]List<BranchDTO> branches) {
            if (branches == null) {
                return BadRequest("BranchDTOs cannot be null");
            }

            branches = branchSvc.AddBranches(branches);
            return Ok(branches);
        }

        [HttpPut]
        [Route("UpdateBranch")]
        public IHttpActionResult UpdateBranch([FromBody]BranchDTO branchDTO) {
            if (branchDTO == null) {
                return BadRequest("BranchDTO cannot be null");
            }

            branchSvc.UpdateBranch(branchDTO);
            return Ok();
        }

        [HttpGet]
        [Route("GetBranch/{branchID}")]
        public IHttpActionResult GetBranch(int? branchID) {
            if (branchID == null || branchID == 0) {
                return BadRequest("BranchID cannot be null");
            }

            BranchDTO branch = branchSvc.GetBranch((int)branchID);
            return Ok(branch);
        }

        [HttpGet]
        [Route("GetBranchesByBusinessId/{businessID}")]
        public IHttpActionResult GetBranchesByBusinessId(int? businessID) {
            if (businessID == null || businessID == 0) {
                return BadRequest("BusinessID cannot be null");
            }

            List<BranchDTO> branch = branchSvc.GetBranchesByBusinessId((int)businessID);
            return Ok(branch);
        }

        [HttpDelete]
        [Route("RemoveBranch/{branchID}")]
        public IHttpActionResult RemoveBranch(int? branchID) {
            if (branchID == null || branchID == 0) {
                return BadRequest("BranchID cannot be null");
            }

            branchSvc.RemoveBranch((int)branchID);
            return Ok();
        }
    }
}
