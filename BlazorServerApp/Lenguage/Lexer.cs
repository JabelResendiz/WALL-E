namespace GOLenguage;
public class Lexer
{
    public  string? Text;
    public int Pos;
    char? CurrentChar;
   // ReservateKeywords? key;
    public Parser parser;
    //public Interpreter interpreter;
    // constructor de la clase Lexer encargada de localizar e identificar los tokens de la cadena
    public Lexer(string text)
    {
        
        Text = text;
        Pos = 0;
        CurrentChar = text[Pos];
        //key = new ReservateKeywords();
    }

    #region 1-LexicalError 
    private void ErrorLexico(string error)
    {
        Console.Write("! LEXICAL ERROR  : "+error);
        throw new Exception();
        //Environment.Exit(0);
    }

    #endregion


    #region 2-GetNextToken() es para devolver el token que continua en la cadena
    
    public Token GetNextToken()
    {
        while (CurrentChar != null)
        {
            if (char.IsWhiteSpace((char)CurrentChar))
            {
                SkipSpace(); 
                continue;
            }

        
            if (CurrentChar == '\"')
            {
                return Cadene();
            }

            if (char.IsLetter((char)CurrentChar))
            {
                return Variable();
            }

            if(char.IsDigit((char)CurrentChar))

                return Number();

            switch (CurrentChar)
            {
                case '{':
                    Next();

                    return new Token(TokenTypes.L_KEY,"{");
                
                case '}':
                    Next();
                    return new Token(TokenTypes.R_KEY,"}");
                
                case '_':
                    Next();
                    return new Token(TokenTypes.UNDERSCORE,"_");
                    
                case '+':

                    Next();

                    if (CurrentChar == '=')
                    {
                        Next();

                        return new Token(TokenTypes.ASSIGN_PLUS, "+=");
                    }

                    return new Token(TokenTypes.PLUS, '+');

                case '-':

                    Next();

                    if (CurrentChar == '=')
                    {
                        Next();

                        return new Token(TokenTypes.ASSIGN_MINUS, "-=");
                    }

                    return new Token(TokenTypes.MINUS, '-');

                case '*':

                    Next();

                    if (CurrentChar == '=')
                    {
                        Next();

                        return new Token(TokenTypes.ASSIGN_MUL, "*=");
                    }

                    return new Token(TokenTypes.MULT, '*');

                case '/':

                    Next();

                    if (CurrentChar == '=')
                    {
                        Next();

                        return new Token(TokenTypes.ASSIGN_DIV, "/=");
                    }

                    return new Token(TokenTypes.FLOAT_DIV, '/');

                case '%':

                    Next();

                    if (CurrentChar == '=')
                    {
                        Next();

                        return new Token(TokenTypes.ASSIGN_MOD, "%=");
                    }

                    return new Token(TokenTypes.MOD, '%');
                
                case '(':

                    Next();

                    return new Token(TokenTypes.L_PARENT, '(');

                case ')':

                    Next();

                    return new Token(TokenTypes.R_PARENT, ')');

                
                case '=':
                    
                    Next();

                    if (CurrentChar == '=')
                    {
                        Next();

                        return new Token(TokenTypes.SAME, "==");
                    }
                    if(CurrentChar == '>')
                    {
                        Next();
                        return new Token(TokenTypes.RETURN,"=>");
                    }
                    return new Token(TokenTypes.ASSIGN, "=");

                case '!':

                    Next();
                    
                    if (CurrentChar == '=')
                    {
                        Next();

                        return new Token(TokenTypes.DIFFERENT, "!=");
                    }

                    return new Token(TokenTypes.NOT, "!");
                
                case '<':

                    Next();
                    
                    if (CurrentChar == '=')
                    {
                        Next();

                        return new Token(TokenTypes.LESS_EQUAL, "<=");
                    }

                    return new Token(TokenTypes.LESS, '<');

                case '>':

                    Next();

                    if (CurrentChar == '=')
                    {
                        Next();

                        return new Token(TokenTypes.GREATER_EQUAL, ">=");
                    }

                    return new Token(TokenTypes.GREATER, '>');
                
                case '&':

                    
                    Next();

                    return new Token(TokenTypes.AND, "&");
                    
                
                case '|':

                    
                    Next();

                    return new Token(TokenTypes.OR, "|");
                   
                case ',':

                    Next();

                    return new Token(TokenTypes.COMMA, ',');


                case ';':

                    Next();

                    return new Token(TokenTypes.SEMI, ';');
                
                case '.':
                    Next();
                    if(CurrentChar=='.'){
                        Next();
                        if(CurrentChar=='.')
                            Next();
                            return new Token(TokenTypes.THREEPOINT,"...");
                    }
                    break;
                    

            }

            ErrorLexico($"' {CurrentChar} ' Unknown token. Col {Pos}");
        }

        return new Token(TokenTypes.EOF, "EOF");
    }
    # endregion
    

    #region 3-metodos para localizar el token auxiliares de GetNextToken()
    private char? SeekNextChar()
    {
        int pos = Pos + 1;

        if (pos == Text.Length)

            return null;
        
        return Text[pos];
    }

   
    // metodo para leer el siguiente char de la cadena 
    private void Next()
    {
        Pos += 1;

        if (Pos == Text.Length)

            CurrentChar = null;
        
        else 

            CurrentChar = Text[Pos];
    }


