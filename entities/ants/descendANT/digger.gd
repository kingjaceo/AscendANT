class_name Digger
extends Node

@export var dig_rate: float = 100# dirt per second
@export var dirt_capacity: float = 100
@export var dirt_held: float = 0

var entity: Entity
var _target_dig_cell: Vector2i

var dig_timer: Timer = Timer.new()


func _ready():
	add_child(dig_timer)
	entity = owner as Entity


func set_dig_cell(cell: Vector2i):
	_target_dig_cell = cell


func dig():
	var amount_to_dig = entity.current_map.dirt_left(_target_dig_cell)
	var max_dig_time = amount_to_dig / dig_rate
	dig_timer.start(max_dig_time)


func finish_digging() -> float:
	var time_dug = dig_timer.wait_time - dig_timer.time_left
	dig_timer.stop()
	var amount_dug = dig_rate * time_dug
	return entity.current_map.excavate_from(entity.current_cell, _target_dig_cell, amount_dug)
