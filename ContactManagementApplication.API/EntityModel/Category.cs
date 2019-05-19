using System;
using System.Collections.Generic;

namespace ContactManagementApplication.API.EntityModel
{
    public partial class Category
    {
        public decimal Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
