class_name CameraZoomer
extends CameraModule

@export var _start_zoom: Vector2 = Vector2(1, 1)

@export var _zoom_factor: float = 1
@export var _zoom_in_ratio: float = 1.5
@export var _zoom_out_ratio: float = 0.66
@export var _zoom_step = 0.001
@export var _zoom_speed: float = 100
@export var _speed_up_multiplier: float = 2
#@export var acceleration: float = 50
@export var _damp_factor: float = 10

#var _velocity: float
var _max_zoom: Vector2 = Vector2(6, 6)
var _min_zoom: Vector2 = Vector2(0.5, 0.5)
var _target_zoom: Vector2 = _start_zoom


func _setup():
	camera.get_viewport().size_changed.connect(_update_min_max_zoom)


func _process(delta):
	var speed_up = false
	if Input.is_action_pressed("fast_move"):
		speed_up = true
	#if speed_up:
		#camera.zoom = camera.zoom.lerp(_target_zoom, _speed_up_multiplier * delta)
	#else:
	#if (camera.zoom - _target_zoom).is_zero_approx():
		#_zoom_factor = 1
		#camera.zoom = _target_zoom
	#else:
	camera.zoom = camera.zoom.lerp(camera.zoom * _zoom_factor, _zoom_speed * delta)
	camera.zoom = camera.zoom.clamp(_min_zoom, _max_zoom)
	if _zoom_factor > 1:
		_zoom_factor -= _zoom_step * delta * _damp_factor
	else:
		_zoom_factor += _zoom_step * delta * _damp_factor
	#_zoom_factor = 1
	#if not (_target_zoom - camera.zoom).is_zero_approx():
		#var zoom_direction = abs(_target_zoom.x - camera.zoom.x) / (_target_zoom.x - camera.zoom.x)
		#if speed_up:
			#_velocity += zoom_direction * acceleration * delta * _speed_up_multiplier
		#else:
			#_velocity += zoom_direction * acceleration * delta
		#
		#var new_zoom = Vector2(camera.zoom.x + _velocity * delta, camera.zoom.x + _velocity * delta)
		#camera.zoom = new_zoom.clamp(_min_zoom, _max_zoom)
#
		#_velocity *= _damp_factor
	#else:
		#camera.zoom = _target_zoom


func _input(event):
	if event is InputEventMouseButton:
		if event.is_pressed() and not event.is_echo():
			if event.button_index == MOUSE_BUTTON_WHEEL_UP:
				_zoom_factor += _zoom_step #* _zoom_in_ratio
				_target_zoom = camera.zoom * _zoom_factor
				#_target_zoom = clamp(camera.zoom * _zoom_in_ratio, _min_zoom, _max_zoom)
			else : if event.button_index == MOUSE_BUTTON_WHEEL_DOWN:
				_zoom_factor -= _zoom_step #* _zoom_out_ratio
				_target_zoom = camera.zoom * _zoom_factor
				#_target_zoom = clamp(camera.zoom * _zoom_out_ratio, _min_zoom, _max_zoom)


func _update_min_max_zoom():
	pass


func get_debug_text() -> String:
	var text = ""
	text += "Zoom: " + str(snapped(camera.zoom,  Vector2(0.1, 0.1))) + "\n"
	text += "Max Zoom: " + str(_max_zoom) + "\n"
	text += "Min Zoom: " + str(_min_zoom)
	return text
