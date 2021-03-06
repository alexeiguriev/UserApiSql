using AutoMapper;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UserApiSql.Models;
using UserApiSql.ModelsDTO;

namespace UserApiSql.Helper
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<User, UserDTO>()
                .ForMember(d => d.Roles, opt => opt.MapFrom(s => Helper.GetRoleIdsFromUserRoleList(s.UserRoles)));
            
            CreateMap<Document, DocumentDTO>()
                .ForMember(d => d.UpdatedByUserId, opt => opt.MapFrom(s => s.UpdatedBy.Id));

            CreateMap<Noti, NotiDTO>()
                .ForMember(d => d.FromUserId, opt => opt.MapFrom(s => s.FromUser.Id))
                .ForMember(d => d.ToUserId, opt => opt.MapFrom(s => s.ToUser.Id))
                .ForMember(d => d.FromUserName, opt => opt.MapFrom(s => $"{s.FromUser.FirstName} {s.FromUser.LastName}"))
                .ForMember(d => d.ToUserName, opt => opt.MapFrom(s => $"{s.ToUser.FirstName} {s.ToUser.LastName}"));

            CreateMap<(string str, IFormFile file), InputDocument>()
                .ForMember(d => d.UpdaterId, opt => opt.MapFrom(s => Newtonsoft.Json.JsonConvert.DeserializeObject<InputDocument>(s.str).UpdaterId))
                .ForMember(d => d.Status, opt => opt.MapFrom(s => Newtonsoft.Json.JsonConvert.DeserializeObject<InputDocument>(s.str).Status))
                .ForMember(d => d.Name, opt => opt.MapFrom(s => s.file.FileName))
                .ForMember(d => d.Type, opt => opt.MapFrom(s => s.file.ContentType))
                .ForMember(d => d.Content, opt => opt.MapFrom(s => Helper.PdfToByteBuffConverter(s.file)));

            CreateMap<Role, RoleDTO>()
                .ForMember(d => d.ReachedByUserIds, opt => opt.MapFrom(u => Helper.GetUserIdsFromUserRoleList(u.UserRoles)));
        }
    }
}
