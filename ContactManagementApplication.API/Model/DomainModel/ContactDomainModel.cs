using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContactManagementApplication.API.Model.DomainModel
{
    public class ContactDomainModel
    {
        public decimal Id { get; set; } = 0;
        public string Name { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Address { get; set; }
        public decimal CategoryId { get; set; }
    }
}