    // si es un espacio en blanco se ira al siguiente char 
    private void SkipSpace()
    {
        while (CurrentChar != null && Char.IsWhiteSpace((char)CurrentChar))

            Next();
    }

    // metodo para almacenar cuando se reconoce una cadena
    private Token Cadene()
    {
        string value = "";

        Next();

        while (CurrentChar != null && CurrentChar != '\"')
        {
            value += CurrentChar;
            Next();
        }
        if(CurrentChar=='\"') Next();
            
        else if(CurrentChar == null) ErrorLexico($"\"{value}\" its has not finished. Char \" ,which closes, has not been found");
        
        
        return new Token(TokenTypes.STRING, value);
    }

    // metodo para almacenar los numeros
    private Token Number()
    {
        
        string value = "";
        int countPoint=0;
        while ((CurrentChar != null && char.IsDigit((char)CurrentChar)) || CurrentChar=='.')
        {
           
            countPoint+=1;
            value += CurrentChar;
            Next();
        }

        if(CurrentChar!= null && !Char.IsWhiteSpace((char)CurrentChar) && (char)CurrentChar!=';')
        {
            int variable=0;
            while((char)CurrentChar!='+' && (char)CurrentChar!='-' && 
                    (char)CurrentChar!='*'&& (char)CurrentChar!='/' && 
                    (char)CurrentChar!='^' && (char)CurrentChar!='%' && 
                    (char)CurrentChar!=')' && !Char.IsWhiteSpace((char)CurrentChar)
                    && (char)CurrentChar!='=' && (char)CurrentChar!=',' &&
                    (char)CurrentChar!='&' && (char)CurrentChar!='|' && (char)CurrentChar!='<'&&
                    (char)CurrentChar!='>' && (char)CurrentChar!='!' && (char)CurrentChar!=';' && (char)CurrentChar!='@' &&
                    (char)CurrentChar!='}'  ){
                value+=CurrentChar;
                variable+=1;
                Next();
                if(CurrentChar ==null)break;
            }
            if(variable!=0)ErrorLexico($"\" {value} \" is not valid token. Col {Pos-value.Length}");
        }
        try{
            double floatValue= double.Parse(value);
        }
        catch(Exception e){
            ErrorLexico($"\"{value}\" is not valid token. Col {Pos-value.Length}");
        }
        return new Token(TokenTypes.NUMBER, double.Parse(value));
        
        
    }
    
    // identificar nombres de variables, palabras reservadas y nombre de funciones
    public Token Variable()
    {
        
        string name = "";

        while (CurrentChar != null && char.IsLetterOrDigit((char)CurrentChar))
        {
            name += CurrentChar;
            Next();
        }

        
        if (ReservateKeywords.Keyword.ContainsKey(name))

            return new Token(ReservateKeywords.Keyword[name], ReservateKeywords.Keyword[name]);

        if(Principal.Functiones.ContainsKey(name) && SeeNextTokenParents())

            return new Token(TokenTypes.CALL,name);
        

        return new Token(TokenTypes.ID, name);
    }
#endregion

    // devuelve un token xq es llamado desde el constructor de la clase Parser
    // devolvera el proximo token en la cadena 
    // esta pensado para saber si lo que sucede a un ID es asignacion(continua un =) o no
    public bool SeeNextTokenEgual(){
        int pos=Pos;
        while(CurrentChar!= null && pos< Text.Length && char.IsWhiteSpace(Text[pos])){
            pos+=1;
        }
        if(pos>= Text.Length-1)return false;
        if(Text[pos]=='+' || Text[pos]=='-' || Text[pos]=='*' || Text[pos]=='/' || Text[pos]=='%'){
            pos+=1;
            return Text[pos]=='=';
        }
        return  (Text[pos]== '=') &&(Text[pos+1]!='=');
    }
   
   public bool SeeNextTokenParenthesis(){
        int pos=Pos;
        while(CurrentChar!=null && pos<Text.Length && char.IsWhiteSpace(Text[pos])){
            pos+=1;
        }
        if(pos>=Text.Length-1)return false;
        return Text[pos]=='(';
   }
   // metodo auxiliar que buscara si el proximo token es ( 
    // esta pensado porque cuando tenemos funciones y variables locales con el mismo nombre
    // es para saber si el Token es del tipo ID(sin parentesis) o CALL(con parentesis)

    private bool SeeNextTokenParents(){
        int pos=Pos;
        while(CurrentChar!= null && pos< Text.Length &&  char.IsWhiteSpace(Text[pos])){
            pos+=1;
        }
        if(pos==Text.Length)return true;
        return  Text[pos]== '(';
    }

    public bool SeeNextTokenComma(){
        int pos=Pos;
        while(CurrentChar!= null && pos< Text.Length && char.IsWhiteSpace(Text[pos])){
            pos+=1;
        }
        if(pos>= Text.Length-1)return false;
        
        return  Text[pos]== ',';
    }
}