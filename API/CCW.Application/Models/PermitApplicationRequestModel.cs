
using CCW.Application.Entities;

namespace CCW.Application.Models;

public class PermitApplicationRequestModel
{

    public Entities.Application Application { get; set; }

    public Guid Id { get; set; }
    public History[] History { get; set; }
}