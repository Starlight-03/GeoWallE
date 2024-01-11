// using Godot;

// public partial class Menu : Control
// {
// 	private string currentFile = "Untitled";

// 	// private readonly Handler handler = new Handler();

// 	public override void _Ready()
// 	{
// 		base._Ready();
		
// 		var nameLabel = GetNode<Label>("Background/Rows/Program/Columns/Name");
// 		nameLabel.Text = "GeoWallE - Untitled";

// 		var canvas = GetNode<Control>("Background/Rows/Columns/Right/CanvasPanel/Canvas");
// 		canvas.AddChild(new Circle(new Point(0, 0), 5));
// 		EmitSignal("draw");
// 	}

// 	public void OnExitPressed() => GetTree().Quit();

// 	public void OnSaveAsPressed() => PopUpSaveFileDialog();

// 	public void OnSavePressed() => SaveFile();

// 	public void OnSaveFileDialogFileSelected(string path)
// 	{
// 		currentFile = path;
// 		var nameLabel = GetNode<Label>("Background/Rows/Program/Columns/Name");
// 		nameLabel.Text = "GeoWallE - " + currentFile;
// 		SaveAsFile();
// 	}

// 	public void OnCompilePressed()
// 	{
// 		var errorPanel = GetNode<TextEdit>("Background/Rows/Columns/Left/CodeEditor/Errors/ErrorPanel");
// 		errorPanel.Clear();
		
// 		var canvas = GetNode<Control>("Background/Rows/Columns/Right/CanvasPanel/Canvas");
// 		var children = canvas.GetChildren();
// 		foreach (var child in children)
// 			canvas.RemoveChild(child);
// 		canvas.QueueRedraw();
		
// 		Compile();
// 	}

// 	private void Compile()
// 	{
// 		if (currentFile is "Untitled") PopUpSaveFileDialog();
// 		var textPanel = GetNode<TextEdit>("Background/Rows/Columns/Left/CodeEditor/CodePanel");
// 		handler.Compile(textPanel.Text);
		
// 		var errorPanel = GetNode<TextEdit>("Background/Rows/Columns/Left/CodeEditor/Errors/ErrorPanel");
// 		var canvas = GetNode<Control>("Background/Rows/Columns/Right/CanvasPanel/Canvas");

// 		if (handler.Errors.Count > 0)
// 			foreach (string error in handler.Errors)
// 				errorPanel.Text += error + '\n';
// 		else{
// 			foreach (GObject obj in handler.DrawingObjects)
// 				canvas.AddChild(obj);
// 			EmitSignal("draw");
// 		}
// 	}

// 	private void PopUpSaveFileDialog() => GetNode<FileDialog>("SaveFileDialog").PopupCentered();

// 	private void SaveAsFile()
// 	{
// 		var file = FileAccess.Open(currentFile, FileAccess.ModeFlags.Write);
// 		var textPanel = GetNode<TextEdit>("Background/Rows/Columns/Left/CodeEditor/CodePanel");
// 		file.StoreString(textPanel.Text);
// 		file.Close();
// 	}

// 	private void SaveFile()
// 	{
// 		if (currentFile == "Untitled") PopUpSaveFileDialog();
// 		else SaveAsFile();
// 	}
// }
