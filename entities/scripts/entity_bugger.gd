class_name EntityDebugger
extends Node2D

var entity: Entity
@onready var state: Label = $State
@onready var info: Label = $DebugInfo


func _ready():
	entity = get_parent()


func _process(_delta):
	rotation = -entity.rotation
	state.text = entity.state_machine.current_behavior_name
	_update_text()
	queue_redraw()


func _draw():
	for module in entity.state_modules:
		var data = module.get_debug_draw()
		draw_circle(data["position"], data["size"], data["color"])
		
	for module in entity.entity_modules:
		var data = module.get_debug_draw()
		draw_circle(data["position"], data["size"], data["color"])
	

func _update_text():
	info.text = entity.entity_name + str(entity.ID) + "\n"
	info.text += str(entity.current_map.local_to_map(entity.position)) + "\n"
	for module in entity.state_modules:
		info.text += module.get_debug_text()
		
	for module in entity.entity_modules:
		info.text += module.get_debug_text()
