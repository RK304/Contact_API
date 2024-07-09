using AutoMapper;
using ContactsApi.DTOs;
using ContactsApi.Models;
using ContactsApi.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ContactsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactsController : ControllerBase
    {
        private readonly IContactRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<ContactsController> _logger;

        public ContactsController(IContactRepository repository, IMapper mapper, ILogger<ContactsController> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ContactDto>>> GetContacts()
        {
            _logger.LogInformation("Fetching all contacts");
            try
            {
                var contacts = await _repository.GetContactsAsync();
                var contactDtos = _mapper.Map<IEnumerable<ContactDto>>(contacts);
                return Ok(contactDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching contacts.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ContactDto>> GetContact(int id)
        {
            try
            {
                var contact = await _repository.GetContactByIdAsync(id);
                if (contact == null)
                {
                    _logger.LogWarning("Contact with ID {ContactId} not found.", id);
                    return NotFound();
                }

                var contactDto = _mapper.Map<ContactDto>(contact);
                return Ok(contactDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching the contact with ID {ContactId}.", id);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<ActionResult<ContactDto>> AddContact(ContactCreateDto contactCreateDto)
        {
            try
            {
                var contact = _mapper.Map<Contact>(contactCreateDto);
                await _repository.AddContactAsync(contact);

                var newContactDto = _mapper.Map<ContactDto>(contact);
                return CreatedAtAction(nameof(GetContact), new { id = newContactDto.Id }, newContactDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding a new contact.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateContact(int id, ContactDto contactDto)
        {
            if (id != contactDto.Id)
            {
                _logger.LogWarning("Contact ID mismatch for update operation.");
                return BadRequest();
            }

            try
            {
                var contact = _mapper.Map<Contact>(contactDto);
                await _repository.UpdateContactAsync(contact);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the contact with ID {ContactId}.", id);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContact(int id)
        {
            try
            {
                var contact = await _repository.GetContactByIdAsync(id);
                if (contact == null)
                {
                    _logger.LogWarning("Contact with ID {ContactId} not found.", id);
                    return NotFound();
                }

                await _repository.DeleteContactAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the contact with ID {ContactId}.", id);
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
