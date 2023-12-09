

namespace InterpreterDyZ;

using System.Diagnostics.Metrics;
using System.Drawing;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

using Microsoft.JSInterop;

// Abstract syntac tree class 
public class AST
{
   /* private Dictionary<string,object>map;
    public void AccederDiccionario(Dictionary<string,object>Scope)
    {
        map= Interpreter.Scope;
        Interpreter.Scope= Scope;
    }
    */
}

public class BinaryOperator : AST
{
    public AST Left, Right;
    public Token Operator;

    public BinaryOperator(AST left, Token op, AST right)
    {
        Left = left;
        Operator = op;
        Right = right;
    }
}

public class UnaryOperator : AST
{
    public Token Operator;
    public AST Expression;

    public UnaryOperator(Token op, AST expr)
    {
        Operator = op;
        Expression = expr;
    }
}

public class Num : AST
{
    public Token Token;
    public object Value;


    public Num(Token token)
    {
        Token = token;
        Value = token.Value;
    }
}

public class Bool : AST
{
    public Token Token;
    public bool Value;

    public Bool(Token token)
    {
        Token = token;
        Value = (bool)token.Value;
    }
}

public class Cadene : AST
{
    public Token Token;
    public string Value;

    public Cadene(Token token)
    {
        Token = token;
        Value = (string)token.Value;
    }
}

public class Type : AST
{
    public Token Token;
    public object Value;

    public Type(Token token)
    {
        Token = token;
        Value = token.Value;
    }
}

public class Instructions : AST
{
    public List<AST>? Commands;

    public Instructions()
    {
        Commands = new List<AST>();
    }
}

public class Declarations : AST
{
    public List<AST>? Commands;
    public AST? instruccion;
    public Dictionary<string,object> Scope;
    public Declarations(AST ins,List<AST> Commands)
    {
        this.Commands = Commands;
        instruccion=ins;
        Scope= new Dictionary<string, object>();
    }

    // una propiedad para llenar el Scope local de cada Declaration
}

public class Assign : AST
{
    public List<Var>? Left;
    public Token? Operator;
    public AST Right;

    public Assign(List<Var> left, Token op, AST right)
    {
        Left = left;
        Operator = op;
        Right = right;
    }
}

public class Var : AST
{
    public Token Token;
    public object Value; 

    public Var(Token token)
    {
        Token = token;
        Value = token.Value;
    }
}

public class VarDecl : AST
{
    public Var Node;
    public TokenTypes Type;
    public AST Value;
    //public Dictionary<string,object>Scope;

    public VarDecl(Var node, TokenTypes type, AST value)
    {
        Node = node;
        Type = type;
        Value=value;
        //Scope = new Dictionary<string, object>();
    }

    
}

public class Empty : AST
{
    
}

public class Condition : AST
{
    public AST Compound;
    public AST StatementList;
    public AST StatementElse;

    public Condition(AST compound, AST statements,AST statement2)
    {
        Compound = compound;
        StatementList = statements;
        StatementElse=statement2;
    }
}
public class PRINT:AST{
    public AST Compound;

    public PRINT(AST Compound){
        this.Compound=Compound;
    }
}

public class SEQUENCE:AST{
    public List<AST> sequence;

    public SEQUENCE(List<AST>sequence){
        
        this.sequence=sequence;
    }

    public object Count() {
        


        return sequence.Count();
    }
}

/*

public class InfiniteSequence<double>:SEQUENCE<double>,IEnumerable<double>{

    private double first{get;}
    public InfiniteSequence(int first){

    }
}

*/
public class Draw:AST{
    public AST name;
    public string tag;
    //Dictionary <string,object> Scope;
    public Draw(AST name,Token tag=null){
        this.name=name;
        if(tag!=null){
             this.tag=(string)tag.Value;
        }
       
    }
}


public class FIGURE:AST{

    public Token name;
    public AST firstParam;
    public AST secondParam;

    public double param1;
    public double param2;
    public double param3;
    public double param4;
    public FIGURE(Token name=null,AST firstParam =null,AST secondParam =null){

        this.name=name;
        this.firstParam=firstParam;
        this.secondParam=secondParam;

        if(!(name is null)){
        // (ejemplos )point c3; circle c2; line s; segment y
        Random rnd = new Random();
        param1= rnd.Next(0,400);
        param2= rnd.Next(0,400);
        param3= rnd.Next(0,400);
        param4= rnd.Next(0,400);
  
        }

    
       
    }

    public FIGURE Clone(){
        return (FIGURE)MemberwiseClone();
    }

    public void Check(object first,object second){

        if(this is POINT){

            param1=Convert.ToDouble(first);
            //firstParam= new Num(new Token(TokenTypes.NUMBER,param1));
            
        }
        else{
            
            param1= ((POINT)first).param1;
            param2=((POINT)first).param3;

            //firstParam=new POINT(null,new Num(new Token(TokenTypes.NUMBER,param1)), new Num(new Token(TokenTypes.NUMBER,param2)));
        }

        if(this is POINT || this is CIRCLE){

            param3=Convert.ToDouble(second);
            //secondParam= new Num(new Token(TokenTypes.NUMBER,param3));
        }
        else{

            param3= ((POINT)second).param1;
            param4=((POINT)second).param3;
            //secondParam=new POINT(null,new Num(new Token(TokenTypes.NUMBER,param3)),new Num(new Token(TokenTypes.NUMBER,param4)));
        }

    }
   
    
}


#region IDraw
public class POINT : FIGURE,IDraw{
    
    public POINT(Token name,AST firstParam,AST secondParam) :base(name,firstParam,secondParam){}

