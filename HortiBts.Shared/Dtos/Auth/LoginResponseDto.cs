using System;
using System.Collections.Generic;
using System.Text;

namespace HortiBts.Shared.Dtos.Auth
{
    public record LoginResponseDto(
      string AccessToken,
      DateTime AccessTokenExpires,
      string RefreshToken,
      DateTime RefreshTokenExpires,
      string UserId,
      string Role,
      string UsernameEn,
      string UsernameHi);
}
