using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UserApiSql.Interfaces;
using UserApiSql.Models;
using UserApiSql.ModelsDTO;

namespace UserApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uof;
        private readonly ILogger _logger;

        public DocumentController(IUnitOfWork uof, IMapper mapper, ILogger<DocumentController> logger)
        {
            _uof = uof;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetDocuments()
        {
            try
            {
                var documents = await _uof.DocumentRepository.Get();
                IEnumerable<DocumentDTO> documentDTO = _mapper.Map<IEnumerable<DocumentDTO>>(documents);
                _logger.LogInformation("Get all documents");
                return Ok(documentDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error: Get all documents error");
                return NoContent();
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDocuments(int id)
        {
            try
            {
                var document = await _uof.DocumentRepository.Get(id);
                DocumentDTO documentDTO = _mapper.Map<DocumentDTO>(document);
                _logger.LogInformation($"Get document: Name: {documentDTO.Name}");
                return Ok(documentDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"ERROR: Get document: id: { id }");
                return NoContent();
            }
        }

        [HttpPost]
        public async Task<IActionResult> PostDocuments([FromBody] InputDocument documentInput)
        {
            try
            {
                var newDocument = await _uof.DocumentRepository.Create(documentInput);
                DocumentDTO documentDTO = _mapper.Map<DocumentDTO>(newDocument);
                _logger.LogInformation($"Post document: Name: {documentDTO.Name} ");
                return Ok(documentDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"ERROR: Post document: Name: {documentInput.Name}");
                return NoContent();
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutDocuments(int id, [FromBody] InputDocument documentInput)
        {

            try
            {
                _logger.LogInformation($"Update document: Name: {documentInput.Name}");

                await _uof.DocumentRepository.Update(id,documentInput);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"ERROR: Update document: Name: {documentInput.Name}");
                return NoContent();
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {

            try
            {
                var documentToDelete = await _uof.DocumentRepository.Get(id);
                if (documentToDelete == null)
                    return NotFound();
                _logger.LogInformation($"Delete document: Name: {documentToDelete.Name}");

                await _uof.DocumentRepository.Delete((int)documentToDelete.Id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Delete document: id: { id }");
                return NoContent();
            }
        }
    }
}
