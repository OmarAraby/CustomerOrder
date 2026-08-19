using System;

namespace CustomerOrder.Application.Dtos.Auth
{
    public class TokenDto
    {
        public string AccessToken { get; set; }
        public string TokenType { get; set; } = "Bearer";
        public DateTime ExpiresAtUtc { get; set; }
        public string UserName { get; set; }
    }
}
