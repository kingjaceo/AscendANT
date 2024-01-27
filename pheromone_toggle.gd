extends CheckButton

var _messenger

func _on_toggled(button_pressed):
	_messenger.toggle_pheromones()
