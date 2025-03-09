using System.ComponentModel.DataAnnotations;

namespace Company.Web.DTO
{
    public class DeleteDepartmentDTO
    {

        public int Id { get; set; }

  
        public string Code { get; set; }

   
        public string Name { get; set; }

     
        public DateTime CreateAt { get; set; }
    }
}
