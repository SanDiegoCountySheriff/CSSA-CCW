using AutoMapper;
using CCW.UserProfile.Entities;
using CCW.UserProfile.Models;
using CCW.UserProfile.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CCW.UserProfile.Controllers;

[ApiController]
[Route(Constants.AppName + "/v1/[controller]")]
public class AdminUserController : ControllerBase
{
    private readonly ICosmosDbService _cosmosDbService;
    private readonly IMapper _mapper;
    private readonly ILogger<AdminUserController> _logger;

    public AdminUserController(
        ICosmosDbService cosmosDbService,
        IMapper mapper,
        ILogger<AdminUserController> logger)
    {
        _cosmosDbService = cosmosDbService;
        _mapper = mapper;
        _logger = logger;
    }

    [Authorize(Policy = "AADUsers")]
    [Route("createAdminUser")]
    [HttpPut]
    public async Task<IActionResult> Put([FromBody] AdminUserProfileRequestModel request)
    {
        try
        {
            GetUserId(out var userId);
            AdminUser newUser = _mapper.Map<AdminUser>(request);
            newUser.Id = userId;
            var createdUser = await _cosmosDbService.AddAdminUserAsync(newUser, cancellationToken: default);

            return Ok(_mapper.Map<AdminUserProfileResponseModel>(createdUser));
        }
        catch (Exception e)
        {
            var originalException = e.GetBaseException();
            _logger.LogError(originalException, originalException.Message);
            return NotFound("An error occur while trying to create new user.");
        }
    }

    [Authorize(Policy = "AADUsers")]
    [Route("getAdminUser")]
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        try
        {
            GetUserId(out var userId);
            var result = await _cosmosDbService.GetAdminUserAsync(userId, cancellationToken: default);

            return (result != null) ? Ok(_mapper.Map<AdminUserProfileResponseModel>(result)) : NotFound();
        }
        catch (Exception e)
        {
            var originalException = e.GetBaseException();
            _logger.LogError(originalException, originalException.Message);
            return NotFound("An error occur while trying to retrieve admin user.");
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

            return (result != null) ? Ok(_mapper.Map<List<AdminUserProfileResponseModel>>(result)) : NotFound();
        }
        catch (Exception e)
        {
            var originalException = e.GetBaseException();
            _logger.LogError(originalException, originalException.Message);
            return NotFound("An error occur while trying to retrieve admin user.");
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
