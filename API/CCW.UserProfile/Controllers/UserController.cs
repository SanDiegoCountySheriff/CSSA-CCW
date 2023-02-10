using CCW.UserProfile.Models;
using CCW.UserProfile.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text;
using CCW.UserProfile.Mappers;
using Microsoft.AspNetCore.Authorization;
using User = CCW.UserProfile.Entities.User;


namespace CCW.UserProfile.Controllers;


[ApiController]
[Route(Constants.AppName + "/v1/[controller]")]
public class UserController : ControllerBase
{
    private readonly ICosmosDbService _cosmosDbService;
    private readonly IMapper<string, UserProfileRequestModel, User> _requestMapper;
    private readonly IMapper<User, UserProfileResponseModel> _responseMapper;
    private readonly ILogger<UserController> _logger;

    public UserController(
        ICosmosDbService cosmosDbService,
        IMapper<string, UserProfileRequestModel, User> requestMapper,
        IMapper<User, UserProfileResponseModel> responseMapper,
        ILogger<UserController> logger)
    {
        _cosmosDbService = cosmosDbService ?? throw new ArgumentNullException(nameof(cosmosDbService));
        _requestMapper = requestMapper;
        _responseMapper = responseMapper;
        _logger = logger;
    }

    [Authorize(Policy = "B2CUsers")]
    [Authorize(Policy = "AADUsers")]
    [Route("verifyEmail")]
    [HttpPost]
    public HttpResponseMessage Post(string userEmail)
    {

        GetUserId(out var userId);

        try
        {
            var user = _cosmosDbService.GetAsync(userId, cancellationToken: default);

            return (user.Result != null!) ? new HttpResponseMessage(HttpStatusCode.OK) : new HttpResponseMessage(HttpStatusCode.NotFound);
        }
        catch (Exception e)
        {
            var originalException = e.GetBaseException();
            _logger.LogError(originalException, originalException.Message);
            return new HttpResponseMessage(HttpStatusCode.ExpectationFailed)
            {
                Content = new StringContent("An error occur trying to verify email.", Encoding.UTF8, "application/json")
            };
        }
    }


    [Authorize(Policy = "B2CUsers")]
    [Authorize(Policy = "AADUsers")]
    [Route("create")]
    [HttpPut]
    public async Task<UserProfileResponseModel> Create([FromBody] UserProfileRequestModel request)
    {
        GetUserId(out var userId);

        try
        {
            User newUser = _requestMapper.Map(userId, request); //user id comes from the token
            var createdUser = await _cosmosDbService.AddAsync(newUser, cancellationToken:default);

            return _responseMapper.Map(createdUser);

        }
        catch (Exception e)
        {
            var originalException = e.GetBaseException();
            _logger.LogError(originalException, originalException.Message);
            throw new Exception("An error occur while trying to create new user.");
        }
    }

    private void GetUserId(out string? userId)
    {
        userId = this.HttpContext.User.Claims
            .Where(c => c.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier")
            .Select(c => c.Value).FirstOrDefault();

        if (userId == null)
        {
            throw new ArgumentNullException("userId", "Invalid token.");
        }
    }
}
