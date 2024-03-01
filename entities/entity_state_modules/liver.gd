class_name Liver
extends ANTiStateModule

@export var lifetime_seconds: float = 120

var lifetime: Timer


func _setup():
	lifetime = _setup_timer(lifetime, lifetime_seconds, true)
	
	death_signals = {lifetime.timeout: "old age",}


func get_debug_text() -> String:
	var text = ""
	text += "Life Left: " + _get_timer_time(lifetime) + "\n"
	return text
