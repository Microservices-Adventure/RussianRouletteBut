using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authorization.Domain.Exceptions;

public class InvalidRegisterException : Exception
{
    public InvalidRegisterException(string? message) : base(message)
    {
    }
}
