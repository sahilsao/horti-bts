using HortiBts.Shared.Enums.Auth;
using System;
using System.Collections.Generic;
using System.Text;

namespace HortiBts.Shared.Dtos.Auth
{
    public class LoginRequestDto
    {
        public LoginType LoginType { get; set; }

        public string UserId { get; set; } = "";

        public string Password { get; set; } = "";

        public bool ForceLogout { get; init; } = false;
    }
}