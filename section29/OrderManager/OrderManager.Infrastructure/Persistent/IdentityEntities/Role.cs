using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManager.Infrastructure.Persistent.IdentityEntities;

public class Role : IdentityRole<Guid>
{
    //public string Id { get; set; }
    //public string Name { get; set; }
}
