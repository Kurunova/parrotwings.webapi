using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using BusinessLogicLayer.Exceptions;
using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using ParrotWings.WebAPI.Core.Models;

namespace ParrotWings.WebAPI.Core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [Route("[action]")]
        [HttpPost]
        public IActionResult Registration([FromBody]User user)
        {
            bool isRegistered = false;

            try
            {
                isRegistered = _userService.Register(user);
            }
            catch (ConditionException exception)
            {
                ModelState.AddModelError("General", exception.Message);
            }
            catch (Exception)
            {
                ModelState.AddModelError("General", "An error occured");
            }
            
            if(!isRegistered)
            {
                return BadRequest(ModelState);
            }

            var json = GetTokenJson(user.Name);

           return new OkObjectResult(json);
        }

        private string GetTokenJson(string name)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, name),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, "Test")
            };
            ClaimsIdentity identity = new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

            // создаем JWT-токен
            var jwt = new JwtSecurityToken(
                issuer: AuthOptions.ISSUER,
                audience: AuthOptions.AUDIENCE,
                notBefore: DateTime.UtcNow,
                claims: identity.Claims,
                expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new
            {
                access_token = encodedJwt,
                username = name
            };

            Response.ContentType = "application/json";
            var json = JsonConvert.SerializeObject(response, new JsonSerializerSettings { Formatting = Formatting.Indented });

            return json;
        }

        [Route("[action]")]
        [HttpPost]
        public IActionResult SignIn(SignInModel model)
        {
            if (!ModelState.IsValid)
            {
            }
            else if (!_userService.CanUserSignIn(model.Email, model.Password))
            {
                ModelState.AddModelError("General", "Incorrect login or password");
            }
            else
            {
                var user = _userService.GetUser(model.Email, model.Password);

                var json = GetTokenJson(user.Name);
                return new OkObjectResult(json);
            }

            return BadRequest(ModelState);
        }

        //[Authorize]
        //public ActionResult SignOut()
        //{
        //    return new OkResult();
        //}

        [Authorize]
        [Route("[action]")]
        [HttpGet]
        public IEnumerable<object> GetUserByPartOfName(string partOfName)
        {
            var userName = this.User.FindFirstValue(ClaimTypes.Name);
            var users = _userService.GetUserByPartOfName(partOfName, userName).ToList();
            return users.Select(p => new { label = p.Name });
        }

        [Authorize]
        [Route("[action]")]
        [HttpGet]
        public IEnumerable<object> GetUserTop(int count)
        {
            var userName = this.User.FindFirstValue(ClaimTypes.Name);
            var users = _userService.GetUserTop(count, userName).Select(p => p.Name).ToList();
            return users;
        }
    }
}