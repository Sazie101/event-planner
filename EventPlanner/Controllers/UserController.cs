using DAL;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Entities.Models;
using Entities.DTOs;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using EventPlanner.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EventPlanner.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;

        public UserController(IUserRepository userService, IMapper mapper, ITokenService tokenService)
        {
            _userRepository = userService;
            _mapper = mapper;
            _tokenService = tokenService;
        }

        // GET: api/<UserController>
        [HttpGet("GetAll")]
        /*        [Authorize(Roles = "host")]
        */
        public async Task<IActionResult> Get()
        {
            var Users = await _userRepository.GetUsersRepo();
            List<UserDTO> userDTOs = _mapper.Map<List<UserDTO>>(Users);
            return Ok(userDTOs);
        }

        [HttpGet("GetAllHosts")]
        public async Task<IActionResult> GetHosts()
        {
            var hosts = await _userRepository.GetHosts();
            List<UserDTO> fixedHosts = _mapper.Map<List<UserDTO>>(hosts);
            return Ok(fixedHosts);
        }

        // GET api/<UserController>/5
        [HttpGet("GetBy{id}")]
        [Authorize(Roles = "host")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _userRepository.GetUserById(id);
            if (result == null)
            {
                return BadRequest();
            }
            UserDTO user = _mapper.Map<UserDTO>(result);
            return Ok(user);
        }

        // POST api/<UserController>
        [HttpPost("Register")]
        public async Task<IActionResult> Post(UserDTO userDTO)
        {
            User user = _mapper.Map<User>(userDTO);
            return Ok(await _userRepository.AddUser(user));
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(string userName, string password)
        {
            var result = await _userRepository.Login(userName, password);
            if (result != null)
            {
                var token = _tokenService.GenerateJwtToken(result);
                return Ok(new { token });
            }
            return Unauthorized("Invalid username or password.");
        }

        // PUT api/<UserController>/5
        [HttpPut("Update")]
        [Authorize(Roles = "host")]
        public async Task<IActionResult> UpdateUserProfile(int id, [FromBody] UserDTO updatedUser)
        {
            var userFound = await _userRepository.GetUserById(id);
            if (userFound == null)
                return NotFound("User not found.");
            var result = await _userRepository.UpdateUser(id, updatedUser);
            UserDTO updatedResult = _mapper.Map<UserDTO>(result);
            return Ok(updatedResult);
        }

        // DELETE api/<UserController>/5
        [HttpDelete("Delete")]
        [Authorize(Roles = "host")]
        public async Task<IActionResult> Delete(int id)
        {
            var userFound = await _userRepository.GetUserById(id);
            if (userFound == null)
                return NotFound("User not found.");
            var result = _userRepository.DeleteUser(id);
            return Ok(result);
        }

        [HttpPut("Make host")]
        [Authorize(Roles = "host")]
        public async Task<IActionResult> MakeUserHost(int id)
        {
            var userFound = await _userRepository.GetUserById(id);
            if (userFound == null)
                return NotFound("User not found.");
            var result = await _userRepository.BecomeHost(id);
            return Ok(result);
        }
    }
}