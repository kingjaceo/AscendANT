class_name CameraFollowable
extends Node2D

var entity: Entity

func _ready():
	entity = get_parent()


func _input(event):
	if event is InputEventMouseButton and event.pressed and event.button_index == MOUSE_BUTTON_LEFT:
		# Get the mouse position in global coordinates
		var mouse_global = get_global_mouse_position()
		var distance = (mouse_global - entity.position).length()
		if distance < 20:
			entity.current_map.camera.follow(entity)
