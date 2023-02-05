using System;
using System.Collections.Generic;

namespace ApiWithUnitTetsting.Entities
{
    public partial class UserInformation
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? City { get; set; }
        public string? Stat { get; set; }
        public string? Country { get; set; }
        public bool? Alive { get; set; }
    }
}
