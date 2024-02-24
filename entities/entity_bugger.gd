class_name EntityDebugger
extends Node2D

var entity: Entity
@onready var state: Label = $State
@onready var info: Label = $DebugInfo


func _ready():
	entity = get_parent()


func _process(_delta):
	rotation = -entity.rotation
	state.text = entity.state_machine.current_state_name
	_update_timers()
	queue_redraw()


func _draw():
	for module in entity.state_modules:
		module.debug_draw()


func _update_timers():
	info.text = ""
	for module in entity.state_modules:
		info.text += module.get_debug_text()
		info.text += "\n"
