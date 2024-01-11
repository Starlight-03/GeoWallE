extends Control

var current_file = "Untitled"
var handler


func _ready() -> void:
	var nameLabel = $"Background/Rows/Program/Columns/Name"
	nameLabel.text = "GeoWallE - Untitled"
	var _handler = load("res://Logic/Handler.cs")
	handler = handler.instantiate()


func _on_exit_pressed() -> void:
	get_tree().quit()


func _on_save_as_pressed() -> void:
	popup_save_file_dialog()


func _on_save_pressed() -> void:
	save_file()

func save_file() -> void:
	if current_file == "Untitled":
		popup_save_file_dialog()
	else:
		save_as_file()


func _on_save_file_dialog_file_selected(path: String) -> void:
	current_file = path
	var nameLabel = $"Background/Rows/Program/Columns/Name"
	nameLabel.text = "GeoWallE - " + current_file
	save_as_file()


func _on_compile_pressed() -> void:
	var error_panel = $"Background/Rows/Columns/Left/CodeEditor/Errors/ErrorPanel"
	error_panel.clear()
	var canvas = $"Background/Rows/Columns/Right/CanvasPanel/Canvas"
	for child in canvas.get_children():
		canvas.remove_child(child)
	canvas.queue_redraw()
	if current_file == "Untitled":
		popup_save_file_dialog()
	_compile()


func save_as_file() -> void:
	var file = FileAccess.open(current_file, FileAccess.WRITE)
	var text_panel = $"Background/Rows/Columns/Left/CodeEditor/CodePanel"
	file.store_string(text_panel.text)
	file.close()


func popup_save_file_dialog() -> void:
	$"SaveFileDialog".popup_centered()


func _compile() -> void:
	var text_panel = $"Background/Rows/Columns/Left/CodeEditor/CodePanel"
	handler.Compile(text_panel.text)
	var error_panel = $"Background/Rows/Columns/Left/CodeEditor/Errors/ErrorPanel"
	var canvas = $"Background/Rows/Columns/Right/CanvasPanel/Canvas"
	var errors = handler.Errors
	var drawing_objects = handler.DrawingObjects
	if handler.Errors.Count > 0:
		for error in handler.Errors:
			error_panel.text += error + '\n'
	else:
		for obj in handler.DrawingObjects:
			canvas.add_child(obj)
		draw.emit()
