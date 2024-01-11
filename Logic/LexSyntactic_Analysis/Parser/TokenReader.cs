using System.Collections.Generic;

public class TokenReader
{
	private readonly List<Token> tokens;

	private int i;

	public int Line { get; private set; }

	public TokenReader(List<Token> tokens)
	{
		this.tokens = tokens;
		Line = 1;
		i = 0;
		JumpLine();
	}

	public bool EOF => i >= tokens.Count;

	public bool EOS => CurrentIs(";");

	public bool CurrentIs(string value) => Current().Value == value;

	public bool NextIs(string value) => LookNext().Value == value;

	public Token Current() => !EOF ? tokens[i] : tokens[0];

	private Token LookNext() => (i < tokens.Count) ? tokens[i + 1] : tokens[^1];

	public Token LookBack(int k = 1) => (i >= k) ? tokens[i - k] : tokens[0];

	public void MoveForward(int k = 1) => i += k;

	public void MoveBack(int k = 1) => i -= k;

	public bool Match(string value)
	{
		if (EOF || Current().Value != value)
			return false;

		MoveForward();
		JumpLine();
		return true;
	}

	public bool Match(TokenType type)
	{
		if (EOF || Current().Type != type)
			return false;

		MoveForward();
		JumpLine();
		return true;
	}

	private void JumpLine()
	{
		if (Match("\n")){
			Line++;
			JumpLine();
		}
	}
}
