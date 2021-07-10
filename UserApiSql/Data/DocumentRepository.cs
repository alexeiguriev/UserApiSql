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

        public Document Create(InputDocument documentInput)
        {
            Document document = new Document();
            document.Name = documentInput.Name;
            document.Type = documentInput.Type;
            document.Status = documentInput.Status;
            document.UploadedDate = DateTime.Now;
            document.UpdatedBy = _context.Users.FirstOrDefault(i => i.Id == documentInput.UpdaterId);


            _context.Documents.Add(document);
            _context.SaveChanges();
            return document;
        }

        public void Delete(int id)
        {
            Document documentToDelete = _context.Documents.FirstOrDefault(i => i.Id == id);
            _context.Documents.Remove(documentToDelete);
            _context.SaveChanges();
        }

        public IEnumerable<Document> Get()
        {
            IEnumerable<User> user = _context.Users.ToList();
            IEnumerable<Document> document = _context.Documents.ToList();

            return document;
        }

        public Document Get(int id)
        {
            Document document = _context.Documents.FirstOrDefault(i => i.Id == id);
            return document;
        }

        public void Update(int id, InputDocument documentInput)
        {
            Document document = new Document();
            document.Id = id;
            document.Name = documentInput.Name;
            document.Type = documentInput.Type;
            document.Status = documentInput.Status;
            document.UploadedDate = DateTime.Now;
            document.UpdatedBy = _context.Users.FirstOrDefault(i => i.Id == id);

            _context.Entry(document).State = EntityState.Modified;
            _context.SaveChanges();
        }
    }
}
