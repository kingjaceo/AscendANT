class_name CameraController
extends Camera2D

var _bounds
var _max_zoom: Vector2
var _min_zoom: Vector2
var _zoom_ratio: float
var _target_zoom: Vector2
var _target_point: Vector2
var _lower_bound: Vector2
var _upper_bound: Vector2

@export var _game_map: GameMap
@export var _start_zoom: Vector2
@export var _base_zoom_in_speed: float = 4
@export var _base_zoom_out_speed: float = 0.25
@export var _base_camera_speed: float = 50
var _actual_move_speed: float
@export var _follow_speed: float = 1
@export var _damp_factor: float

var _position_change: Vector2 = Vector2.ZERO
var _move_direction: Vector2 = Vector2.ZERO

var _follow: Node2D


func _ready() -> void:
	_set_attributes()
	get_viewport().size_changed.connect(_set_attributes)
	zoom = _start_zoom
	set_process_input(false)


func _process(delta) -> void:
	_move_direction = Vector2.ZERO
	
	if Input.is_action_pressed("up"):
		_move_direction -= Vector2(0, 1) 
		_unfollow()
	if Input.is_action_pressed("down"):
		_move_direction += Vector2(0, 1) 
		_unfollow()
	if Input.is_action_pressed("left"):
		_move_direction -= Vector2(1, 0) 
		_unfollow()
	if Input.is_action_pressed("right"):
		_move_direction += Vector2(1, 0) 
		_unfollow()
	
	_target_point += _move_direction * _base_camera_speed
	
	if _follow:
		_target_point = _follow.position
	if _target_zoom.x < zoom.x:
		zoom = zoom.lerp(_target_zoom, 3 * delta)
	elif _target_zoom.x > zoom.x:
		zoom = zoom.lerp(_target_zoom, delta)
	position = position.lerp(_target_point, 2*delta)
	_target_point = _target_point.lerp(position, delta)
	position = position.clamp(_lower_bound, _upper_bound)
	zoom = zoom.clamp(_min_zoom, _max_zoom)


func _zoom_to(target_zoom: Vector2, delta) -> void:
	var zoom_change = target_zoom - zoom
	if zoom_change.x < 0:
		zoom
	if zoom_change.x > 0:
		_set_attributes()
	zoom += zoom_change


func _move_to(target_position: Vector2, delta: float) -> void:
	var direction = target_position - position
	position += direction * _follow_speed * delta


func _input(event):
	if event is InputEventMouseButton:
		if event.is_pressed() and not event.is_echo():
			var mouse_position = event.position
			if event.button_index == MOUSE_BUTTON_WHEEL_UP:
				_target_zoom = clamp(zoom * _base_zoom_in_speed, _min_zoom, _max_zoom)
				#_target_point = mouse_position
			else : if event.button_index == MOUSE_BUTTON_WHEEL_DOWN:
				_target_zoom = clamp(zoom * _base_zoom_out_speed, _min_zoom, _max_zoom)
				#_target_point = mouse_position


func _damp(_position_change):
	_position_change *= _damp_factor


func _on_mouse_entered():
	set_process_input(true)


func _on_mouse_exited():
	set_process_input(false)


func _set_attributes():
	_bounds = _game_map.get_bounds()
	var viewport = get_viewport()
	var viewport_width = viewport.size.x

	var max_zoom = 8
	_max_zoom = Vector2(max_zoom, max_zoom)
	var min_zoom =  float(viewport_width) / float(_bounds[1] - _bounds[0])
	_min_zoom = Vector2(min_zoom, min_zoom)
	zoom = clamp(zoom, _min_zoom, _max_zoom)
	
	var visible_half_width = viewport.size.x / (2 * zoom.x)
	var min_x = _bounds[0] + visible_half_width
	var max_x = _bounds[1] - visible_half_width
	
	var visible_half_height = viewport.size.y / (2 * zoom.y)
	var min_y = _bounds[2] + visible_half_height - 800
	var max_y = _bounds[3] - visible_half_height + 1000
	
	_lower_bound = Vector2(min_x, min_y)
	_upper_bound = Vector2(max_x, max_y)
	
	#_actual_move_speed = _base_camera_speed * (1 / zoom.x)
	#_actual_zoom_speed = _base_zoom_speed * (1 / zoom.x)


func follow(node: Node2D):
	node.died.connect(_unfollow)
	_follow = node


func _unfollow() -> void:
	if _follow:
		_follow.died.disconnect(_unfollow)
	_follow = null
