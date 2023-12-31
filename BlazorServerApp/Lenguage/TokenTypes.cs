namespace GOLenguage;


// enum de todos los tokens y los colores permitidos por la gramatica 
public enum TokenTypes
{

    #region Types of Datas

    NUMBER,
    STRING,
    BOOLEAN,
    #endregion

    #region Binarys Operators

    PLUS,
    MINUS,
    MULT,
    FLOAT_DIV,
    MOD,
    ASSIGN_PLUS,
    ASSIGN_MINUS,
    ASSIGN_MUL,
    ASSIGN_MOD,
    ASSIGN_DIV,

    #endregion

    #region Compare Operators

    SAME,
    DIFFERENT,
    LESS,
    GREATER,
    LESS_EQUAL,
    GREATER_EQUAL,
    NOT,
    AND,
    OR,

    #endregion

    #region Symbols

    L_PARENT,
    R_PARENT,
    L_KEY,
    R_KEY,

    PI,
    #endregion

    #region Reserved Keywords
    LET,
    IN,
    IF,
    THEN,
    ELSE,
    SECUENCE,
    UNDEFINED,
    LINE,
    POINT,
    SEGMENT,
    RAY,
    CIRCLE,
    ARC,
    TRUE,
    FALSE,

    #endregion
    #region Comandos
    DRAW,
    COLOR,
    RESTORE,

    #endregion

    #region Functions

    INTERSECT,
    COUNT,
    SAMPLES,
    RANDOMS,
    MEASURE,
    POINTS,
    
    #endregion

    #region Auxiliars Tokens

    ID,
    COMMA,
    SEMI,
    ASSIGN,
    L_COMMENT,
    R_COMMENT,
    UNDERSCORE,
    RETURN,
    PRINT,
    EOF,
    CALL,
    SEN,
    COS,
    FUNCTION,
    LOG,
    REST,
    THREEPOINT,
    IMPORT,

    #endregion
}

public enum ColorType
{

    red,
    magenta,
    blue,
    green,
    yellow,
    cyan,
    white,
    black,
    gray
}


public enum WalleFigures
{

    POINT,
    SEGMENT,
    LINE,
    RAY,
    CIRCLE,
    ARC
}