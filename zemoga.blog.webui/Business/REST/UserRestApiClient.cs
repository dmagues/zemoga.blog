using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Threading.Tasks;
using zemoga.blog.webui.Business.DTO;
using zemoga.blog.webui.Models;
using zemoga.blog.webui.Runtime;

namespace zemoga.blog.webui.Business.REST
{
    public interface IUserRestApiClient
    {
        Task<UserModel> Login(string username, string password);
        Task Register(RegisterModel register);
    }
    public class UserRestApiClient: RestApiClientBase, IUserRestApiClient
    {
        public UserRestApiClient(IOptions<AppSettings> appSettings):base(appSettings) { }

        public async Task<UserModel> Login(string username, string password)
        {
            var response = await Get($"users/?userName={username}&password={password}");
            response.EnsureSuccessStatusCode();
            var userDtoString = await response.Content.ReadAsStringAsync();

            if (!string.IsNullOrEmpty(userDtoString))
            {
                var dto = JsonConvert.DeserializeObject<UserDTO>(userDtoString);
                return new UserModel()
                {
                    UserId = dto.UserId,
                    Username = dto.Username,
                    Password = dto.Password,
                    Roles = dto.Roles
                };
            }

            return null;
            
        }

        public async Task Register(RegisterModel register)
        {
            var registerDto = new UserRegisterDTO()
            {
                Username = register.Username,
                Password = register.Password,
                Roles = register.Roles.ToArray()
            };

            var json = JsonConvert.SerializeObject(registerDto);
            var response = await Post("users", json);
            response.EnsureSuccessStatusCode();
        }
    }
}
