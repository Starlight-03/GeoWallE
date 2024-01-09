using System.Collections.Generic;

public class Handler
{
	public List<GObject> DrawingObjects { get; private set; } = new();

	public List<string> Errors { get; private set; } = new();

	private string statements = "";

	public void Compile(string statements)
	{
		if (this.statements == statements)
			return;
		
		this.statements = statements;
		ILexer lexer = new Lexer(statements);
		if (lexer.Errors.Count > 0){
			Errors = lexer.Errors;
			return;
		}
		IParser parser = new Parser(lexer.Tokens);
		if (parser.Errors.Count > 0){
			Errors = parser.Errors;
			return;
		}
		GeoWallE_Program program = parser.Program;
		if (!program.Validate()){
			Errors = program.Errors;
			return;
		}
		program.Evaluate();
		DrawingObjects = program.DrawingObjects;
	}
}
