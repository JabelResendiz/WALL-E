


namespace InterpreterDyZ;

using System.ComponentModel.Design;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Security.AccessControl;
using Microsoft.JSInterop;


public class Interpreter : NodeVisitor
{

    
    private double recursiveCount;
    private Parser? Parser;
    public Dictionary<string, object> Scope;// variables declaradas con let (no puede ser global en todo el programa)
    //public Dictionary<string,AST>Function= new Dictionary<string, AST>();

    
    //public ReservateKeywords reserved= new ReservateKeywords();
    //public Principal principal= new Principal();
    public Interpreter(Parser parser)
    {
        
        Parser = parser;
        Scope = new Dictionary<string, object>();// esto es una variable local
        
        appendText();// inicializando el lienzo una sola vez
    }

    # region 1 Semantic Error
    private void SemanticError(string error)
    {   
        Console.WriteLine("!SEMANTIC ERROR: "+error);
        throw new Exception();
    }

    #endregion

    #region 2 Evaluador de cada nodo del AST
    //evaluador de una funcion logaritmo


    // metodo llamado para inicalizar el lienzo de la function setup de la liberia P5.JS
 

    public async void appendText(){
        
        try{
        await Principal._jsRuntime.InvokeAsync<string>("setup","Text will append when you close this alert");
        }

        catch(TaskCanceledException){
            Console.WriteLine("wtf men 123445567890-48958309485093840958");
        }
    }


    public override object VisitColor(COLOR node,Dictionary<string,object>Scope){

        
        
        if(node.token is TokenTypes.COLOR){

            node.Push();
        }

        else{
            node.Restore();
        }
       
        return 0;
    }


    public override object VisitFunWalle(FunctionWALLE node, Dictionary<string, object> Scope)
    {
        object first= Visit(node.first,Scope);
        object second= Visit(node.second,Scope);

        if(node.token.Type==TokenTypes.MEASURE){
            
            
            if(first is POINT && second is POINT){
                return node.Measure((POINT)first,(POINT)second);
            }
            SemanticError($"{((first is POINT) ? second : first)} is not a point");
        }

        else if(node.token.Type==TokenTypes.INTERSECT){

            if(first is FIGURE && second is FIGURE){
                
                return node.Intersect((FIGURE) first,(FIGURE) second).Select(x=>Visit((AST)x,Scope)).ToList();
            }
            SemanticError($"{((first is FIGURE) ? second : first)} is not a figure");
            
        }

        return 0;
    }

    public override object VisitSequence(SEQUENCE node, Dictionary<string, object> Scope)
    {
        List<object> list= new List<object>();
        
        foreach(AST i in node.sequence){
            list.Add(Visit(i,Scope));
        }
        return list;
    }

    public override object VisitDraw(Draw node, Dictionary<string, object> Scope)
    {
        
        
        object x=Visit(node.name,Scope);// secuencia o un objeto 
        
        if(!(x is List<object>)){

            x=new List<object>{x};
            //h=Enumerable.Repeat(h,1);

        }

        foreach(var item in (List<object>)x){
                
                if(item is IDraw){
                 ((IDraw)item).Draw();
                }
                else{
                    SemanticError($"Variable \"{item}\" cannot be draw");
                }
        }
       
        return 0;
    }

    public override object VisitFigure(FIGURE node, Dictionary<string, object> Scope)
    {
        FIGURE l=node.Clone();
        // (ejemplos) point (12,23) circle(point p2,)
        if(node.name is null ){
            Dictionary<string,object> ScopeClon= new Dictionary<string, object>(Scope);
            // llamando a la funcion que devuelve una figura
            object f= Visit(node.firstParam,ScopeClon);
            object g= Visit(node.secondParam,ScopeClon);
            
            

            if(!(f is POINT) && !(f is double)){
                SemanticError($"The first Param : {((Var)node.firstParam).Value} of value {f} is not of type POINT or DOUBLE");
            }

            if(!(g is POINT) && !(g is double)){
                SemanticError($"The second Param : {((Var)node.secondParam).Value} of value {g} is not of type POINT or DOUBLE");
            }
            
            l.Check(f,g);


        }
        else{
        
        Scope.Add((string)node.name.Value,node);
        }
        
        
        return l;
        
    }

