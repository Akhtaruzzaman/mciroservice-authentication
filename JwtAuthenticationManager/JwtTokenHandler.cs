using JwtAuthenticationManager.Model;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JwtAuthenticationManager
{
    public class JwtTokenHandler
    {
        public const string JWT_SECUTIRY_KEY = "ouexUdkOlskjJJhndY84JJksq22zxXmYd0093kdMdARjuKhtARManUzzaolL";
        private const int JWT_TOKEN_VALIDITY_MINS = 20;
        private readonly List<UserAccount> _userAccountList;
        public JwtTokenHandler()
        {
            _userAccountList = new List<UserAccount> {
                new UserAccount { UserName ="admin01", Password="admin123", Role = "Admin"},
                new UserAccount { UserName ="user01", Password="user123", Role = "User"},
            };
        }
        public List<UserAccount> GetUserAccount()
        {
            return _userAccountList;
        }
        public bool PostUserAccount(UserAccount userAccount)
        {
            _userAccountList.Add(userAccount);
            return true;
        }
        public AuthenticationResponse? GenerateToken(AuthenticationRequest authenticationRequest)
        {
            if (string.IsNullOrWhiteSpace(authenticationRequest.UserName) || string.IsNullOrWhiteSpace(authenticationRequest.Password))
                return null;

            // validation
            var useracc = _userAccountList.Where(_ => _.UserName.Equals(authenticationRequest.UserName) && _.Password.Equals(authenticationRequest.Password)).FirstOrDefault();
            if (useracc == null)
                return null;

            var tokenExpired = DateTime.Now.AddMinutes(JWT_TOKEN_VALIDITY_MINS);
            var tokenKey = Encoding.ASCII.GetBytes(JWT_SECUTIRY_KEY);
            var claimsIdentity = new ClaimsIdentity(new List<Claim> {
                new Claim(JwtRegisteredClaimNames.Name , authenticationRequest.UserName),
                new Claim(ClaimTypes.Role , useracc.Role),
            });
            var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature);
            var securityTokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claimsIdentity,
                Expires = tokenExpired,
                SigningCredentials = signingCredentials
            };
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var securityToken = jwtSecurityTokenHandler.CreateToken(securityTokenDescriptor);
            var token = jwtSecurityTokenHandler.WriteToken(securityToken);

            return new AuthenticationResponse
            {
                UserName = authenticationRequest.UserName,
                ExpiresIn = (int)tokenExpired.Subtract(DateTime.Now).TotalSeconds,
                Token = token,
            };
        }
    }
}
