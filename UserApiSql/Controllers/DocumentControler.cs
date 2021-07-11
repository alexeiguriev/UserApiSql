﻿using System;
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
                // Get all the documents from database
                var documents = await _uof.DocumentRepository.Get();

                // Data map convertion
                IEnumerable<DocumentDTO> documentDTO = _mapper.Map<IEnumerable<DocumentDTO>>(documents);

                // Log the information about all documents getting.
                _logger.LogInformation("Get all documents");

                //Return all the documents
                return Ok(documentDTO);
            }
            catch (Exception ex)
            {
                // Log the error information
                _logger.LogError(ex, "Error: Get all documents error");

                // Return error
                return NotFound(404);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDocuments(int id)
        {
            try
            {
                // Get the documents from database
                var document = await _uof.DocumentRepository.Get(id);

                // Data map convertion
                DocumentDTO documentDTO = _mapper.Map<DocumentDTO>(document);

                // Log the information about document storage
                _logger.LogInformation($"Get document: Name: {documentDTO.Name}");

                //Return the document
                return Ok(documentDTO);
            }
            catch (Exception ex)
            {
                // Log the error information
                _logger.LogError(ex, $"ERROR: Get document: id: { id }");

                // Return error
                return NotFound(404);
            }
        }

        [HttpPost]
        public async Task<IActionResult> PostDocuments([FromBody] InputDocument documentInput)
        {
            try
            {
                // Put on db new document
                var newDocument = await _uof.DocumentRepository.Create(documentInput);

                // Map data convertion
                DocumentDTO documentDTO = _mapper.Map<DocumentDTO>(newDocument);

                // Log the post data information
                _logger.LogInformation($"Post document: Name: {documentDTO.Name} ");

                //Return statuc
                return Ok(documentDTO);
            }
            catch (Exception ex)
            {
                // Log the error information
                _logger.LogError(ex, $"ERROR: Post document: Name: {documentInput.Name}");

                // Return error
                return NotFound(404);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutDocuments(int id, [FromBody] InputDocument documentInput)
        {

            try
            {
                // Update the document.
                await _uof.DocumentRepository.Update(id,documentInput);

                // Log the information
                _logger.LogInformation($"Update document: Name: {documentInput.Name}");
                
                //Return statuc
                return Ok("Document " + documentInput.Name +" updated");
            }
            catch (Exception ex)
            {
                // Log the error information
                _logger.LogError(ex, $"ERROR: Update document: Name: {documentInput.Name}");

                // Return error
                return NotFound(404);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {

            try
            {
                // Get the document according ID
                var documentToDelete = await _uof.DocumentRepository.Get(id);

                // Check if document exists
                if (documentToDelete == null)
                {
                    // Log about document with this ID not found
                    _logger.LogInformation($"Not found document with this ID");

                    // Return not found
                    return NotFound();
                }
                
                // Delete the document from database
                await _uof.DocumentRepository.Delete((int)documentToDelete.Id);

                // Log information about deleted document
                _logger.LogInformation($"Delete document: Name: {documentToDelete.Name}");

                // Return information about deleted document
                return Ok("Document " + documentToDelete.Name + " deleted");
            }
            catch (Exception ex)
            {
                // Log information about delete error
                _logger.LogError(ex, $"Delete document: id: { id }");

                // Return not fond.
                return NotFound(404);
            }
        }
    }
}
