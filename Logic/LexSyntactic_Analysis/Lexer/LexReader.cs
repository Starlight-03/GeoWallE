using System.Collections.Generic;

public class LexReader
{
    private readonly List<string> errors;

    private readonly string statements;

    private int i;

    private int line;

    public LexReader(List<string> errors, string statements)
    {
        this.errors = errors;
        this.statements = statements;
        i = 0;
        line = 1;
    }

    public void AddError(string info) => errors.Add($"Lex Error: {info}. At line: {line}.");

    public bool EOF => i >= statements.Length;

    public bool EOL => Look() == '\n';

    public bool IsWhiteSpace() => char.IsWhiteSpace(Look());

    public bool IsLetterOrUnderscore() => char.IsLetter(Look()) || Look() == '_';

    public bool IsNumber() => char.IsDigit(Look());

    public bool IsComment() => Look() == '/' && (LookAhead() == '/' || LookAhead() == '*');

    public bool IsString() => Look() == '\"';

    public char Look() => !EOF ? statements[i] : ' ';

    public bool CanLookAhead() => !EOF && i < statements.Length - 1;

    public char LookAhead() => CanLookAhead() ? statements[i + 1] : ' ';

    private char Read() => !EOF ? statements[i++] : ' ';

    public void Move(int k = 1)
    {
        for (; k > 0; k--){
            if (EOL)
                line++;
            
            i++;
        }
    }

    public Token GetKeywordOrId() // Devuelve un token de tipo keyword o literal booleano o identificador
    {
        // Si se encuentra una letra o un underscore, se busca si el token resultante es una palabra reservada o un identificador

        string token = Read().ToString();
        while (!EOF && char.IsLetterOrDigit(Look()))
            token += Read();

        // Si al terminarse el token, este pertenece a los tokens predefinidos, entonces es una palabra reservada y se guarda como tal
        // De lo contrario, se guarda como identificador

        return Token.IsToken(token) ? Token.GetToken(token) : new Token(token, TokenType.Identifier);
    }

    public Token GetNumber() // Devuelve un token de tipo numérico
    {
        // Si se encuentra un dígito, se busca si el token es de tipo numérico

        string number = Read().ToString();
        bool decPoint = false;

        while (!EOF && (char.IsDigit(Look()) || (Look() == '.' && !decPoint))){
            decPoint |= Look() == '.';
            number += Read();
        }

        if ((decPoint && Look() == '.') || char.IsLetter(Look())){
            while (!EOF && char.IsLetterOrDigit(Look()))
                number += Read();
            AddError($"'{number}' is not a valid token");
            return null;
        }

        return new Token(number, TokenType.Number);
    }

    public bool IsOperatorOrSeparator(out Token token)
    {
        foreach (string op in Token.GetTokens(t => t.Type == TokenType.Operator && t.Value.Length == 2)){
            if (Look() == op[0] && LookAhead() == op[1]){
                token = Token.GetToken(op);
                Move(2);
                return true;
            }
        }
        foreach (string op in Token.GetTokens(t => (t.Type == TokenType.Operator && t.Value.Length == 1) || t.Type == TokenType.Separator)){
            if (Look() == op[0]){
                token = Token.GetToken(op);
                Move();
                return true;
            }
        }
        token = null;
        return false;
    }
    
    public Token GetText()
    {
        Move();

        string s = "";
        while (!EOF && !EOL && Look() != '\"')
            s += Read();
        
        if (EOL || EOF){
            AddError("Missing closing \'\"\' in string expression");
            return null;
        }

        Move();
        return new Token(s, TokenType.Text);
    }

    public void ReadComment(out int i)
    {
        bool inline = LookAhead() == '/';
        int line = this.line;

        while (inline ? (!EOF && !EOL) : (!EOF && !(Look() == '*' && LookAhead() == '/')))
            Move();
        
        if (!inline && EOF)
            AddError("Missing 'closing comment' operator");
        else if (!inline)
            Move(2);
        else
            Move();
        
        i = this.line - line;
    }
}