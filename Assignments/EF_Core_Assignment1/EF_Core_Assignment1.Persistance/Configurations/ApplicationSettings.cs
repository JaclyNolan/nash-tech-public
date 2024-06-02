using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF_Core_Assignment1.Persistance.Configurations
{
    public static class ApplicationSettings
    {
        public static BorrowingSettings BorrowingSettings { get; set; } = new BorrowingSettings();

        // other options here...
    }
}
