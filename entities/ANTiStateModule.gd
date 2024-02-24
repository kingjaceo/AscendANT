class_name ANTiStateModule
extends Node

var entity: Entity
var priority: float = 0
var behavior: Callable = _nothing
var choice_making_signals: Array[Signal]
var death_signals: Array[Signal]


func _ready():
	entity = owner as Entity


func clear_connections() -> void:
	pass


func connect_modules(modules: Array[Node]):
	pass


func connect_map_modules(map_modules: Array[MapModule]) -> void:
	pass


func get_debug_text() -> String:
	return "ANTiStateModule: NONE"


func debug_draw() -> void:
	return


func _nothing() -> void:
	# represents no behavior, so that states can "go dormant"
	pass
