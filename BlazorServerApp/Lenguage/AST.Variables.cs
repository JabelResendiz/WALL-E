

namespace InterpreterDyZ;

using Microsoft.JSInterop;


#region SuperClass Variables

public class Variables:AST{

    public Token Token;
    public object Value;

    
}
public class Num : Variables
{
    public Num(Token token)
    {
        Token = token;
        Value = (double)token.Value;
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

public class Cadene : Variables
{

    public Cadene(Token token)
    {
        Token = token;
        Value = (string)token.Value;
    }
}

public class Empty : Variables
{
    
}


#endregion



#region Figure


public interface IDraw{
    
    
    void Draw();
    //object Function();
    string Name();
}



public class FIGURE:AST{

    public Token Token;
    public AST firstParam;
    public AST secondParam;

    public double param1;
    public double param2;
    public double param3;
    public double param4;
    public FIGURE(Token name=null,AST firstParam =null,AST secondParam =null){

        Token=name;
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

        //Value=Clone();
    
       
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
            //param1= (double)((Num)((POINT)first).firstParam).Value;
            //param2=(double)((Num)((POINT)first).secondParam).Value;

            //firstParam=new POINT(null,new Num(new Token(TokenTypes.NUMBER,param1)), new Num(new Token(TokenTypes.NUMBER,param2)));
        }

        if(this is POINT || this is CIRCLE){

            param3=Convert.ToDouble(second);
            //secondParam= new Num(new Token(TokenTypes.NUMBER,param3));
        }
        else{
            param3= ((POINT)second).param1;
            param4=((POINT)second).param3;
            //param3= (double)((Num)((POINT)second).firstParam).Value;
            //param4=(double)((Num)((POINT)second).secondParam).Value;
            //secondParam=new POINT(null,new Num(new Token(TokenTypes.NUMBER,param3)),new Num(new Token(TokenTypes.NUMBER,param4)));
        }

    }
   
    
}

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
        return (string)Token.Value;
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
        return (!(Token is null))?(string)Token.Value :"";
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
        
        return (!(Token is null))?(string)Token.Value :"";
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
        return (!(Token is null))?(string)Token.Value:"";
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
        return (!(Token is null))?(string)Token.Value:"";
    }
}

#endregion


#region Color

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



#endregion