using Microsoft.EntityFrameworkCore;
using TodoListApi.Data;

namespace TodoListApi.Services;

public class AuthService(TodoApiContext context)
{
    private readonly TodoApiContext _context = context;

    public async Task<int?> GetIdUserFromToken(HttpContext httpContext)
    {
        var userToken = httpContext.Request.Headers["UserToken"].ToString();
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Token == userToken);
        if (user is null) return null;
        return user.Id;
    }
}
