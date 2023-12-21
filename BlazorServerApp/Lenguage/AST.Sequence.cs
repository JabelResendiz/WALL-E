
using System.Collections.Generic;
using System.Collections;
using System.Globalization;
using System.Runtime.CompilerServices;
namespace GOLenguage;




#region SEQUENCE2
// Finite sale de circulacion 
public class SEQUENCE2:AST,IEnumerable<Variables>{
    public IEnumerable<Variables> sequence;
    public IEnumerator<Variables> enumerator;
    public object counter;
    public IEnumerable<AST>creativo;
    
   public virtual IEnumerator<Variables> GetEnumerator(){
        
        return enumerator;
    }
    IEnumerator IEnumerable.GetEnumerator(){return GetEnumerator();}

    
}
#endregion

#region FiniteSequence
public class FiniteSequence2<T> : SEQUENCE2,IEnumerator<T> where T:Variables{

    public FiniteSequence2(IEnumerable<T>sequence=null){
       
        this.sequence=sequence;
        enumerator=sequence.GetEnumerator();
        counter=sequence.Count();
        creativo= new List<AST>();
    }
    
     public override IEnumerator<T> GetEnumerator(){
        
        return sequence.Select(x=>(T)x).GetEnumerator();
    }

    public bool MoveNext(){return enumerator.MoveNext();}

    public T Current{

        get{
            if(MoveNext()) return (T)enumerator.Current;
            else
                throw new InvalidOperationException("Enumerador fuera de posicion");
        }
    }
    object IEnumerator.Current {get{return Current;}}
    //start es el enumerable de Num que tengo al principio
    // lastNumber es el double que guardara el valor entero actual
    // final es el Num final
    public void Reset(){enumerator.Reset(); }

    public void Dispose(){enumerator.Dispose();}
}
#endregion


#region RangoSequence
public class RangoSequence2:SEQUENCE2,IEnumerator<Num>{
    // tiene su propio para devolver los elementos del enumerable y devolver el sucesor del ultimo elemento agregado
    
    private Num final;
    private double lastNumber;
    bool hayCurrent;


    //public object count;
    public RangoSequence2(IEnumerable<Num> sequence,Num final){

         creativo= new List<AST>();
        this.sequence=sequence;
        this.final=final;
        enumerator=sequence.GetEnumerator();
        Reset();
        counter=(int)Math.Abs((double)final.Value-(double)sequence.LastOrDefault().Value) +sequence.Count();
        
    }

   public override IEnumerator<Variables> GetEnumerator(){

    return new RangoSequence2(sequence.Select(y=>(Num)y),final);
   }
    object IEnumerator.Current {get{return Current;}}

    public bool MoveNext(){

        
        // sequence es el enumerador del enumerador actual
        if(enumerator.MoveNext()){

            lastNumber=(double)enumerator.Current.Value;
            return true;
        }
        else{
            if((double)sequence.LastOrDefault().Value>(double)final.Value){
                lastNumber--;
                hayCurrent=lastNumber>=(double)final.Value;
            }
            else{

                lastNumber++;
                hayCurrent=lastNumber<=(double)final.Value;
            }
            
            
        }

        return hayCurrent;
    }

    public Num Current{

        get{
            if(hayCurrent) return new Num(new Token(TokenTypes.NUMBER,lastNumber));
            else
                throw new InvalidOperationException("Enumerador fuera de posicion");
        }
    }

    //start es el enumerable de Num que tengo al principio
    // lastNumber es el double que guardara el valor entero actual
    // final es el Num final
    public void Reset(){

        if((double)sequence.LastOrDefault().Value>(double) final.Value){
            lastNumber=(double)sequence.LastOrDefault().Value+1;

        }
        else{
            lastNumber=(double)sequence.LastOrDefault().Value-1;
        }
        hayCurrent=true;
    }

    public void Dispose(){}

}

#endregion


#region InfiniteSequence Generic
public class InfiniteSequence2:SEQUENCE2{
    
    public InfiniteSequence2(){
        counter=TokenTypes.UNDEFINED;
         creativo= new List<AST>();
    }
    
}
public class OpenIntervalo:InfiniteSequence2,IEnumerator<Num>{

    public double lastNumber;
    
    public OpenIntervalo(IEnumerable<Num>sequence){

        this.sequence=sequence;
        lastNumber=(double)sequence.FirstOrDefault().Value;
        enumerator=sequence.GetEnumerator();
        

    }
    public override IEnumerator<Variables> GetEnumerator(){

        return new OpenIntervalo(sequence.Select(y=>(Num)y));
    }

    object IEnumerator.Current {get{return Current;}}
    public bool MoveNext(){

        if(enumerator.MoveNext()){

            lastNumber=(double)enumerator.Current.Value;
            
        }
        else{

            lastNumber++;
            
        }
        return true;

    }

    public Num Current=> new Num(new Token(TokenTypes.NUMBER,lastNumber));

    public void Reset(){}

    public void Dispose(){

        enumerator.Dispose();
    }
}


public class SequencePointsInFigure:InfiniteSequence2,IEnumerator<POINT>{

    private FIGURE figure;
    private Random random = new Random();

    public POINT Current { get; private set; }

    public override IEnumerator<Variables> GetEnumerator()
    {
        return new SequencePointsInFigure(figure);
    }

    public SequencePointsInFigure(FIGURE figure){
        this.figure=figure;
    }
    

    object IEnumerator.Current => Current;

