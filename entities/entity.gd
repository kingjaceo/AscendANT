class_name Entity
extends Node2D
'''
Entities expect:
	1. An entity controller resource, containing:
		1. Brain - makes decisions for the entity; signals mover with a new target
		2. Body - tracks lifetime, health, and physical attributes; grabs food / digs; signals completion of timers
		3. Mover - turns and moves; signals arrival at a new cell
	
Entities should all:
	1. Handle movement between worlds
	2. Track their current map
	3. Load a resource pointing to all the appropriate components
'''

var entity_count: int = 0
var entity_id: int

@export var _current_map: TileMap

@onready var _body = %EntityBody
@onready var _mover = %EntityMover

var _hungry: bool = false
var _target_cell: Vector2i
var _target: Vector2
var _current_cell: Vector2i
var _point_path: Array[Vector2i]

signal new_target

func _ready():
	entity_count += 1
	entity_id = entity_count
	
	_connect_body()
	_connect_brain()
	_connect_mover()
	
	#_brain.update_target()


func die(cause: String):
	print("Entity", entity_id, " died: ", cause)
	queue_free()


func _connect_body():
	_body.lifetime.timeout.connect(die.bind("old age"))
	_body.starvation.timeout.connect(die.bind("starvation"))
	#_body.hunger.timeout.connect(_brain.seek_food)

func _connect_brain():
	#_brain.new_target.connect(_mover.update_target)
	#_brain.current_map = _current_map

func _connect_mover():
	#_mover.arrived_at_target.connect(_brain.on_arrival_at_target)


func _update_target():
	if _hungry:
		_target_cell = _current_map.get_food_cell()
	else:
		_target_cell = _current_map.get_random_cell()
	
	_point_path = _current_map.get_point_path(_current_cell, _target_cell)
	_target = _current_map.map_to_local(_target_cell)
	emit_signal("new_target", _target)


func _seek_food():
	pass
	
func _on_arrival_at_target():
	_current_cell = _target_cell
	# check if there is food around
	
	# perform the current task, if any
	
	# change world, if necessary
	
	# update target
	_update_target()
	
	
# this simple brain should choose a random valid cell from the current world,
# tell the mover to walk toward it, wait a few seconds, then tell the mover to
# find a new target

# this simple brain should listen for a hungry signal and change its behavior
# upon becoming hungry: no longer should it give the mover new targets