    public override object VisitLOG(LOG node, Dictionary<string, object> Scope)
    {
        if(node.bases is null && node.Statement is null){
            SemanticError("The log function takes at least one arg.");
        }
        
        object tree= Visit(node.Statement,Scope);
            if(tree is string || tree is bool){
                SemanticError("The args of logarithm function is a double variable");
            }

            if(Convert.ToSingle(tree)<=0){
                SemanticError("The arg of the logarithm function is less than 0");
        }

        if(!(node.bases is null))
        {   
            object tree2=Visit(node.bases,Scope);
             if(!(tree2 is double)){
                SemanticError("The base of logarithm is a double variable");
            }

            if(Convert.ToSingle(tree2)<=0 || Convert.ToSingle(tree2)==1 ){
                SemanticError("Logarithm to base less 0 or 1 is not defined");
            }

            return Math.Log(Convert.ToSingle(tree))/Math.Log(Convert.ToSingle(tree2));
        }
        return Math.Log(Convert.ToSingle(tree));
    }
    public override object VisitSen(Sen node, Dictionary<string,object> Scope)
    {
        if(node.Statement is null){
            SemanticError("The function \"sen\" take one arg");
        }
        return Math.Sin(Convert.ToSingle(Visit(node.Statement,Scope)));
    }


    // evaluador de una funcion coseno
    public override object VisitCos(Cos node, Dictionary<string, object> Scope)
    {
        if(node.Statement is null){
            SemanticError("The function \"cos\" take one arg");
        }
        //Console.WriteLine(Math.Cos(Convert.ToSingle(Visit(node.Statement,Scope))));
        return Math.Cos(Convert.ToSingle(Visit(node.Statement,Scope)));   
    }
    public override object VisitFunctional(FUNCTIONAL node,Dictionary<string,object>Scope)
    {
        //ReservateKeywords.Keyword.Add($"node.name",TokenTypes.CALL);
        Principal.Functiones[node.name]= node;
        //Principal.Function.Add(node.name}",node);
        return 0;
    }

// metodo evaluador para llamadas de funciones
    public override object VisitCallFunction(CallFUNCTION node,Dictionary<string,object>Scope)
    {
        recursiveCount+=1;

   /*     
        if(recursiveCount>= 389983){
            Console.WriteLine("RecursionError: maximum recursion depth exceeded");
            throw new Exception();
        }
*/
        
          if(Principal.Functiones.ContainsKey(node.name)){
            int i=0;
            
            Dictionary<string,object> local = new Dictionary<string, object>(((FUNCTIONAL)Principal.Functiones[node.name]).argumentos);
            // se crea un diccionario local para modificar los valores de local sin cambiar los de la variable estatica
            
            if(local.Count!= node.arg.Count){
                SemanticError($"Function \" {node.name}\" receives {local.Count} argument(s), but {node.arg.Count} were gives");
            }

            
            foreach(KeyValuePair<string,object> item in local)
            {
                object g=Visit(node.arg[i],Scope);
                if(g is null){
                    SemanticError($"Empty args of the \"{node.name} \" function have been detected");
                    
                }
                local[item.Key]=g;
                i+=1;
                
            }

            
            
            object tree= Visit(((FUNCTIONAL)Principal.Functiones[node.name]).Statement,local);
           
            return tree;
        }

        Console.WriteLine($"Function {node.name } succesfull passed");
        Console.WriteLine();
        return 0;
    }
    
    // metodo evaluador para la funcion print
    public override object VisitShowLine(PRINT node,Dictionary<string,object>Scope)
    {

        object tree=Visit(node.Compound,Scope);
        Console.WriteLine(tree);
        return tree;
    }
    
