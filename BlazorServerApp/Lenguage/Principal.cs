
namespace GOLenguage;

using System.Runtime.CompilerServices;
using Microsoft.JSInterop;

//Dictionary<string,AST>functional= new Dictionary<string, AST>();
public static class Principal
{

    public static IJSRuntime _jsRuntime;
    static Lexer lexer;
    static Parser parser;
    public static string Text;
    static Interpreter interpreter;
    public static Dictionary<string, AST> Functiones = new Dictionary<string, AST>();
    public static string console;
    public static void Principal2(IJSRuntime jsRuntime)
    {

 _jsRuntime = jsRuntime;

        try
        {
            Method();
        }
        catch (Exception ex)
        {
            Console.WriteLine("An error has ocurred in your program eexecepiu45");
            Functiones.Clear();
            COLOR.stackColor.Clear();
            Interpreter.Scope.Clear();
            //Principal.console.Split('\n').ToList().ForEach(x=>Console.WriteLine(x));
        }

        Console.WriteLine(123);

    }


    public static void Method()
    {


        lexer = new Lexer(Text);
        parser = new Parser(lexer);
        interpreter = new Interpreter(parser);

        //Console.ForegroundColor = ConsoleColor.Green;

        interpreter.Interpret();


    }
}
