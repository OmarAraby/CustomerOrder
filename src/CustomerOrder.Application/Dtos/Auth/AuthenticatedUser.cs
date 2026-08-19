using System.Collections.Generic;

namespace CustomerOrder.Application.Dtos.Auth
{
    public class AuthenticatedUser
    {
        public string UserName { get; set; }
        public IReadOnlyList<string> Roles { get; set; } = new List<string>();
    }
}
