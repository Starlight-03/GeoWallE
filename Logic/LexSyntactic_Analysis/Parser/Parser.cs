using Godot;
using System.Collections.Generic;

public class Parser : IParser
{
    public List<string> Errors { get; private set; }

    public GeoWallE_Program Program { get; private set; }

    private readonly TokenReader reader;

    public Parser(List<Token> tokens)
    {
        Errors = new List<string>();
        reader = new TokenReader(tokens);
        Program = ParseProgram();
    }

    private void AddError(int line, string info, bool next = false)
    {
        Errors.Add($"Parsing Error: {info}. At line: {line}.");
        if (next) 
            reader.MoveForward();
    }

    private GeoWallE_Program ParseProgram()
    {
        var statements = new List<Statement>();

        while (reader.Match("import")){
            if (!reader.Match(TokenType.Text))
                AddError(reader.Line, "Expected name of \'.geo\' file as \'text\' to import", reader.NextIs(";"));
            else
                statements.Add(new Import(reader.LookBack().Value, reader.Line));

            if (!reader.Match(";"))
                AddError(reader.Line, "Missing \';\' at end of statement");
        }

        while (!reader.EOF){
            Statement statement = ParseStatement();
            if (statement is not null)
                statements.Add(statement);
        }

        return new GeoWallE_Program(statements);
    }

    private Statement ParseStatement()
    {
        if (IsStatementBeginning()){
            if (reader.Match("point") || reader.Match("line") || reader.Match("segment") || reader.Match("ray") 
                || reader.Match("circle") || reader.Match("arc") || reader.Match("measure") || reader.Match("number")) 
                return ParseStatement_(ParseVarDecl());
            else if (reader.Match("color")) return ParseStatement_(ParseColorChange());
            else if (reader.Match("restore")) return ParseStatement_(new Restore(reader.Line));
            else if (reader.Match("draw")){
                Expression expression = ParseExpression();
                var nameTag = new Text(reader.Match(TokenType.Text) ? reader.LookBack().Value : "");
                return ParseStatement_((expression is not null) ? new Draw(expression, nameTag, reader.Line) : null);
            }
            else if (reader.Match("_"))
                return ParseStatement_(reader.Match(",") ? ParseMatchDecl() : ParseVarDef());
            else if (reader.Match(TokenType.Identifier))
                return ParseStatement_(reader.Match("(") ? ParseFuncDef() : reader.Match(",") ? ParseMatchDecl() : ParseVarDef());
        }

        int l = reader.Line;
        while (!reader.EOF && !reader.EOS && !IsStatementBeginning()){
            if (l < reader.Line) l = reader.Line;
            reader.MoveForward();
        }
        if (reader.Match(";"))
            AddError(l, "Only assign, calling, or new object are valid statements");
        else if (IsStatementBeginning())
            AddError(l, "Missing \';\' at end of statement");

        return null;

        bool IsStatementBeginning()
        => reader.CurrentIs("point") || reader.CurrentIs("line") || reader.CurrentIs("ray") || reader.CurrentIs("segment") || 
            reader.CurrentIs("circle") || reader.CurrentIs("arc") || reader.CurrentIs("measure") || reader.CurrentIs("number") || 
            reader.CurrentIs("color") || reader.CurrentIs("restore") || reader.CurrentIs("draw") || reader.CurrentIs("_") || 
            reader.Current().Type == TokenType.Identifier;

        Statement ParseStatement_(Statement statement){
            if (!reader.Match(";"))
                AddError(reader.Line, "Missing \';\' at end of statement");
            return statement;
        }
    }

