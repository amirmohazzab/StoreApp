using AutoMapper;
using StoreApp.Domain.Entities;
using StoreApp.Domain.Entities.User;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Dtos.UserProfile
{
    public class UserProfileDto
    {
        public string Id { get; set; }               
        public string? UserName { get; set; }        
        public string? FirstName { get; set; }       
        public string? LastName { get; set; }        
        public string? Email { get; set; }           
        public string? Number { get; set; }     
        public string? City { get; set; }           
        public string? State { get; set; }           
        public string? FullName => $"{FirstName} {LastName}".Trim();

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Address, UserProfileDto>();
        }
    }
}
