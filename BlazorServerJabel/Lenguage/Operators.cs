

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


public abstract class FIGURE:AST{

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


public class FunctionWALLE:AST {
    public AST first;
    public AST second;
    public Token token;

    public FunctionWALLE(Token token,AST first,AST second){

        this.token=token;
        this.first=first;
        this.second=second;
    }

    public double Measure(POINT p1,POINT p2){
        
        return Math.Sqrt(Math.Pow(p1.param1-p2.param1,2)+ Math.Pow(p1.param3-p2.param3,2));
    }

    public List<object> Intersect(FIGURE f1,FIGURE f2){
        
        List<object> listIntersect= new List<object>();




        if(f1 is POINT){
            if(f2 is POINT && ((POINT)f1).param1==((POINT)f2).param1 && ((POINT)f1).param3==((POINT)f2).param3){
                
                 listIntersect.Add((POINT)f1.Clone());
                 
            }

            else if(f2 is CIRCLE){

                if(Math.Pow(((POINT)f1).param1-((CIRCLE)f2).param1,2)+ Math.Pow(((POINT)f1).param3-((CIRCLE)f2).param2,2)== Math.Pow(((CIRCLE)f2).param3,2)){
                    listIntersect.Add((POINT)f1.Clone());
                }
            }

            else if(f2 is SEGMENT){

                double minAbsc= Math.Min(((SEGMENT)f2).param1,((SEGMENT)f2).param3);
                double maxAbsc=Math.Max(((SEGMENT)f2).param1,((SEGMENT)f2).param3);
                if(((POINT)f1).param1>=minAbsc && ((POINT)f1).param1<= maxAbsc){

                    double m= (  ((SEGMENT)f2).param2-((SEGMENT)f2).param4)/( ((SEGMENT)f2).param1-((SEGMENT)f2).param3);

                    double n= ((SEGMENT)f2).param2- m*((SEGMENT)f2).param1;

                    if(((POINT)f1).param1*m+n==((POINT)f1).param3){
                        listIntersect.Add((POINT)f1.Clone());
                    }
                }
                
            }

            else if(f2 is LINE){

                double m= (  ((LINE)f2).param2-((LINE)f2).param4)/( ((LINE)f2).param1-((LINE)f2).param3);

                double n= ((LINE)f2).param2- m*((LINE)f2).param1;

                if(((POINT)f1).param1*m+n==((POINT)f1).param3){
                        listIntersect.Add((POINT)f1.Clone());
                }
            }


            else if(f2 is RAY){

                if(((RAY)f2).param1==((RAY)f2).param3){

                    if(((POINT)f1).param1==((RAY)f2).param1)        
                        listIntersect.Add((POINT)f1.Clone());

                }
                else{

                    
                }

            }
            
        }
        





        else if(f1 is LINE){

            if(f2 is SEGMENT){

                
                double minAbsc= Math.Min(((SEGMENT)f2).param1,((SEGMENT)f2).param3);
                double maxAbsc=Math.Max(((SEGMENT)f2).param1,((SEGMENT)f2).param3);


                if(((LINE)f1).param1-((LINE)f1).param3!=0  && ((SEGMENT)f2).param1-((SEGMENT)f2).param3==0)

                {
                    double mRect= (((LINE)f1).param2-((LINE)f1).param4)/( ((LINE)f1).param1-((LINE)f1).param3);
                    double nRect= ((LINE)f1).param2- mRect*((LINE)f1).param1;

                    double y =mRect*((SEGMENT)f2).param1+nRect;

                    

                    if(y >= Math.Min(((SEGMENT)f2).param2,((SEGMENT)f2).param4) && y<=Math.Max(((SEGMENT)f2).param2,((SEGMENT)f2).param4)){

                        listIntersect.Add(new POINT(null,new Num(new Token(TokenTypes.NUMBER,((SEGMENT)f2).param3)),new Num(new Token(TokenTypes.NUMBER,y))));
                    }
                }
                else if(((LINE)f1).param1-((LINE)f1).param3==0  && ((SEGMENT)f2).param1-((SEGMENT)f2).param3!=0)

                {
                    double mSeg= (((SEGMENT)f2).param2-((SEGMENT)f2).param4)/( ((SEGMENT)f2).param1-((SEGMENT)f2).param3);
                    double nSeg= ((SEGMENT)f2).param2- mSeg*((SEGMENT)f2).param1;

                    double y =mSeg*((LINE)f1).param1+nSeg;

                    

                    if(((LINE)f1).param1 >=minAbsc && ((LINE)f1).param1<= maxAbsc){

                        listIntersect.Add(new POINT(null,new Num(new Token(TokenTypes.NUMBER,((LINE)f1).param1)),new Num(new Token(TokenTypes.NUMBER,y))));
                    }
                }

                else if(((LINE)f1).param1==((LINE)f1).param3 && ((SEGMENT)f2).param1==((SEGMENT)f2).param3 && ((SEGMENT)f2).param1==((LINE)f1).param1)
                {
                    listIntersect.Add((SEGMENT)f2.Clone());
                    
                }

                else if(((LINE)f1).param1-((LINE)f1).param3!=0  && ((SEGMENT)f2).param1-((SEGMENT)f2).param3!=0){


                    double mRect= (((LINE)f1).param2-((LINE)f1).param4)/( ((LINE)f1).param1-((LINE)f1).param3);

                // pendiente dividiendo por 0
                    double nRect= ((LINE)f1).param2- mRect*((LINE)f1).param1;

                    double mSeg= (  ((SEGMENT)f2).param2-((SEGMENT)f2).param4)/( ((SEGMENT)f2).param1-((SEGMENT)f2).param3);

                    double nSeg= ((SEGMENT)f2).param2- mSeg*((SEGMENT)f2).param1;

                    if(mRect==mSeg && nRect==nSeg){

                        listIntersect.Add((SEGMENT)f2.Clone());

                    }
                    else if(mRect!=nSeg){
                    
                        double xIntersection= (nSeg-nRect )/(mRect-mSeg);

                        double yIntersection= mRect*xIntersection +nRect;

                        if(xIntersection >= minAbsc && xIntersection<=maxAbsc){

                            listIntersect.Add(new POINT(null,new Num(new Token(TokenTypes.NUMBER,xIntersection)),new Num(new Token(TokenTypes.NUMBER,yIntersection))));
                        }
                    }
                }
            }

            else if(f2 is LINE){


                double mRectf1= (  ((LINE)f1).param2-((LINE)f1).param4)/( ((LINE)f1).param1-((LINE)f1).param3);

                double nRectf1= ((LINE)f1).param2- mRectf1*((LINE)f1).param1;

                double mRectf2= (  ((LINE)f2).param2-((LINE)f2).param4)/( ((LINE)f2).param1-((LINE)f2).param3);

                double nRectf2= ((LINE)f2).param2- mRectf2*((LINE)f2).param1;


                if(mRectf1==mRectf2 && nRectf1==nRectf2){
                    listIntersect.Add((LINE)f2.Clone());

                }
                else if(mRectf1!=nRectf2){
                    
                    double xIntersection= (nRectf2-nRectf1 )/(mRectf1-mRectf2);

                    double yIntersection= mRectf1*xIntersection +nRectf1;

                    listIntersect.Add(new POINT(null,new Num(new Token(TokenTypes.NUMBER,xIntersection)),new Num(new Token(TokenTypes.NUMBER,yIntersection))));

                }
            }


            else if(f2 is CIRCLE){
                
                // pendiente de la recta

                if(((LINE)f1).param1!=((LINE)f1).param3){

                    double m= (  ((LINE)f1).param2-((LINE)f1).param4)/( ((LINE)f1).param1-((LINE)f1).param3);


                    double n= ((LINE)f1).param2- m*((LINE)f1).param1;

                    double a=Math.Pow(m,2)+1;
                    double b= 2*m*n-2*((CIRCLE)f2).param1-2*m*((CIRCLE)f2).param2;
                    double c= Math.Pow(((CIRCLE)f2).param1,2)-Math.Pow(((CIRCLE)f2).param3,2)+Math.Pow(n-((CIRCLE)f2).param2,2);


                    if(b*b-4*a*c >=0)
                    {
                        double x= (-b + Math.Sqrt(b*b-4*a*c))/(2*a);
                        double x41= (-b - Math.Sqrt(b*b-4*a*c))/(2*a);

                        double y= m*x+n;
                        double y41=m*x41+n;

                        
                        listIntersect.Add(new POINT(null,new Num(new Token(TokenTypes.NUMBER,x)),new Num(new Token(TokenTypes.NUMBER,y))));
                    
                        if(y!=y41){

                            listIntersect.Add(new POINT(null,new Num(new Token(TokenTypes.NUMBER,x41)),new Num(new Token(TokenTypes.NUMBER,y41))));
                        }
                    }
                }

                else{
                    
                    if(Math.Pow(((CIRCLE)f2).param3,2) - Math.Pow(((CIRCLE)f2).param1-((LINE)f1).param1,2)>=0){
                        double y= Math.Sqrt(Math.Pow(((CIRCLE)f2).param3,2) - Math.Pow(((CIRCLE)f2).param1-((LINE)f1).param1,2)) + ((CIRCLE)f2).param2;
                    
                        double y41= -Math.Sqrt(Math.Pow(((CIRCLE)f2).param3,2) - Math.Pow(((CIRCLE)f2).param1-((LINE)f1).param1,2)) + ((CIRCLE)f2).param2;
                        
                        listIntersect.Add(new POINT(null,new Num(new Token(TokenTypes.NUMBER,((LINE)f1).param1)),new Num(new Token(TokenTypes.NUMBER,y))));
                    
                        if(y!=y41){

                            listIntersect.Add(new POINT(null,new Num(new Token(TokenTypes.NUMBER,((LINE)f1).param1)),new Num(new Token(TokenTypes.NUMBER,y41))));
                        }
                    }

                   
                }   

            }
       



        }

        else if(f1 is SEGMENT){

            if(f2 is SEGMENT){

            }
            else if(f2 is CIRCLE){

            }
        }


        else if(f1 is RAY){
            
            Console.WriteLine("que ha pasado");
            
            
            if(f2 is CIRCLE){
                Console.WriteLine("entramos");


                if(((RAY)f1).param1!=((RAY)f1).param3){

                    double m= (  ((RAY)f1).param2-((RAY)f1).param4)/( ((RAY)f1).param1-((RAY)f1).param3);


                    double n= ((RAY)f1).param2- m*((RAY)f1).param1;

                    double a=Math.Pow(m,2)+1;
                    double b= 2*m*n-2*((CIRCLE)f2).param1-2*m*((CIRCLE)f2).param2;
                    double c= Math.Pow(((CIRCLE)f2).param1,2)-Math.Pow(((CIRCLE)f2).param3,2)+Math.Pow(n-((CIRCLE)f2).param2,2);


                    if(b*b-4*a*c >=0)
                    {
                        double x= (-b + Math.Sqrt(b*b-4*a*c))/(2*a);
                        double x41= (-b - Math.Sqrt(b*b-4*a*c))/(2*a);

                        double y= m*x+n;
                        double y41=m*x41+n;

                        

                        if((((RAY)f1).param1  >((RAY)f1).param3 && x<=((RAY)f1).param1 ) || (((RAY)f1).param1  < ((RAY)f1).param3 && x>=((RAY)f1).param1 )){// vector director negativo

        
                                listIntersect.Add(new POINT(null,new Num(new Token(TokenTypes.NUMBER,x)),new Num(new Token(TokenTypes.NUMBER,y))));
                                                        
                        }
                            
                        if(x!=x41 && ( (((RAY)f1).param1  >((RAY)f1).param3 && x41<=((RAY)f1).param1 ) || (((RAY)f1).param1  < ((RAY)f1).param3 && x41>=((RAY)f1).param1 ))){// vector director negativo

        
                                listIntersect.Add(new POINT(null,new Num(new Token(TokenTypes.NUMBER,x41)),new Num(new Token(TokenTypes.NUMBER,y41))));
                                                        
                        }
                        
                        Console.WriteLine(x);
                        Console.WriteLine(x41);
                    }
                }

                else{
                    
                    if(Math.Pow(((CIRCLE)f2).param3,2) - Math.Pow(((CIRCLE)f2).param1-((RAY)f1).param1,2)>=0){
                        double y= Math.Sqrt(Math.Pow(((CIRCLE)f2).param3,2) - Math.Pow(((CIRCLE)f2).param1-((RAY)f1).param1,2)) + ((CIRCLE)f2).param2;
                    
                        double y41= -Math.Sqrt(Math.Pow(((CIRCLE)f2).param3,2) - Math.Pow(((CIRCLE)f2).param1-((RAY)f1).param1,2)) + ((CIRCLE)f2).param2;
                        
                        if((((RAY)f1).param2  >((RAY)f1).param4 && y<=((RAY)f1).param2 ) || (((RAY)f1).param2  < ((RAY)f1).param4 && y>=((RAY)f1).param2 )){// vector director negativo

        
                                listIntersect.Add(new POINT(null,new Num(new Token(TokenTypes.NUMBER,((RAY)f1).param1)),new Num(new Token(TokenTypes.NUMBER,y))));
                                                        
                        }
                            
                        if(y!=y41 && ( (((RAY)f1).param2  >((RAY)f1).param4 && y41<=((RAY)f1).param2 ) || (((RAY)f1).param2  < ((RAY)f1).param4 && y41>=((RAY)f1).param2 ))){// vector director negativo

        
                                listIntersect.Add(new POINT(null,new Num(new Token(TokenTypes.NUMBER,((RAY)f1).param1)),new Num(new Token(TokenTypes.NUMBER,y41))));
                                                        
                        }
                    }

                   
                }   

            }
            Console.WriteLine("salimos");
            
        }




        else if(f1 is CIRCLE){

            if(f2 is CIRCLE){

                double x1=((CIRCLE)f1).param1;
                double y1=((CIRCLE)f1).param2;
                double r1=((CIRCLE)f1).param3;

                double x2=((CIRCLE)f2).param1;
                double y2=((CIRCLE)f2).param2;
                double r2=((CIRCLE)f2).param3;

                double distCenter=Math.Sqrt(Math.Pow(x2-x1,2)+ Math.Pow(y2-y1,2));

                if(distCenter > r1+r2 || distCenter<Math.Abs(r1-r2)){

                }

                else{
                    
                    double n= (Math.Pow(r1,2)-Math.Pow(r2,2)+ Math.Pow(x2,2)-Math.Pow(x1,2)+Math.Pow(y2,2)-Math.Pow(y1,2))/2;

                    double t=x2-x1;
                    double w=y2-y1;

                    if(t==0){
                        double y4=n/w;

                        double x4=Math.Sqrt(Math.Pow(r1,2)-Math.Pow((n-w*y1)/w,2))+x1;
                        double x41= -Math.Sqrt(Math.Pow(r1,2)-Math.Pow((n-w*y1)/w,2))+x1;

                        listIntersect.Add(new POINT(null,new Num(new Token(TokenTypes.NUMBER,x4)),new Num(new Token(TokenTypes.NUMBER,y4))));
                    
                        if(x4!=x41){

                            listIntersect.Add(new POINT(null,new Num(new Token(TokenTypes.NUMBER,x41)),new Num(new Token(TokenTypes.NUMBER,y4))));
                        }
                    
                    
                    }
                    else if(w==0){
                        double x4=n/t;

                        double y4=Math.Sqrt(Math.Pow(r1,2)-Math.Pow((n-t*x1)/t,2))+y1;
                        double y41= -Math.Sqrt(Math.Pow(r1,2)-Math.Pow((n-t*x1)/t,2))+y1;

                        listIntersect.Add(new POINT(null,new Num(new Token(TokenTypes.NUMBER,x4)),new Num(new Token(TokenTypes.NUMBER,y4))));
                    
                        if(y4!=y41){

                            listIntersect.Add(new POINT(null,new Num(new Token(TokenTypes.NUMBER,x4)),new Num(new Token(TokenTypes.NUMBER,y41))));
                        }
                    
                    
                    }
                    else{
                    double a=Math.Pow(w,2)+Math.Pow(t,2);
                    double b=2*w*x1*t-2*n*w-2*y1*t*t;
                    double c=Math.Pow(n-x1*t,2)+ t*t*(y1*y1-r1*r1);
                    double D= Math.Pow(b,2) -4*a*c ;

                    double y4= (-b+Math.Sqrt(D))/(2*a);
                    double y41=(-b-Math.Sqrt(D))/(2*a);

                    double x4=(n-y4*w)/t;
                    double x41=(n-y41*w)/t;
                    
                    listIntersect.Add(new POINT(null,new Num(new Token(TokenTypes.NUMBER,x4)),new Num(new Token(TokenTypes.NUMBER,y4))));

                    if(y4!=y41){

                        listIntersect.Add(new POINT(null,new Num(new Token(TokenTypes.NUMBER,x41)),new Num(new Token(TokenTypes.NUMBER,y41))));
                        
                    }
                    }
                }
            }
        }

        
    

    return listIntersect;
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