    private VarDecl ParseVarDecl()
    {
        // <varType> (sequence) <identifier>;
        string varType = reader.LookBack().Value;

        bool TypeIs(string type) => reader.LookBack().Value == type;

        ExpType type =  TypeIs("point")     ? ExpType.Point 
                        : TypeIs("line")    ? ExpType.Line 
                        : TypeIs("segment") ? ExpType.Segment 
                        : TypeIs("ray")     ? ExpType.Ray 
                        : TypeIs("circle")  ? ExpType.Circle 
                        : TypeIs("arc")     ? ExpType.Arc 
                        : TypeIs("measure") ? ExpType.Measure 
                        : TypeIs("number")  ? ExpType.Number : ExpType.Undefined;
        
        VarDecl decl;

        if (reader.Match("sequence"))
            decl = new SequenceDecl(type, reader.Current().Value, reader.Line);
        else
            decl = new VarDecl(type, reader.Current().Value, reader.Line);

        if (!reader.Match(TokenType.Identifier))
            AddError(reader.Line, $"Expected \'identifier\' after \'{varType} variable\' declaration", reader.NextIs(";"));

        return decl;
    }

    private ColorChange ParseColorChange()
    {
        // color <colorType>
        Color color =   reader.Match("black")       ? Colors.Black
                        : reader.Match("blue")      ? Colors.Blue
                        : reader.Match("red")       ? Colors.Red
                        : reader.Match("yellow")    ? Colors.Yellow
                        : reader.Match("green")     ? Colors.Green
                        : reader.Match("cyan")      ? Colors.Cyan
                        : reader.Match("magenta")   ? Colors.Magenta
                        : reader.Match("white")     ? Colors.White
                        : reader.Match("gray")      ? Colors.Gray : Colors.Violet;

        if (color == Colors.Violet)
            AddError(reader.Line, "Expected \'color type\' after \'color change\' declaration", reader.NextIs(";"));

        return new ColorChange(color, reader.Line);
    }

    private VarDef ParseVarDef()
    {
        // <identifier> = <expression>;
        string id = reader.LookBack().Value;
        if (!reader.Match("="))
            AddError(reader.Line, $"Missing \'=\' after variable \'{id}\'");

        Expression arg = ParseExpression();
        if (arg is null)
            AddError(reader.Line, $"Invalid expression after variable \'{id}\'");

        return (arg is not null) ? new VarDef(id, arg, reader.Line) : null;
    }

    private FuncDef ParseFuncDef()
    {
        // <identifier>(<args>) = <expression>;
        string id = reader.LookBack(2).Value;
        var args = new List<string>();

        while (!reader.EOS && !reader.Match(")") && !reader.CurrentIs("=")) {
            if (!reader.Match(TokenType.Identifier))
                AddError(reader.Line, $"Expected \'identifier\' at \'{id}\' function definition argument list", reader.NextIs(",") || reader.NextIs(")"));
            else
                args.Add(reader.LookBack().Value);

            if (!reader.Match(",") && !reader.CurrentIs(")"))
                AddError(reader.Line, $"Missing \',\' at \'{id}\' function definition argument list");
        }
        if (reader.LookBack().Value is not ")")
            AddError(reader.Line, $"Missing \')\' at the end of the \'{id}\' function definition argument list");

        if (!reader.Match("="))
            AddError(reader.Line, $"Missing \'=\' at \'{id}\' function definition");

        Expression body = ParseExpression();
        if (body is null)
            AddError(reader.Line, $"Invalid expression at \'{id}\' function definition body");

        return (body is not null) ? new FuncDef(id, args, body, reader.Line) : null;
    }

    private MatchDecl ParseMatchDecl()
    {
        reader.MoveBack(2);
        var IDs = new List<string>();

        do {
            if (!reader.Match(TokenType.Identifier) && !reader.Match("_"))
                AddError(reader.Line, "Expected \'identifier\' or \'_\' at \'match declaration\'", reader.NextIs(","));
            else
                IDs.Add(reader.LookBack().Value);
        } while (reader.Match(","));

        if (!reader.Match("="))
            AddError(reader.Line, "Missing \'=\' after \'match declaration\'");

        Expression expression = ParseExpression();
        if (expression is null)
            AddError(reader.Line, "Invalid expression at \'match declaration\'");

        return (expression is not null) ? new MatchDecl(IDs, expression, reader.Line) : null;
    }

