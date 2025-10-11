using AutoMapper;
using StoreApp.Application.Common.Mapping;
using StoreApp.Domain.Entities.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Dtos.Account
{
    public class UserDto : IMapFrom<User>
    {
        public string DisplayName { get; set; }

        public string Email { get; set; }

        public string UserName { get; set; }

        public string Token { get; set; }

        public string NationalCode { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<User, UserDto>().ReverseMap();
        }
    }
}
