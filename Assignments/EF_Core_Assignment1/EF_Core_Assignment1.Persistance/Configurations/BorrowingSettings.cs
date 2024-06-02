using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF_Core_Assignment1.Persistance.Configurations
{
    public class BorrowingSettings
    {
        public int MaxBooksPerRequest { get; set; }
        public int MaxRequestsPerMonth { get; set; }
    }
}
