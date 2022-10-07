using CCW.UserProfile.Models;
using CCW.UserProfile.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text;
using User = CCW.UserProfile.Models.User;

namespace CCW.UserProfile.Controllers;

[ApiController]
[Route("/api/v1/[controller]")]
public class UserController : ControllerBase
{
    private readonly ICosmosDbService _cosmosDbService;

    private readonly ILogger<UserController> _logger;

    public UserController(ICosmosDbService cosmosDbService, ILogger<UserController> logger)
    {
        _cosmosDbService = cosmosDbService ?? throw new ArgumentNullException(nameof(cosmosDbService));
        _logger = logger;
    }

    [Route("verifyEmail")]
    [HttpPost]
    public HttpResponseMessage Post(string userEmail)
    {
        try
        {
            var user = _cosmosDbService.GetAsync(userEmail, cancellationToken: default);

            return (user.Result != null!) ? new HttpResponseMessage(HttpStatusCode.OK) : new HttpResponseMessage(HttpStatusCode.NotFound);
        }
        catch (Exception e)
        {
            _logger.LogWarning($"An error occur trying to verify email: {e.Message}");

            return new HttpResponseMessage(HttpStatusCode.ExpectationFailed)
            {
                Content = new StringContent("An error occur.", Encoding.UTF8, "application/json")
            };
        }
    }

    [Route("create")]
    [HttpPut]
    public async Task<User> Create([FromBody] Email email)
    {
        try
        {
            var user = _cosmosDbService.GetAsync(email.EmailAddress, cancellationToken: default);

            if (user.Result != null)
            {
                _logger.LogWarning($"Email address already exists: {email.EmailAddress}");
                throw new ArgumentException("Email address already exists.");
            }

            User newUser = new User
            {
                Id = Guid.NewGuid().ToString(),
                Email = email.EmailAddress,
            };

            var createdUser = await _cosmosDbService.AddAsync(newUser);

            return createdUser;

        }
        catch (Exception e)
        {
            _logger.LogWarning($"An error occur while trying to create new user: {e.Message}");

            throw new Exception("An error occur while trying to create new user.");
        }
    }

    //[Route("{id}/delete")]
    //[HttpDelete]
    //public async Task<IActionResult> Delete(string id)
    //{
    //    await _cosmosDbService.DeleteAsync(id);
    //    return NoContent();
    //}
}