    private Expression ParseExpression()
    {
        if (reader.Match("not")){
            Expression exp = ParseExpression_();
            if (exp is null) AddError(reader.Line, "Invalid expression after \'not\' operator");
            return (exp is not null) ? ParseLogicOp(new LogicNot(exp, reader.Line)) : null;
        }
        return ParseLogicOp(ParseExpression_());

        Expression ParseExpression_(){
            if (reader.Match("point") || reader.Match("line") || reader.Match("ray") || reader.Match("segment") 
                || reader.Match("measure") || reader.Match("circle") || reader.Match("arc") || reader.Match("intersect")) 
                                                                return ParseObjectDef(reader.LookBack().Value);
            else if (reader.Match("if"))                        return ParseIfThenElse();
            else if (reader.Match("let"))                       return ParseLetIn();
            else if (reader.Match("{"))                         return ParseSequence();
            else if (reader.Match("undefined")){
                if (reader.Match("+")){
                    Expression right = reader.Match("{") ? ParseSequence() : reader.Match("undefined") ? new Undefined() : null;
                    if (right is null) AddError(reader.Line, "Invalid expression after \'+\' operator");
                    return (right is not null) ? new Concat(new Undefined(), right, reader.Line) : null;
                }
                return new Undefined();
            }
            else if (reader.Match(TokenType.Text))              return new Text(reader.LookBack().Value);
            else if (reader.Current().Type == TokenType.Number) return ParseCompOp(ParseNumericalExpression());
            else if (reader.Match(TokenType.Identifier))        return ParseCompOp(X(Y(reader.Match("(") ? ParseFuncCall() : new VarCall(reader.LookBack().Value, reader.Line))));
            else if (reader.Match("(")){
                Expression exp = ParseExpression();
                if (exp is null) AddError(reader.Line, "Invalid expression after \'(\'");
                if (!reader.Match(")")) AddError(reader.Line, $"Missing \')\' after \'{reader.LookBack().Value}\'");
                return ParseCompOp(X(Y(exp)));
            }
            return null;
        }
    }

    private Expression ParseObjectDef(string obj)
    {
        // <object>(<arg1>, <arg2> (, <arg3>, <arg4>) )
        if (!reader.Match("(")) AddError(reader.Line, $"Missing \'(\' after {obj} definition");

        var args = new List<Expression>();
        for (int i = 1; !reader.EOS && !reader.CurrentIs(")"); i++){
            Expression expr = ParseExpression();
            if (expr is null) AddError(reader.Line, $"Invalid expression as arg No.{i} at {obj} definition");

            args.Add((expr is not null) ? expr : new Number("0"));

            if (!reader.Match(",") && !reader.CurrentIs(")")) AddError(reader.Line, $"Missing \',\' after arg No.{i} at {obj} definition", reader.NextIs(";"));
            else if (reader.CurrentIs(")") && reader.LookBack().Value == ",") AddError(reader.Line, $"Invalid expression as erg No.{i + 1} at {obj} definition");
        }
        if (!reader.Match(")")) AddError(reader.Line, $"Missing \')\' at {obj} definition");

        if ((obj is "point" || obj is "line" || obj == "ray" || obj is "segment" || obj is "measure" || obj is "circle" || obj is "intersect") && args.Count != 2)
            AddError(reader.Line, $"Object {obj} must have 2 args");
        else if (obj is "arc" && args.Count != 4)
            AddError(reader.Line, $"Object arc must have 4 args");

        return (args.Count == 2) ? 
                    ((obj is "point") ? new PointDef(args[0], args[1], reader.Line) : 
                    (obj is "line") ? new LineDef(args[0], args[1], reader.Line) : 
                    (obj is "ray") ? new RayDef(args[0], args[1], reader.Line) : 
                    (obj is "segment") ? new SegmentDef(args[0], args[1], reader.Line) : 
                    (obj is "measure") ? new MeasureDef(args[0], args[1], reader.Line) : 
                    (obj is "circle") ? new CircleDef(args[0], args[1], reader.Line) : 
                    (obj is "intersect") ? new IntersectionDef(args[0], args[1], reader.Line) : null) :
                (args.Count == 4 && obj is "arc") ? 
                    new ArcDef(args[0], args[1], args[2], args[3], reader.Line) : null;
    }

