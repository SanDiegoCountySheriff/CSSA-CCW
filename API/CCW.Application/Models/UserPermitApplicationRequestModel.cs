using CCW.Application.Entities;

namespace CCW.Application.Models;

public class UserPermitApplicationRequestModel
{
    public UserApplication Application { get; set; }
    public Guid Id { get; set; }
}