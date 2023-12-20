
using System.Collections;

namespace GOLenguage;

public class Parser
{

    private Lexer Lexer;
    private Token CurrentToken;
    //ReservateKeywords reserved;

    //public Declarations declarations;

    // constructor de la clase Parser
    public Parser(Lexer lexer)
    {
        Lexer = lexer;
        CurrentToken = lexer.GetNextToken();// CurrentToken es un objeto Token
        //reserved= new ReservateKeywords();
        //declarations = new Declarations(null);
    }
    #region SyntaxError
    private async void SyntaxError(string error)
    {
        Console.WriteLine("! SYNATX ERROR: " + error);
      
        await Principal._jsRuntime.InvokeAsync<string>("alert", new object[] { "! SYNATX ERROR: " + error });
        throw new Exception();
        //Environment.Exit(1);
    }
    #endregion

    #region Parse
    // metodo que creara el node final del bloque del codigo 
    // se crea una instancia de AST que sera el arbol
    public AST Parse()
    {

        AST node = StatementList();

        if (CurrentToken.Type != TokenTypes.EOF)

            SyntaxError("Expected token EOF");

        return node;
    }
    #endregion


    // mostrara el token en pantalla y lee el siguiente token de la cadena
    // realcionado con el metodo GetNextToken() de la clase Lexer
    private void Process(TokenTypes type, string msg)
    {
        Console.WriteLine($"{CurrentToken.Show()} {type}");

        if (CurrentToken.Type == type)

            CurrentToken = Lexer.GetNextToken();

        else

            SyntaxError($"Unexpected token {CurrentToken.Value}.Missing " + $" {msg}");
    }

    #region Gramatica (Compounds,Comparer,Expression, Termine,Exponenciation,Factor)


    // Gramatica para BooleanOperator (and , not , or)
    private AST Compounds()
    {
        AST node = Comparer();
        Token token = new Token(CurrentToken);
        while (CurrentToken.Type == TokenTypes.AND || CurrentToken.Type == TokenTypes.OR
        || CurrentToken.Type == TokenTypes.NOT
            )
        {
            token = new Token(CurrentToken);
            if (token.Type == TokenTypes.AND)

                Process(TokenTypes.AND, "and");

            else if (token.Type == TokenTypes.OR)

                Process(TokenTypes.OR, "or");

            else if (token.Type == TokenTypes.NOT)

            {
                Process(TokenTypes.NOT, "not");
                node = new UnaryOperator(token, Comparer());
                continue;
            }

            node = new BinaryOperator(node, token, Comparer());


        }

        return node;
    }
    // metodo para identificar operadores 
    // es un auxiliar de Comparer()
    private bool IsBooleanOperator()
    {
        Token type = CurrentToken;

        return type.Type == TokenTypes.SAME || type.Type == TokenTypes.DIFFERENT ||
                type.Type == TokenTypes.LESS || type.Type == TokenTypes.GREATER ||
                type.Type == TokenTypes.LESS_EQUAL || type.Type == TokenTypes.GREATER_EQUAL;

    }

    // Gramatica para Comparadores 
    private AST Comparer()
    {
        AST node = Expression();
        Token token = new Token(CurrentToken);

        while (IsBooleanOperator())
        {
            token = new Token(CurrentToken);

            if (token.Type == TokenTypes.SAME)

                Process(TokenTypes.SAME, "same");

            else if (token.Type == TokenTypes.DIFFERENT)

                Process(TokenTypes.DIFFERENT, "different");

            else if (token.Type == TokenTypes.LESS)

                Process(TokenTypes.LESS, "less");

            else if (token.Type == TokenTypes.LESS_EQUAL)

                Process(TokenTypes.LESS_EQUAL, "less equal");

            else if (token.Type == TokenTypes.GREATER)

                Process(TokenTypes.GREATER, "greater");

            else if (token.Type == TokenTypes.GREATER_EQUAL)

                Process(TokenTypes.GREATER_EQUAL, "greater equal");



            node = new BinaryOperator(node, token, Expression());
        }


        return node;
    }

