extends CheckButton

var _messenger: Node2D

func _on_toggled(button_pressed):
	_messenger.toggle_outline()
