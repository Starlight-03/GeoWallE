public abstract class Expression : ASTNode
{
    public ExpType Type { get; protected set; }

    public string Value { get; protected set; }

    public Sequence Seq { get; protected set; }

    public GObject Object { get; protected set; }

    public Expression(int line) : base(line) => Type = ExpType.NotSet;
}

public enum ExpType
{
    NotSet,
    Number,
    Text,
    Point,
    Line,
    Ray,
    Segment,
    Measure,
    Circle,
    Arc,
    Sequence,
    Undefined
}