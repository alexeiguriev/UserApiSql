using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserApiSql.Interfaces;
using UserApiSql.Models;

namespace UserApiSql.Data
{
    public class NotiRepository : IRepository<Noti, NotiInput>
    {
        private readonly UserContext _context;
        public NotiRepository(UserContext context)
        {
            _context = context;
        }

        public async Task<Noti> Create(NotiInput entity)
        {
            Noti noti = new Noti()
            {
                FromUser = _context.Users.FirstOrDefault(i => i.Id == entity.FromUserId),
                ToUser = _context.Users.FirstOrDefault(i => i.Id == entity.ToUserId),
                NotiHeader = entity.NotiHeader,
                NotiBody = entity.NotiBody,
                IsRead = entity.IsRead,
                Url = entity.Url,
                CreatedDate = entity.CreatedDate,
                Message = entity.Message
            };

            await _context.Noti.AddAsync(noti);
            await _context.SaveChangesAsync();
            return noti;
        }

        public async Task Delete(int id)
        {
            Noti noti = await _context.Noti.FirstOrDefaultAsync(i => i.Id == id);
            _context.Noti.Remove(noti);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Noti>> Get()
        {
            IEnumerable<Noti> notis = await _context.Noti
                .Include(u => u.FromUser)
                .Include(u => u.ToUser)
                .ToListAsync();

            return notis;
        }

        public async Task<Noti> Get(int id)
        {
            Noti noti = await _context.Noti
                .Include(u => u.FromUser)
                .Include(u => u.ToUser)
                .FirstOrDefaultAsync(i => i.Id == id);

            return noti;
        }

        public async Task<Noti> Update(int id, NotiInput entity)
        {
            await UpdateNotiAvoidNulls(id, entity);

            Noti noti = await Get(id);

            return noti;
        }

        private async Task UpdateNotiAvoidNulls(int notiId, NotiInput notiInput)
        {
            Noti noti = _context.Noti.First(a => a.Id == notiId);
            if (notiInput.FromUserId != 0)
            {
                noti.FromUser = _context.Users.FirstOrDefault(i => i.Id == notiInput.FromUserId);
            }
            if (notiInput.ToUserId != 0)
            {
                noti.ToUser = _context.Users.FirstOrDefault(i => i.Id == notiInput.ToUserId);
            }
            if (!string.IsNullOrEmpty(notiInput.NotiHeader))
            {
                noti.NotiHeader = notiInput.NotiHeader;
            }
            if (!string.IsNullOrEmpty(notiInput.NotiBody))
            {
                noti.NotiBody = notiInput.NotiBody;
            }
            noti.IsRead = notiInput.IsRead;
            if (!string.IsNullOrEmpty(notiInput.Url))
            {
                noti.Url = notiInput.Url;
            }
            if (notiInput.CreatedDate != DateTime.MinValue)
            {
                noti.CreatedDate = notiInput.CreatedDate;
            }
            if (!string.IsNullOrEmpty(notiInput.Message))
            {
                noti.Message = notiInput.Message;
            }
            await _context.SaveChangesAsync();
        }
    }
}
