using System;
using System.Collections.Generic;
using System.Text;

namespace HortiBts.Shared.Dtos.Auth
{
    public record ChangePasswordDto(string CurrentPassword, string NewPassword);
}
