
namespace InterpreterDyZ;

public class AST
{
   
   
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

// esta demas 
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

public class Draw:AST{
    public AST name;
    public string tag;
    //Dictionary <string,object> Scope;
    public Draw(AST name,Token tag=null){
        this.name=name;
        if(tag!=null){
             this.tag=(string)tag.Value;// lanzar un error al castear una variable
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


