using Microsoft.EntityFrameworkCore;

namespace ToDo.Api.Interfaces
{
    public interface ITodoContext
    {
        DbSet<TEntity> Set<TEntity>() where TEntity : class;
    }
}
