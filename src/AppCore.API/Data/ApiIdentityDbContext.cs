using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AppCore.API.Data;

public class ApiIdentityDbContext : IdentityDbContext
{
    public ApiIdentityDbContext(DbContextOptions<ApiIdentityDbContext> options) : base(options) { }

}
