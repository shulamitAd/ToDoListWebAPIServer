using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPIServer.CustomExceptions;
using WebAPIServer.Models;
using WebAPIServer.Repositories;
using WebAPIServer.Services;

namespace WebAPIServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TODOController : ControllerBase
    {
        private readonly IToDoService _iToDoService;
        public TODOController(IToDoService iToDoService) 
        { 
            _iToDoService = iToDoService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            IActionResult actionResult = null;
            try
            {
                var items = await _iToDoService.GetAll();
                 actionResult = Ok(items);
            }
            catch (Exception ex)
            {
                actionResult = StatusCode(500, ex.Message);
            }
            return actionResult;
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] TODOItem item)
        {
            IActionResult actionResult = null;
            try
            {
                await _iToDoService.Add(item);
                actionResult = Ok();
            }
            catch(ArgumentException ex)
            {
                actionResult = StatusCode(400, ex.Message);
            }
            catch (Exception ex)
            {
                actionResult = StatusCode(500, ex.Message);
            }
            return actionResult;
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            IActionResult actionResult = null;
            try
            {
                var item = await _iToDoService.Get(id);
                actionResult = Ok(item);
            }
            catch (ItemNotFoundException ex)
            {
                actionResult = StatusCode(404, ex.Message);
            }
            catch (Exception ex)
            {
                actionResult = StatusCode(500, ex.Message);
            }
            return actionResult;

        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, TODOItem updateToDoRequest)
        {
            IActionResult actionResult = null;
            try
            {
                await _iToDoService.Update(id, updateToDoRequest);
                actionResult = Ok();
            }
            catch (ItemNotFoundException ex)
            {
                actionResult = StatusCode(404, ex.Message);
            }
            catch (ArgumentException ex)
            {
                actionResult = StatusCode(400, ex.Message);
            }
            catch (Exception ex)
            {
                actionResult = StatusCode(500, ex.Message);
            }
            return actionResult;
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            IActionResult actionResult = null;
            try
            {
                await _iToDoService.Delete(id);
                actionResult = Ok();
            }
            catch (ItemNotFoundException ex)
            {
                actionResult = StatusCode(404, ex.Message);
            }
            catch (Exception ex)
            {
                actionResult = StatusCode(500, ex.Message);
            }
            return actionResult;
        }
    }
}
