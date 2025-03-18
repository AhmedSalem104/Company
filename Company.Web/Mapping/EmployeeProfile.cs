using AutoMapper;
using Company.Data.Models;
using Company.Web.DTO;

namespace Company.Web.Mapping
{
    public class EmployeeProfile : Profile
    {
        public EmployeeProfile() {

            CreateMap<CreateEmployeeDTO, Employee>().ReverseMap();
            //CreateMap<CreateEmployeeDTO, Employee>().ForMember(E=>E.Name,o=>o.MapFrom(s=>s.EmpName)).ReverseMap();
        }
    }
}
