using System.Collections.Generic;

public interface ILexer
{
    public List<string> Errors { get; }

    public List<Token> Tokens { get; }
}