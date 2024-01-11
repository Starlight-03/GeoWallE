using Godot;
using System.Collections.Generic;

public partial class Handler
{
	public List<GObject> DrawingObjects { get; private set; } = new();

	public List<string> Errors { get; private set; } = new();

	public GeoWallE_Program Program { get; private set; } = new(new List<Statement>());

	private string statements = "";

	private void OnMenuCompile(string statements)
	{
		Compile(statements);
	}

	public void Compile(string statements)
	{
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
		Program = parser.Program;
		if (!Program.Validate()){
			Errors = Program.Errors;
			return;
		}
		Program.Evaluate();
		DrawingObjects = Program.DrawingObjects;
	}
}
