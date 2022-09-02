using AutoMapper;
using MCS.Email.Domain.Contracts.Models;
using MCS.Email.Web.Contracts.Models;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCS.Email.Infrastructure.ServiceMapping
{
    public class ServiceToWebProfile: Profile
    {
        public ServiceToWebProfile()
        {
            CreateMap<EmailWeb, EmailInfo>(MemberList.None)
                .ForMember(info => info.Content, x => x.MapFrom(web => web.Content))
                .ForMember(info => info.Subject, x => x.MapFrom(web => web.Subject));                
        }
    }
}
