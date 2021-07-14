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
                .ForMember(d => d.FirstName, opt => opt.MapFrom(s => s.FirstName))
                .ForMember(d => d.LastName, opt => opt.MapFrom(s => s.LastName))
                .ForMember(d => d.Roles, opt => opt.MapFrom(s => s.UserRoles.Select(x => x.Role.Name)));
            
            CreateMap<Document, DocumentDTO>()
                .ForMember(d => d.UpdatedBy, opt => opt.MapFrom(s => s.UpdatedBy.FirstName + " " + s.UpdatedBy.LastName));

            CreateMap<(string str, IFormFile file), InputDocument>()
                .ForMember(d => d.UpdaterId, opt => opt.MapFrom(s => Newtonsoft.Json.JsonConvert.DeserializeObject<InputDocument>(s.str).UpdaterId))
                .ForMember(d => d.Status, opt => opt.MapFrom(s => Newtonsoft.Json.JsonConvert.DeserializeObject<InputDocument>(s.str).Status))
                .ForMember(d => d.Name, opt => opt.MapFrom(s => s.file.FileName))
                .ForMember(d => d.Type, opt => opt.MapFrom(s => s.file.ContentType))
                .ForMember(d => d.Content, opt => opt.MapFrom(s => Helper.PdfToByteBuffConverter(s.file)));

            CreateMap<Role, RoleDTO>()
                .ForMember(d => d.ReachedByUsers, opt => opt.MapFrom(u => Helper.ListOfUsersToUserString(u.UserRoles)));
        }
    }
}
