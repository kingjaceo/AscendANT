class_name StateMachine
extends Node

@export var initial_state: State
var _current_state: State
var current_state_name: String

var _states: Dictionary = {}

signal transitioned(state_name)


func _ready() -> void:
	#await owner.ready
	
	for child in get_children():
		if child is State:
			#await child.ready
			child.state_machine = self
			_states[child.name] = child
			#child.transitioned.connect(change_state)
		
	if initial_state:
		_current_state = initial_state
		_current_state.enter()


func change_state(new_state_name: String, data: Dictionary = {}) -> void:	
	if not _states[new_state_name]:
		push_warning("Cannot transition to state that does not exist: ", new_state_name)
		return
		
	if _current_state:
		_current_state.exit()
		
	current_state_name = new_state_name
	_current_state = _states[current_state_name]
	_current_state.enter(data)
	
	transitioned.emit(new_state_name)
