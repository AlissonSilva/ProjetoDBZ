using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoDBZ.Models.User
{
    public class Role
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        public ICollection<User> Users { get; set; } = new List<User>();

    }
}