    // Gramatica para Expresion(incluye suma,resta y concatenacion)
    private AST Expression()
    {
        AST node = Termine();
        Token token = new Token(CurrentToken);

        while (CurrentToken.Type == TokenTypes.PLUS || CurrentToken.Type == TokenTypes.MINUS)
        {
            token = new Token(CurrentToken);// esta innecesario

            if (token.Type == TokenTypes.PLUS)

                Process(TokenTypes.PLUS, "plus");

            else if (token.Type == TokenTypes.MINUS)

                Process(TokenTypes.MINUS, "minus");



            node = new BinaryOperator(node, token, Termine());
        }


        return node;
    }

    // Gramatica de Termine(incluye multiplicacion,division y modulo)
    private AST Termine()
    {
        AST node = Factor();
        Token token = new Token(CurrentToken);

        while (CurrentToken.Type == TokenTypes.MULT || CurrentToken.Type == TokenTypes.FLOAT_DIV || CurrentToken.Type == TokenTypes.MOD)
        {
            token = new Token(CurrentToken);

            if (token.Type == TokenTypes.MULT)

                Process(TokenTypes.MULT, "mult");


            else if (token.Type == TokenTypes.FLOAT_DIV)

                Process(TokenTypes.FLOAT_DIV, "div");

            else if (token.Type == TokenTypes.MOD)

                Process(TokenTypes.MOD, "mod");

            node = new BinaryOperator(node, token, Factor());
        }


        return node;
    }

    //metodo para reconocer simbolos terminales
    private AST Factor()
    {
        AST node = new AST();
        Token token = new Token(CurrentToken);
        if ((ReservateKeywords.Keyword.ContainsValue(CurrentToken.Type) && (CurrentToken.Type != TokenTypes.PI)) ||
            token.Type is TokenTypes.CALL || token.Type is TokenTypes.L_KEY)
        {
            node = FunctionWAllE();
            return node;
        }

        switch (token.Type)
        {

            case TokenTypes.ID:

                Process(TokenTypes.ID, "ID");

                node = new Var(token);

                break;

            case TokenTypes.PI:

                Process(TokenTypes.PI, "PI");
                node = new Var(token);
                break;

            case TokenTypes.PLUS:

                Process(TokenTypes.PLUS, "Plus");

                node = new UnaryOperator(token, Factor());

                break;

            case TokenTypes.MINUS:

                Process(TokenTypes.MINUS, "Minus");

                node = new UnaryOperator(token, Factor());

                break;

            case TokenTypes.NUMBER:

                Process(TokenTypes.NUMBER, "number");

                node = new Num(token);

                break;


            case TokenTypes.BOOLEAN:

                Process(TokenTypes.BOOLEAN, "boolean");

                node = new Bool(token);

                break;

            case TokenTypes.STRING:

                Process(TokenTypes.STRING, "string");

                node = new Cadene(token);

                break;

            case TokenTypes.TRUE:

                Process(TokenTypes.TRUE, "true");

                node = new Bool(new Token(TokenTypes.TRUE, true));

                break;

            case TokenTypes.FALSE:

                Process(TokenTypes.FALSE, "false");

                node = new Bool(new Token(TokenTypes.FALSE, false));

                break;

            case TokenTypes.L_PARENT:

                Process(TokenTypes.L_PARENT, "L_Parent");

                node = Compounds();

                Process(TokenTypes.R_PARENT, $" in col {Lexer.Pos}.");

                break;
            default:
                node=null;
                break;
        }

        return node;
    }
    private AST FunctionWAllE()
    {

        AST node = new AST();
        if (CurrentToken.Type == TokenTypes.LET)// comprueba si el token actual es un let

            node = Declaration();

        else if (CurrentToken.Type == TokenTypes.IF)

            node = Conditional();

        else if (CurrentToken.Type == TokenTypes.PRINT)

            node = PRINT();

        else if (CurrentToken.Type == TokenTypes.CALL)

            node = CallFunction();

        else if (CurrentToken.Type == TokenTypes.MEASURE || CurrentToken.Type == TokenTypes.INTERSECT || CurrentToken.Type == TokenTypes.COUNT ||
        CurrentToken.Type == TokenTypes.POINTS || CurrentToken.Type == TokenTypes.RANDOMS || CurrentToken.Type == TokenTypes.SAMPLES)

            node = WalleFunction();

        else if (CurrentToken.Type == TokenTypes.POINT)
            node = Point();

        else if (CurrentToken.Type == TokenTypes.CIRCLE)
            node = Circle();

        else if (CurrentToken.Type == TokenTypes.SEGMENT)
            node = Segment();

        else if (CurrentToken.Type == TokenTypes.LINE)
            node = Line();

        else if (CurrentToken.Type == TokenTypes.RAY)
            node = Ray();

        else if (CurrentToken.Type == TokenTypes.ARC)
            node = Arc();

        else if (CurrentToken.Type == TokenTypes.L_KEY)
            node = Sequence();

        return node;
    }

