class_name EggHatcher
extends ANTiStateModule

@export var egg_hatch_time: float = 2
@export var hatch_at_ready: bool = false
var _egg_hatch_timer: Timer
var _egg_module: MapModule
var _spawner: MapModule

var entity_model = load("res://entities/entity.tscn")

signal egg_hatched


func _ready():
	_egg_hatch_timer = Timer.new()
	_egg_hatch_timer.timeout.connect(_hatch_egg)
	add_child(_egg_hatch_timer)
	if hatch_at_ready:
		_egg_hatch_timer.start(0.2)
	_egg_hatch_timer.wait_time = egg_hatch_time
	#choice_making_signals = [egg_hatched]


func clear_connections():
	_egg_module = null


func connect_map_modules(map_modules: Array[Node]) -> void:
	for map_module in map_modules:
		if map_module.name == "Eggs":
			_egg_module = map_module
			egg_hatched.connect(_egg_module.hatch_egg)
		if map_module.name == "Spawner":
			_spawner = map_module
			egg_hatched.connect(_spawner.spawn)


func get_debug_text() -> String:
	var time_left = str(snappedf(_egg_hatch_timer.time_left, 0.01))
	return "EggHatcher: " + time_left + " s left"


func _hatch_egg():
	#if _egg_module and _egg_module.num_eggs > 0:
	var entity = entity_model.instantiate()
	egg_hatched.emit(entity)

