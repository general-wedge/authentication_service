using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using authentication_service.Models;
using authentication_service.Repositories;
using authentication_service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace authentication_service.Controllers
{
    [Authorize("Bearer")]
    [Produces("application/json")]
    [Route("api/v1/[controller]")]
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            this._userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository)); // Guard to ensure user service isn't null
        }

        // GET: api/v1/User
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<User>), StatusCodes.Status200OK)]
        public IActionResult GetAll()
        {
            try
            {
                //var allUsers = _userService.GetAll();
                IEnumerable<UserViewModel> users = AutoMapper.Mapper.Map<IEnumerable<User>, IEnumerable<UserViewModel>>(_userRepository.GetAll());
                return Ok(users);
            }
            catch (NullReferenceException ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }

        }

        // GET: api/v1/User/5
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(id);
                return Ok(user);
            }
            catch (ArgumentNullException)
            {
                return NotFound();
            }

        }

        // POST: api/User
        [HttpPost]
        [ProducesResponseType(typeof(User), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateUserAsync([FromBody]User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdUser = await _userRepository.CreateUserAsync(user);
            return CreatedAtAction(
                nameof(GetByIdAsync),
                new { id = createdUser.Id },
                createdUser
            );
        }

        // PUT: api/User/5
        [HttpPut]
        [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateUserAsync(int id, [FromBody]User user)
        {
            try
            {
                var updatedUser = await _userRepository.UpdateUserAsync(id, user);
                return Ok(updatedUser);
            }
            catch (ArgumentNullException)
            {
                return NotFound();
            }
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteUserAsync(int id)
        {
            try
            {
                var deletedUser = await _userRepository.DeleteUserAsync(id);
                return Ok(deletedUser);
            }
            catch (ArgumentNullException)
            {
                return NotFound();
            }
        }
    }
}