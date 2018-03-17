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
    public class PermissionController : Controller
    {
        private readonly IPermissionRepository _permissionRepository;

        public PermissionController(IPermissionRepository permissionRepository)
        {
            this._permissionRepository = permissionRepository ?? throw new ArgumentNullException(nameof(permissionRepository)); // Guard to ensure user service isn't null
        }

        // GET: api/v1/permission
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Permission>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                //var allUsers = _userService.GetAll();
                IEnumerable<PermissionViewModel> permissions = AutoMapper.Mapper.Map<IEnumerable<Permission>, IEnumerable<PermissionViewModel>>(await _permissionRepository.Get());
                return Ok(permissions);
            }
            catch (NullReferenceException ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }

        }

        // GET: api/v1/group/5
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Permission), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            try
            {
                var permission = await _permissionRepository.Get(id);
                return Ok(permission);
            }
            catch (ArgumentNullException)
            {
                return NotFound();
            }

        }

        // POST: api/v1/group
        [HttpPost]
        [ProducesResponseType(typeof(Permission), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateUserAsync([FromBody]Permission permission)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdPermission = await _permissionRepository.Add(permission);
            return CreatedAtAction(
                nameof(GetByIdAsync),
                new { id = createdPermission.PermissionId },
                createdPermission
            );
        }

        // PUT: api/v1/group/5
        [HttpPut]
        [ProducesResponseType(typeof(Permission), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateUserAsync([FromBody]Permission permission)
        {
            try
            {
                var updatedPermission = await _permissionRepository.Update(permission);
                return Ok(updatedPermission);
            }
            catch (ArgumentNullException)
            {
                return NotFound();
            }
        }

        // DELETE: api/v1/group/5
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(Permission), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteUserAsync([FromBody] Permission permission)
        {
            try
            {
                var deletedPermission = await _permissionRepository.Delete(permission);
                return Ok(deletedPermission);
            }
            catch (ArgumentNullException)
            {
                return NotFound();
            }
        }
    }
}