class_name Digger
extends Node

@export var dig_rate: float = 100# dirt per second
@export var dirt_capacity: float = 100
@export var dirt_held: float = 0

var entity: Entity
var _target_dig_cell: Vector2i
var _max_dig_time: float

var dig_timer: Timer = Timer.new()


func _ready():
	dig_timer.one_shot = true
	add_child(dig_timer)
	entity = owner as Entity


func set_dig_cell(cell: Vector2i):
	_target_dig_cell = cell


func dig():
	var amount_to_dig = entity.current_map.dirt_left(_target_dig_cell)
	_max_dig_time = amount_to_dig / dig_rate
	dig_timer.start(_max_dig_time)


func finish_digging(max_amount: float) -> float:
	var time_left = dig_timer.get_time_left()
	var time_dug = _max_dig_time - time_left
	dig_timer.stop()
	var amount_dug = min(dig_rate * time_dug, max_amount)
	return entity.current_map.excavate_from(entity.current_cell, _target_dig_cell, amount_dug)
