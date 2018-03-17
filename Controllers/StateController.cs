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
    public class StateController : Controller
    {
        private readonly IAcctStateRepository _acctStateRepository;

        public StateController(IAcctStateRepository acctStateRepository)
        {
            this._acctStateRepository = acctStateRepository ?? throw new ArgumentNullException(nameof(acctStateRepository)); // Guard to ensure user service isn't null
        }

        // GET: api/v1/state
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<AcctState>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                IEnumerable<AcctStateViewModel> states = AutoMapper.Mapper.Map<IEnumerable<AcctState>, IEnumerable<AcctStateViewModel>>(await _acctStateRepository.Get());
                return Ok(states);
            }
            catch (NullReferenceException ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }

        }

        // GET: api/v1/state/5
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(AcctState), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            try
            {
                var state = await _acctStateRepository.Get(id);
                return Ok(state);
            }
            catch (ArgumentNullException)
            {
                return NotFound();
            }

        }

        // POST: api/v1/state
        [HttpPost]
        [ProducesResponseType(typeof(AcctState), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateAsync([FromBody]AcctState state)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdState = await _acctStateRepository.Add(state);
            return CreatedAtAction(
                nameof(GetByIdAsync),
                new { id = createdState.AcctStateId },
                createdState
            );
        }

        // PUT: api/v1/state/5
        [HttpPut]
        [ProducesResponseType(typeof(AcctState), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateUserAsync([FromBody]AcctState state)
        {
            try
            {
                var updatedState = await _acctStateRepository.Update(state);
                return Ok(updatedState);
            }
            catch (ArgumentNullException)
            {
                return NotFound();
            }
        }

        // DELETE: api/v1/state/5
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(AcctState), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteUserAsync([FromBody] AcctState state)
        {
            try
            {
                var deletedState = await _acctStateRepository.Delete(state);
                return Ok(deletedState);
            }
            catch (ArgumentNullException)
            {
                return NotFound();
            }
        }
    }
}