    #endregion

    #region StatementList and Statement (mayor rango)

    // AST que contendra las listas de instrucciones de todo tipo
    private AST StatementList()
    {
        Instructions instructions = new Instructions();// se ha creado una lista Commands de AST
        instructions.Commands.Add(Statement());

        while (CurrentToken.Type == TokenTypes.SEMI)
        {
            Process(TokenTypes.SEMI, $"semicolon. Token \" ; \" must end each line. Col {Lexer.Pos}");
            if (CurrentToken.Type == TokenTypes.EOF) break;
            //Process(TokenTypes.SEMI);// se buscara el proximo token despues del punto y coma
            instructions.Commands.Add(Statement());// se agregara a la lista

        }

        Process(TokenTypes.EOF, $"semicolon. Token \" ; \" must end each line. Col {Lexer.Pos}");
        return instructions;// se retornara despues de finalizado el trabajo
    }

    // identifica la instruccion de cada linea


    private AST Statement()
    {
        AST node = new AST();


        if (CurrentToken.Type == TokenTypes.UNDERSCORE || (CurrentToken.Type == TokenTypes.ID && (Lexer.SeeNextTokenEgual() || Lexer.SeeNextTokenComma())))//&& Lexer.GetNextToken().Type== TokenTypes.ASSIGN)

            node = Assignment();

        //else if (CurrentToken.Type == TokenTypes.WHILE)

        //node = Cicle();
        else if (CurrentToken.Type == TokenTypes.COLOR || CurrentToken.Type == TokenTypes.RESTORE)
            node = Color();

        else if (CurrentToken.Type == TokenTypes.ID && Lexer.SeeNextTokenParenthesis())

            node = Function();
        else if (CurrentToken.Type == TokenTypes.IMPORT)
            Import();


        else if (CurrentToken.Type == TokenTypes.DRAW)
            node = Draw();
        // estas palabras reservadas no devuelven un valor por tanto no inician una instruccion
        else if (CurrentToken.Type == TokenTypes.IN || CurrentToken.Type == TokenTypes.ELSE ||
        CurrentToken.Type == TokenTypes.TRUE || CurrentToken.Type == TokenTypes.FALSE ||
        CurrentToken.Type == TokenTypes.RETURN)

            SyntaxError($"\"{CurrentToken.Value}\" Invalid Token. Col {Lexer.Pos - CurrentToken.Value.ToString().Length}");

        else

            node = Compounds();

        return node;
    }
    #endregion


    #region Metodos para funciones de las KeyWords

    // En cada metodo se creara una instancia de AST

