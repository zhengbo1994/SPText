using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SPCoreApiText.Utiltiy
{
    public interface IJWTService
    {
        string GetToken(string UserName);
    }

    /// <summary>
    /// 备注下：代码演示的是对称加密，所以只有一个key，在返回的信息里面是没有的
    ///         PPT介绍时，说的是非对称的，那样是把解密key公开的，前面是后台用私钥加密的，
    /// 可以保证别人解密后 拿到的数据  跟前面2部分hash后的结果一致 保证没有篡改
    ///  此外，公钥不是在返回结果，那只是个打比方~
    /// </summary>
    public class JWTService : IJWTService
    {
        private readonly IConfiguration _configuration;
        public JWTService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetToken(string UserName)
        {
            Claim[] claims = new[]
            {
               new Claim(ClaimTypes.Name, UserName),
               new Claim("NickName","Richard"),
               new Claim("Role","Administrator"),//传递其他信息  
               new Claim("abc","abccc")
               //new Claim
            };
            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["SecurityKey"]));
            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            /**
             *  Claims (Payload)
                Claims 部分包含了一些跟这个 token 有关的重要信息。 JWT 标准规定了一些字段，下面节选一些字段: 
                iss: The issuer of the token，token 是给谁的
                sub: The subject of the token，token 主题
                exp: Expiration Time。 token 过期时间，Unix 时间戳格式
                iat: Issued At。 token 创建时间， Unix 时间戳格式
                jti: JWT ID。针对当前 token 的唯一标识
                除了规定的字段外，可以包含其他任何 JSON 兼容的字段。
             * */
            var token = new JwtSecurityToken(
                issuer: _configuration["issuer"],
                audience: _configuration["audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(5),//5分钟有效期
                signingCredentials: creds);
            string returnToken = new JwtSecurityTokenHandler().WriteToken(token);
            return returnToken;
        }
    }
}
