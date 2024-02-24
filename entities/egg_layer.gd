class_name EggLayer
extends ANTiStateModule

@export var egg_lay_time: float = 2
var _egg_lay_timer: Timer

var _egg_module: MapModule

signal egg_laid

func _ready():
	priority = 1
	behavior = _lay_egg
	
	_egg_lay_timer = Timer.new()
	add_child(_egg_lay_timer)
	
	choice_making_signals = [egg_laid]


func clear_connections():
	_egg_module = null


func connect_map_modules(map_modules: Array[MapModule]) -> void:
	for map_module in map_modules:
		if map_module.name == "Eggs":
			_egg_module = map_module


func get_debug_text() -> String:
	var time_left = str(snappedf(_egg_lay_timer.time_left, 0.01))
	return "EggLayer: " + time_left + " s left"


func _lay_egg():
	_egg_lay_timer.start(egg_lay_time)
	priority = 0
	behavior = _nothing
	await _egg_lay_timer.timeout
	priority = 1
	behavior = _lay_egg
	egg_laid.emit()
