using System;

namespace DAL.Interfaces
{
    public interface ICurrentUserService
    {
        Guid? UserId { get; }
        string? UserName { get; }
        string? FullName { get; }
    }
}
