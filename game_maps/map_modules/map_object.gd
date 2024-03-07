class_name MapObject
extends Node2D

@export var object_name: String
@onready var modules = %Modules
var current_map: GameMap
var ID: int
static var ID_counter: int = 0

signal removed


func _ready() -> void:
	ID_counter += 1
	ID = ID_counter
	
	modules = modules.get_children()
	
	_connect_map_modules(current_map.map_modules)
	_connect_module_signals()
	_setup()


func _setup():
	pass


func _connect_map_modules(map_modules: Array[Node]) -> void:
	for module in modules:
		module.clear_connections()
		module.connect_map_modules(map_modules)


func _connect_module_signals():
	for module in modules:
		for death_signal in module.death_signals:
			var reason = module.death_signals[death_signal]
			death_signal.connect(_remove.bind(reason))


func _remove(cause: String) -> void:
	print(self, " removed: ", cause, "!")
	removed.emit()
	queue_free()


func _to_string() -> String:
	return object_name + str(ID)

