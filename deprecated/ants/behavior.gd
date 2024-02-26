class_name Behavior extends Node2D

#var _colony_map
#var _pheromone_map: PheromoneMap

#func _ready():
	#_colony_map = Messenger.colony_map
	#_pheromone_map = Messenger.pheromone_map
	#print("behavior ready!")
#
#func get_world_target_cell(world: BaseANT.World, current_cell: Vector2i) -> Vector2i:
	#if world == BaseANT.World.COLONY:
		#return _get_colony_target_cell(_colony_map)
	#if world == BaseANT.World.OVERWORLD:
		#return _get_overworld_target_cell(current_cell)
	#
	#return Vector2i.ZERO
#
#func get_world_point_path(world: BaseANT.World, current_cell: Vector2i, target_cell: Vector2i):
	#if world == BaseANT.World.COLONY:
		#return _get_colony_point_path(current_cell, target_cell)
	#if world == BaseANT.World.OVERWORLD:
		#return _get_overworld_point_path(current_cell, target_cell)
#
	#return []
#
#
#func get_food_target_cell(map: TileMap) -> Vector2i:
	#return map.get_food_cell()
	#
#
#
#func _get_colony_target_cell(map: TileMap) -> Vector2i: # should be overwridden
	#return _colony_map.choose_random_cell()
#
#
#func _get_colony_point_path(current_cell: Vector2i, target_cell: Vector2i):
	#return _colony_map.get_point_path(current_cell, target_cell)
#
#
#func _get_overworld_target_cell(current_cell: Vector2i) -> Vector2i: # should be overwridden
	#return _pheromone_map.choose_random_neighbor(current_cell)
#
#
#func _get_overworld_point_path(current_cell: Vector2i, target_cell: Vector2i):
	#return _pheromone_map.get_point_path(current_cell, target_cell)
#
#
#func move_to_new_cell(world: BaseANT.World, current_cell: Vector2i) -> void:
	#pass
