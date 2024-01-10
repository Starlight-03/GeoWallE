using System;
using System.Collections.Generic;

public class Token  // Utilizaremos esta clase como herramienta para crear los "tokens"
{
    // Los tokens son una herramienta para facilitar el trabajo a la hora del parseo
    // Consisten en una dupla de tipo y valor:
    // - En el valor se guardará el "nombre" del token, lo que nosotros podemos leer
    // - En el tipo de guarda el tipo de token que sea:
    //   - Identificador
    //   - Palabra reservada (Keyword)
    //   - Separador
    //   - Operador
    //   - Número
    //   - Objeto
    //   - Comentario

    public string Value { get; private set; }

    public TokenType Type { get; private set; }

    public Token(string value, TokenType type)
    {
        Value = value;
        Type = type;
    }

    public static bool IsToken(string tokenValue) => Values.ContainsKey(tokenValue);
    // Devuelve si un token dado pertenece a la lista de tokens predeterminados

    public static Token GetToken(string tokenValue) => Values[tokenValue];
    // Devuelve el token correspondiente al valor del string tokenValue

    public static IEnumerable<string> GetTokens(Func<Token, bool> filter)
    {
        foreach (Token token in Values.Values)
            if (filter(token))
                yield return token.Value;
    }

    private static readonly Dictionary<string, Token> Values = new()
    {
        {"+", new Token("+", TokenType.Operator)},
        {"-", new Token("-", TokenType.Operator)},
        {"*", new Token("*", TokenType.Operator)},
        {"/", new Token("/", TokenType.Operator)},
        {"%", new Token("%", TokenType.Operator)},
        {"^", new Token("^", TokenType.Operator)},
        {"_", new Token("_", TokenType.Operator)},
        {"=", new Token("=", TokenType.Operator)},
        {"=>", new Token("=>", TokenType.Operator)},
        {"<", new Token("<", TokenType.Operator)},
        {"<=", new Token("<=", TokenType.Operator)},
        {">", new Token(">", TokenType.Operator)},
        {">=", new Token(">=", TokenType.Operator)},
        {"==", new Token("==", TokenType.Operator)},
        {"!=", new Token("!=", TokenType.Operator)},
        {"...", new Token("...", TokenType.Operator)},
        {"(", new Token("(", TokenType.Separator)},
        {")", new Token(")", TokenType.Separator)},
        {"{", new Token("{", TokenType.Separator)},
        {"}", new Token("}", TokenType.Separator)},
        {",", new Token(",", TokenType.Separator)},
        {";", new Token(";", TokenType.Separator)},
        {"\n", new Token("\n", TokenType.Separator)},
        {"not", new Token("not", TokenType.Keyword)},
        {"and", new Token("and", TokenType.Keyword)},
        {"or", new Token("or", TokenType.Keyword)},
        {"import", new Token("import", TokenType.Keyword)},
        {"draw", new Token("draw", TokenType.Keyword)},
        {"let", new Token("let", TokenType.Keyword)},
        {"in", new Token("in", TokenType.Keyword)},
        {"if", new Token("if", TokenType.Keyword)},
        {"then", new Token("then", TokenType.Keyword)},
        {"else", new Token("else", TokenType.Keyword)},
        {"number", new Token("number", TokenType.Keyword)},
        {"point", new Token("point", TokenType.Keyword)},
        {"line", new Token("line", TokenType.Keyword)},
        {"ray", new Token("ray", TokenType.Keyword)},
        {"segment", new Token("segment", TokenType.Keyword)},
        {"circle", new Token("circle", TokenType.Keyword)},
        {"arc", new Token("arc", TokenType.Keyword)},
        {"measure", new Token("measure", TokenType.Keyword)},
        {"intersect", new Token("intersect", TokenType.Keyword)},
        {"undefined", new Token("undefined", TokenType.Keyword)},
        {"color", new Token("color", TokenType.Keyword)},
        {"restore", new Token("restore", TokenType.Keyword)},
        {"black", new Token("black", TokenType.Keyword)},
        {"blue", new Token("blue", TokenType.Keyword)},
        {"red", new Token("red", TokenType.Keyword)},
        {"yellow", new Token("yellow", TokenType.Keyword)},
        {"green", new Token("green", TokenType.Keyword)},
        {"cyan", new Token("cyan", TokenType.Keyword)},
        {"magenta", new Token("magenta", TokenType.Keyword)},
        {"white", new Token("white", TokenType.Keyword)},
        {"gray", new Token("gray", TokenType.Keyword)}
    };
}

public enum TokenType
{
    Keyword,
    Separator,
    Operator,
    Identifier,
    Number,
    Text
}