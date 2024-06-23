using Microsoft.AspNetCore.Mvc;
using WebAPIServer.Models;
using WebAPIServer.Repositories;
using WebAPIServer.Validators;

namespace WebAPIServer.Services
{
    public interface IToDoService
    {
        Task<IEnumerable<TODOItem>> GetAll();
        Task<TODOItem> Get(int id);
        Task Add(TODOItem item);
        Task Update(int id, TODOItem item);
        Task Delete(int id);
    }
    public class ToDoService: IToDoService
    {
        private readonly IToDoRepository _iToDoRepository;
        private readonly ToDoItemValidator _validator;
        public ToDoService(IToDoRepository iToDoRepository, ToDoItemValidator validator)
        {
            _iToDoRepository = iToDoRepository;
            _validator = validator;
        }
        public async Task Add(TODOItem item)
        {
            var validationResult = _validator.Validate(item);
            if (!validationResult.IsValid)
            {
                throw new ArgumentException(string.Join(".", validationResult.Errors));
            }
            await _iToDoRepository.Add(item);
        }

        public async Task Delete(int id)
        {
            await _iToDoRepository.Delete(id);
        }

        public async Task<TODOItem> Get(int id)
        {
            var item = await _iToDoRepository.Get(id);
            return item;
        }

        public async Task<IEnumerable<TODOItem>> GetAll()
        {
            var items = await _iToDoRepository.GetAll();
            return items;
        }

        public async Task Update(int id, TODOItem item)
        {
            var validationResult = _validator.Validate(item);
            if (!validationResult.IsValid)
            {
                throw new ArgumentException(string.Join(".", validationResult.Errors));
            }
            item.Id = id;
            await _iToDoRepository.Update(item);
        }
    }
}
