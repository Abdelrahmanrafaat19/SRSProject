using System;
using System.Collections.Generic;
using System.Text;

namespace SRSProject.Application.Contracts
{
    public interface IJwtCreator
    {
        public string CreateToken(string email, string userName, string id, IList<string>? Roles, CancellationToken cancellationToken = default!);
    }
}
