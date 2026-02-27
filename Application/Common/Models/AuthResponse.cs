using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Models;

public class AuthResponse
{
    public TokenPair? Tokens { get; set; }
    public string? UserId { get; set; }
}
