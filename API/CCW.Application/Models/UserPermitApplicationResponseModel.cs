using CCW.Application.Entities;

namespace CCW.Application.Models;

public class UserPermitApplicationResponseModel
{
    public UserApplication Application { get; set; }
    public Guid Id { get; set; }
}