    public async void Draw(){
        
        Console.WriteLine($"Drawing a point ... ");
        Console.WriteLine($"{param1};{param3}");
        var parameters = new{
            param1,
            param3
        };
        await Principal._jsRuntime.InvokeAsync<object>("DibujarePoint",parameters);
    }
    public string Name(){
        return (string)name.Value;
    }
}

public class CIRCLE : FIGURE,IDraw{
    
    public CIRCLE(Token name,AST firstParam,AST secondParam) :base(name,firstParam,secondParam){}
    public async void Draw(){
        

        Console.WriteLine($"Drawing a circle ... ");
        Console.WriteLine($"{param1};{param2};{param3}");
        var parameters = new{
            param1,
            param2,
            param3,
        };
        await Principal._jsRuntime.InvokeAsync<object>("DibujareEllipse",parameters);
    }
    public string Name(){
        return (!(name is null))?(string)name.Value :"";
    }
}


public class LINE : FIGURE,IDraw{
    
    public LINE(Token name,AST firstParam,AST secondParam) :base(name,firstParam,secondParam){}
    
    // expresion que constituye una nueva lista de instrucciones
    
    public async void Draw(){
        

        Console.WriteLine($"Drawing a line ... ");
        
        var parameters = new{
            param1,
            param2,
            param3,
            param4
        };

        Console.WriteLine($"{param1};{param2};{param3};{param4}");
        await Principal._jsRuntime.InvokeAsync<object>("DibujarLine",parameters);
    }

    public string Name(){
        
        return (!(name is null))?(string)name.Value :"";
    }
}

public class SEGMENT : FIGURE,IDraw{
   
    
    public SEGMENT(Token name,AST firstParam,AST secondParam) :base(name,firstParam,secondParam){}
        
    public async void Draw(){
        

        Console.WriteLine($"Drawing a segment ... ");
        
        var parameters = new{
            param1,
            param2,
            param3,
            param4
        };

        Console.WriteLine($"{param1};{param2};{param3};{param4}");
        await Principal._jsRuntime.InvokeAsync<object>("DibujarSegment",parameters);
    }

    public string Name(){
        return (!(name is null))?(string)name.Value:"";
    }
}



public class RAY : FIGURE,IDraw{
   
    
    public RAY(Token name,AST firstParam,AST secondParam) :base(name,firstParam,secondParam){}
        
    public async void Draw(){
        

        Console.WriteLine($"Drawing a ray ... ");
        
        var parameters = new{
            param1,
            param2,
            param3,
            param4
        };

        Console.WriteLine($"{param1};{param2};{param3};{param4}");
        await Principal._jsRuntime.InvokeAsync<object>("DibujarRay",parameters);
    }

    public string Name(){
        return (!(name is null))?(string)name.Value:"";
    }
}



#endregion


public class COLOR:AST{

    public ColorType color{get;}
    public TokenTypes token {get;}
    public static Stack<ColorType> stackColor= new Stack<ColorType>();
    public COLOR(TokenTypes token,ColorType color=ColorType.black)
    
    {     
        this.token=token;
        this.color=color;
    }


    public async void  Push(){

        stackColor.Push(color);
        Console.WriteLine($"Changed color to {color} ... ");
        
        await Principal._jsRuntime.InvokeAsync<object>("Color",color.ToString());
    }

    public async void Restore(){
        

        if(stackColor.Count>0){

            Console.WriteLine($"{stackColor.Peek().ToString()} color has been drawn from the stack");
            stackColor.Pop();
  
        if(stackColor.Count==0){
            await Principal._jsRuntime.InvokeAsync<object>("Color","black");
        }

        else{
            
        
        await Principal._jsRuntime.InvokeAsync<object>("Color",stackColor.Peek().ToString());
        }

        }
    }

}


public class FUNCTIONAL:AST
{
    public string name;
    public Dictionary<string,object>argumentos;
    public AST Statement;
    //public List<AST>arg;

    public FUNCTIONAL(Token names,Dictionary<string,object>argumentos,AST Statement){
        name=(string)names.Value;
        this.argumentos=argumentos;
        this.Statement=Statement;
    }

    
   /* public FUNCTIONAL(Token names,List<AST>arg){
        name=(string)names.Value;
        this.arg=arg;
    }
    */
}
public class CallFUNCTION:AST
{
    public string name;
    public List<AST>arg;
    public CallFUNCTION(Token names,List<AST>arg){
        name= (string)names.Value;
        this.arg=arg;
    }
    
    
}



public class Sen:AST
{
    public AST Statement;

    public Sen(AST Statement){
        this.Statement=Statement;
    }
}
public class Cos:AST
{
    public AST Statement;

    public Cos(AST Statement){
        this.Statement=Statement;
    }
}

public class LOG:AST{

    public AST? bases;
    public AST? Statement;
    public LOG(AST bases,AST Statement){
        this.bases=bases;
        this.Statement=Statement;
    }

    public LOG(AST Statement){
        this.Statement=Statement;
    }
}
/*
public class CallFUNCTION:FUNCTIONAL
{
    public string name;
    public List<object>arg;
    public CallFUNCTION(Token names,List<object>arg):base(names,null,null){
        //name=(string)names.Value;
        this.arg=arg;
        this.Statement=Statement;
        int i=0;
     /*   foreach(KeyValuePair<string,object> item in argumentos)
        {
            argumentos[item.Key]=arg[i];
            i+=1;
        }
    
    }

}
*/
public class Cicle : AST
{
    public AST Compound;// expresion a evaluar
    public AST StatementList;// expresion que constituye una nueva lista de instrucciones

    public Cicle(AST compound, AST statements)
    {
        Compound = compound;
        StatementList = statements;
    }
}


public interface IDraw{
    
    
    void Draw();
    //object Function();
    string Name();
}