    // metodo para trabajar con operador binarios
    public override object VisitBinaryOperator(BinaryOperator node,Dictionary<string,object>Scope)
    {
        object result = 0;// puede ser lo mismo un int,float,bool

        object left = Visit(node.Left,Scope);
        object right = Visit(node.Right,Scope);

        if(left is null || right is null)
            SemanticError($"{((left is null)?"Left":"Right")} node has not been detected");
        // si los object left and right no son del mismo tipo salta una excepcion
       

        switch (node.Operator.Type)
        {
           
            case TokenTypes.PLUS:

                if (left is string && right is string)

                    result = (string)left + (string)right;
                else if(left is bool  || right is bool)
                    SemanticError($"Operator \" + \" cannot be used between a \"bool\" ");

                else if(left is List<object> || right is List<object>){

                    result= ((List<object>)left).Concat((List<object>)right).ToList();
                }
                else{
                    if(left is string || right is string){
                        SemanticError($"Operator \" + \" cannot be used between \"{left.GetType()}\" and \"{right.GetType()}\"");
                    }
                    else{
                        result = Convert.ToDouble(left) + Convert.ToDouble(right);
                    }
                }
                break;

            case TokenTypes.MINUS:
                
                if(left is string || left is bool || right is string || right is bool){
                    SemanticError($"Operator \"- \" cannot be used between not \"{left.GetType()}\" and \"{right.GetType()}\"");
                }

                result =Convert.ToDouble(left) - Convert.ToDouble(right);
                
                break;
            
            case TokenTypes.MULT:

                if(left is string || left is bool || right is string || right is bool){
                    SemanticError($"Operator \"* \" cannot be used between not \"{left.GetType()}\" and \"{right.GetType()}\"");
                }

                result =Convert.ToDouble(left) * Convert.ToDouble(right);
                
                break;
            
            case TokenTypes.FLOAT_DIV:

                if(left is string || left is bool || right is string || right is bool){
                    SemanticError($"Operator \"/ \" cannot be used between not \"{left.GetType()}\" and \"{right.GetType()}\"");
                }
                if(Convert.ToDouble(right)==0){
                    SemanticError("Division by constant 0 is not defined");
                }
                result = Convert.ToDouble(left) / Convert.ToDouble(right);
                
                break;

            
            case TokenTypes.MOD:

            {
                if(left is string || left is bool || right is string || right is bool){
                    SemanticError($"Operator \"% \" cannot be used between not \"{left.GetType()}\" and \"{right.GetType()}\"");
                }

                result = Convert.ToDouble(left)% Convert.ToDouble(right);
                
                break;
            }

            
            case TokenTypes.SAME:
                
                if (left is string && right is string)

                    result = (string)left == (string)right;
                else if(left is bool  && right is bool)
                    result = Convert.ToSingle(left) == Convert.ToSingle (right);
                else{
                    if(left is string || left is bool){
                        SemanticError($"Operator \"== \" cannot be used between not \"{left.GetType()}\" and \"{right.GetType()}\"");
                    }
                    if(right is string || right is bool){
                        SemanticError($"Operator \"== \" cannot be used between not \"{left.GetType()}\" and \"{right.GetType()}\"");
                    }
                    else{
                        result =Convert.ToDouble(left) == Convert.ToDouble(right);
                    }
                }
                break;
            
            case TokenTypes.DIFFERENT:
            
                if (left is string && right is string)

                    result = (string)left != (string)right;
                else if(left is bool  && right is bool)
                    result = Convert.ToSingle(left) != Convert.ToSingle (right);
                else{
                    if(left is string || left is bool){
                        SemanticError($"Operator \"!= \" cannot be used between not \"{left.GetType()}\" and \"{right.GetType()}\"");
                    }
                    if(right is string || right is bool){
                        SemanticError($"Operator \"!= \" cannot be used between not \"{left.GetType()}\" and \"{right.GetType()}\"");
                    }
                    else{
                        result = Convert.ToDouble(left) != Convert.ToDouble(right);
                    }
                }
                break;
            
            case TokenTypes.LESS:

                if(left is string || left is bool || right is string || right is bool){
                    SemanticError($"Operator \"< \" cannot be used between not \"{left.GetType()}\" and \"{right.GetType()}\"");
                }
                result = Convert.ToDouble(left) < Convert.ToDouble(right);
                
                break;

            case TokenTypes.GREATER:

                if(left is string || left is bool || right is string || right is bool){
                    SemanticError($"Operator \"> \" cannot be used between not \"{left.GetType()}\" and \"{right.GetType()}\"");
                }
                result = Convert.ToDouble(left) > Convert.ToDouble(right);
                
                break;
            
            case TokenTypes.LESS_EQUAL:

                if(left is string || left is bool || right is string || right is bool){
                    SemanticError($"Operator \"<= \" cannot be used between not \"{left.GetType()}\" and \"{right.GetType()}\"");
                }
                result = Convert.ToDouble(left) <= Convert.ToDouble(right);
                
                break;
            
            case TokenTypes.GREATER_EQUAL:
                if(left is string || left is bool || right is string || right is bool){
                    SemanticError($"Operator \">= \" cannot be used between not \"{left.GetType()}\" and \"{right.GetType()}\"");
                }

                result = Convert.ToDouble(left) >= Convert.ToDouble(right);
                
                break;
            
            case TokenTypes.AND:

                object nodeLeftBool=Visit(node.Left,Scope);
                object nodeRightBool=Visit(node.Right,Scope);
                if( nodeLeftBool is bool && nodeRightBool is bool)
                    result = (bool)nodeLeftBool && (bool)nodeRightBool;
                else
                    SemanticError($"{((nodeLeftBool is bool)? nodeRightBool : nodeLeftBool)} is not a boolean expression");
                break;
            
            case TokenTypes.OR:

                object nodeLeftBool2=Visit(node.Left,Scope);
                object nodeRightBool2=Visit(node.Right,Scope);
                if( nodeLeftBool2 is bool && nodeRightBool2 is bool)
                    result = (bool)nodeLeftBool2 || (bool)nodeRightBool2;
                else
                    SemanticError($"{((nodeLeftBool2 is bool)? nodeRightBool2 : nodeLeftBool2)} is not a boolean expression");
                break;
            
                
            
        }

        return result;
    }


