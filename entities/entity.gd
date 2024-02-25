class_name Entity
extends Node2D

@export var entity_name: String
var ID

@onready var state_machine = %ANTiStateMachine
@onready var state_modules = %ANTiStateModules
@onready var entity_modules = %Modules

var home_map: GameMap
@export var current_map: GameMap
var current_cell: Vector2i

static var ID_counter: int = 0

signal died


func _ready() -> void:
	ID_counter += 1
	ID = ID_counter
	
	state_modules = state_modules.get_children()
	entity_modules = entity_modules.get_children()
	
	home_map = current_map
	
	_connect_module_signals()
	#state_module.connect_modules(modules)
	_make_a_choice()


func connect_map_modules(map_modules: Array[Node]) -> void:
	for state_module in state_modules:
		state_module.clear_connections()
		state_module.connect_map_modules(map_modules)
		
	for entity_module in entity_modules:
		entity_module.clear_connections()
		entity_module.connect_map_modules(map_modules)


func _connect_module_signals():
	for module in state_modules:
		for choice_making_signal in module.choice_making_signals:
			choice_making_signal.connect(_make_a_choice)

		for death_signal in module.death_signals:
			var reason = module.death_signals[death_signal]
			death_signal.connect(_die.bind(reason))
	
	for module in entity_modules:
		for choice_making_signal in module.choice_making_signals:
			choice_making_signal.connect(_make_a_choice)
		
		for death_signal in module.death_signals:
			var reason = module.death_signals[death_signal]
			death_signal.connect(_die.bind(reason))


func _make_a_choice() -> void:
	# the entity should assess its choices:
	var max_priority = 0
	var max_priority_module = state_modules[0]
	for state_module in state_modules:
		state_module.update_priority()
		if state_module.priority > max_priority:
			max_priority = state_module.priority
			max_priority_module = state_module
			
	var behavior = max_priority_module.behavior
	var exit_behavior = max_priority_module.exit_behavior
	state_machine.change_behavior(behavior, exit_behavior)


func _die(cause: String) -> void:
	print(entity_name, ID, " died: ", cause, "!")
	died.emit()
	queue_free()
