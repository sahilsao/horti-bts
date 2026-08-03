using System;
using System.Collections.Generic;
using System.Text;

namespace HortiBts.Shared.Enums.Auth
{
    public enum PasswordUpdateResult
    {
        Success,
        UserNotFound,
        WrongCurrentPassword
    }
}
