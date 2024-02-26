class_name CameraZoomer
extends CameraModule

@export var _start_zoom: Vector2 = Vector2(1, 1)
@export var _base_zoom_in_speed: float = 2
@export var _base_zoom_out_speed: float = 0.5
@export var _speed_up_multiplier: float = 2

var _max_zoom: Vector2 = Vector2(8, 8)
var _min_zoom: Vector2 = Vector2(0.1, 0.1)
var _target_zoom: Vector2 = _start_zoom


func _setup():
	camera.get_viewport().size_changed.connect(_update_min_max_zoom)


func _process(delta):
	var speed_up = false
	if Input.is_action_pressed("fast_move"):
		speed_up = true
	
	#if _target_zoom.x < camera.zoom.x:
		#camera.zoom = camera.zooms.lerp(_target_zoom, _speed_up_multiplier * delta)
	#elif _target_zoom.x > camera.zoom.x:
	if speed_up:
		camera.zoom = camera.zoom.lerp(_target_zoom, _speed_up_multiplier * delta)
	else:
		camera.zoom = camera.zoom.lerp(_target_zoom, delta)


func _input(event):
	if event is InputEventMouseButton:
		if event.is_pressed() and not event.is_echo():
			var mouse_position = event.position
			if event.button_index == MOUSE_BUTTON_WHEEL_UP:
				_target_zoom = clamp(camera.zoom * _base_zoom_in_speed, _min_zoom, _max_zoom)
			else : if event.button_index == MOUSE_BUTTON_WHEEL_DOWN:
				_target_zoom = clamp(camera.zoom * _base_zoom_out_speed, _min_zoom, _max_zoom)


func _update_min_max_zoom():
	pass


func get_debug_text() -> String:
	var text = ""
	text += "Zoom: " + str(snapped(camera.zoom,  Vector2(0.1, 0.1))) + "\n"
	text += "Max Zoom: " + str(_max_zoom) + "\n"
	text += "Min Zoom: " + str(_min_zoom)
	return text
