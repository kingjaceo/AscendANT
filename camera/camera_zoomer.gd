class_name CameraZoomer
extends CameraModule

@export var _start_zoom: Vector2 = Vector2(1, 1)

#@export var _zoom_in_ratio: float = 1.5
#@export var _zoom_out_ratio: float = 0.66
@export var _zoom_step: Vector2 = Vector2(0.1, 0.1)
@export var _zoom_speed: float = 1
@export var _speed_up_multiplier: float = 2


var _max_zoom: Vector2 = Vector2(8, 8)
var _min_zoom: Vector2 = Vector2(0.3, 0.3)
var _target_zoom: Vector2 = _start_zoom


func _setup():
	camera.get_viewport().size_changed.connect(_update_min_max_zoom)


func _process(_delta):
	var speed_up = false
	if Input.is_action_pressed("fast_move"):
		speed_up = true
	_target_zoom = _target_zoom.clamp(_min_zoom, _max_zoom)
	if speed_up:
		camera.zoom = camera.zoom.lerp(_target_zoom, 0.1 * _zoom_speed  * _speed_up_multiplier)
	else:
		camera.zoom = camera.zoom.lerp(_target_zoom, 0.1 * _zoom_speed)	
	camera.zoom = camera.zoom.clamp(_min_zoom, _max_zoom)


func _input(event):
	if event is InputEventMouseButton:
		if event.is_pressed() and not event.is_echo():
			if event.button_index == MOUSE_BUTTON_WHEEL_UP:
				_target_zoom += _zoom_step
			else : if event.button_index == MOUSE_BUTTON_WHEEL_DOWN:
				_target_zoom -= _zoom_step
				


func _update_min_max_zoom():
	pass


func get_debug_text() -> String:
	var text = ""
	text += "Zoom: " + str(snapped(camera.zoom,  Vector2(0.1, 0.1))) + "\n"
	text += "Max Zoom: " + str(_max_zoom) + "\n"
	text += "Min Zoom: " + str(_min_zoom)
	return text
