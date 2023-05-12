using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs.User;
using Core.Commands.User;
using Core.Models.User;
using Core.Queries.User;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        // GET: api/User
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/User/5
        [HttpGet("{externalId}", Name = "Get")]
        public async Task<UserModel?> Get(string externalId)
        {
            Console.WriteLine($"Retrieving User by externalId: '{externalId}'");
            var userModel = await _mediator.Send(new GetUserQuery(externalId));
            if (userModel == null)
            {
                Response.StatusCode = 404;
            }
            return userModel;
        }

        [HttpPost ("Register")]
        public async Task<UserModel> Register([FromBody] UserRegistrationDto userRegistrationDto)
        {
            Console.WriteLine($"Registering User: ${userRegistrationDto.Email}");
            return await _mediator.Send(new RegisterUserCommand(userRegistrationDto.FirstName, userRegistrationDto.LastName, userRegistrationDto.Email, userRegistrationDto.ExternalId));
        }

        // PUT: api/User/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/User/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
