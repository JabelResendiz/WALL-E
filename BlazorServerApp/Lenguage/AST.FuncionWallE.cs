
using System.Runtime.CompilerServices;

namespace GOLenguage;


#region FunctionWAllE


public class FunctionWALLE : AST
{
    public AST first;
    public AST second;
    public Token token;

    public FunctionWALLE(Token token, AST first, AST second)
    {

        this.token = token;
        this.first = first;
        this.second = second;
    }

    

    public double Measure(POINT p1, POINT p2)
    {

        return Math.Sqrt(Math.Pow(p1.param1 - p2.param1, 2) + Math.Pow(p1.param3 - p2.param3, 2));
    }

    public object Count(SEQUENCE2 seq)=>seq.counter;
    public InfiniteSequence2 Random(){

        if(!Interpreter.Scope.ContainsKey("randoms")){
            Interpreter.Scope.Add("randoms",new RandomNumber());
        }
        
        return (RandomNumber)Interpreter.Scope["randoms"];
    }
    public InfiniteSequence2 Samples(){

        if(!Interpreter.Scope.ContainsKey("samples")){
            Interpreter.Scope.Add("samples",new SequencePointSamples());
        }
        
        return (SequencePointSamples)Interpreter.Scope["samples"];
    }
    public InfiniteSequence2 PointsRandomInFigure(FIGURE figure){

        if(!Interpreter.Scope.ContainsKey("points")){
            Interpreter.Scope.Add("points",new SequencePointsInFigure(figure));
        }
        
        return (SequencePointsInFigure)Interpreter.Scope["points"];
    }
    public List<object>Intersect(FIGURE f1, FIGURE f2)
    {

        List<object> lista = new List<object>();


        if ((int)Enum.Parse(typeof(WalleFigures), f1.ToString()!.Substring(11)) > (int)Enum.Parse(typeof(WalleFigures), f2.ToString()!.Substring(11)))
        {

            FIGURE s = f1;
            f1 = f2;
            f2 = s;
        }


        if (f1 is POINT)
        {
            if (f2 is POINT && ((POINT)f1).param1 == ((POINT)f2).param1 && ((POINT)f1).param3 == ((POINT)f2).param3)
            {

                lista.Add((POINT)f1.Clone());

            }

            else if (f2 is CIRCLE)
            {

                if (Math.Pow(((POINT)f1).param1 - ((CIRCLE)f2).param1, 2) + Math.Pow(((POINT)f1).param3 - ((CIRCLE)f2).param2, 2) == Math.Pow(((CIRCLE)f2).param3, 2))
                {
                    lista.Add((POINT)f1.Clone());
                }
            }

            else if (f2 is SEGMENT)
            {

                double minAbsc = Math.Min(((SEGMENT)f2).param1, ((SEGMENT)f2).param3);
                double maxAbsc = Math.Max(((SEGMENT)f2).param1, ((SEGMENT)f2).param3);
                if (((POINT)f1).param1 >= minAbsc && ((POINT)f1).param1 <= maxAbsc)
                {

                    if (((SEGMENT)f2).param1 == ((SEGMENT)f2).param3 && ((POINT)f1).param1 == ((SEGMENT)f2).param1 && ((POINT)f1).param2 == ((SEGMENT)f2).param2)
                    {
                        lista.Add((POINT)f1.Clone());
                    }
                    else if (((SEGMENT)f2).param1 != ((SEGMENT)f2).param3)
                    {
                        double m = (((SEGMENT)f2).param2 - ((SEGMENT)f2).param4) / (((SEGMENT)f2).param1 - ((SEGMENT)f2).param3);

                        double n = ((SEGMENT)f2).param2 - m * ((SEGMENT)f2).param1;

                        if (((POINT)f1).param1 * m + n == ((POINT)f1).param3)
                        {
                            lista.Add((POINT)f1.Clone());
                        }
                    }
                }

            }

            else if (f2 is LINE)
            {


                if (((LINE)f2).param1 == ((LINE)f2).param3 && ((POINT)f1).param1 == ((LINE)f2).param1 && ((POINT)f1).param2 == ((LINE)f2).param4)
                {
                    lista.Add((POINT)f1.Clone());
                }
                else if (((LINE)f2).param1 != ((LINE)f2).param3)
                {
                    double m = (((LINE)f2).param2 - ((LINE)f2).param4) / (((LINE)f2).param1 - ((LINE)f2).param3);

                    double n = ((LINE)f2).param2 - m * ((LINE)f2).param1;

                    if (((POINT)f1).param1 * m + n == ((POINT)f1).param3)
                    {
                        lista.Add((POINT)f1.Clone());
                    }
                }
            }


            else if (f2 is RAY)
            {


                if (((RAY)f2).param1 == ((RAY)f2).param3 && ((POINT)f1).param1 == ((RAY)f2).param1)
                {



                    if ((((RAY)f2).param2 == ((RAY)f2).param4 && ((POINT)f1).param2 == ((RAY)f2).param2) || (((RAY)f2).param2 > ((RAY)f2).param4 && ((POINT)f1).param2 <= ((RAY)f2).param2) || (((RAY)f2).param2 < ((RAY)f2).param4 && ((POINT)f1).param2 >= ((RAY)f2).param2))
                    {// vector director negativo


                        lista.Add((POINT)f1.Clone());

                    }


                }
                else if (((LINE)f2).param1 != ((LINE)f2).param3)
                {

                    double m = (((LINE)f2).param2 - ((LINE)f2).param4) / (((LINE)f2).param1 - ((LINE)f2).param3);

                    double n = ((LINE)f2).param2 - m * ((LINE)f2).param1;

                    if (((POINT)f1).param1 * m + n == ((POINT)f1).param3)
                    {

                        if ((((RAY)f2).param1 > ((RAY)f2).param3 && ((POINT)f1).param1 <= ((RAY)f2).param1) || (((RAY)f2).param1 < ((RAY)f2).param3 && ((POINT)f1).param1 >= ((RAY)f2).param1))
                        {// vector director negativo


                            lista.Add((POINT)f1.Clone());

                        }
                    }
                }

            }

        }


        else if (f1 is LINE)
        {

            if (f2 is SEGMENT)
            {


                double minAbsc = Math.Min(((SEGMENT)f2).param1, ((SEGMENT)f2).param3);
                double maxAbsc = Math.Max(((SEGMENT)f2).param1, ((SEGMENT)f2).param3);


                if (((LINE)f1).param1 - ((LINE)f1).param3 != 0 && ((SEGMENT)f2).param1 - ((SEGMENT)f2).param3 == 0)

                {
                    double mRect = (((LINE)f1).param2 - ((LINE)f1).param4) / (((LINE)f1).param1 - ((LINE)f1).param3);
                    double nRect = ((LINE)f1).param2 - mRect * ((LINE)f1).param1;

                    double y = mRect * ((SEGMENT)f2).param1 + nRect;



                    if (y >= Math.Min(((SEGMENT)f2).param2, ((SEGMENT)f2).param4) && y <= Math.Max(((SEGMENT)f2).param2, ((SEGMENT)f2).param4))
                    {

                        lista.Add(new POINT(null, new Num(new Token(TokenTypes.NUMBER, ((SEGMENT)f2).param3)), new Num(new Token(TokenTypes.NUMBER, y))));
                    }
                }
                else if (((LINE)f1).param1 - ((LINE)f1).param3 == 0 && ((SEGMENT)f2).param1 - ((SEGMENT)f2).param3 != 0)

                {
                    double mSeg = (((SEGMENT)f2).param2 - ((SEGMENT)f2).param4) / (((SEGMENT)f2).param1 - ((SEGMENT)f2).param3);
                    double nSeg = ((SEGMENT)f2).param2 - mSeg * ((SEGMENT)f2).param1;

                    double y = mSeg * ((LINE)f1).param1 + nSeg;



                    if (((LINE)f1).param1 >= minAbsc && ((LINE)f1).param1 <= maxAbsc)
                    {

                        lista.Add(new POINT(null, new Num(new Token(TokenTypes.NUMBER, ((LINE)f1).param1)), new Num(new Token(TokenTypes.NUMBER, y))));
                    }
                }

                else if (((LINE)f1).param1 == ((LINE)f1).param3 && ((SEGMENT)f2).param1 == ((SEGMENT)f2).param3 && ((SEGMENT)f2).param1 == ((LINE)f1).param1)
                {
                    lista.Add((SEGMENT)f2.Clone());

                }

                else if (((LINE)f1).param1 - ((LINE)f1).param3 != 0 && ((SEGMENT)f2).param1 - ((SEGMENT)f2).param3 != 0)
                {


                    double mRect = (((LINE)f1).param2 - ((LINE)f1).param4) / (((LINE)f1).param1 - ((LINE)f1).param3);

                    // pendiente dividiendo por 0
                    double nRect = ((LINE)f1).param2 - mRect * ((LINE)f1).param1;

                    double mSeg = (((SEGMENT)f2).param2 - ((SEGMENT)f2).param4) / (((SEGMENT)f2).param1 - ((SEGMENT)f2).param3);

                    double nSeg = ((SEGMENT)f2).param2 - mSeg * ((SEGMENT)f2).param1;

                    if (mRect == mSeg && nRect == nSeg)
                    {

                        lista.Add((SEGMENT)f2.Clone());

                    }
                    else if (mRect != nSeg)
                    {

                        double xIntersection = (nSeg - nRect) / (mRect - mSeg);

                        double yIntersection = mRect * xIntersection + nRect;

                        if (xIntersection >= minAbsc && xIntersection <= maxAbsc)
                        {

                            lista.Add(new POINT(null, new Num(new Token(TokenTypes.NUMBER, xIntersection)), new Num(new Token(TokenTypes.NUMBER, yIntersection))));
                        }
                    }
                }
            }

            else if (f2 is LINE)
            {

                if (((LINE)f1).param1 == ((LINE)f1).param3 && ((LINE)f2).param1 == ((LINE)f2).param3 && ((LINE)f2).param1 == ((LINE)f1).param1)
                {

                    lista.Add((LINE)f2.Clone());
                }
                else if (((LINE)f1).param1 != ((LINE)f1).param3 || ((LINE)f2).param1 != ((LINE)f2).param3)
                {
                    if (((LINE)f1).param1 == ((LINE)f1).param3)
                    {
                        double mRectf2 = (((LINE)f2).param2 - ((LINE)f2).param4) / (((LINE)f2).param1 - ((LINE)f2).param3);

                        double nRectf2 = ((LINE)f2).param2 - mRectf2 * ((LINE)f2).param1;

                        double y = mRectf2 * ((LINE)f1).param1 + nRectf2;

                        lista.Add(new POINT(null, new Num(new Token(TokenTypes.NUMBER, ((LINE)f1).param1)), new Num(new Token(TokenTypes.NUMBER, y))));
                    }

                    else if (((LINE)f2).param1 == ((LINE)f2).param3)
                    {

                        double mRectf1 = (((LINE)f1).param2 - ((LINE)f1).param4) / (((LINE)f1).param1 - ((LINE)f1).param3);

                        double nRectf1 = ((LINE)f1).param2 - mRectf1 * ((LINE)f1).param1;

                        double y = mRectf1 * ((LINE)f2).param1 + nRectf1;

                        lista.Add(new POINT(null, new Num(new Token(TokenTypes.NUMBER, ((LINE)f2).param1)), new Num(new Token(TokenTypes.NUMBER, y))));

                    }
                    else if (((LINE)f1).param1 != ((LINE)f1).param3 && ((LINE)f2).param1 != ((LINE)f2).param3)
                    {

                        double mRectf1 = (((LINE)f1).param2 - ((LINE)f1).param4) / (((LINE)f1).param1 - ((LINE)f1).param3);

                        double nRectf1 = ((LINE)f1).param2 - mRectf1 * ((LINE)f1).param1;

                        double mRectf2 = (((LINE)f2).param2 - ((LINE)f2).param4) / (((LINE)f2).param1 - ((LINE)f2).param3);

                        double nRectf2 = ((LINE)f2).param2 - mRectf2 * ((LINE)f2).param1;


                        if (mRectf1 == mRectf2 && nRectf1 == nRectf2)
                        {
                            lista.Add((LINE)f2.Clone());

                        }
                        else if (mRectf1 != mRectf2)
                        {

                            double xIntersection = (nRectf2 - nRectf1) / (mRectf1 - mRectf2);

                            double yIntersection = mRectf1 * xIntersection + nRectf1;

                            lista.Add(new POINT(null, new Num(new Token(TokenTypes.NUMBER, xIntersection)), new Num(new Token(TokenTypes.NUMBER, yIntersection))));

                        }
                    }
                }


            }


            else if (f2 is CIRCLE)
            {

                // pendiente de la recta

                if (((LINE)f1).param1 != ((LINE)f1).param3)
                {

                    double m = (((LINE)f1).param2 - ((LINE)f1).param4) / (((LINE)f1).param1 - ((LINE)f1).param3);


                    double n = ((LINE)f1).param2 - m * ((LINE)f1).param1;

                    double a = Math.Pow(m, 2) + 1;
                    double b = 2 * m * n - 2 * ((CIRCLE)f2).param1 - 2 * m * ((CIRCLE)f2).param2;
                    double c = Math.Pow(((CIRCLE)f2).param1, 2) - Math.Pow(((CIRCLE)f2).param3, 2) + Math.Pow(n - ((CIRCLE)f2).param2, 2);


                    if (b * b - 4 * a * c >= 0)
                    {
                        double x = (-b + Math.Sqrt(b * b - 4 * a * c)) / (2 * a);
                        double x41 = (-b - Math.Sqrt(b * b - 4 * a * c)) / (2 * a);

                        double y = m * x + n;
                        double y41 = m * x41 + n;


                        lista.Add(new POINT(null, new Num(new Token(TokenTypes.NUMBER, x)), new Num(new Token(TokenTypes.NUMBER, y))));

                        if (y != y41)
                        {

                            lista.Add(new POINT(null, new Num(new Token(TokenTypes.NUMBER, x41)), new Num(new Token(TokenTypes.NUMBER, y41))));
                        }
                    }
                }

                else
                {

                    if (Math.Pow(((CIRCLE)f2).param3, 2) - Math.Pow(((CIRCLE)f2).param1 - ((LINE)f1).param1, 2) >= 0)
                    {
                        double y = Math.Sqrt(Math.Pow(((CIRCLE)f2).param3, 2) - Math.Pow(((CIRCLE)f2).param1 - ((LINE)f1).param1, 2)) + ((CIRCLE)f2).param2;

                        double y41 = -Math.Sqrt(Math.Pow(((CIRCLE)f2).param3, 2) - Math.Pow(((CIRCLE)f2).param1 - ((LINE)f1).param1, 2)) + ((CIRCLE)f2).param2;

                        lista.Add(new POINT(null, new Num(new Token(TokenTypes.NUMBER, ((LINE)f1).param1)), new Num(new Token(TokenTypes.NUMBER, y))));

                        if (y != y41)
                        {

                            lista.Add(new POINT(null, new Num(new Token(TokenTypes.NUMBER, ((LINE)f1).param1)), new Num(new Token(TokenTypes.NUMBER, y41))));
                        }
                    }


                }

            }


            else if (f2 is RAY)
            {

                if (((LINE)f1).param1 == ((LINE)f1).param3 && ((RAY)f2).param1 == ((RAY)f2).param3 && ((RAY)f2).param1 == ((LINE)f1).param1)
                {

                    lista.Add((RAY)f2.Clone());
                }
                else if (((LINE)f1).param1 != ((LINE)f1).param3 || ((RAY)f2).param1 != ((RAY)f2).param3)
                {
                    if (((LINE)f1).param1 == ((LINE)f1).param3)
                    {
                        double mRectf2 = (((RAY)f2).param2 - ((RAY)f2).param4) / (((RAY)f2).param1 - ((RAY)f2).param3);

                        double nRectf2 = ((RAY)f2).param2 - mRectf2 * ((RAY)f2).param1;

                        double y = mRectf2 * ((LINE)f1).param1 + nRectf2;
                        if ((((RAY)f2).param1 > ((RAY)f2).param3 && ((LINE)f1).param1 <= ((RAY)f2).param1) || (((RAY)f2).param1 < ((RAY)f2).param3 && ((LINE)f1).param1 >= ((RAY)f2).param1))
                            lista.Add(new POINT(null, new Num(new Token(TokenTypes.NUMBER, ((LINE)f1).param1)), new Num(new Token(TokenTypes.NUMBER, y))));
                    }

                    else if (((RAY)f2).param1 == ((RAY)f2).param3)
                    {

                        double mRectf1 = (((LINE)f1).param2 - ((LINE)f1).param4) / (((LINE)f1).param1 - ((LINE)f1).param3);

                        double nRectf1 = ((LINE)f1).param2 - mRectf1 * ((LINE)f1).param1;

                        double y = mRectf1 * ((RAY)f2).param1 + nRectf1;

                        if ((((RAY)f2).param2 > ((RAY)f2).param4 && y <= ((RAY)f2).param2) || (((RAY)f2).param2 < ((RAY)f2).param4 && y >= ((RAY)f2).param2))
                            lista.Add(new POINT(null, new Num(new Token(TokenTypes.NUMBER, ((RAY)f2).param1)), new Num(new Token(TokenTypes.NUMBER, y))));

                    }
                    else if (((LINE)f1).param1 != ((LINE)f1).param3 && ((RAY)f2).param1 != ((RAY)f2).param3)
                    {

                        double mRectf1 = (((LINE)f1).param2 - ((LINE)f1).param4) / (((LINE)f1).param1 - ((LINE)f1).param3);

                        double nRectf1 = ((LINE)f1).param2 - mRectf1 * ((LINE)f1).param1;

                        double mRectf2 = (((RAY)f2).param2 - ((RAY)f2).param4) / (((RAY)f2).param1 - ((RAY)f2).param3);

                        double nRectf2 = ((RAY)f2).param2 - mRectf2 * ((RAY)f2).param1;


                        if (mRectf1 == mRectf2 && nRectf1 == nRectf2)
                        {
                            lista.Add((RAY)f1.Clone());

                        }
                        else if (mRectf1 != mRectf2)
                        {

                            double xIntersection = (nRectf2 - nRectf1) / (mRectf1 - mRectf2);

                            double yIntersection = mRectf1 * xIntersection + nRectf1;

                            if ((((RAY)f2).param1 > ((RAY)f2).param3 && xIntersection <= ((RAY)f2).param1) || (((RAY)f2).param1 < ((RAY)f2).param3 && xIntersection >= ((RAY)f2).param1))
                                lista.Add(new POINT(null, new Num(new Token(TokenTypes.NUMBER, xIntersection)), new Num(new Token(TokenTypes.NUMBER, yIntersection))));

                        }
                    }
                }

            }
        }



        else if (f1 is SEGMENT)
        {

            double minAbsc = Math.Min(((SEGMENT)f1).param1, ((SEGMENT)f1).param3);
            double maxAbsc = Math.Max(((SEGMENT)f1).param1, ((SEGMENT)f1).param3);

            double minAbscY = Math.Min(((SEGMENT)f1).param1, ((SEGMENT)f1).param3);
            double maxAbscY = Math.Max(((SEGMENT)f1).param1, ((SEGMENT)f1).param3);

            if (f2 is SEGMENT)
            {

                if (((SEGMENT)f1).param1 == ((SEGMENT)f1).param3 && ((SEGMENT)f2).param1 == ((SEGMENT)f2).param3 && ((SEGMENT)f2).param1 == ((SEGMENT)f1).param1 && ((SEGMENT)f2).param2 == ((SEGMENT)f1).param2)
                {

                    lista.Add((SEGMENT)f1.Clone());
                }
                else if (((SEGMENT)f1).param1 != ((SEGMENT)f1).param3 || ((SEGMENT)f2).param1 != ((SEGMENT)f2).param3)
                {
                    if (((SEGMENT)f1).param1 == ((SEGMENT)f1).param3)
                    {

                        double mRectf2 = (((SEGMENT)f2).param2 - ((SEGMENT)f2).param4) / (((SEGMENT)f2).param1 - ((SEGMENT)f2).param3);

                        double nRectf2 = ((SEGMENT)f2).param2 - mRectf2 * ((SEGMENT)f2).param1;

                        double y = mRectf2 * ((SEGMENT)f1).param1 + nRectf2;

                        double MinYf1 = Math.Min(((SEGMENT)f1).param2, ((SEGMENT)f1).param4);
                        double MaxYf1 = Math.Max(((SEGMENT)f1).param2, ((SEGMENT)f1).param4);

                        double MinYf2 = Math.Min(((SEGMENT)f2).param2, ((SEGMENT)f2).param4);
                        double MaxYf2 = Math.Max(((SEGMENT)f2).param2, ((SEGMENT)f2).param4);

                        if (y >= Math.Max(MinYf1, MinYf2) && y <= Math.Min(MaxYf1, MaxYf2))
                            lista.Add(new POINT(null, new Num(new Token(TokenTypes.NUMBER, ((SEGMENT)f1).param1)), new Num(new Token(TokenTypes.NUMBER, y))));
                    }

                    else if (((SEGMENT)f2).param1 == ((SEGMENT)f2).param3)
                    {

                        return Intersect(f2, f1);
                    }
                    else if (((SEGMENT)f1).param1 != ((SEGMENT)f1).param3 && ((SEGMENT)f2).param1 != ((SEGMENT)f2).param3)
                    {

                        double mRectf1 = (((SEGMENT)f1).param2 - ((SEGMENT)f1).param4) / (((SEGMENT)f1).param1 - ((SEGMENT)f1).param3);

                        double nRectf1 = ((SEGMENT)f1).param2 - mRectf1 * ((SEGMENT)f1).param1;

                        double mRectf2 = (((SEGMENT)f2).param2 - ((SEGMENT)f2).param4) / (((SEGMENT)f2).param1 - ((SEGMENT)f2).param3);

                        double nRectf2 = ((SEGMENT)f2).param2 - mRectf2 * ((SEGMENT)f2).param1;


                        if (mRectf1 == mRectf2 && nRectf1 == nRectf2)
                        {
                            lista.Add((SEGMENT)f1.Clone());

                        }
                        else if (mRectf1 != mRectf2)
                        {

                            double xIntersection = (nRectf2 - nRectf1) / (mRectf1 - mRectf2);

                            double yIntersection = mRectf1 * xIntersection + nRectf1;

                            double MinXf2 = Math.Min(((SEGMENT)f2).param1, ((SEGMENT)f2).param3);
                            double MaxXf2 = Math.Max(((SEGMENT)f2).param1, ((SEGMENT)f2).param3);

                            if (xIntersection >= Math.Max(minAbsc, MinXf2) && xIntersection <= Math.Min(maxAbsc, MaxXf2))
                                lista.Add(new POINT(null, new Num(new Token(TokenTypes.NUMBER, xIntersection)), new Num(new Token(TokenTypes.NUMBER, yIntersection))));

                        }
                    }
                }


            }
            else if (f2 is CIRCLE)
            {

                if (((SEGMENT)f1).param1 != ((SEGMENT)f1).param3)
                {

                    double m = (((SEGMENT)f1).param2 - ((SEGMENT)f1).param4) / (((SEGMENT)f1).param1 - ((SEGMENT)f1).param3);


                    double n = ((SEGMENT)f1).param2 - m * ((SEGMENT)f1).param1;

                    double a = Math.Pow(m, 2) + 1;
                    double b = 2 * m * n - 2 * ((CIRCLE)f2).param1 - 2 * m * ((CIRCLE)f2).param2;
                    double c = Math.Pow(((CIRCLE)f2).param1, 2) - Math.Pow(((CIRCLE)f2).param3, 2) + Math.Pow(n - ((CIRCLE)f2).param2, 2);


                    if (b * b - 4 * a * c >= 0)
                    {
                        double x = (-b + Math.Sqrt(b * b - 4 * a * c)) / (2 * a);
                        double x41 = (-b - Math.Sqrt(b * b - 4 * a * c)) / (2 * a);

                        double y = m * x + n;
                        double y41 = m * x41 + n;




                        if (x >= minAbsc && x <= maxAbsc)
                        {// vector director negativo


                            lista.Add(new POINT(null, new Num(new Token(TokenTypes.NUMBER, x)), new Num(new Token(TokenTypes.NUMBER, y))));

                        }

                        if (x != x41 && x41 >= minAbsc && x41 <= maxAbsc)
                        {// vector director negativo


                            lista.Add(new POINT(null, new Num(new Token(TokenTypes.NUMBER, x41)), new Num(new Token(TokenTypes.NUMBER, y41))));

                        }


                    }
                }

                else
                {

                    if (Math.Pow(((CIRCLE)f2).param3, 2) - Math.Pow(((CIRCLE)f2).param1 - ((SEGMENT)f1).param1, 2) >= 0)
                    {
                        double y = Math.Sqrt(Math.Pow(((CIRCLE)f2).param3, 2) - Math.Pow(((CIRCLE)f2).param1 - ((SEGMENT)f1).param1, 2)) + ((CIRCLE)f2).param2;

                        double y41 = -Math.Sqrt(Math.Pow(((CIRCLE)f2).param3, 2) - Math.Pow(((CIRCLE)f2).param1 - ((SEGMENT)f1).param1, 2)) + ((CIRCLE)f2).param2;

                        double MinY = Math.Min(((SEGMENT)f1).param2, ((SEGMENT)f2).param4);
                        double MaxY = Math.Max(((SEGMENT)f1).param2, ((SEGMENT)f2).param4);

                        if (y >= MinY && y <= MaxY)
                        {// vector director negativo


                            lista.Add(new POINT(null, new Num(new Token(TokenTypes.NUMBER, ((SEGMENT)f1).param1)), new Num(new Token(TokenTypes.NUMBER, y))));

                        }

                        if (y != y41 && y41 >= MinY && y41 <= MaxY)
                        {

                            lista.Add(new POINT(null, new Num(new Token(TokenTypes.NUMBER, ((SEGMENT)f1).param1)), new Num(new Token(TokenTypes.NUMBER, y41))));

                        }
                    }


                }

            }

            else if (f2 is RAY)
            {


                if (((SEGMENT)f1).param1 == ((SEGMENT)f1).param3 && ((RAY)f2).param1 == ((RAY)f2).param3 && ((RAY)f2).param1 == ((SEGMENT)f1).param1 && ((RAY)f2).param4 <= maxAbscY && ((RAY)f2).param4 >= minAbscY)
                {

                    lista.Add((SEGMENT)f1.Clone());
                }
                else if (((SEGMENT)f1).param1 != ((SEGMENT)f1).param3 || ((RAY)f2).param1 != ((RAY)f2).param3)
                {
                    if (((SEGMENT)f1).param1 == ((SEGMENT)f1).param3)
                    {
                        double mRectf2 = (((RAY)f2).param2 - ((RAY)f2).param4) / (((RAY)f2).param1 - ((RAY)f2).param3);

                        double nRectf2 = ((RAY)f2).param2 - mRectf2 * ((RAY)f2).param1;

                        double y = mRectf2 * ((SEGMENT)f1).param1 + nRectf2;

                        if ((((RAY)f2).param1 > ((RAY)f2).param3 && ((SEGMENT)f1).param1 <= ((RAY)f2).param1 && y <= maxAbscY && y >= minAbscY) || (((RAY)f2).param1 < ((RAY)f2).param3 && ((SEGMENT)f1).param1 >= ((RAY)f2).param1 && y <= maxAbscY && y >= minAbscY))
                            lista.Add(new POINT(null, new Num(new Token(TokenTypes.NUMBER, ((SEGMENT)f1).param1)), new Num(new Token(TokenTypes.NUMBER, y))));
                    }

                    else if (((RAY)f2).param1 == ((RAY)f2).param3)
                    {

                        double mRectf1 = (((SEGMENT)f1).param2 - ((SEGMENT)f1).param4) / (((SEGMENT)f1).param1 - ((SEGMENT)f1).param3);

                        double nRectf1 = ((SEGMENT)f1).param2 - mRectf1 * ((SEGMENT)f1).param1;

                        double y = mRectf1 * ((RAY)f2).param1 + nRectf1;

                        if ((((RAY)f2).param2 > ((RAY)f2).param4 && y <= ((RAY)f2).param2 && ((RAY)f2).param1 >= minAbsc && ((RAY)f2).param1 <= maxAbsc) || (((RAY)f2).param2 < ((RAY)f2).param4 && y >= ((RAY)f2).param2 && ((RAY)f2).param1 >= minAbsc && ((RAY)f2).param1 <= maxAbsc))
                            lista.Add(new POINT(null, new Num(new Token(TokenTypes.NUMBER, ((RAY)f2).param1)), new Num(new Token(TokenTypes.NUMBER, y))));

                    }
                    else if (((SEGMENT)f1).param1 != ((SEGMENT)f1).param3 && ((RAY)f2).param1 != ((RAY)f2).param3)
                    {

                        double mRectf1 = (((SEGMENT)f1).param2 - ((SEGMENT)f1).param4) / (((SEGMENT)f1).param1 - ((SEGMENT)f1).param3);

                        double nRectf1 = ((SEGMENT)f1).param2 - mRectf1 * ((SEGMENT)f1).param1;

                        double mRectf2 = (((RAY)f2).param2 - ((RAY)f2).param4) / (((RAY)f2).param1 - ((RAY)f2).param3);

                        double nRectf2 = ((RAY)f2).param2 - mRectf2 * ((RAY)f2).param1;


                        if (mRectf1 == mRectf2 && nRectf1 == nRectf2)
                        {

                            double RayMinX = Math.Min(((RAY)f2).param1, ((RAY)f2).param3);
                            double RayMaxX = Math.Max(((RAY)f2).param1, ((RAY)f2).param3);


                            if ((((RAY)f2).param1 >= maxAbsc && RayMinX == ((RAY)f2).param3) || (((RAY)f2).param1 <= minAbsc && RayMaxX == ((RAY)f2).param3))
                            {
                                lista.Add((SEGMENT)f1.Clone());
                            }
                            else if (((RAY)f2).param1 <= maxAbsc && ((RAY)f2).param1 >= maxAbsc)
                            {

                                lista.Add((SEGMENT)f1.Clone());
                            }

                        }
                        else if (mRectf1 != mRectf2)
                        {

                            double xIntersection = (nRectf2 - nRectf1) / (mRectf1 - mRectf2);

                            double yIntersection = mRectf1 * xIntersection + nRectf1;

                            if ((((RAY)f2).param1 > ((RAY)f2).param3 && xIntersection <= ((RAY)f2).param1 && xIntersection <= maxAbsc && xIntersection >= minAbsc) || (((RAY)f2).param1 < ((RAY)f2).param3 && xIntersection >= ((RAY)f2).param1 && xIntersection <= maxAbsc && xIntersection >= minAbsc))
                                lista.Add(new POINT(null, new Num(new Token(TokenTypes.NUMBER, xIntersection)), new Num(new Token(TokenTypes.NUMBER, yIntersection))));

                        }
                    }
                }

            }
        }


        else if (f1 is RAY)
        {
            Console.WriteLine("lololololo");


            if (f2 is CIRCLE)
            {

                if (((RAY)f1).param1 != ((RAY)f1).param3)
                {

                    double m = (((RAY)f1).param2 - ((RAY)f1).param4) / (((RAY)f1).param1 - ((RAY)f1).param3);


                    double n = ((RAY)f1).param2 - m * ((RAY)f1).param1;

                    double a = Math.Pow(m, 2) + 1;
                    double b = 2 * m * n - 2 * ((CIRCLE)f2).param1 - 2 * m * ((CIRCLE)f2).param2;
                    double c = Math.Pow(((CIRCLE)f2).param1, 2) - Math.Pow(((CIRCLE)f2).param3, 2) + Math.Pow(n - ((CIRCLE)f2).param2, 2);


                    if (b * b - 4 * a * c >= 0)
                    {
                        double x = (-b + Math.Sqrt(b * b - 4 * a * c)) / (2 * a);
                        double x41 = (-b - Math.Sqrt(b * b - 4 * a * c)) / (2 * a);

                        double y = m * x + n;
                        double y41 = m * x41 + n;



                        if ((((RAY)f1).param1 > ((RAY)f1).param3 && x <= ((RAY)f1).param1) || (((RAY)f1).param1 < ((RAY)f1).param3 && x >= ((RAY)f1).param1))
                        {// vector director negativo


                            lista.Add(new POINT(null, new Num(new Token(TokenTypes.NUMBER, x)), new Num(new Token(TokenTypes.NUMBER, y))));

                        }

                        if (x != x41 && ((((RAY)f1).param1 > ((RAY)f1).param3 && x41 <= ((RAY)f1).param1) || (((RAY)f1).param1 < ((RAY)f1).param3 && x41 >= ((RAY)f1).param1)))
                        {// vector director negativo


                            lista.Add(new POINT(null, new Num(new Token(TokenTypes.NUMBER, x41)), new Num(new Token(TokenTypes.NUMBER, y41))));

                        }


                    }
                }

                else
                {

                    if (Math.Pow(((CIRCLE)f2).param3, 2) - Math.Pow(((CIRCLE)f2).param1 - ((RAY)f1).param1, 2) >= 0)
                    {
                        double y = Math.Sqrt(Math.Pow(((CIRCLE)f2).param3, 2) - Math.Pow(((CIRCLE)f2).param1 - ((RAY)f1).param1, 2)) + ((CIRCLE)f2).param2;

                        double y41 = -Math.Sqrt(Math.Pow(((CIRCLE)f2).param3, 2) - Math.Pow(((CIRCLE)f2).param1 - ((RAY)f1).param1, 2)) + ((CIRCLE)f2).param2;

                        if ((((RAY)f1).param2 > ((RAY)f1).param4 && y <= ((RAY)f1).param2) || (((RAY)f1).param2 < ((RAY)f1).param4 && y >= ((RAY)f1).param2))
                        {// vector director negativo


                            lista.Add(new POINT(null, new Num(new Token(TokenTypes.NUMBER, ((RAY)f1).param1)), new Num(new Token(TokenTypes.NUMBER, y))));

                        }

                        if (y != y41 && ((((RAY)f1).param2 > ((RAY)f1).param4 && y41 <= ((RAY)f1).param2) || (((RAY)f1).param2 < ((RAY)f1).param4 && y41 >= ((RAY)f1).param2)))
                        {// vector director negativo


                            lista.Add(new POINT(null, new Num(new Token(TokenTypes.NUMBER, ((RAY)f1).param1)), new Num(new Token(TokenTypes.NUMBER, y41))));

                        }
                    }


                }

            }


            else if (f2 is RAY)
            {


                double DirecRayf1 = ((RAY)f1).param1 - ((RAY)f1).param3;
                double DirecRayf2 = ((RAY)f2).param1 - ((RAY)f2).param3;

                double NormRayf1 = ((RAY)f1).param2 - ((RAY)f1).param4;
                double NormRayf2 = ((RAY)f2).param2 - ((RAY)f2).param4;

                if (DirecRayf1 == 0 && DirecRayf2 == 0)
                {



                    if ((((RAY)f1).param1 == ((RAY)f2).param1) && ((NormRayf1 > 0 && NormRayf2 > 0) || (NormRayf1 < 0 && NormRayf2 < 0)))
                        lista.Add((RAY)f1.Clone());


                    else if (((RAY)f1).param1 == ((RAY)f2).param1 && ((RAY)f1).param2 == ((RAY)f2).param2)
                    {
                        lista.Add(new POINT(null, new Num(new Token(TokenTypes.NUMBER, ((RAY)f1).param1)), new Num(new Token(TokenTypes.NUMBER, ((RAY)f1).param3))));

                    }


                }
                else
                {

                    if (DirecRayf1 == 0)
                    {
                        double mRectf2 = (((RAY)f2).param2 - ((RAY)f2).param4) / (((RAY)f2).param1 - ((RAY)f2).param3);

                        double nRectf2 = ((RAY)f2).param2 - mRectf2 * ((RAY)f2).param1;

                        double y = mRectf2 * ((RAY)f1).param1 + nRectf2;

                        if (DirecRayf2 > 0 && ((RAY)f1).param1 <= ((RAY)f2).param1 && ((NormRayf1 >= 0 && y <= ((RAY)f1).param1) || (NormRayf1 <= 0 && y >= ((RAY)f1).param1)))
                            lista.Add(new POINT(null, new Num(new Token(TokenTypes.NUMBER, ((RAY)f1).param1)), new Num(new Token(TokenTypes.NUMBER, y))));

                        else if (DirecRayf2 < 0 && ((RAY)f1).param1 >= ((RAY)f2).param1 && ((NormRayf1 >= 0 && y <= ((RAY)f1).param1) || (NormRayf1 <= 0 && y >= ((RAY)f1).param1)))
                            lista.Add(new POINT(null, new Num(new Token(TokenTypes.NUMBER, ((RAY)f1).param1)), new Num(new Token(TokenTypes.NUMBER, y))));
                    }

                    else if (DirecRayf2 == 0)
                    {

                        return Intersect(f2, f1);

                    }
                    else
                    {

                        double mRectf1 = (((RAY)f1).param2 - ((RAY)f1).param4) / (((RAY)f1).param1 - ((RAY)f1).param3);

                        double nRectf1 = ((RAY)f1).param2 - mRectf1 * ((RAY)f1).param1;

                        double mRectf2 = (((RAY)f2).param2 - ((RAY)f2).param4) / (((RAY)f2).param1 - ((RAY)f2).param3);

                        double nRectf2 = ((RAY)f2).param2 - mRectf2 * ((RAY)f2).param1;


                        if (mRectf1 == mRectf2 && nRectf1 == nRectf2)
                        {


                            if ((DirecRayf1 > 0 && DirecRayf2 > 0) || (DirecRayf1 < 0 && DirecRayf2 < 0))
                                lista.Add((RAY)f1.Clone());


                            else if (((RAY)f1).param1 == ((RAY)f2).param1)
                            {
                                lista.Add(new POINT(null, new Num(new Token(TokenTypes.NUMBER, ((RAY)f1).param1)), new Num(new Token(TokenTypes.NUMBER, ((RAY)f1).param3))));


                            }
                        }
                        else if (mRectf1 != mRectf2)
                        {

                            double xIntersection = (nRectf2 - nRectf1) / (mRectf1 - mRectf2);


                            double yIntersection = mRectf1 * xIntersection + nRectf1;

                            if (DirecRayf2 > 0 && xIntersection <= ((RAY)f2).param1 && ((DirecRayf1 > 0 && xIntersection <= ((RAY)f1).param1) || (DirecRayf1 < 0 && xIntersection >= ((RAY)f1).param1)))
                                lista.Add(new POINT(null, new Num(new Token(TokenTypes.NUMBER, xIntersection)), new Num(new Token(TokenTypes.NUMBER, yIntersection))));


                            else if (DirecRayf2 < 0 && xIntersection >= ((RAY)f2).param1 && ((DirecRayf1 > 0 && xIntersection <= ((RAY)f1).param1) || (DirecRayf1 < 0 && xIntersection >= ((RAY)f1).param1)))
                            {
                                Console.WriteLine("aqui se puede ahcer mas");
                                lista.Add(new POINT(null, new Num(new Token(TokenTypes.NUMBER, xIntersection)), new Num(new Token(TokenTypes.NUMBER, yIntersection))));
                            }

                        }
                    }
                }


            }

        }




        else if (f1 is CIRCLE)
        {

            if (f2 is CIRCLE)
            {

                double x1 = ((CIRCLE)f1).param1;
                double y1 = ((CIRCLE)f1).param2;
                double r1 = ((CIRCLE)f1).param3;

                double x2 = ((CIRCLE)f2).param1;
                double y2 = ((CIRCLE)f2).param2;
                double r2 = ((CIRCLE)f2).param3;

                double distCenter = Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));

                if (distCenter > r1 + r2 || distCenter < Math.Abs(r1 - r2))
                {

                }

                else
                {

                    double n = (Math.Pow(r1, 2) - Math.Pow(r2, 2) + Math.Pow(x2, 2) - Math.Pow(x1, 2) + Math.Pow(y2, 2) - Math.Pow(y1, 2)) / 2;

                    double t = x2 - x1;
                    double w = y2 - y1;

                    if (t == 0)
                    {
                        double y4 = n / w;

                        double x4 = Math.Sqrt(Math.Pow(r1, 2) - Math.Pow((n - w * y1) / w, 2)) + x1;
                        double x41 = -Math.Sqrt(Math.Pow(r1, 2) - Math.Pow((n - w * y1) / w, 2)) + x1;

                        lista.Add(new POINT(null, new Num(new Token(TokenTypes.NUMBER, x4)), new Num(new Token(TokenTypes.NUMBER, y4))));

                        if (x4 != x41)
                        {

                            lista.Add(new POINT(null, new Num(new Token(TokenTypes.NUMBER, x41)), new Num(new Token(TokenTypes.NUMBER, y4))));
                        }


                    }
                    else if (w == 0)
                    {
                        double x4 = n / t;

                        double y4 = Math.Sqrt(Math.Pow(r1, 2) - Math.Pow((n - t * x1) / t, 2)) + y1;
                        double y41 = -Math.Sqrt(Math.Pow(r1, 2) - Math.Pow((n - t * x1) / t, 2)) + y1;

                        lista.Add(new POINT(null, new Num(new Token(TokenTypes.NUMBER, x4)), new Num(new Token(TokenTypes.NUMBER, y4))));

                        if (y4 != y41)
                        {

                            lista.Add(new POINT(null, new Num(new Token(TokenTypes.NUMBER, x4)), new Num(new Token(TokenTypes.NUMBER, y41))));
                        }


                    }
                    else
                    {
                        double a = Math.Pow(w, 2) + Math.Pow(t, 2);
                        double b = 2 * w * x1 * t - 2 * n * w - 2 * y1 * t * t;
                        double c = Math.Pow(n - x1 * t, 2) + t * t * (y1 * y1 - r1 * r1);
                        double D = Math.Pow(b, 2) - 4 * a * c;

                        double y4 = (-b + Math.Sqrt(D)) / (2 * a);
                        double y41 = (-b - Math.Sqrt(D)) / (2 * a);

                        double x4 = (n - y4 * w) / t;
                        double x41 = (n - y41 * w) / t;

                        lista.Add(new POINT(null, new Num(new Token(TokenTypes.NUMBER, x4)), new Num(new Token(TokenTypes.NUMBER, y4))));

                        if (y4 != y41)
                        {

                            lista.Add(new POINT(null, new Num(new Token(TokenTypes.NUMBER, x41)), new Num(new Token(TokenTypes.NUMBER, y41))));

                        }
                    }
                }
            }
        }

        //IEnumerable<Variables> d= lista.Select(f=> new Variables{Value= f.Value});

        return lista;

    }

}



#endregion


// metodo para calcular los puntos de interseccion entre dos rectas
// metodo para calcular los puntos de interseccion entre dos circulos
// metodo para calcular los puntos de interseccion entre una recta y un circulo

// un punto pertenece a un segmento si la suma de la distancia a los extremos es igual a la longitud del segmento
// un punto pertenece a un rayo si el producto escalar entre el vector que va desde el origen hasta le punto y el vector director del rayo es mayor o igual a 0