using Microsoft.EntityFrameworkCore;
using WebAPIServer.CustomExceptions;
using WebAPIServer.Models;

namespace WebAPIServer.Repositories
{
    public interface IToDoRepository
    {
        Task<IEnumerable<TODOItem>> GetAll();
        Task<TODOItem> Get(int id);
        Task Add(TODOItem item);
        Task Update(TODOItem item);
        Task Delete(int id);
    }
    public class ToDoRepository : IToDoRepository
    {
        private readonly TODODBContext _toDoDbContext;
        public ToDoRepository(TODODBContext toDoDbContext)
        {
            _toDoDbContext = toDoDbContext;
        }
        public async Task<IEnumerable<TODOItem>> GetAll()
        {
            var items = await _toDoDbContext.TODOItems.ToListAsync();
            return items;
        }

        public async Task Add(TODOItem item)
        {
            await _toDoDbContext.TODOItems.AddAsync(item);
            await _toDoDbContext.SaveChangesAsync();
        }

        public async Task<TODOItem> Get(int id)
        {
            var item = await _toDoDbContext.TODOItems.FirstOrDefaultAsync(x => x.Id == id);
            if (item == null)
                throw new ItemNotFoundException(id);
            return item;
        }

        public async Task Update(TODOItem item)
        {
            var itemToUpdate = await _toDoDbContext.TODOItems.FirstOrDefaultAsync(x => x.Id == item.Id);

            if (itemToUpdate == null)
                throw new ItemNotFoundException(item.Id);

            itemToUpdate.Title = item.Title;
            itemToUpdate.Description = item.Description;
            itemToUpdate.PlannedDate = item.PlannedDate;

            await _toDoDbContext.SaveChangesAsync();

        }

        public async Task Delete(int id)
        {
            var itemToDelete = await _toDoDbContext.TODOItems.FirstOrDefaultAsync(x => x.Id == id);

            if (itemToDelete == null)
                throw new ItemNotFoundException(id);

            _toDoDbContext.TODOItems.Remove(itemToDelete);
            await _toDoDbContext.SaveChangesAsync();

        }
    }
}
