using System;

public class Import : Statement
{
    private readonly string file;

    public Import(string file, int line) : base(line) { this.file = file; }

    public override bool Validate(IContext context) => true;

    public override void Evaluate()
    {
        throw new NotImplementedException();
    }
}