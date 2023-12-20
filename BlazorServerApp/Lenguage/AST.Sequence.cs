
using System.Collections.Generic;
using System.Collections;
using System.Globalization;
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
    
    //public object count;
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
        
        Current = new POINT(null,new Num(new Token(TokenTypes.NUMBER,(double)random.Next(1,800))), new Num(new Token(TokenTypes.NUMBER,(double)random.Next(1,800))));
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
        Current = new POINT(null,new Num(new Token(TokenTypes.NUMBER,(double)random.Next(1,400))), new Num(new Token(TokenTypes.NUMBER,(double)random.Next(1,400))));
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





