using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using authentication_service.Models;
using authentication_service.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace authentication_service.Controllers
{
    [Authorize("Bearer")]
    [Produces("application/json")]
    [Route("api/v1/[controller]")]
    public class GroupController : Controller
    {
        private readonly IGroupRepository _groupRepository;

        public GroupController(IGroupRepository groupRepository)
        {
            this._groupRepository = groupRepository ?? throw new ArgumentNullException(nameof(groupRepository)); // Guard to ensure user service isn't null
        }

        // GET: api/v1/group
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<User>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                //var allUsers = _userService.GetAll();
                IEnumerable<GroupViewModel> groups = AutoMapper.Mapper.Map<IEnumerable<Group>, IEnumerable<GroupViewModel>>(await _groupRepository.Get());
                return Ok(groups);
            }
            catch (NullReferenceException ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }

        }

        // GET: api/v1/group/5
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            try
            {
                var group = await _groupRepository.Get(id);
                return Ok(group);
            }
            catch (ArgumentNullException)
            {
                return NotFound();
            }

        }

        // POST: api/v1/group
        [HttpPost]
        [ProducesResponseType(typeof(Group), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateUserAsync([FromBody]Group group)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdGroup = await _groupRepository.Add(group);
            return CreatedAtAction(
                nameof(GetByIdAsync),
                new { id = createdGroup.GroupId },
                createdGroup
            );
        }

        // PUT: api/v1/group/5
        [HttpPut]
        [ProducesResponseType(typeof(Group), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateUserAsync([FromBody]Group group)
        {
            try
            {
                var updatedGroup = await _groupRepository.Update(group);
                return Ok(updatedGroup);
            }
            catch (ArgumentNullException)
            {
                return NotFound();
            }
        }

        // DELETE: api/v1/group/5
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(Group), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteUserAsync([FromBody] Group group)
        {
            try
            {
                var deletedUser = await _groupRepository.Delete(group);
                return Ok(deletedUser);
            }
            catch (ArgumentNullException)
            {
                return NotFound();
            }
        }
    }
}