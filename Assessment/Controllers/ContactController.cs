using Assessment.Attributes;
using Assessment.Interfaces;
using Assessment.Models.Requests;
using Microsoft.AspNetCore.Mvc;

namespace Assessment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private readonly IContactRepository _contactRepository;

        private readonly ILogger<ContactController> _logger;

        public ContactController(IContactRepository contactRepository, ILogger<ContactController> logger)
        {
            _contactRepository = contactRepository;
            _logger = logger;
        }

        [Cached()]
        [HttpGet("Contacts")]
        public async Task<IActionResult> GetContacts()
        {
            var result = await _contactRepository.GetContacts();

            _logger.LogInformation($"Retrieved {result.Count()} contacts");

            return Ok(result);  
        }

        [Cached(isUpdate:true)]
        [HttpPost("Contact")]
        public async Task<IActionResult> SaveContact([FromBody]ContactRequest contactRequest)
        {
            _logger.LogInformation($"Creating new contact with name {contactRequest.Name}");

            if (await _contactRepository.Exists(contactRequest.Name))
                return BadRequest($"Contact with name '{contactRequest.Name}' already exist");

            await _contactRepository.AddContact(contactRequest);

            _logger.LogInformation($"Created new contact with name {contactRequest.Name}");

            return Ok();
        }

        [Cached(isUpdate: true)]
        [HttpPut("Contact/{id:guid}")]
        public async Task<IActionResult> UpdateContact(Guid id, [FromBody] ContactUpdate contactUpdate)
        {
            _logger.LogInformation($"Updating contact with id {id}");

            await _contactRepository.UpdateContact(contactUpdate, id);

            _logger.LogInformation($"Updated contact with id {id}");

            return Ok();
        }

        [Cached(isUpdate: true)]
        [HttpDelete("Contact/{id:guid}")]
        public async Task<IActionResult> DeleteContact(Guid id)
        {
            _logger.LogInformation($"Deleting contact with id {id}");

            await _contactRepository.DeleteContact(id);

            _logger.LogInformation($"Deleted contact with id {id}");

            return Ok();
        }
    }
}
