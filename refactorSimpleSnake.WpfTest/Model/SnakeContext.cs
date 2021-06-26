using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace refactorSimpleSnake.WpfTest.Model
{
    public class SnakeContext:DbContext
    {
        public DbSet<Record> Records { get; set; }
        public SnakeContext()
        {
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("server=localdb; database=snakedb; trusted_connection=true;");
        }
    }
}
