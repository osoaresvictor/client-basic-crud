using ClientBasicCrud.DTO;
using ClientBasicCrud.Model;
using ClientBasicCrud.Repository.Client;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace ClientBasicCrud.Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly IClientRepository clientRepository;
        public ClientsController(IClientRepository clientRepository)
        {
            this.clientRepository = clientRepository;
        }

        [HttpGet]

        [ProducesResponseType(typeof(IEnumerable<Client>), StatusCodes.Status200OK), Produces("application/json")]
        public IActionResult GetClients()
        {
            return Ok(this.clientRepository.ListClients());
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Client), StatusCodes.Status200OK), Produces("application/json")]
        public IActionResult GetClient([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var client = this.clientRepository.Get(id);

            if (client == null)
            {
                return NotFound();
            }

            return Ok(client);
        }

        [HttpPut("{id}")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent), Produces("application/json")]
        public IActionResult PutClient([FromRoute] int id, [FromBody] Client client)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != client.Id)
            {
                return BadRequest();
            }

            try
            {
                this.clientRepository.Update(client);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (this.clientRepository.IsClientExists(id) == false)
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Client), StatusCodes.Status201Created), Produces("application/json")]
        public IActionResult PostClient([FromBody] PostClientDTO client)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var newClient = this.clientRepository.Create(
                new Client
                {
                    Name = client.Name,
                    Birthdate = client.Birthdate,
                    RegistrationDate = client.RegistrationDate
                });

            return CreatedAtAction("GetClient", new { id = newClient.Id }, newClient);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(Client), StatusCodes.Status200OK), Produces("application/json")]
        public IActionResult DeleteClient([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var client = this.clientRepository.Delete(id);

            if (client == null)
            {
                return NotFound();
            }

            return Ok(client);
        }
    }
}