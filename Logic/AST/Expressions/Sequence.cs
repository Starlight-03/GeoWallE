using System.Collections.Generic;

public class Sequence : Expression
{
    public readonly List<Expression> Values;

    public ExpType ValType { get; private set; }

    public Sequence(int line, List<Expression> values, ExpType valType = ExpType.Undefined) : base(line)
    {
        Values = values;
        this.ValType = valType;
        Type = ExpType.Sequence;
    }

    public Sequence(int line, int start, int end = int.MaxValue) : base(line)
    {
        Type = ExpType.Sequence;
        ValType = ExpType.Number;
        Values = new List<Expression>();
        for (int i = start; i <= end; i++)
            Values.Add(new Number(i));
    }

    public int Count => Values.Count;
    
    public Expression this[int index] { get => (index >= 0 || index < Count) ? Values[index] : null; }

    public override bool Validate(IContext context)
    {
        foreach (var item in Values){
            if (!item.Validate(context))
                return false;
            if (ValType == ExpType.Undefined)
                ValType = item.Type;
            else if (item.Type != ValType)
                return false;
        }
        
        return true;
    }

    public override void Evaluate()
    {
        foreach (var item in Values)
            item.Evaluate();
        Seq = this;
    }

    public void Concat(Sequence other)
    {
        foreach (Expression item in other.Values)
            Values.Add(item);
    }
}