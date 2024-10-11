using MediatR;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Profiling;
using StudentApp.Application.CQRS.StudentCommandQuery.Command;
using StudentApp.Application.CQRS.StudentCommandQuery.Query;
using StudentApp.Infrastructure;

namespace StudentApp.API.Controllers
{
    public class StudentController : BaseController
    {
        private readonly IMediator mediator;

        public StudentController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        #region Commands

        [HttpPost]
        [Route("Create")]
        [AccessControl("create")]
        public async Task<IActionResult> Create(CreateStudentCommand createStudentCommand)
        {
            var result = await mediator.Send(createStudentCommand);
            if (result.Status == Status.Success)
            {
                return Ok(result);
            }

            return BadRequest(result.Message);
        }

        [HttpPost]
        [Route("Edit")]
        [AccessControl("edit")]
        public async Task<IActionResult> Edit(UpdateStudentCommand updateStudentCommand)
        {
            var result = await mediator.Send(updateStudentCommand);
            if (result.Status == Status.Success)
            {
                return Ok(result);
            }

            return BadRequest(result.Message);
        }

        [HttpDelete("{id}")]
        [AccessControl("delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await mediator.Send(new DeleteStudentCommand { Id = id });
            if (result.Status == Status.Success)
            {
                return Ok(result);
            }

            return BadRequest(result.Message);
        }

        #endregion

        #region Query

        [HttpGet("{id}")]
        [AccessControl("get")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await mediator.Send(new GetByIdStudentQuery { Id = id });
            if (result.Status == Status.Success)
            {
                return Ok(result);
            }

            return BadRequest(result.Message);
        }

        [HttpGet]
        [Route("GetAll")]
        [AccessControl("get-all")]
        public async Task<IActionResult> GetAllStudents()
        {
            using (MiniProfiler.Current.Step("Get All Student"))
            {
                var result = await mediator.Send(new GetAllStudentsQuery());

                if (result.Status == Status.Success)
                {
                    return Ok(result);
                }

                return BadRequest(result.Message);
            }
        }

        #endregion
    }
}
