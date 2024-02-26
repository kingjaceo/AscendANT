class_name ANTiStateMachine
extends Node

@export var initial_state: State
var _current_state: State
var _current_exit_behavior: Callable = _none
var current_behavior_name: String

var _states: Dictionary = {}

signal transitioned(state_name)


func _ready() -> void:
	for child in get_children():
		if child is State:
			child.state_machine = self
			_states[child.name] = child
		
	if initial_state:
		_current_state = initial_state
		_current_state.enter()

func change_behavior(new_behavior: Callable, exit_behavior: Callable = _none):
	var new_behavior_name = new_behavior.get_method()
	if new_behavior_name != current_behavior_name:
		_current_exit_behavior.call()
	_current_exit_behavior = exit_behavior
	current_behavior_name = new_behavior_name
	new_behavior.call()


func _none():
	return


#func change_state(new_state_name: String, data: Dictionary = {}) -> void:	
	#if not _states[new_state_name]:
		#push_warning("Cannot transition to state that does not exist: ", new_state_name)
		#return
		#
	#if _current_state:
		#_current_state.exit()
		#
	#current_state_name = new_state_name
	#_current_state = _states[current_state_name]
	#_current_state.enter(data)
	#
	#transitioned.emit(new_state_name)
