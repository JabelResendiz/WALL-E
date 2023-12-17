
using System.Collections.Generic;
using System.Collections;
namespace GOLenguage;



#region Sequences


public class Sequence : AST
{
    public object count2;
}

interface ISequence<T> : IEnumerable<T>, IEnumerator<T>
{


    object count { get; }
    IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }
    IEnumerator<T> GetEnumerator();
    bool MoveNext();
    T Current { get; }
    object IEnumerator.Current { get { return Current; } }
    void Dispose();
    void Reset();

}

// Finite sale de circulacion 
public class SEQUENCE : AST
{
    public List<AST> sequence;
    public object count;
    public SEQUENCE(List<AST> sequence)
    {

        this.sequence = sequence;
        count = sequence.Count();

        if (sequence.First().GetType().ToString() == "GOLenguage.AST") sequence.RemoveAt(0);
        Console.WriteLine(sequence.Count());
    }

}


/*
public class FiniteSequence<T>:Sequence,ISequence<T>{

    public IEnumerator<T>sequence;
    public IEnumerable<T>k;
    
    //public object count;
    public FiniteSequence(IEnumerable<T>list){
        
        k=list;
        sequence=list.GetEnumerator();
        //type=k.FirstOrDefault().GetType();
        count2=k.Count();
    }
    

    public double count=>count2;
    public IEnumerator<T> GetEnumerator(){
        return k.GetEnumerator();
    }
   
    public bool MoveNext(){

        if(sequence.MoveNext()){

            return true;
            
        }
        return false;
        
    }

    public T Current=> sequence.Current;

    public void Reset(){}

    public void Dispose(){}

}
*/

public class InfiniteSequence<T> : Sequence, ISequence<Variables>
{
    // tiene su propio para devolver los elementos del enumerable y devolver el sucesor del ultimo elemento agregado
    public IEnumerator<Variables> sequence { get; set; }
    public IEnumerable<Variables> k { get; set; }
    public double lastNumber;

    //public object count;
    public InfiniteSequence(IEnumerable<Variables> list)
    {

        k = list;
        lastNumber = (double)k.FirstOrDefault().Value;
        sequence = list.GetEnumerator();


        count2 = TokenTypes.UNDEFINED;
    }


    public object count => TokenTypes.UNDEFINED;
    public IEnumerator<Variables> GetEnumerator()
    {
        return new InfiniteSequence<T>(k);
    }

    public bool MoveNext()
    {

        if (sequence.MoveNext())
        {

            lastNumber = (double)sequence.Current.Value;

        }
        else
        {

            lastNumber++;

        }
        return true;

    }

    public Variables Current => new Num(new Token(TokenTypes.NUMBER, lastNumber));

    public void Reset() { }

    public void Dispose()
    {

        sequence.Dispose();
    }

}

public class RangoSequence : Sequence, ISequence<Variables>
{
    // tiene su propio para devolver los elementos del enumerable y devolver el sucesor del ultimo elemento agregado

    public IEnumerator<Num> sequence { get; set; }
    public IEnumerable<Num> start { get; set; }
    private Num final;
    private double lastNumber;
    bool hayCurrent;


    //public object count;
    public RangoSequence(IEnumerable<Num> k, Num final)
    {


        start = k;
        this.final = final;
        sequence = start.GetEnumerator();
        Reset();
        count2 = Math.Abs((double)final.Value - (double)start.LastOrDefault().Value) + start.Count();

    }

    public object count => count2;
    public IEnumerator<Variables> GetEnumerator()
    {

        return new RangoSequence(start, final);
    }
    public bool MoveNext()
    {


        // sequence es el enumerador del enumerador actual
        if (sequence.MoveNext())
        {

            lastNumber = (double)sequence.Current.Value;
            return true;
        }
        else
        {
            if ((double)start.LastOrDefault().Value > (double)final.Value)
            {
                lastNumber--;
                hayCurrent = lastNumber >= (double)final.Value;
            }
            else
            {

                lastNumber++;
                hayCurrent = lastNumber <= (double)final.Value;
            }


        }

        return hayCurrent;
    }

    public Variables Current
    {

        get
        {
            if (hayCurrent) return new Num(new Token(TokenTypes.NUMBER, lastNumber));
            else
                throw new InvalidOperationException("Enumerador fuera de posicion");
        }
    }

    //start es el enumerable de Num que tengo al principio
    // lastNumber es el double que guardara el valor entero actual
    // final es el Num final
    public void Reset()
    {

        if ((double)start.LastOrDefault().Value > (double)final.Value)
        {
            lastNumber = (double)start.LastOrDefault().Value + 1;

        }
        else
        {
            lastNumber = (double)start.LastOrDefault().Value - 1;
        }
        hayCurrent = true;
    }

    public void Dispose() { }

}


#endregion