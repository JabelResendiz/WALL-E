namespace GOLenguage;

public class Token
{
    public TokenTypes Type;
    public object Value; 

    public Token(TokenTypes type, object value)
    {
        this.Type = type;
        this.Value = value;
    }

    public Token(Token other)
    {
        Type = other.Type;
        Value = other.Value;
    }


    public string Show()
    {
        return $"Token({Type}, {Value})";
    }
}

// palabras reservadas del lenguaje
public static class ReservateKeywords
{
    public static List<(string,TokenTypes)>tuplas= new List<(string, TokenTypes)>{
        ("let",TokenTypes.LET),// bien 
        ("in",TokenTypes.IN),// no
        ("if", TokenTypes.IF),// si
        ("else",TokenTypes.ELSE),//no
        ("then",TokenTypes.THEN),
        //("sequence",TokenTypes.SECUENCE),
        ("draw",TokenTypes.DRAW),
        //("undefined",TokenTypes.UNDEFINED),
        ("line",TokenTypes.LINE),
        ("point",TokenTypes.POINT),
        ("segment",TokenTypes.SEGMENT),
        ("ray",TokenTypes.RAY),
        ("circle",TokenTypes.CIRCLE),
        ("arc",TokenTypes.ARC),
        ("color",TokenTypes.COLOR),
        ("restore",TokenTypes.RESTORE),
        ("intersect",TokenTypes.INTERSECT),
        ("measure",TokenTypes.MEASURE),
        ("count",TokenTypes.COUNT),
        ("samples",TokenTypes.SAMPLES),
        ("randoms",TokenTypes.RANDOMS),
        ("print",TokenTypes.PRINT),
        ("import",TokenTypes.IMPORT),
        
    };
    public static Dictionary<string, TokenTypes> Keyword{get;set;}= tuplas.ToDictionary(t=>t.Item1,t=>t.Item2);
    
}