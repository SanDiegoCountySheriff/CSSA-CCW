using AutoMapper;
using CCW.Common.Models;
using CCW.Common.RequestModels;
using CCW.Common.ResponseModels;
using CCW.UserProfile.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CCW.UserProfile.Controllers;

[ApiController]
[Route(Constants.AppName + "/v1/[controller]")]
public class UserProfileController : ControllerBase
{
    private readonly ICosmosDbService _cosmosDbService;
    private readonly IMapper _mapper;
    private readonly ILogger<UserProfileController> _logger;

    public UserProfileController(
        ICosmosDbService cosmosDbService,
        IMapper mapper,
        ILogger<UserProfileController> logger)
    {
        _cosmosDbService = cosmosDbService;
        _mapper = mapper;
        _logger = logger;
    }

    [Authorize(Policy = "B2CUsers")]
    [Route("createUserProfile")]
    [HttpPut]
    public async Task<IActionResult> Put([FromBody] UserProfileRequestModel request)
    {
        try
        {
            GetUserId(out var userId);
            User newUser = _mapper.Map<User>(request);
            newUser.Id = userId;
            var userEmail = User.Claims.FirstOrDefault(c => c.Type == "emails")?.Value;
            newUser.Email = userEmail;
            var createdUser = await _cosmosDbService.AddUserAsync(newUser, cancellationToken: default);

            return Ok(_mapper.Map<UserProfileResponseModel>(createdUser));
        }
        catch (Exception e)
        {
            var originalException = e.GetBaseException();
            _logger.LogError(originalException, originalException.Message);
            return NotFound("An error occur while trying to create new user.");
        }
    }

    [Authorize(Policy = "B2CUsers")]
    [Route("getUserProfile")]
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        try
        {
            GetUserId(out var userId);
            var result = await _cosmosDbService.GetUserAsync(userId, cancellationToken: default);

            return (result != null) ? Ok(_mapper.Map<UserProfileResponseModel>(result)) : NotFound();
        }
        catch (Exception e)
        {
            var originalException = e.GetBaseException();
            _logger.LogError(originalException, originalException.Message);
            return NotFound("An error occur while trying to retrieve user.");
        }
    }

    private void GetUserId(out string userId)
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
