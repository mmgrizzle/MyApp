using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyApp.Application.Students.Commands;
using MyApp.Application.Students.Models;
using MyApp.Application.Students.Queries;
using MyApp.WebApi.Infrastructure;
using MyApp.WebApi.Services;

namespace MyApp.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class StudentsController : BaseController
    {
        private readonly IMyAppHttpClient _myAppHttpClient;

        public StudentsController(IMyAppHttpClient myAppHttpClient)
        {
            _myAppHttpClient = myAppHttpClient;
        }

        public async Task<bool> Validation()
        {
            //gets user info about authorization from the UserInfoEndpoint
            var discoveryClient = new DiscoveryClient("https://localhost:44357/");
            var metaDataResponse = await discoveryClient.GetAsync();
            var userInfoClient = new UserInfoClient(metaDataResponse.UserInfoEndpoint);
            var accessToken = await HttpContext
                .GetTokenAsync(OpenIdConnectParameterNames.AccessToken);

            //response from user endpoint
            var userResponse = await userInfoClient.GetAsync(accessToken);

            if (userResponse.IsError)  //error handling for user info endpoint access
            {
                throw new Exception(
                    "Problem accessing the UserInfo endpoint."
                    , userResponse.Exception);
            }
        }

        // GET: api/Students
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public Task<StudentListViewModel> GetStudents()
        {
            return Mediator.Send(new GetAllStudentsQuery());
        }

        // GET: api/Students/5
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(StudentViewModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetStudent(int id)
        {
            return Ok(await Mediator.Send(new GetStudentQuery(id)));
        }

        // POST: api/Students
        [HttpPost]
        [ProducesResponseType(typeof(StudentViewModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> PostStudent([FromBody] CreateStudentCommand command)
        {
            var viewModel = await Mediator.Send(command);

            return CreatedAtAction("GetStudents", new { id = viewModel.Student.StudentId, viewModel });
        }

        // PUT: api/Students/5
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(StudentDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> PutStudent(
           [FromRoute] int id,
           [FromBody] UpdateStudentCommand command)
        {
            if(id != command.Student.StudentId)
            {
                return BadRequest();
            }
            return Ok(await Mediator.Send(command));
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            await Mediator.Send(new DeleteStudentCommand(id));

            return NoContent();
        }
    }
}
