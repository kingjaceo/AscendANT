class_name Mover 
extends EntityModule

var moving = false

@export var _walk_speed: float = 100
@export var _turn_speed: float = 10

var _target: Vector2
var _point_path_to_target: PackedVector2Array
var _current_index

const TOLERANCE = 10
const EPSILON = 0.001

signal arrived_at_next_cell
signal arrived_at_target


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
	#var pos = owner.current_map.to_global(_target)
	var pos = _target - owner.position
	return {"position": pos, "color": Color.BLUE, "size": 10}


func _setup() -> void:
	choice_making_signals = [arrived_at_target]
	death_signals = {}


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
		arrived_at_target.emit(_target)
		#arrived_at_next_cell.emit()
		#_current_index += 1
		#var arrived_at_last_cell_in_point_path = _current_index >= len(_point_path_to_target)
		#if arrived_at_last_cell_in_point_path:
			#moving = false
			#arrived_at_target.emit()
		#else:
			#_target = _point_path_to_target[_current_index]
