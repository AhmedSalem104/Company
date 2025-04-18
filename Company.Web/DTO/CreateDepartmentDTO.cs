﻿using System.ComponentModel.DataAnnotations;

namespace Company.Web.DTO
{
    public class CreateDepartmentDTO
    {
        [Required(ErrorMessage = "Code Is Requierd")]
        public string Code { get; set; }

        [Required(ErrorMessage = "Name Is Requierd")]
        public string Name { get; set; }

        [Required(ErrorMessage = "CreateAt Is Requierd")]
        public DateTime CreateAt { get; set; }
    }
}