    private IfThenElse ParseIfThenElse()
    {
        Expression condition = ParseExpression();
        if (condition is null)
            AddError(reader.Line, "Invalid condition expression after \'if\' keyword at \'if-then-else\' expression");

        if (!reader.Match("then"))
            AddError(reader.Line, "Missing \'then\' keyword after condition expression at \'if-then-else\' expression");

        Expression positive = ParseExpression();
        if (positive is null)
            AddError(reader.Line, "Invalid positive expression after \'then\' keyword at \'if-then-else\' expression");

        if (!reader.Match("else"))
            AddError(reader.Line, "Missing \'else\' keyword after positive expression at \'if-then-else\' expression");

        Expression negative = ParseExpression();
        if (negative is null)
            AddError(reader.Line, "Invalid negative expression after \'else\' keyword at \'if-then-else\' expression");

        return (condition is not null && positive is not null && negative is not null) ? 
                new IfThenElse(condition, positive, negative, reader.Line) : null;
    }

    private LetIn ParseLetIn()
    {
        var statements = new List<Statement>();
        while (reader.Current().Type == TokenType.Identifier){
            Statement statement = ParseStatement();
            if (statement is null)
                AddError(reader.Line, "Invalid \'let\' statement in \'let-in\' expression");
            else
                statements.Add(statement);
        }

        if (!reader.Match("in"))
            AddError(reader.Line, "Missing \'in\' keyword in \'let-in\' expression");

        Expression body = ParseExpression();
        if (body is null) 
            AddError(reader.Line, "Invalid expression after \'in\' in \'let-in\' expression");

        return (body is null) ? null : new LetIn(statements, body, reader.Line);
    }

    private Expression ParseSequence()
    {
        // { <e1>, <e2>, ... , <en> } (+ <sequency> | undefined)
        var values = new List<Expression>();
        while (!reader.EOS && !reader.CurrentIs("}")) {
            var expression = ParseExpression();

            if (expression is null) AddError(reader.Line, "Invalid expression at sequence definition");
            else values.Add(expression);
            
            if (!reader.Match(",") && !reader.CurrentIs("}")) AddError(reader.Line, "Expected \',\' at sequence definition");
            else if (reader.CurrentIs("}") && reader.LookBack().Value == ",") AddError(reader.Line, "Invalid expression at sequence definition");
        }
        if (!reader.Match("}"))
            AddError(reader.Line, "Missing \'}\' at sequence definition");

        if (reader.Match("+")){
            Expression expr = SequenceSum(new Sequence(values, reader.Line));
            if (expr is null) AddError(reader.Line, "Expected sequence expression");
            return (expr is not null) ? expr : null;
        }
        return new Sequence(values, reader.Line);

        Concat SequenceSum(Expression left){
            if (reader.Match("{")){
                Expression right = ParseSequence();
                if (right is null) AddError(reader.Line, "Invalid sequence expression after \'+\' operator");
                return (right is not null) ? new Concat(left, right, reader.Line) : null;
            }
            if (reader.Match("undefined")){
                if (reader.Match("+")){
                    Expression right = SequenceSum(new Undefined());
                    if (right is null) AddError(reader.Line, "Invalid sequence expression after \'+\' operator");
                    return (right is not null) ? new Concat(left, right, reader.Line) : null;
                }
                return new Concat(left, new Undefined(), reader.Line);
            }
            return null;
        }
    }

    private FuncCall ParseFuncCall()
    {
        string id = reader.LookBack(2).Value;
        var args = new List<Expression>();

        while (!reader.EOS && !reader.CurrentIs(")")){
            Expression arg = ParseExpression();
            if (arg is null)
                AddError(reader.Line, $"Invalid argument expression at \'{id}\' function call");
            else 
                args.Add(arg);

            if (!reader.Match(",") && !reader.CurrentIs(")"))
                AddError(reader.Line, $"Expected \',\' at \'{id}\' function call");
        }

        if (!reader.Match(")"))
            AddError(reader.Line, $"Missing \')\' at \'{id}\' function call");

        return new FuncCall(id, args, reader.Line);
    }

    public Expression ParseNumericalExpression()
    => X(Term());

