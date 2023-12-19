namespace GOLenguage;
using Microsoft.JSInterop;


// espacio para guardar las clases de los tipos de Variables (Num,Cadene,Bool) ,Figure (point,circle...) y Color

#region SuperClass Variables

public class Variables : AST
{

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


public interface IDraw
{


    void Draw(string tag);
    //object Function();

}



public class FIGURE : Variables
{

    public Token Token;
    public AST firstParam;
    public AST secondParam;
    public AST thirdParam;
    public AST measure;
    public double param1;
    public double param2;
    public double param3;
    public double param4;
    public double param5;
    public double param6;
    public double param7;

    public FIGURE(Token name = null, AST firstParam = null, AST secondParam = null)
    {

        Token = name;
        this.firstParam = firstParam;
        this.secondParam = secondParam;

        if (!(name is null))
        {
            // (ejemplos )point c3; circle c2; line s; segment y
            Random rnd = new Random();
            param1 = rnd.Next(0, 400);
            param2 = rnd.Next(0, 400);
            param3 = rnd.Next(0, 400);
            param4 = rnd.Next(0, 400);
            param5 = rnd.Next(0, 400);
            param6 = rnd.Next(0, 400);
            param7 = rnd.Next(0, 400);
        }

        //Value=Clone();


    }

    public FIGURE Clone()
    {
        return (FIGURE)MemberwiseClone();
    }

    public void Check(object first, object second, object third, object measure)
    {


        if (this is POINT)
        {

            param1 = Convert.ToDouble(first);


        }
        else
        {
            param1 = ((POINT)first).param1;
            param2 = ((POINT)first).param3;
            param5 = (third is null) ? 0 : ((POINT)third).param1;
        }

        if (this is POINT || this is CIRCLE)
        {

            param3 = Convert.ToDouble(second);

        }
        else
        {

            param3 = ((POINT)second).param1;
            param4 = ((POINT)second).param3;
            param6 = (third is null) ? 0 : ((POINT)third).param3;
            param7 = (third is null) ? 0 : Convert.ToDouble(measure);
        }

    }


}

public class POINT : FIGURE, IDraw
{

    public POINT(Token name, AST firstParam, AST secondParam) : base(name, firstParam, secondParam) { 

        if(firstParam is Num){
            param1=(double)((Num)firstParam).Value;
        }
        if(secondParam is Num){
            param3=(double)((Num)secondParam).Value;
        }
    }

    public async void Draw(string tag)
    {

        Console.WriteLine($"Drawing a point ... ");
        Console.WriteLine($"{param1};{param3}");
        var parameters = new
        {
            param1,
            param3,
            tag,
        };
        await Principal._jsRuntime.InvokeAsync<object>("DibujarePoint", parameters);
    }

}

public class CIRCLE : FIGURE, IDraw
{

    public CIRCLE(Token name, AST firstParam, AST secondParam) : base(name, firstParam, secondParam) { }
    public async void Draw(string tag)
    {


        Console.WriteLine($"Drawing a circle ... ");
        Console.WriteLine($"{param1};{param2};{param3}");
        var parameters = new
        {
            param1,
            param2,
            param3,
            tag
        };
        await Principal._jsRuntime.InvokeAsync<object>("DibujareEllipse", parameters);
    }

}

public class LINE : FIGURE, IDraw
{

    public LINE(Token name, AST firstParam, AST secondParam) : base(name, firstParam, secondParam) { }

    // expresion que constituye una nueva lista de instrucciones

    public async void Draw(string tag)
    {


        Console.WriteLine($"Drawing a line ... ");

        var parameters = new
        {
            param1,
            param2,
            param3,
            param4,
            tag
        };

        Console.WriteLine($"{param1};{param2};{param3};{param4}");
        await Principal._jsRuntime.InvokeAsync<object>("DibujarLine", parameters);
    }


}
public class SEGMENT : FIGURE, IDraw
{


    public SEGMENT(Token name, AST firstParam, AST secondParam) : base(name, firstParam, secondParam) { }

    public async void Draw(string tag)
    {


        Console.WriteLine($"Drawing a segment ... ");

        var parameters = new
        {
            param1,
            param2,
            param3,
            param4,
            tag
        };

        Console.WriteLine($"{param1};{param2};{param3};{param4}");
        await Principal._jsRuntime.InvokeAsync<object>("DibujarSegment", parameters);
    }


}

public class RAY : FIGURE, IDraw
{


    public RAY(Token name, AST firstParam, AST secondParam) : base(name, firstParam, secondParam) { }

    public async void Draw(string tag)
    {


        Console.WriteLine($"Drawing a ray ... ");

        var parameters = new
        {
            param1,
            param2,
            param3,
            param4,
            tag
        };

        Console.WriteLine($"{param1};{param2};{param3};{param4}");
        await Principal._jsRuntime.InvokeAsync<object>("DibujarRay", parameters);
    }


}

public class ARC : FIGURE, IDraw
{

    public ARC(Token name, AST firstParam, AST secondParam, AST thirdParam, AST measure) : base(name, firstParam, secondParam)
    {
        this.thirdParam = thirdParam;
        this.measure = measure;

    }
    public async void Draw(string tag)
    {


        Console.WriteLine($"Drawing an arc ... ");
        Console.WriteLine(param6);
        var parameters = new
        {
            param1,
            param2,
            param3,
            param4,
            param5,
            param6,
            param7,
            tag
        };

        Console.WriteLine($"{param1};{param2};{param3};{param4};{param5};{param6};{param7}");
        await Principal._jsRuntime.InvokeAsync<object>("DibujarArc", parameters);
    }
}
#endregion


#region Color

public class COLOR : AST
{

    public ColorType color { get; }
    public TokenTypes token { get; }
    public static Stack<ColorType> stackColor = new Stack<ColorType>();
    public COLOR(TokenTypes token, ColorType color = ColorType.black)

    {
        this.token = token;
        this.color = color;
    }


    public async void Push()
    {

        stackColor.Push(color);
        Console.WriteLine($"Changed color to {color} ... ");

        await Principal._jsRuntime.InvokeAsync<object>("Color", color.ToString());
    }

    public async void Restore()
    {


        if (stackColor.Count > 0)
        {

            Console.WriteLine($"{stackColor.Peek().ToString()} color has been drawn from the stack");
            stackColor.Pop();

            if (stackColor.Count == 0)
            {
                await Principal._jsRuntime.InvokeAsync<object>("Color", "black");
            }

            else
            {


                await Principal._jsRuntime.InvokeAsync<object>("Color", stackColor.Peek().ToString());
            }

        }
    }

}



#endregion