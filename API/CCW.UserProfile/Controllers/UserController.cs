using CCW.UserProfile.Models;
using CCW.UserProfile.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text;
using CCW.UserProfile.Mappers;
using User = CCW.UserProfile.Entities.User;

namespace CCW.UserProfile.Controllers;

[ApiController]
[Route("/Api/" + Constants.AppName + "/v1/[controller]")]
public class UserController : ControllerBase
{
    private readonly ICosmosDbService _cosmosDbService;
    private readonly IMapper<UserProfileRequestModel, User> _requestMapper;
    private readonly IMapper<User, UserProfileResponseModel> _responseMapper;
    private readonly ILogger<UserController> _logger;

    public UserController(
        ICosmosDbService cosmosDbService,
        IMapper<UserProfileRequestModel, User> requestMapper,
        IMapper<User, UserProfileResponseModel> responseMapper,
        ILogger<UserController> logger)
    {
        _cosmosDbService = cosmosDbService ?? throw new ArgumentNullException(nameof(cosmosDbService));
        _requestMapper = requestMapper;
        _responseMapper = responseMapper;
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

    [Route("verifyObjectId")]
    [HttpPut]
    public HttpResponseMessage Put(string id)
    {
        try
        {
            var user = _cosmosDbService.GetAsync(id, cancellationToken: default);

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
    public async Task<UserProfileResponseModel> Create([FromBody] UserProfileRequestModel email)
    {
        try
        {
            var user = _cosmosDbService.GetAsync(email.EmailAddress, cancellationToken: default);

            if (user.Result != null)
            {
                _logger.LogWarning($"Email address already exists: {email.EmailAddress}");
                throw new ArgumentException("Email address already exists.");
            }

            User newUser = _requestMapper.Map(email); //mapper adds the new guid id
            var createdUser = await _cosmosDbService.AddAsync(newUser, cancellationToken:default);

            return _responseMapper.Map(createdUser);

        }
        catch (ArgumentException e)
        {
            _logger.LogWarning($"Email address already exists: {e.Message}");
            throw new Exception("Email address already exists.");
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
