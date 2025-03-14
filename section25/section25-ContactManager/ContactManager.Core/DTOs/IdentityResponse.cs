using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactManager.Core.DTOs;

public class IdentityResponse
{
    public bool Succeeded { get; set; }
    public List<string>? Errors { get; set; }
}
