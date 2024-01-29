using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;
using WebApplication1.Data;
using WebApplication1.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContactsController : Controller
    {
        private readonly ContactEntity _dbContext;
        private readonly ILogger<ContactsController> _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        public ContactsController(ContactEntity dbContext, ILogger<ContactsController> logger, IHttpClientFactory httpClientFactory)
        {
            this._dbContext = dbContext;
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public async Task<IActionResult> GetContacts()
        {
            _logger.LogInformation("get api called ");
            return Ok(await _dbContext.Contact.ToListAsync());
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetContact([FromRoute] long id)
        {
            var contact = await _dbContext.Contact.FindAsync(id);

            if (contact == null)
            {
                return NotFound();
            }

            return Ok(contact);
        }

        [HttpGet]
        [Route("number/{number}")]
        public async Task<IActionResult> GetNumberInfo(int number)
        {
            using (var client = _httpClientFactory.CreateClient())
            {
                // Make a request to Numbers API
                var response = await client.GetStringAsync($"http://numbersapi.com/{number}");

                // Return the response from Numbers API
                return Ok(response);
            }
        }

        //[HttpGet]
        //[Route("byaddress/{address}")]
        //public async Task<IActionResult> GetContactsByAddress([FromRoute] string address)
        //{
        //    var contacts = await _dbContext.Contact.Where(c => c.Address == address).ToListAsync();

        //    if (contacts.Count == 0)
        //    {
        //        return NotFound($"No contacts found with the address: {address}");
        //    }

        //    return Ok(contacts);
        //}


        [HttpPost]
        public async Task<IActionResult> AddContacts(ContactRequestModel addContextRequest)
        {
            var contact = new ContactDetails()
            {
                Address = addContextRequest.Address,
                Email = addContextRequest.Email,
                FullName = addContextRequest.FullName,
                Phone = addContextRequest.Phone,
            };

            await _dbContext.Contact.AddAsync(contact);
            await _dbContext.SaveChangesAsync();

            return Ok(contact);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateContact([FromRoute] long id, UpdateContactRequestModel updateContextRequest)
        {
            var contact = await _dbContext.Contact.FindAsync(id);

            if (contact != null)
            {
                contact.FullName = updateContextRequest.FullName;
                contact.Address = updateContextRequest.Address;
                contact.Phone = updateContextRequest.Phone;
                contact.Email = updateContextRequest.Email;

                await _dbContext.SaveChangesAsync();

                return Ok(contact);
            }

            return NotFound();
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteContact([FromRoute] long id)
        {
            var contact = await _dbContext.Contact.FindAsync(id);
            _logger.LogInformation("delete api called ");
            if (contact != null)
            {
                _dbContext.Remove(contact);
                _logger.LogInformation("remove data succd ");
                await _dbContext.SaveChangesAsync();
                return Ok(contact);
            }
            _logger.LogInformation("Data not found in delete api");
            return NotFound();
        }
    }
}
