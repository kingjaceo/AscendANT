class_name EntityBrain 
extends Node
'''
The brain is concerned with tracking:
	1. relevant attributes associated with its decision making
The brain is concerned with controlling:
	1. choosing the next target
The brain signals:
	1. arrival at food	
'''
#var current_map: TileMap:
	#get:
		#return current_map
	#set(map):
		#current_map = map
#var target: Vector2
#
#var _parent: Node2D
#var _hungry: bool = false
#var _current_cell: Vector2i
#var _next_cell: Vector2i
#var _next_position: Vector2
#var _target_cell: Vector2i
#var _point_path: PackedVector2Array
#var _current_index: int = -1
#var food_cell: Vector2i
#
#const TOLERANCE: float = 20
#
#signal target_updated(new_target)
#signal food_found

#func _ready():
	#current_map = Messenger.colony.colony_map
	#_parent = get_parent()


#func _process(_delta):
	## TODO: decide whether we should distance-based or cell-based movement
	#var distance = (target - _parent.position).length()
	##_current_cell = current_map.local_to_map(_parent.position)
	#if distance < TOLERANCE:
		#next_cell()

#
#func set_food_target():
	#_target_cell = current_map.get_food_cell()
	#_point_path = current_map.get_point_path(_current_cell, _target_cell)
	#_current_index = 0

#
#func set_normal_target():
	#_target_cell = current_map.get_random_cell()
	#_point_path = current_map.get_point_path(_current_cell, _target_cell)
	#_current_index = 0


#func next_cell():
	#check_food()
	#set_current_cell()
	#if _current_index >= len(_point_path) or _current_index == -1:
		#set_normal_target()
		#
	#_next_position = _point_path[_current_index]
	#_next_cell = current_map.local_to_map(_next_position)
	#target = current_map.map_to_local(_next_cell)
	#target_updated.emit(target)
	#_current_index += 1


#func set_current_cell():
	#_current_cell = current_map.local_to_map(_parent.position)
#
#
#func check_food():
	#if current_map.food_around(_current_cell):
		#food_cell = current_map.find_food_around(_current_cell)
		#food_found.emit(_current_cell)
