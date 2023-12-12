
namespace GOLenguage;

using System.Runtime.CompilerServices;
using Microsoft.JSInterop;

    //Dictionary<string,AST>functional= new Dictionary<string, AST>();
    public static class Principal{
        public static  IJSRuntime _jsRuntime;
        static Lexer lexer;
        static Parser parser;
        public static string Text;
        static Interpreter interpreter;
        public static Dictionary<string,AST> Functiones= new Dictionary<string, AST>();
        public static void Principal2(IJSRuntime jsRuntime){



          _jsRuntime=jsRuntime;
         try
          {
            Method();
          }
         catch(Exception ex){
            Console.WriteLine("An error has ocurred in your program");
            Functiones.Clear();
            COLOR.stackColor.Clear();
            Interpreter.Scope.Clear();
         }
            
            //Console.ForegroundColor= ConsoleColor.Red;
            
           Console.WriteLine("PRESS ENTER OR ESCAPE FOR SOME FUNCTIONALITY");
          /*  while(true)
            {
                
                if(KeyAvailable)
                {   
                    
                    var key= ReadKey(true).Key;

                    if(key==ConsoleKey.Enter)
                    {   

                        Write(">>");
                        Text=ReadLine();
                        Text=@$"{Text}";
                        try{
                            Method();
                        }
                        catch(StackOverflowException ex){
                            WriteLine("RecursionError: maximum recursion depth exceeded");
                        }
                        catch(Exception ex){
                            var g= ForegroundColor;
                            ForegroundColor=ConsoleColor.Yellow;
                            WriteLine(".Try again please.");
                            ForegroundColor=g;
                        }
                    }

                    else if(key==ConsoleKey.Escape)
                    {
                        
                        break;
                    }
                }
            }
            
            */
    
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
