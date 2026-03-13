using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Interfaces;

public interface IEmailSender
{
    public Task SendEmailAsync(string email, string subject, string message);
}
