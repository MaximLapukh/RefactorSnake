using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace refactorSimpleSnake.WpfTest.Model
{
    public class Record
    {
        [Key]
        public int Id { get; set; }
        public string Username { get; set; }
        public int Score { get; set; }
    }
}