    public bool MoveNext()
    {
        if(figure is LINE || figure is SEGMENT || figure is RAY){
            double x1= figure.param1;
            double y1=figure.param2;
            double x2= figure.param3;
            double y2=figure.param4;
            double deltaX= Math.Abs(x2-x1);
            double MinX= Math.Min(x1,x2);
            double MinY=Math.Min(y1,y2);
            if(x1==x2){
                double DeltaY= Math.Abs(y2-y1);
                double Ypoint= random.NextDouble()*DeltaY+MinY;
                Current=new POINT(null,new Num(new Token(TokenTypes.NUMBER,x1)), new Num(new Token(TokenTypes.NUMBER,Ypoint)));
            }
            else{
                double Xpoint= random.NextDouble()*deltaX+MinX;
                double n= y2- x2*(y1-y2)/(x1-x2);
                double Ypoint= Xpoint*(y1-y2)/(x1-x2) +n;
                Current=new POINT(null,new Num(new Token(TokenTypes.NUMBER,Xpoint)), new Num(new Token(TokenTypes.NUMBER,Ypoint)));
         
            }
        }
        else if(figure is CIRCLE){
            double x1 = figure.param1;
            double y1 = figure.param2;
            double r1 = figure.param3;
            double Xpoint= random.NextDouble()*2*r1+ x1-r1;

            int numeroRandom= random.Next(2) ;
            double Ypoint= (numeroRandom==0)?y1-Math.Sqrt(r1*r1-Math.Pow(x1-Xpoint,2)):y1+Math.Sqrt(r1*r1-Math.Pow(x1-Xpoint,2));
            Current=new POINT(null,new Num(new Token(TokenTypes.NUMBER,Xpoint)), new Num(new Token(TokenTypes.NUMBER,Ypoint)));

        }
        else if(figure is POINT){
            Current= (POINT)figure;
        }
        else if(figure is ARC){
            double x1 = figure.param1;
            double y1 = figure.param2;
            double r1 = figure.param7;
            double x2 = figure.param3;
            double y2 = figure.param4;
            double x3 = figure.param5;
            double y3= figure.param6;

            double distanceP1P2= Math.Sqrt(Math.Pow(x1 - x2, 2) + Math.Pow(y1 - y2, 2));
            double distanceP1P3= Math.Sqrt(Math.Pow(x1-x3, 2) + Math.Pow(y1- y3, 2));

            double XmLeft= x1-distanceP1P2/(r1*Math.Abs(x1-x2));
            double XmRight= x1-distanceP1P3/(r1*Math.Abs(x1-x3));

            double Xpoint= random.NextDouble()*Math.Abs(XmLeft-XmRight)+ Math.Min(XmLeft,XmRight);
            double Ypoint1= y1-Math.Sqrt(r1*r1-Math.Pow(x1-Xpoint,2));
            double Ypoint2= y1+Math.Sqrt(r1*r1-Math.Pow(x1-Xpoint,2));

            if(y2>y3){
                if(Ypoint1>=y3 && Ypoint1<=y2) Current=new POINT(null,new Num(new Token(TokenTypes.NUMBER,Xpoint)), new Num(new Token(TokenTypes.NUMBER,Ypoint1)));
                else Current=new POINT(null,new Num(new Token(TokenTypes.NUMBER,Xpoint)), new Num(new Token(TokenTypes.NUMBER,Ypoint2)));
            }
            else if (y2<y3){
                if(Ypoint1>=y2 && Ypoint1<=y3) Current=new POINT(null,new Num(new Token(TokenTypes.NUMBER,Xpoint)), new Num(new Token(TokenTypes.NUMBER,Ypoint1)));
                else Current=new POINT(null,new Num(new Token(TokenTypes.NUMBER,Xpoint)), new Num(new Token(TokenTypes.NUMBER,Ypoint2)));
            }
            else if(x2>x3){
                if(Ypoint1>y2)Current=new POINT(null,new Num(new Token(TokenTypes.NUMBER,Xpoint)), new Num(new Token(TokenTypes.NUMBER,Ypoint1)));
                else Current=new POINT(null,new Num(new Token(TokenTypes.NUMBER,Xpoint)), new Num(new Token(TokenTypes.NUMBER,Ypoint2)));
            }   
            else {
                if(Ypoint1<y2)Current=new POINT(null,new Num(new Token(TokenTypes.NUMBER,Xpoint)), new Num(new Token(TokenTypes.NUMBER,Ypoint1)));
                else Current=new POINT(null,new Num(new Token(TokenTypes.NUMBER,Xpoint)), new Num(new Token(TokenTypes.NUMBER,Ypoint2)));

            }
        }
        return true;
    }

    public void Reset(){}

    public void Dispose(){}
    
}
public class SequencePointSamples:InfiniteSequence2,IEnumerator<POINT>{

    public override IEnumerator<Variables> GetEnumerator()
    {
        return new SequencePointSamples();
    }

    private Random random = new Random();

    public POINT Current { get; private set; }

    object IEnumerator.Current => Current;

    public bool MoveNext()
    {
        Current = new POINT(null,new Num(new Token(TokenTypes.NUMBER,(double)random.Next(1,800))), new Num(new Token(TokenTypes.NUMBER,(double)random.Next(1,800))));
        return true;
    }

    public void Reset(){}

    public void Dispose(){}
    
}


public class RandomNumber: InfiniteSequence2
{
    public override IEnumerator<Variables> GetEnumerator()
    {
        return new RandomPositiveIntegersEnumerator();
    }

    
}

public class RandomPositiveIntegersEnumerator : IEnumerator<Num>
{
    
    private Random random = new Random();

    public Num Current { get; private set; }

    object IEnumerator.Current => Current;

    public bool MoveNext()
    {
        Current = new Num(new Token(TokenTypes.NUMBER,(double)random.Next(1, int.MaxValue)));
        return true;
    }

    public void Reset(){}

    public void Dispose(){}
    
}
#endregion





