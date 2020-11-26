using Qnify.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Qnify.Service.Interface
{
    public interface IAuthService
    {
        User ValidateCredential(string username, string password);
    }
}
