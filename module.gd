class_name Module
extends Node

var map_object: MapObject
var choice_making_signals: Array[Signal] = []
var death_signals: Dictionary = {} # {signal death_signal: String reason}


func _ready():
	map_object = owner as MapObject
	_setup()


func _setup() -> void:
	pass


func clear_connections() -> void:
	pass


func connect_map_modules(_map_modules: Array[Node]) -> void:
	pass


func get_debug_text() -> String:
	return ""


func get_debug_draw() -> Dictionary:
	return {"position": Vector2.ZERO, "color": Color.WHITE, "size": 0}


func _setup_timer(timer: Timer, init_time: float, start_now: bool) -> Timer:
	timer = Timer.new()
	timer.wait_time = init_time
	add_child(timer)
	if start_now:
		timer.start()
	return timer

func _get_timer_time(timer: Timer) -> String:
	return str(snappedf(timer.time_left, 0.01)) + " s"

