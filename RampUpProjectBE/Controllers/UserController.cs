using RampUpProjectBE.Filters;
using RampUpProjectBE.Models.DTOs;
using RampUpProjectBE.Services;
using RampUpProjectBE.Utils;
using System;
using System.Web.Http;

namespace RampUpProjectBE.Controllers {
    [ExceptionFilter]
    [RoutePrefix("api/User")]
    public class UserController : ApiController {
        private UserService userSvc;

        public UserController(UserService userSvc) {
            this.userSvc = userSvc;
        }

        [HttpPost]
        [Route("Login")]
        public IHttpActionResult Login([FromBody]UserDTO userDTO) {
            if (userDTO == null) {
                return BadRequest("UserDTO cannot be null");
            }
            if (string.IsNullOrWhiteSpace(userDTO.Email)) {
                return BadRequest("User Email cannot be null or empty");
            }
            if (string.IsNullOrWhiteSpace(userDTO.Password)) {
                return BadRequest("User Password cannot be null or empty");
            }

            userDTO = userSvc.Login(userDTO.Email, userDTO.Password.ConvertToSecureString());

            if (userDTO != null) {
                return Ok(userDTO);
            } else {
                return Unauthorized();
            }
        }

        [HttpPost]
        [Route("IsUserRegistered")]
        public IHttpActionResult IsUserRegistered([FromBody]UserDTO userDTO) {
            if (userDTO == null) {
                return BadRequest("UserDTO cannot be null");
            }
            if (string.IsNullOrWhiteSpace(userDTO.Email)) {
                return BadRequest("User Email cannot be null or empty");
            }

            bool isUserRegistered = userSvc.IsUserRegistered(userDTO.Email);

            return Ok(isUserRegistered);
        }

        [HttpPost]
        [Route("AddUser")]
        public IHttpActionResult AddUser([FromBody]UserDTO userDTO) {
            if (userDTO == null) {
                return BadRequest("UserDTO cannot be null");
            }

            userDTO = userSvc.AddUser(userDTO);
            return Ok(userDTO);
        }

        [ApiAuthenticationFilter]
        [HttpPut]
        [Route("UpdateUser")]
        public IHttpActionResult UpdateUser([FromBody]UserDTO userDTO) {
            if (userDTO == null) {
                return BadRequest("UserDTO cannot be null");
            }

            userSvc.UpdateUser(userDTO);
            return Ok();
        }

        [ApiAuthenticationFilter]
        [HttpGet]
        [Route("GetUser/{userID}")]
        public IHttpActionResult GetUser(int? userID) {
            if (userID == null || userID == 0) {
                return BadRequest("UserID cannot be null");
            }

            UserDTO user = userSvc.GetUser((int)userID);
            return Ok(user);
        }
    }
}
