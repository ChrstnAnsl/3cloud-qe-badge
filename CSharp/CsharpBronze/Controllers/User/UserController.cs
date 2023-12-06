using Microsoft.AspNetCore.Mvc;
using CsharpBronze.Models.User;

namespace CsharpBronze.Controllers.User
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private static readonly List<UserModel> _userStorage = new();

        [HttpGet]
        public IActionResult GetAllUsers()
        {
            if (_userStorage.Count == 0)
            {
                return NotFound("No users found");
            }

            var response = new { users = _userStorage };
            return new ObjectResult(response);
        }

        [HttpGet("{userId}", Name = "GetUser")]
        public IActionResult GetUser(int userId)
        {
            var user = _userStorage.Find(u => u.UserId == userId);

            if (user == null)
            {
                return NotFound($"User with ID {userId} not found");
            }

            var response = new { user };
            return new ObjectResult(response);
        }

        [HttpPost]
        public IActionResult CreateUser([FromBody] UserModel userModel)
        {
            try
            {
                if (userModel == null)
                {
                    return BadRequest("Invalid user data. Please provide valid data.");
                }

                if (userModel.Password != userModel.PasswordConfirmed)
                {
                    return BadRequest("Passwords do not match. Please re-enter passwords.");
                }

                userModel.UserId = _userStorage.Count + 1;
                _userStorage.Add(userModel);

                return Ok(new { message = "User created successfully", user = userModel });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating user: {ex.Message}");
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }

        [HttpDelete("{userId}", Name = "DeleteUser")]
        public IActionResult DeleteUser(int userId)
        {
            var user = _userStorage.Find(u => u.UserId == userId);

            if (user == null)
            {
                return NotFound($"User with ID {userId} not found");
            }

            _userStorage.Remove(user);

            var response = new { user, message = $"Successfully deleted user with ID {userId}" };
            return new ObjectResult(response);
        }
    }
}