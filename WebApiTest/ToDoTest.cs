using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using Moq;
using WebAPIServer.Controllers;
using WebAPIServer.CustomAttributes;
using WebAPIServer.Models;
using WebAPIServer.Repositories;
using WebAPIServer.Services;
using Xunit;

namespace WebApiTest
{
    public class ToDoFixture
    {
        private readonly Mock<IDbContextFactory<TODODBContext>> _mockDbFactory;
        public readonly TODOController TODOController;
        public readonly TODODBContext DB;

        public ToDoFixture()
        {
            _mockDbFactory = new Mock<IDbContextFactory<TODODBContext>>();

            var options = new DbContextOptionsBuilder<TODODBContext>()
                                .UseInMemoryDatabase(databaseName: "ToDoDatabaseInMemory")
                                .Options;

            // Insert seed data into the database using an instance of the context
            using (var context = new TODODBContext(options))
            {
                context.TODOItems.Add(new TODOItem { Id = 1, Title = "item1", Description = "desc1", PlannedDate = DateTime.Today.AddDays(1) });
                context.TODOItems.Add(new TODOItem { Id = 2, Title = "item2", Description = "desc2", PlannedDate = DateTime.Today.AddDays(2) });
                context.SaveChanges();
            }

            // Now the in-memory db already has data, we don't have to seed everytime the factory returns the new DbContext:
            _mockDbFactory.Setup(f => f.CreateDbContext()).Returns(() => new TODODBContext(options));
            DB = _mockDbFactory.Object.CreateDbContext();
            var toDoRepository = new ToDoRepository(DB);
            var toDoService = new ToDoService(toDoRepository, new ToDoItemValidator());
            TODOController = new TODOController(toDoService);
        }
    }
    public class ToDoTest: IClassFixture<ToDoFixture>
    {
        private readonly TODOController _tODOController;
        private readonly TODODBContext _db;

        public ToDoTest(ToDoFixture a)
        {
            _db = a.DB;
            _tODOController = a.TODOController;
        }

        [Fact]
        public async Task GetAll()
        {
            var expectedCount = _db.TODOItems.Count();
            var actionResult = await _tODOController.GetAll();
            Assert.IsType<OkObjectResult>(actionResult);
            var okResult = actionResult as OkObjectResult;
            var todos = okResult.Value as IEnumerable<TODOItem>;
            Assert.NotNull(todos);
            Assert.Equal(expectedCount, todos.Count());
        }
        [Fact]
        public async Task GetById()
        {
            var item = _db.TODOItems.First();
            var actionResult = await _tODOController.GetById(item.Id);
            Assert.IsType<OkObjectResult>(actionResult);
            var okResult = actionResult as OkObjectResult;
            var todo = okResult.Value as TODOItem;
            Assert.NotNull(todo);
            Assert.Equal(item.Id, todo.Id);
        }
        [Fact]
        public async Task Add()
        {
            var todosCount = _db.TODOItems.Count();
            var actionResult = await _tODOController.Add(new TODOItem() { Id = 3, Title = "item3", Description = "desc3", PlannedDate = DateTime.Today.AddDays(3) });
            Assert.IsType<OkResult>(actionResult);
            Assert.Equal(todosCount+1, _db.TODOItems.Count());
        }

        [Fact]
        public async Task Put()
        {
            var id = _db.TODOItems.First().Id;
            var item = new TODOItem() {Id = id, Title = "updatedTitle", Description = "updatedDescription", PlannedDate=DateTime.Today.AddDays(4) };
            var actionResult = await _tODOController.Update(item.Id, item);
            Assert.IsType<OkResult>(actionResult);
            var updatedItem = _db.TODOItems.First(x => x.Id == item.Id);
            Assert.Equal(item.Title, updatedItem.Title);
            Assert.Equal(item.Description, updatedItem.Description);
            Assert.Equal(item.PlannedDate, updatedItem.PlannedDate);
        }

        [Fact]
        public async Task Validate()
        {
            var todosCount = _db.TODOItems.Count();
            var actionResult = await _tODOController.Add(new TODOItem() { Id = 5, Title = "item5", Description = "desc5", PlannedDate = DateTime.Today.AddDays(-3) });
            Assert.IsType<ObjectResult>(actionResult);
            var result = actionResult as ObjectResult;
            Assert.Equal(StatusCodes.Status400BadRequest, result.StatusCode);
            Assert.Equal(todosCount, _db.TODOItems.Count());

            var id = _db.TODOItems.First().Id;
            var item = new TODOItem() { Id = id, Title = "updatedTitleNotValid", Description = "updatedDescriptionNotValid", PlannedDate = DateTime.Today.AddDays(-1) };
            var actionResult2 = await _tODOController.Update(item.Id, item);
            Assert.IsType<ObjectResult>(actionResult);
            var result2 = actionResult2 as ObjectResult;
            Assert.Equal(StatusCodes.Status400BadRequest, result2.StatusCode);
            var updatedItem = _db.TODOItems.First(x => x.Id == item.Id);
            Assert.NotEqual(item.Title, updatedItem.Title);
            Assert.NotEqual(item.Description, updatedItem.Description);
            Assert.NotEqual(item.PlannedDate, updatedItem.PlannedDate);
        }

        [Fact]
        public async Task Delete()
        {
            var todosCount = _db.TODOItems.Count();
            var actionResult = await _tODOController.Delete(1);
            Assert.IsType<OkResult>(actionResult);
            Assert.Equal(todosCount-1, _db.TODOItems.Count());
        }

        [Fact]
        public void DeveloperNameHeader()
        {
            //Arrange
            var modelState = new ModelStateDictionary();
            var httpContext = new DefaultHttpContext() {  };
            var context = new ActionExecutingContext(
                new ActionContext(
                    httpContext: httpContext,
                    routeData: new RouteData(),
                    actionDescriptor: new ActionDescriptor(),
                    modelState: modelState
                ),
                new List<IFilterMetadata>(),
                new Dictionary<string, object>(),
                new Mock<Controller>().Object);

            var att = new DeveloperNameLoggingAttribute(new LoggerService(LoggerService.LogLevel.Information));

            //Act
            att.OnActionExecuting(context);

            //Assert
            Assert.IsType<BadRequestObjectResult>(context.Result);

            //Arrange
            context.Result = null;
            httpContext.Request.Headers[DeveloperNameLoggingAttribute.DEVELOPER_HEADER_KEY] = "developer-name";

            //Act
            att.OnActionExecuting(context);

            //Assert
            Assert.Null(context.Result);// Request allowed if result is null

        }
    }
}