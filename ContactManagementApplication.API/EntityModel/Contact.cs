using System;
using System.Collections.Generic;

namespace ContactManagementApplication.API.EntityModel
{
    public partial class Contact
    {
        public decimal Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Address { get; set; }
        public byte[] ImageData { get; set; }
        public string ImageMimeType { get; set; }
        public decimal CategoryId { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
