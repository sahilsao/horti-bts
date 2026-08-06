using HortiBts.Shared.Enums.Auth;
using System;
using System.Collections.Generic;
using System.Text;

namespace HortiBts.Shared.Dtos.Auth
{
    public class RefreshTokenRequestDto
    {
        public string RefreshToken { get; set; } = string.Empty;
    }
}