    // metodo para evaluar operadores unarios
    public override object VisitUnaryOperator(UnaryOperator node,Dictionary<string,object>Scope)
    {
        object result = 0;
        if(node.Operator.Type== TokenTypes.NOT){
            object resultado=!(bool)Visit(node.Expression,Scope);

            return resultado;
        }
        object exp= Visit(node.Expression,Scope);

        if(!(exp is double))
        {
            SemanticError($"Unary Operator \"+ \" cannot be used between if not is \"double\"");
        
        }


        switch (node.Operator.Type)
        {
            case TokenTypes.PLUS:

                result = +(double)exp;

                break;
            
            case TokenTypes.MINUS:

                result = -(double)exp;

                break;
        }

        return result;
    }

    // metodo para evaluar cada instruccion del codigo
    public override object VisitInstructions(Instructions node,Dictionary<string,object>Scope)
    {
        foreach (var item in node.Commands)
        {
            recursiveCount=0;
            object output =Visit(item,Scope);
            Console.WriteLine((output is string)?(string)output: (output is bool)? (bool)output : (output is double)?Convert.ToDouble(output):(output is TokenTypes.UNDEFINED)?"undefined":"debe ser una secuencia");
            //Scope.Clear();
            
        }

        Console.WriteLine(recursiveCount);

        Principal.Functiones.Clear();
        COLOR.stackColor.Clear();
        Scope.Clear();
        
        return 0;
    }

    public override object VisitDeclarations(Declarations node,Dictionary<string,object>Scope)
    {   

        Dictionary<string,object>cloneScope= new Dictionary<string, object>(Scope);
        
        foreach (var item in node.Commands)
        {
            Visit(item,cloneScope);
        }
        
        object f = Visit(node.instruccion,cloneScope);
        node.Scope=new Dictionary<string, object>(node.Scope);
        return f;
    }
    
