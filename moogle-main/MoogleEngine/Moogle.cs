
using Microsoft.JSInterop;
//using IJSRuntime;
namespace MoogleEngine;

// dotnet add package Microsoft.JSInterop version 7.0.0
// referenciar al proyecto MoogleServer
// dotnet add reference ..\MoogleServer\MoogleServer.csproj
public static class Moogle
{
    
    
    public static SearchResult Query(string query) {
        // Modifique este método para responder a la búsqueda

        SearchItem[] items = new SearchItem[3] {
            new SearchItem("Hello World", "Lorem ipsum dolor sit amet", 0.9f),
            new SearchItem("Hello World", "Lorem ipsum dolor sit amet", 0.5f),
            new SearchItem("Hello World", "Lorem ipsum dolor sit amet", 0.1f),
        };
        
//await.appendText("HOLA BUENAS");
        return new SearchResult(items, query);
    }
}
