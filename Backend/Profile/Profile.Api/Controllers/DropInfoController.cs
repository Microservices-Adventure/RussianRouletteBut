using Microsoft.AspNetCore.Mvc;
using Profile.Api.Models;
using Profile.Api.Services;

namespace Profile.Api.Controllers
{
    [ApiController]
    [Route("api/dropinfo")]
    public class DropInfoController : ControllerBase
    {
        private readonly IDropInfoService _dropInfoService;

        public DropInfoController(IDropInfoService dropInfoService)
        {
            _dropInfoService = dropInfoService;
        }

        // GET: /api/dropinfo/getlist
        [HttpGet("getlist")]
        public async Task<IActionResult> GetDropInfos([FromQuery] GetDropInfoRequest request)
        {
            try
            {
                var (totalRecords, dropInfos) = await _dropInfoService.GetDropInfosAsync(request);

                return Ok(new
                {
                    TotalRecords = totalRecords,
                    DropInfos = dropInfos
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        // POST: /api/dropinfo/add
        [HttpPost("add")]
        public async Task<IActionResult> AddDropInfo([FromBody] AddDropInfoRequest request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest("DropInfo data is null.");
                }

                // Добавление DropInfo через сервис
                var dropInfo = await _dropInfoService.AddDropInfoAsync(request);

                // Возврат результата с кодом 201 (Created) и ссылкой на созданный ресурс
                return CreatedAtAction(nameof(GetDropInfos), new { id = dropInfo.Id }, dropInfo);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
    }
}