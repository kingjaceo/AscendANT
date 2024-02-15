class_name EntityMover 
extends Node

var moving = true
var entity: Entity

@export var _walk_speed: float = 100
@export var _turn_speed: float = 10

var _target: Vector2
var _point_path_to_target: PackedVector2Array
var _current_index

const TOLERANCE = 10
const EPSILON = 0.001

signal arrived_at_next_cell
signal arrived_at_target

func _ready():
	entity = get_parent()


func path_to(target_cell: Vector2i) -> void:
	_point_path_to_target = entity.current_map.get_point_path(entity.current_cell, target_cell)
	_current_index = 0
	_target = _point_path_to_target[_current_index]

	moving = true
	
	# TODO: start the animation


func idle() -> void:
	moving = false


func _physics_process(delta) -> void:
	if moving:
		_turn_and_move(delta)
		_check_distance()


func _turn_and_move(delta) -> void:
	var direction = _target - entity.position
	_rotate_toward(delta, direction)
	_move_forward(delta)


func _rotate_toward(delta, direction) -> void:
	if direction.length() > EPSILON:
		var angle_to = entity.transform.x.angle_to(direction)
		entity.rotate(sign(angle_to) * min(delta * _turn_speed, abs(angle_to)))


func _move_forward(delta: float) -> void:
	entity.position += entity.transform.x * delta * _walk_speed


func _check_distance() -> void:
	var distance = (_target - entity.position).length()
	if distance < TOLERANCE:
		arrived_at_next_cell.emit()
		_current_index += 1
		var arrived_at_last_cell_in_point_path = _current_index >= len(_point_path_to_target)
		if arrived_at_last_cell_in_point_path:
			moving = false
			arrived_at_target.emit()
		else:
			_target = _point_path_to_target[_current_index]
