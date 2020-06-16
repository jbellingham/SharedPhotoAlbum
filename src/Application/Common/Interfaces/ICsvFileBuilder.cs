using SharedPhotoAlbum.Application.TodoLists.Queries.ExportTodos;
using System.Collections.Generic;

namespace SharedPhotoAlbum.Application.Common.Interfaces
{
    public interface ICsvFileBuilder
    {
        byte[] BuildTodoItemsFile(IEnumerable<TodoItemRecord> records);
    }
}
