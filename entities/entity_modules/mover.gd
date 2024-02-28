class_name Mover 
extends EntityModule

var moving = false

@export var _walk_speed: float = 100
@export var _turn_speed: float = 10

var _target: Vector2
var _point_path_to_target: PackedVector2Array
var _current_index: int

var current_cell: Vector2i
var previous_cell: Vector2i

const TOLERANCE = 10
const EPSILON = 0.001

signal arrived_at_next_cell
signal arrived_at_target


func _setup() -> void:
	choice_making_signals = [arrived_at_target]
	death_signals = {}


func path_to(target_cell: Vector2i) -> void:
	_point_path_to_target = entity.current_map.get_point_path(entity.current_cell, target_cell)
	_current_index = 0
	_target = _point_path_to_target[_current_index]
	moving = true


func move_to(target: Vector2) -> void:
	moving = true
	_target = target


func idle() -> void:
	moving = false


func get_debug_text() -> String:
	return "Mover Target: " + str(_target)


func get_debug_draw() -> Dictionary:
	var pos = _target - owner.position
	return {"position": pos, "color": Color.BLUE, "size": 10}


func _physics_process(delta) -> void:
	if moving:
		_turn_and_move(delta)
		_check_distance()
		_check_cell()


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
		entity.current_cell = _target
		arrived_at_target.emit()


func _check_cell() -> void:
	current_cell = entity.current_map.local_to_map(entity.position)
	if current_cell != previous_cell:
		arrived_at_next_cell.emit()
		previous_cell = current_cell