    // Metodo para funcion seno 
    private void Import()
    {
        Process(TokenTypes.IMPORT, "");

        if (CurrentToken.Type == TokenTypes.STRING) // cambiar ID por string
        {
            string path = $"../Tests/{(string)CurrentToken.Value}";

            string j = "";
            try
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    j = sr.ReadToEnd();
                }
            }// DirectoryNot
            catch (Exception ex)
            {
                SyntaxError($"{ex.Message}");
            }
            Lexer.Text = j + Lexer.Text.Substring(Lexer.Pos + 1);
            Lexer.Pos = -1;
            Process(CurrentToken.Type, "");
            //Console.WriteLine(Lexer.Text);

        }

        else SyntaxError($"Invalid Token {CurrentToken.Value}.\"String \" expected");

    }
    private AST Color()
    {

        AST node = new AST();
        Token token = CurrentToken;
        Process(token.Type, "");
        if (token.Type == TokenTypes.COLOR)
        {

            try
            {


                node = new COLOR(TokenTypes.COLOR, (ColorType)Enum.Parse(typeof(ColorType), (string)CurrentToken.Value));

                /*cualquier error ya sea porque se intenta castear CurrentToken.Value a string y no lo es(InvalidCastException),
                 como que siendo un string y no sea un color valido por el programa(ArgumentException)
                */
                Process(CurrentToken.Type, "type");
            }
            catch (Exception ex)
            {

                SyntaxError($"{CurrentToken.Value} is not a valid color");
            }



        }

        else
            node = new COLOR(TokenTypes.RESTORE);
        return node;
    }

    private AST Draw()
    {
        AST node = new AST();

        Process(TokenTypes.DRAW, "");
        AST token = Statement();

        //Process(TokenTypes.ID,"id");
        if (CurrentToken.Type == TokenTypes.STRING)
        {
            Token token2 = CurrentToken;
            Process(TokenTypes.STRING, "");
            node = new Draw(token, token2);
            return node;
        }
        node = new Draw(token);
        return node;
    }

    private AST Ray()
    {
        AST node = new AST();

        Process(TokenTypes.RAY, "");
        if (CurrentToken.Type == TokenTypes.ID)
        {


            Token token = CurrentToken;

            Process(TokenTypes.ID, "");

            node = new RAY(token, null, null);

            return node;
        }
        Process(TokenTypes.L_PARENT, $" open parenthesis before \" ray \" declaration");
        AST token1 = Compounds();
        Process(TokenTypes.COMMA, "comma");
        AST token2 = Compounds();
        Process(TokenTypes.R_PARENT, $" closed parenthesis after expressions \" ray \" declaration");
        node = new RAY(null, token1, token2);
        return node;
    }

    private AST Point()
    {
        AST node = new AST();

        Process(TokenTypes.POINT, "");
        //Process(TokenTypes.ID,$" open parenthesis before \"sen\" function args .Col {Lexer.Pos-CurrentToken.Value.ToString().Length}");
        if (CurrentToken.Type == TokenTypes.ID)
        {
            Token token = CurrentToken;
            Process(TokenTypes.ID, "");
            node = new POINT(token, null, null);
            return node;
        }

        Process(TokenTypes.L_PARENT, $" open parenthesis before \" point \" declaration");
        AST token1 = Compounds();
        Process(TokenTypes.COMMA, "comma");
        AST token2 = Compounds();
        Process(TokenTypes.R_PARENT, $" closed parenthesis after expressions \" point \" declaration");
        node = new POINT(null, token1, token2);
        return node;
    }
    // MEtodo para funcion coseno
    private AST Circle()
    {
        AST node = new AST();

        Process(TokenTypes.CIRCLE, "");
        if (CurrentToken.Type == TokenTypes.ID)
        {
            Token token = CurrentToken;
            Process(TokenTypes.ID, "");
            //Process(TokenTypes.L_PARENT,$" open parenthesis before \"cos\" function args. Col {Lexer.Pos-CurrentToken.Value.ToString().Length}");
            node = new CIRCLE(token, null, null);
            return node;
        }
        Process(TokenTypes.L_PARENT, $" open parenthesis before \" circle \" declaration");
        AST token1 = Compounds();
        Process(TokenTypes.COMMA, "comma");
        AST token2 = Compounds();
        Process(TokenTypes.R_PARENT, $" closed parenthesis after expressions \" circle \" declaration");
        node = new CIRCLE(null, token1, token2);
        return node;
        //Process(TokenTypes.R_PARENT,$" closed parenthesis after \"cos\" function args . Col {Lexer.Pos-CurrentToken.Value.ToString().Length}");

    }
    private AST Arc()
    {
        AST node = new AST();

        Process(TokenTypes.ARC, "");

        if (CurrentToken.Type == TokenTypes.ID)
        {
            Token token = CurrentToken;
            Process(TokenTypes.ID, "");

            node = new ARC(token, null, null, null, null);
            return node;
        }

        Process(TokenTypes.L_PARENT, $" open parenthesis before \" arc \" declaration");
        AST token1 = Compounds();
        Process(TokenTypes.COMMA, "comma");
        AST token2 = Compounds();
        Process(TokenTypes.COMMA, "comma");
        AST token3 = Compounds();
        Process(TokenTypes.COMMA, "comma");
        AST token4 = Compounds();
        Process(TokenTypes.R_PARENT, $" closed parenthesis after expressions \" arc \" declaration");
        node = new ARC(null, token1, token2, token3, token4);
        return node;
        //Process(TokenTypes.R_PARENT,$" closed parenthesis after \"cos\" function args . Col {Lexer.Pos-CurrentToken.Value.ToString().Length}");

    }
    private AST Segment()
    {
        AST node = new AST();

        Process(TokenTypes.SEGMENT, "");
        if (CurrentToken.Type == TokenTypes.ID)
        {


            Token token = CurrentToken;

            Process(TokenTypes.ID, "");

            node = new SEGMENT(token, null, null);

            return node;
        }
        Process(TokenTypes.L_PARENT, $" open parenthesis before \" segment \" declaration");
        AST token1 = Compounds();
        Process(TokenTypes.COMMA, "comma");
        AST token2 = Compounds();
        Process(TokenTypes.R_PARENT, $" closed parenthesis after expressions \" segment \" declaration");
        node = new SEGMENT(null, token1, token2);
        return node;
    }

    private AST Line()
    {

        AST node = new AST();

        Process(TokenTypes.LINE, "");
        if (CurrentToken.Type == TokenTypes.ID)
        {


            Token token = CurrentToken;

            Process(TokenTypes.ID, "");

            node = new LINE(token, null, null);

            return node;
        }
        Process(TokenTypes.L_PARENT, $" open parenthesis before \" line \" declaration");
        AST token1 = Compounds();
        Process(TokenTypes.COMMA, "comma");
        AST token2 = Compounds();
        Process(TokenTypes.R_PARENT, $" closed parenthesis after expressions \" line \" declaration");
        node = new LINE(null, token1, token2);
        return node;
    }
    // Metodo para funcion logaritmo (puede tomar hasta dos argumentos y al menos uno)
    // Si toma dos , el primero es la base del logaritmo y el segundo es el argumento
    // Si toma solo uno, pues el argumento, entendiendo que la base es E 
     private AST Sequence(){
        
        Process(TokenTypes.L_KEY,"L_key");

        List<AST> k= new List<AST>(){Compounds()};
        
        
        while(CurrentToken.Type==TokenTypes.COMMA){
            Process(TokenTypes.COMMA,"The elements of a sequence must be separated by commas");
            
            k.Add(Compounds());
        }
   


        AST node;
        if(CurrentToken.Type==TokenTypes.THREEPOINT){
            bool allAreDouble= k.All(x=> x is Num);
            if(!allAreDouble){
                SyntaxError("Las declaraciones de secuencias de tipo rango , ya sea intervalo como infinitas solo puede contener valores doubles");
            }
            Process(TokenTypes.THREEPOINT,"...");
            if(CurrentToken.Type!=TokenTypes.R_KEY){
                
                
                AST final= Compounds();
                if(!(final is Num)){
                    SyntaxError("Las declaraciones de secuencias de tipo rango , ya sea cerrado como abierto solo puede contener valores doubles");
                }
                node=new RangoSequence2(k.Select(x=>(Num)x),(Num)final);
            }

            else{
                node=new OpenIntervalo(k.Select(x=>(Num)x));
            }

        }
        else{
            node=new SEQUENCE2();
            if(k.First()is null && k.Count()>1) {
                SyntaxError("candela no puedes tener eso ahi bro");
            }
            if(k.First()is null)k.RemoveAt(0);
            ((SEQUENCE2)node).creativo=k;
        }
        Process(TokenTypes.R_KEY,$"A closed key is missing from the declaration of a sequence");
        return node;
    }

    // MEtodo para cuando se reconoce un llamado de funcion
    private AST CallFunction()
    {
        AST node = new AST();
        Token token = CurrentToken;
        Process(TokenTypes.CALL, "Call");
        Process(TokenTypes.L_PARENT, $"open parenthesis before \"{token.Value}\" function args .Col {Lexer.Pos - CurrentToken.Value.ToString().Length}");
        // esta es la lista de argumentos que tiene la funcion
        List<AST> arguments = new List<AST>();
        //AST tree= Compounds();
        // Se agrega cada argumento
        arguments.Add(Compounds());

        while (CurrentToken.Type == TokenTypes.COMMA)
        {
            Process(TokenTypes.COMMA, $"Function args must be separated by commas.Col {Lexer.Pos - CurrentToken.Value.ToString().Length}");


            arguments.Add(Compounds());

        }
        node = new CallFUNCTION(token, arguments);
        Process(TokenTypes.R_PARENT, $"closed parenthesis after \"{token.Value}\" function args .Col {Lexer.Pos - CurrentToken.Value.ToString().Length}");
        return node;
    }

    public AST WalleFunction()
    {
        Token token = CurrentToken;
        Process(CurrentToken.Type, "Invalid Token");
        Process(TokenTypes.L_PARENT, $"open parenthesis before \" {token.Value} \" function args ");
        
        AST first = Compounds();
        AST second = new Empty();

        if (token.Type != TokenTypes.COUNT && token.Type != TokenTypes.POINTS && token.Type != TokenTypes.RANDOMS &&
        token.Type != TokenTypes.SAMPLES)
        {
            Console.WriteLine(909);
            Process(TokenTypes.COMMA, "Function args must be separated by commas");

            second = Compounds();
        }


        Process(TokenTypes.R_PARENT, "closed parenthesis after \"measure \" function args");

        return new FunctionWALLE(token, first, second);
    }

    // metodo para cuando se reconoce la declaracion de function
    private AST Function()
    {
        AST node = new AST();
        Dictionary<string, object> arguments = new Dictionary<string, object>();
        //Process(TokenTypes.FUNCTION,"Function");
        Token token = CurrentToken;
        //var g= (string)token.Value;
        Process(TokenTypes.ID, $" function name after its declaration (Surely there is a function with the same name). Col {Lexer.Pos - CurrentToken.Value.ToString().Length}");
        //ReservateKeywords.Keyword.Add((string)token.Value,TokenTypes.CALL);
        Principal.Functiones.Add((string)token.Value, node);
        Process(TokenTypes.L_PARENT, $"open parenthesis before \"{token.Value}\" function args .Col {Lexer.Pos - CurrentToken.Value.ToString().Length}");
        arguments.Add((string)CurrentToken.Value, 0);
        Process(TokenTypes.ID, $"variable name.Col {Lexer.Pos - CurrentToken.Value.ToString().Length}");
        // no es obligado el uso de arguemtnos
        while (CurrentToken.Type == TokenTypes.COMMA)
        {
            Process(TokenTypes.COMMA, "Function args must be separated by commas");
            //var j= new Var(CurrentToken);
            arguments.Add((string)CurrentToken.Value, 0);
            Process(TokenTypes.ID, $"variable name.Col {Lexer.Pos - CurrentToken.Value.ToString().Length}");
        }

        Process(TokenTypes.R_PARENT, $"closed parenthesis after \"{token.Value}\" function args .Col {Lexer.Pos - CurrentToken.Value.ToString().Length}");
        Process(TokenTypes.ASSIGN, $" return before function body.Col {Lexer.Pos - CurrentToken.Value.ToString().Length}");

        node = new FUNCTIONAL(token, arguments, Statement());

        return node;
    }

    // metodo para la funcion print
    private AST PRINT()
    {
        AST node = new AST();
        Process(TokenTypes.PRINT, "Print");
        //Process(TokenTypes.L_PARENT,$"open parenthesis before \"print\" function arg .Col {Lexer.Pos-CurrentToken.Value.ToString().Length}");
        //ReservateKeywords reserved= new ReservateKeywords();
        node = new PRINT(Statement());
        //Process(TokenTypes.R_PARENT,$"closed parenthesis after \"print\" function args .Col {Lexer.Pos-CurrentToken.Value.ToString().Length}");

        return node;
    }

    //Metodo para declaraciones de variables
    private AST Declaration()
    {

        List<AST> node = new List<AST>();
        TokenTypes type = (TokenTypes)(TypeData());


        node.Add(Statement());


        /* var f= new Var(CurrentToken);
         Process(TokenTypes.ID,$"variable name.Col {Lexer.Pos-CurrentToken.Value.ToString().Length}");
         node.Add(new VarDecl(f,type,AssignmentDecl()));
         */
        // el bucle es para leer varias declaraciones de variables
        while (CurrentToken.Type == TokenTypes.SEMI)
        {
            Process(TokenTypes.SEMI, "Comma");
            //var j= new Var(CurrentToken);
            //Process(TokenTypes.ID,$"variable name");
            if (CurrentToken.Type == TokenTypes.IN) break;
            node.Add(Statement());

        }

        Process(TokenTypes.IN, $"\"in\" keyword in expression let-in");
        Declarations arbol = new Declarations(Statement(), node);
        return arbol;
    }
    // metodo para la lectura de variables
    private AST Variable()
    {
        AST node = new Var(CurrentToken);
        //Process(TokenTypes.ID);

        return node;
    }

    private AST Assignment()
    {

        Token token = new Token(CurrentToken);// se guarda el token
        List<Var> listVar = new List<Var>();

        do
        {



            if (CurrentToken.Type == TokenTypes.ASSIGN) break;
            if (CurrentToken.Type == TokenTypes.COMMA)
                Process(TokenTypes.COMMA, "The args of an assignment must be separated by commas");

            if (CurrentToken.Type == TokenTypes.UNDERSCORE)
            {

                listVar.Add(new Var(CurrentToken));
                Process(TokenTypes.UNDERSCORE, "undescore");

            }
            else
            {
                listVar.Add((Var)Variable());
                Process(TokenTypes.ID, $"variable name .Col {Lexer.Pos - CurrentToken.Value.ToString().Length}");
            }

        } while (CurrentToken.Type == TokenTypes.COMMA);


        /*
        if(CurrentToken.Type == TokenTypes.ASSIGN)
            Process(TokenTypes.ASSIGN,$" \"equal\" sign to declare variable. Col {Lexer.Pos-CurrentToken.Value.ToString().Length}");
        else if(CurrentToken.Type ==TokenTypes.ASSIGN_DIV || CurrentToken.Type ==TokenTypes.ASSIGN_PLUS || 
        CurrentToken.Type ==TokenTypes.ASSIGN_MUL || CurrentToken.Type ==TokenTypes.ASSIGN_MINUS || CurrentToken.Type ==TokenTypes.ASSIGN_MOD)
        { 
            
            if(CurrentToken.Type==TokenTypes.ASSIGN_DIV){
                Process((TokenTypes)CurrentToken.Type,$" \" equal\" sign to declare variable. Col {Lexer.Pos-CurrentToken.Value.ToString().Length}");
                AST sumando=new BinaryOperator(node,new Token(TokenTypes.FLOAT_DIV,"/"),Compounds());
                return new Assign((Var)node,token,sumando);
            }
            
            if(CurrentToken.Type==TokenTypes.ASSIGN_MUL){
                Process((TokenTypes)CurrentToken.Type,$" \" equal\" sign to declare variable. Col {Lexer.Pos-CurrentToken.Value.ToString().Length}");
                AST sumando=new BinaryOperator((Var)node,new Token(TokenTypes.MULT,"*"),Compounds());
                return new Assign((Var)node,token,sumando);
            }
            
            if(CurrentToken.Type==TokenTypes.ASSIGN_PLUS){
                Process((TokenTypes)CurrentToken.Type,$" \" equal\" sign to declare variable. Col {Lexer.Pos-CurrentToken.Value.ToString().Length}");
                AST sumando=new BinaryOperator((Var)node,new Token(TokenTypes.PLUS,"+"),Compounds());
                return new Assign((Var)node,token,sumando);
            }

            if(CurrentToken.Type==TokenTypes.ASSIGN_MINUS){
                Process((TokenTypes)CurrentToken.Type,$" \" equal\" sign to declare variable. Col {Lexer.Pos-CurrentToken.Value.ToString().Length}");
                AST sumando=new BinaryOperator((Var)node,new Token(TokenTypes.MINUS,"-"),Compounds());
                return new Assign((Var)node,token,sumando);
            }
            
            if(CurrentToken.Type==TokenTypes.ASSIGN_MOD){
                Process((TokenTypes)CurrentToken.Type,$" \" equal\" sign to declare variable. Col {Lexer.Pos-CurrentToken.Value.ToString().Length}");
                AST sumando=new BinaryOperator((Var)node,new Token(TokenTypes.MOD,"%"),Compounds());
                return new Assign((Var)node,token,sumando);
            }
            
            
        }
        */
        Process(TokenTypes.ASSIGN, "An assigment symbol must follow each variable(s) list");

        return new Assign(listVar, token, Statement());// no hay necesidad de poner Statement sino Compounds
    }
    // metodo para condicionales 
    private AST Conditional()
    {
        Process(TokenTypes.IF, "IF");


        AST node = Statement();


        Process(TokenTypes.THEN, "THEN");
        AST node2 = Statement();

        Process(TokenTypes.ELSE, $" token \" else \" in conditional expression. Col {Lexer.Pos - CurrentToken.Value.ToString().Length}");

        if (CurrentToken.Type == TokenTypes.RETURN)
        {
            Process(TokenTypes.RETURN, "Return ");
        }
        node = new Condition(node, node2, Statement());

        //Process(TokenTypes.R_KEYS);

        return node;
    }

    // metodo para ciclos (en HULK no es necesario)
    /*  private AST Cicle()
      {
          Process(TokenTypes.WHILE);
          Process(TokenTypes.L_PARENT);

          AST node = Statement();

          Process(TokenTypes.R_PARENT);
          Process(TokenTypes.L_KEYS);

          node = new Cicle(node, StatementList());

          Process(TokenTypes.R_KEYS);

          return node;
      }
  */




    // metodo para buscar datos sencillos tales como num,string,bool
    private TokenTypes TypeData()
    {
        TokenTypes token = TokenTypes.NUMBER;// por defecto

        if (CurrentToken.Type == TokenTypes.LET)
        {
            Process(TokenTypes.LET, $"let keyword in expression let-in. Col {Lexer.Pos - CurrentToken.Value.ToString().Length}");
            token = TokenTypes.LET;
        }
        return token;
    }
    #endregion

}