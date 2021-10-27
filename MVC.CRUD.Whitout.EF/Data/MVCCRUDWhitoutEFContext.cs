using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MVC.CRUD.Whitout.EF.Models;

namespace MVC.CRUD.Whitout.EF.Data
{
    public class MVCCRUDWhitoutEFContext : DbContext
    {
        public MVCCRUDWhitoutEFContext (DbContextOptions<MVCCRUDWhitoutEFContext> options)
            : base(options)
        {
        }

        public DbSet<MVC.CRUD.Whitout.EF.Models.BookViewModel> BookViewModel { get; set; }
    }
}
