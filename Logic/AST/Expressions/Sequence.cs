using System.Collections.Generic;

public class Sequence : Expression
{
    public readonly List<Expression> Values;

    public ExpType ValType { get; private set; }

    public Sequence(int line, List<Expression> values, ExpType valType = ExpType.Undefined) : base(line)
    {
        Values = values;
        ValType = valType;
        Type = ExpType.Sequence;
    }

    public Sequence(int line, int start, int end = int.MaxValue) : base(line)
    {
        Type = ExpType.Sequence;
        ValType = ExpType.Number;
        Values = new List<Expression>();
        for (int i = start; i <= end; i++)
            Values.Add(new Number(i.ToString()));
    }

    public int Count => Values.Count;
    
    public Expression this[int index] { get => (index >= 0 || index < Count) ? Values[index] : null; }

    public override bool Validate(IContext context)
    {
        foreach (var item in Values){
            if (!item.Validate(context))
                AddError($"Invalid argument in {ValType} sequence", item);
            if (ValType is ExpType.Undefined)
                ValType = item.Type;
            else if (item.Type != ValType)
                AddError($"{ValType} sequence doesn't accept {item.Type} expressions");
        }
        
        return IsValid();
    }

    public override void Evaluate(IContext context)
    {
        foreach (var item in Values)
            item.Evaluate(context);
        Seq = this;
    }

    public void Concat(Sequence other)
    {
        foreach (Expression item in other.Values)
            Values.Add(item);
    }
}