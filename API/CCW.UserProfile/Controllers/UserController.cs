using AutoMapper;
using CCW.Common.RequestModels;
using CCW.Common.ResponseModels;
using CCW.UserProfile.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using User = CCW.Common.Models.User;

namespace CCW.UserProfile.Controllers;


[ApiController]
[Route(Constants.AppName + "/v1/[controller]")]
public class UserController : ControllerBase
{
    private readonly ICosmosDbService _cosmosDbService;
    private readonly IMapper _mapper;
    private readonly ILogger<UserController> _logger;

    public UserController(
        ICosmosDbService cosmosDbService,
        IMapper mapper,
        ILogger<UserController> logger)
    {
        _cosmosDbService = cosmosDbService ?? throw new ArgumentNullException(nameof(cosmosDbService));
        _mapper = mapper;
        _logger = logger;
    }

    [Authorize(Policy = "B2CUsers")]
    [Authorize(Policy = "AADUsers")]
    [Route("verifyEmail")]
    [HttpPost]
    public IActionResult Post()
    {
        GetUserId(out var userId);

        try
        {
            var user = _cosmosDbService.GetAsync(userId, cancellationToken: default);

            return (user.Result != null!) ? Ok() : NotFound();
        }
        catch (Exception e)
        {
            var originalException = e.GetBaseException();
            _logger.LogError(originalException, originalException.Message);
            return NotFound("An error occur trying to verify email.");
        }
    }


    [Authorize(Policy = "B2CUsers")]
    [Authorize(Policy = "AADUsers")]
    [Route("create")]
    [HttpPut]
    public async Task<IActionResult> Create([FromBody] UserProfileRequestModel request)
    {
        GetUserId(out var userId);

        try
        {
            var newUser = _mapper.Map<User>(request);
            newUser.Id = userId;
            var createdUser = await _cosmosDbService.AddAsync(newUser, cancellationToken: default);

            return Ok(_mapper.Map<UserProfileResponseModel>(createdUser));

        }
        catch (Exception e)
        {
            var originalException = e.GetBaseException();
            _logger.LogError(originalException, originalException.Message);
            return NotFound("An error occur while trying to create new user.");
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
