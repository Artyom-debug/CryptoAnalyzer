using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Auth;

public class AuthMessageSenderOptions
{
    public string? SendGridKey { get; set; }
    public string? FromEmail { get; set; }
    public string? FromName { get; set; }
}
