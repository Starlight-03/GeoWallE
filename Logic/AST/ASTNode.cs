using System.Collections.Generic;

public abstract class ASTNode
{
    public List<string> Errors { get; protected set; }

    private readonly int line;

    public ASTNode(int line)
    {
        this.line = line;
        Errors = new List<string>();
    }

    protected void AddError(string info, ASTNode child = null)
    {
        if (child is not null)
            foreach (string error in child.Errors)
                Errors.Add(error);

        Errors.Add($"Syntax Error: {info}. At line: {line}.");
    }

    protected bool IsValid() => Errors.Count == 0;

    public abstract bool Validate(IContext context);

    public abstract void Evaluate(IContext context = null);
}