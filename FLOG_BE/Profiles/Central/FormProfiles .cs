using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG_BE.Profiles.Central
{
    public class FormProfiles : Profile
    {
        public FormProfiles()
        {

            CreateMap<FLOG_BE.Model.Central.Entities.Form, Features.Central.SecurityRoles.Forms.GetFormGroup.Response>()
              .ForMember(dest => dest.FormGroup, opt => opt.MapFrom(src => src.Module));


            CreateMap<FLOG_BE.Model.Central.Entities.Form, Features.Central.SecurityRoles.Forms.GetForm.ReponseForms>()
              .ForMember(dest => dest.FormId, opt => opt.MapFrom(src => src.FormId))
              .ForMember(dest => dest.FormName, opt => opt.MapFrom(src => src.FormName))
              .ForMember(dest => dest.Module, opt => opt.MapFrom(src => src.Module));



        }
    }
}
