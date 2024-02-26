extends Behavior

var _dig_state: DigState
var _current_choice: Vector2i

enum DigState {EXPAND, DESCEND, COLLECT}

# a living entity is the root node for all living creatures:
# - it can move between worlds
# - knows about the worlds and maps
# (one for each unique entity class, eg spider, ant, etc)
# a living entity has three components:
# - a mover:
#	- tracks current/previous/target cells and current map
#	- signals arrival at new cells
#	- moves and turns the ant
#	(one per entity superclass, eg all walking entities could even share the same mover)
# - a body
#	- contains timers tracking rest, lifetime, hunger, starvation
#	- holds data about the entity body, like its capacity, damage, health, and speed
#	(single body for each ant type, for now, should be one per in the future)
# - a brain
#	- makes the decisions for the entity
#	- signals the mover for a new target
#	(unique brain for each ant class)





#func _ready():
	#_dig_state = DigState.EXPAND
	#super()
	#
#func get_world_target_cell(world: BaseANT.World, current_cell: Vector2i) -> Vector2i:
	#if world == BaseANT.World.COLONY and _current_choice == current_cell:
		#_colony_map.excavate_cell(current_cell + Vector2i(0, 1), 20)
		#get_parent()._change_movement_state(BaseANT.MovementState.IDLE)
	#if world == BaseANT.World.COLONY:
		#return _choose_next_colony_cell(current_cell)
	#if world == BaseANT.World.OVERWORLD:
		#return _pheromone_map.entrance
		#
	#push_warning("WARNING: descendANT get_world_target_cell fails to return value, returning (0, 0)")
	#return Vector2.ZERO
#
#
#func get_world_point_path(world: BaseANT.World, current_cell: Vector2i, target_cell: Vector2i):
	#if world == BaseANT.World.COLONY:
		#return _colony_map.get_point_path(current_cell, target_cell)
	#if world == BaseANT.World.OVERWORLD:
		#return [current_cell, _pheromone_map.entrance]
		#push_warning("WARNING: descendANT pathing in overworld!")
		#
	#push_warning("WARNING: descendANT get_world_point_path fails to return value, returning [current, target]")
	#return [current_cell, current_cell]
#
#
#func _choose_next_colony_cell(current_cell: Vector2i) -> Vector2i:
	#var _choice: Vector2i
	#if _dig_state == DigState.EXPAND:
		#_choice = _choose_expand_cell(current_cell)
	#if _dig_state == DigState.DESCEND:
		#_choice = _choose_descend_cell(current_cell)
	#if _dig_state == DigState.COLLECT:
		#_choice = _choose_collect_cell(current_cell)
		#
	#return _choice
#
#
#func _choose_expand_cell(current_cell: Vector2i) -> Vector2i:
	#_current_choice = _colony_map.expand_choice
	#return _current_choice
	#
	#
#func _choose_descend_cell(current_cell: Vector2i) -> Vector2i:
	#push_warning("_choose_descend_cell not implemented")
	#return Vector2.ZERO
	#
#
#func _choose_collect_cell(current_cell: Vector2i) -> Vector2i:
	#push_warning("_choose_collect_cells not implemented")
	#return Vector2.ZERO
	#
#
##func get_food_target_cell(map: TileMap) -> Vector2i:
	##pass
