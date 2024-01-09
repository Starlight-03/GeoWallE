using System.Collections.Generic;

public class Lexer : ILexer
{
    public List<string> Errors { get; private set; }

    public List<Token> Tokens { get; private set; }

    private readonly LexReader reader;

    public Lexer(string statements)
    {
        Errors = new List<string>();
        Tokens = new List<Token>();
        reader = new LexReader(Errors, statements);

        Tokenize();
    }
    
    private void Tokenize()
    {
        void Add(Token token){
            if (token is not null)
                Tokens.Add(token);
        }

        while (!reader.EOF) {
            if      (reader.IsWhiteSpace())                         reader.Move();
            else if (reader.IsLetterOrUnderscore())                 Add(reader.GetKeywordOrId());
            else if (reader.IsNumber())                             Add(reader.GetNumber());
            else if (reader.IsOperatorOrSeparator(out Token op))    Add(op);
            else if (reader.IsString())                             Add(reader.GetText());
            else if (reader.IsComment()){
                reader.ReadComment(out int i);
                for (; i > 0; i--) Add(Token.GetToken("\n"));
            }
            else if (reader.EOL){
                reader.Move();
                Add(Token.GetToken("\n"));
            }
        }
    }
}