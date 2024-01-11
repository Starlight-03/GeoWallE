using System;
using System.Collections.Generic;

public class MatchDecl : Statement
{
    private readonly List<string> variables;

    private readonly Expression arg;

    private IContext context;

    public MatchDecl(int line, List<string> variables, Expression arg) : base(line)
    {
        this.variables = variables;
        this.arg = arg;
    }

    public override bool Validate(IContext context)
    {
        this.context = context;

        if (!arg.Validate(context))
            AddError("Invalid expression at match declaration", arg);
        if (arg.Type is not ExpType.Sequence)
            AddError("Expression must be a sequence in a match declaration");

        foreach (string variable in variables){
            if (variable is "_") 
                continue;
            else if (context.VariableIsDefined(variable))
                AddError($"Variable {variable} is already defined");
            else
                context.DefineVariable(variable, arg.Type);
        }

        return IsValid();
    }

    public override void Evaluate(IContext context)
    {
        arg.Evaluate(context);
        if (arg is Sequence seq){
            int i = 0;
            for (; i < MathF.Min(variables.Count, seq.Count); i++){
                if (variables[i] is "_") continue;
                else context.SetVariableValue(variables[i], seq[i]);
            }
            while (i < variables.Count){
                if (variables[i] is "_") continue;
                else context.SetVariableValue(variables[i++], new Undefined());
            }
        }
        else if (arg is Undefined){
            foreach (string variable in variables){
                if (variable is "_") continue;
                else context.SetVariableValue(variable, new Undefined());
            }
        }
    }
}