    public Expression Term()
    {
        if (reader.Match(TokenType.Number))
            return Y(new Number(reader.LookBack().Value));

        else if (reader.Match("measure"))
            return Y(ParseObjectDef("measure"));

        else if (reader.Match(TokenType.Identifier))
            return Y(reader.Match("(") ? ParseFuncCall() : new VarCall(reader.Current().Value, reader.Line));

        else if (reader.Match("(")){
            Expression term = ParseNumericalExpression();
            if (term is null)
                AddError(reader.Line, "Invalid expression after \'(\' at numerical expression");
            if (!reader.Match(")")){
                AddError(reader.Line, $"Missing \')\' after \'{reader.LookBack().Value}\'");
                reader.MoveForward();
            }
            return Y(term);
        }

        return Y(ParseExpression());
    }

    public Expression X(Expression left)
    {
        if (left is null) return null;

        if (!reader.Match("+") && !reader.Match("-"))
            return left;

        string op = reader.LookBack().Value;
        Expression right;

        if (reader.Match("(")){
            right = Y(ParseNumericalExpression());
            if (!reader.Match(")"))
                AddError(reader.Line, $"Missing \')\' after \'{reader.LookBack().Value}\'");
        }
        else
            right = Y(reader.Match(TokenType.Number) ? new Number(reader.LookBack().Value) : 
                        reader.Match(TokenType.Identifier) ? reader.Match("(") ? ParseFuncCall() : 
                            new VarCall(reader.LookBack().Value, reader.Line) : 
                        null);

        if (right is null){
            AddError(reader.Line, $"Invalid expression after \'{op}\' operator");
            return null;
        }

        return X(op is "+" ? new Sum(left, right, reader.Line) : new Sub(left, right, reader.Line));
    }

    public Expression Y(Expression left)
    {
        if (left is null) return null;

        if (!reader.Match("*") && !reader.Match("/") && !reader.Match("%") && !reader.Match("^"))
            return left;

        string op = reader.LookBack().Value;
        Expression right;

        if (reader.Match("(")){
            right = ParseNumericalExpression();
            if (!reader.Match(")"))
                AddError(reader.Line, $"Missing \')\' after \'{reader.LookBack().Value}\'");
        }
        else
            right = reader.Match(TokenType.Number) ? new Number(reader.LookBack().Value) : 
                        reader.Match(TokenType.Identifier) ? reader.Match("(") ? ParseFuncCall() : 
                            new VarCall(reader.LookBack().Value, reader.Line) : 
                        null;

        if (right is null){
            AddError(reader.Line, $"Invalid expression after \'{op}\' operator");
            return null;
        }

        return Y(op is "*" ? new Mul(left, right, reader.Line) : 
                op is "/" ? new Div(left, right, reader.Line) : 
                op is "%" ? new Mod(left, right, reader.Line) : new Pow(left, right, reader.Line));
    }

    public Expression ParseCompOp(Expression left)
    {
        if (left is null) return null;

        if (!reader.Match("<") && !reader.Match("<=") && !reader.Match(">") 
            && !reader.Match(">=") && !reader.Match("==") && !reader.Match("!="))
                return left;

        string op = reader.LookBack().Value;
        Expression right = ParseNumericalExpression();

        if (right is null){
            AddError(reader.Line, $"Invalid expression after \'{op}\' operator");
            return null;
        }

        return op is "<" ? new Minor(left, right, reader.Line) :
                op is "<=" ? new MinorEqual(left, right, reader.Line) : 
                op is ">" ? new Major(left, right, reader.Line) : 
                op is ">=" ? new MajorEqual(left, right, reader.Line) : 
                op is "==" ? new Equals(left, right, reader.Line) : new NotEqual(left, right, reader.Line);
    }

    public Expression ParseLogicOp(Expression left)
    {
        if (left is null) return null;

        if (!reader.Match("and") && !reader.Match("or"))
            return left;
        
        string op = reader.LookBack().Value;
        Expression right = ParseExpression();

        if (right is null){
            AddError(reader.Line, $"Invalid expression after \'{op}\' operator");
            return null;
        }

        return op is "and" ? new And(left, right, reader.Line) : new Or(left, right, reader.Line);
    }
}