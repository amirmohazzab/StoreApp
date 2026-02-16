using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Dtos.Admin.AdminContactMessage
{
    public class AdminReplyDto
    {
        public string Message { get; set; }
        public List<IFormFile>? Attachments { get; set; }
    }
}
