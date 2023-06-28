using CCW.UserProfile.Models;
using CCW.UserProfile.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using CCW.UserProfile.Mappers;
using CCW.UserProfile.Entities;

namespace CCW.UserProfile.Controllers;

[ApiController]
[Route(Constants.AppName + "/v1/[controller]")]
public class AdminUserController : ControllerBase
{
    private readonly ICosmosDbService _cosmosDbService;
    private readonly IMapper<string, AdminUserProfileRequestModel, AdminUser> _requestMapper;
    private readonly IMapper<AdminUser, AdminUserProfileResponseModel> _responseMapper;
    private readonly IMapper<IEnumerable<AdminUser>, IEnumerable<AdminUserProfileResponseModel>> _allUsersResponseModel;
    private readonly ILogger<AdminUserController> _logger;

    public AdminUserController(
        ICosmosDbService cosmosDbService, 
        ILogger<AdminUserController> logger,
        IMapper<string, AdminUserProfileRequestModel, AdminUser> requestMapper,
        IMapper<AdminUser, AdminUserProfileResponseModel> responseMapper,
        IMapper<IEnumerable<AdminUser>, IEnumerable<AdminUserProfileResponseModel>> allUsersResponseModel)
    {
        _cosmosDbService = cosmosDbService;
        _requestMapper = requestMapper;
        _responseMapper = responseMapper;
        _allUsersResponseModel = allUsersResponseModel;
        _logger = logger;
    }

    [Authorize(Policy = "AADUsers")]
    [Route("createAdminUser")]
    [HttpPut]
    public async Task<IActionResult> Put([FromBody] AdminUserProfileRequestModel request)
    {
        GetUserId(out var userId);

        try
        {
            AdminUser newUser = _requestMapper.Map(userId, request);
            var createdUser = await _cosmosDbService.AddAdminUserAsync(newUser, cancellationToken: default);

            return Ok(_responseMapper.Map(createdUser));

        }
        catch (Exception e)
        {
            var originalException = e.GetBaseException();
            _logger.LogError(originalException, originalException.Message);
            throw new Exception("An error occur while trying to create new user.");
        }
    }

    [Authorize(Policy = "AADUsers")]
    [Route("getAdminUser")]
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        GetUserId(out var userId);
        
        try
        {
            var result = await _cosmosDbService.GetAdminUserAsync(userId, cancellationToken: default);

            return (result != null) ? Ok(_responseMapper.Map(result)) : NotFound();
        }
        catch (Exception e)
        {
            var originalException = e.GetBaseException();
            _logger.LogError(originalException, originalException.Message);
            throw new Exception("An error occur while trying to retrieve admin user.");
        }
    }

    [Authorize(Policy = "RequireAdminOrSystemAdminOnly")]
    [Route("getAllAdminUsers")]
    [HttpGet]
    public async Task<IActionResult> GetAllAdminUsers()
    {
        try
        {
            var result = await _cosmosDbService.GetAllAdminUsers(cancellationToken: default);

            return (result != null) ? Ok(_allUsersResponseModel.Map(result)) : NotFound();
        }
        catch (Exception e)
        {
            var originalException = e.GetBaseException();
            _logger.LogError(originalException, originalException.Message);
            throw new Exception("An error occur while trying to retrieve admin user.");
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
