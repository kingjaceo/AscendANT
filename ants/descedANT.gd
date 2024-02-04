extends Node2D

@export var _colony_map: ColonyMap
@onready var animation_player = $AnimationPlayer

var _state
var _target_cell: Vector2i
var _current_cell: Vector2i
var _point_path
var _current_point_index
var _walk_speed = 30
var _target_position: Vector2
var _prev_position: Vector2
var _next_cell_position: Vector2
var _next_next_cell_position: Vector2
var _turn_speed = 6 # seconds to rotate 360 degrees
enum AntState {NONE, MOVING, TURNING, CHOOSING}

const EPSILON: float = 0.01

# Called when the node enters the scene tree for the first time.
func _ready():
	animation_player.play("walk")
	#_current_cell = _colony_map.local_to_map(position)
	#position = _colony_map.map_to_local(_current_cell)
	_state = AntState.CHOOSING


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	_current_cell = _colony_map.local_to_map(position)
	if _state == AntState.CHOOSING:
		var allowed_cells = _colony_map.get_used_cells(1)
		var choice = allowed_cells[randi() % len(allowed_cells)]
		_target_cell = choice
		_point_path = _colony_map.astar_grid.get_point_path(_current_cell, _target_cell)
		_current_point_index = min(len(_point_path) - 1, 1)
		_next_cell_position = _point_path[_current_point_index] + Vector2(8, 8)
		var next_point_index = min(len(_point_path) - 1, _current_point_index)
		_next_next_cell_position = _point_path[next_point_index] + Vector2(8, 8)
		_target_position = _colony_map.map_to_local(_target_cell)
		_prev_position = position
		_state = AntState.TURNING
		
func _physics_process(delta):
	if _state == AntState.MOVING:
		_rotate_to_target(delta)
		_move_forward(delta)

		
	if _state == AntState.TURNING:
		_rotate_to_target(delta)

func _move_forward(delta):
	var direction = (_next_cell_position - _prev_position)
	var distance_left = (position - _next_cell_position).length_squared()
	
	if distance_left < EPSILON:
		_arrive_at_new_tile()
	else:
		position += transform.x * delta * _walk_speed
		
func _move_to_target(delta):
	var direction = (_next_cell_position - _prev_position)
	var distance_left = (position - _next_cell_position).length_squared()
	
	if distance_left < EPSILON:
		_arrive_at_new_tile()
	else:
		position += direction * delta * _walk_speed
		
func _arrive_at_new_tile():
	_current_cell = _colony_map.local_to_map(position)
	_current_point_index += 1
	_prev_position = position
	#_state = AntState.TURNING
	if _current_point_index == len(_point_path):
		_state = AntState.CHOOSING
	else:
		_next_cell_position = _point_path[_current_point_index] + Vector2(8, 8)
		var next_point_index = min(len(_point_path) - 1, _current_point_index)
		_next_next_cell_position = _point_path[next_point_index] + Vector2(8, 8)
		
func _rotate_to_target(delta):
	var direction = (_next_next_cell_position - position)
	if direction.length() > EPSILON:
		var angle_to = transform.x.angle_to(direction)
		if -PI / 2 < angle_to and angle_to < PI / 2:
			_state = AntState.MOVING
		rotate(sign(angle_to) * min(delta * _turn_speed, abs(angle_to)))