    public override object VisitAssign(Assign node,Dictionary<string,object>Scope)
    {
        object v=Visit(node.Right,Scope);

        // a= secuencia
        
        /*
        if(node.Left.Count ==1){

            string name = (string)node.Left[0].Value;

            if(Scope.ContainsKey(name)){
                Scope.Remove(name);
            }
            Scope.Add(name,Visit(node.Right,Scope));
            
        }
*/
        
        // a,_=secuencia
        // a,b,rest= secuencia
         
        if(v is List<object>){
            
            
        
            for(int i=0;i<node.Left.Count;i++){
                
                if(node.Left[i].Token.Type != TokenTypes.UNDERSCORE){
                    
                    if((string)node.Left[i].Value== "rest"){
                        if(i>= ((List<object>)v).Count){
                            Scope.Add("rest",new List<object>());
                        }
                        else{
                            
                            List<object> s =((List<object>)v).Skip(i).ToList();
                            Scope.Add("rest",s);
                            ((List<object>)v).Clear();// se vacia la lista 
                        }
                    }

                    else if(i<((List<object>)v).Count){
                        if(i==node.Left.Count-1){
                             List<object> s =((List<object>)v).Skip(i).ToList();
                            Scope.Add((string)node.Left[i].Value,s);
                        }
                        else{
                            Scope.Add((string)node.Left[i].Value,((List<object>)v)[i]);
                            
                            
                        }
                    }
                    else{
                        Scope.Add((string)node.Left[i].Value,TokenTypes.UNDEFINED);
                    }
                }
            }
        }
        else{
            // a,b,c,_= 1;
            //a=1 y el resto es undefined
            
            foreach(Var g in node.Left){
                if(g==node.Left[0]) Scope.Add((string)g.Value,v);

                else Scope.Add((string)g.Value,TokenTypes.UNDEFINED);
            }
        }

        return 0;
    }
    // metodo para evaluar expressiones condicionales
    public override object VisitCondition(Condition node,Dictionary<string,object>Scope)
    {
        object condition= Visit(node.Compound,Scope);
        
        if(!(condition is bool ) && !(condition is double)){
            SemanticError("A \"boolean\" expression was not detected in the expression conditional");

        }
        if(condition is double){
            condition=Convert.ToBoolean((double)condition);
        }
        if ((bool)condition)

            return Visit(node.StatementList,Scope);
        

        return Visit(node.StatementElse,Scope);
    }
    
    // metodo para evaluar expression en ciclo(aunque no es expression para este interprete)
    public override object VisitCicle(Cicle node)
    {
        
        while ((bool)Visit(node.Compound,Scope))
        {
            Visit(node.StatementList,Scope);
        }

        return 0;
    }
    

    // metodo que retorna el valor de una variable local
    public override object VisitVar(Var node,Dictionary<string,object>Scope)
    {
        
        if(node.Token.Type==TokenTypes.PI)
            return Math.PI ;
        
        string name = (string)node.Value;
        object value=null;

        if(Scope.ContainsKey(name))
        {
            value = Scope[name];
        }
        if (value is null)

            SemanticError($"Variable \"{name}\" was not found");
        
        return value;
    }


    // metodo que retorna el valor que contiene una clase Num
    public override object VisitNum(Num node)
    {
        return node.Value;
    }
    

    // metodo que retorna e valor de un bool
    public override object VisitBool(Bool node,Dictionary<string,object>Scope)
    {
        return node.Value;
    }


    // metodo que retorna el valor de una clase Cadene
    public override object VisitCadene(Cadene node)
    {
        return node.Value;
    }

    // metodo que guarda el valor de una variable en Scope local
    public override object VisitVarDecl(VarDecl node,Dictionary<string,object>Scope) 
    {
        
        if(node.Type == TokenTypes.LET)
        {   
            // si la variable ya existia se sobreescribe entonces
            if(Scope.ContainsKey((string)node.Node.Value)){
                Scope.Remove((string)node.Node.Value);
            }
            Scope.Add((string)node.Node.Value,Visit(((Assign)node.Value).Right,Scope));
        }
        return 0; 
    }
    
    //public override object VisitType(Type node) { return 0; }

    public override object VisitEmpty(Empty node) { return 0; }

// se va a crear el AST y se va a dar a llenar el diccionario Scope
    public object Interpret()
    {
        
        AST tree = Parser.Parse();

        if (tree == null)

            return -1;
        
        //return tree;
        return Visit(tree,Scope);
    }
    #endregion
}

