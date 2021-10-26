using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserApiSql.Interfaces;
using UserApiSql.Models;
using UserApiSql.ModelsDTO;

namespace UserApiSql.Data
{
    public class DocumentRepository : IRepository<Document,InputDocument>
    {
        private readonly UserContext _context;

        public DocumentRepository(UserContext context)
        {
            _context = context;
        }

        public async Task<Document> Create(InputDocument documentInput)
        {
            Document document = new Document();
            document.Name = documentInput.Name;
            document.Type = documentInput.Type;
            document.Status = documentInput.Status;
            document.Content = documentInput.Content;
            document.UploadedDate = DateTime.Now;
            document.UpdatedBy = _context.Users.FirstOrDefault(i => i.Id == documentInput.UpdaterId);


            await _context.Documents.AddAsync(document);
            await _context.SaveChangesAsync();
            return document;
        }

        public async Task Delete(int id)
        {
            Document documentToDelete = await _context.Documents.FirstOrDefaultAsync(i => i.Id == id);
            _context.Documents.Remove(documentToDelete);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Document>> Get()
        {
            IEnumerable<Document> document = await _context.Documents
                .Include(u => u.UpdatedBy)
                .ToListAsync();

            return document;
        }

        public async Task<Document> Get(int id)
        {
            Document document = await _context.Documents
                .Include(u => u.UpdatedBy)
                .FirstOrDefaultAsync(i => i.Id == id);
            return document;
        }

        public async Task<Document> Update(int id, InputDocument documentInput)
        {
            Document document = new Document();
            document.Id = id;
            document.Name = documentInput.Name;
            document.Type = documentInput.Type;
            document.Status = documentInput.Status;
            document.Content = documentInput.Content;
            document.UploadedDate = DateTime.Now;
            document.UpdatedBy = await _context.Users.FirstOrDefaultAsync(i => i.Id == id);

            _context.Entry(document).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return document;
        }
    }
}
