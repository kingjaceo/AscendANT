class_name CameraMover
extends CameraModule

@export var acceleration: float = 1
@export var _damp_factor: float = 0.95
@export var _speed_up_multiplier: float = 2

var _velocity: Vector2 = Vector2.ZERO
var _move_direction: Vector2


signal input_pressed


func _process(delta):
	var speed_up = false
	_move_direction = Vector2.ZERO
	
	if Input.is_action_pressed("fast_move"):
		speed_up = true
	if Input.is_action_pressed("up"):
		_move_direction -= Vector2(0, 1) 
		input_pressed.emit()
	if Input.is_action_pressed("down"):
		_move_direction += Vector2(0, 1) 
		input_pressed.emit()
	if Input.is_action_pressed("left"):
		_move_direction -= Vector2(1, 0) 
		input_pressed.emit()
	if Input.is_action_pressed("right"):
		_move_direction += Vector2(1, 0) 
		input_pressed.emit()
	
	_move_direction = _move_direction.normalized()
	if speed_up:
		_velocity += _move_direction * acceleration * _speed_up_multiplier
	else:
		_velocity += _move_direction * acceleration
	camera.position += _velocity
	_velocity *= _damp_factor


func get_debug_text():
	var text = ""
	text += "Position: " + str(snapped(camera.position, Vector2(0.1, 0.1))) + "\n"
	text += "Velocity: " + str(snapped(_velocity, Vector2(0.1, 0.1))) + "\n"
	text += "Acceleration: " + str(acceleration)
	return text
