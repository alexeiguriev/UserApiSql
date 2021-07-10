using AutoMapper;
using System;
using System.Collections.Generic;
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
            //CreateMap<User, UserDTO>();
            CreateMap<User, UserDTO>()
                .ForMember(d => d.FirstName, opt => opt.MapFrom(s => s.FirstName))
                .ForMember(d => d.LastName, opt => opt.MapFrom(s => s.LastName))
                .ForMember(d => d.Roles, opt => opt.MapFrom(s => s.UserRoles.Select(x => x.Role.Name)));
            //CreateMap<Document, DocumentDTO>();
            CreateMap<Document, DocumentDTO>()
                .ForMember(d => d.UpdatedBy, opt => opt.MapFrom(s => s.UpdatedBy.FirstName + " " + s.UpdatedBy.LastName));
        }
    }
}
