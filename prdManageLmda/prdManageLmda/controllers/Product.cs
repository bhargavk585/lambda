using System;
using System.Collections.Generic;
using System.Text;

namespace prdManageLmda.controllers
{
    public sealed class Product
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string Shortdesc { get; set; }
        public string Detailedesc { get; set; }
        public string Category { get; set; }
        public string StartPrice { get; set; }
        public string Bidenddt { get; set; }
    }
}
