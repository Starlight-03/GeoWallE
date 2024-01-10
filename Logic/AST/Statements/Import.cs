using Godot;

public class Import : Statement
{
    private readonly string file;

    public Import(int line, string file) : base(line) => this.file = file;

    public override bool Validate(IContext context)
    {
        if (!file.EndsWith(".geo"))
            AddError("");
        else{
            Handler handler = new();
            var f = FileAccess.Open("res://GeoFiles//" + file, FileAccess.ModeFlags.Read);
            if (f is null)
                AddError($"There wasn't found a .geo file that matches {file} file name");
            else{
                handler.Compile(f.GetAsText());
                f.Close();

                foreach (string error in handler.Errors)
                    AddError(error);

                context.Merge(handler.Program.GlobalContext);
            }
        }
        return IsValid();
    }

    public override void Evaluate() { }
}