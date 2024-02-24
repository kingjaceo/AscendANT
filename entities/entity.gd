class_name Entity
extends Node2D

@onready var state_machine = %ANTiStateMachine
@onready var state_modules = %ANTiStateModules
@onready var modules = %Modules

var home_map: GameMap
@export var current_map: GameMap
var current_cell: Vector2i

var ID: int
var entity_name: String


func _ready() -> void:
	state_modules = state_modules.get_children()
	modules = modules.get_children()
	
	home_map = current_map
	
	for state_module in state_modules:
		for choice_making_signal in state_module.choice_making_signals:
			choice_making_signal.connect(_make_a_choice)

		for death_signal in state_module.death_signals:
			death_signal.connect(_die)
			
		state_module.connect_modules(modules)
			
	_make_a_choice()


func connect_map_modules(map_modules: Array[MapModule]):
	for state_module in state_modules:
		state_module.clear_connections()
		state_module.connect_map_modules(map_modules)


func _make_a_choice() -> void:
	# the entity should assess its choices:
	var max_priority = 0
	var max_priority_module = state_modules[0]
	for state_module in state_modules:
		if state_module.priority > max_priority:
			max_priority = state_module.priority
			max_priority_module = state_module
			
	var behavior = max_priority_module.behavior
	state_machine.change_behavior(behavior)


func _die(cause: String) -> void:
	print(entity_name, ID, " died: ", cause, "!")
	queue_free()
