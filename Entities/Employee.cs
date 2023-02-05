using System;
using System.Collections.Generic;

namespace ApiWithUnitTetsting.Entities
{
    public partial class Employee
    {
        public int Empid { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Address { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public bool? Alive { get; set; }
    }
}
