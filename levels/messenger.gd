extends Node2D

@export var _pheromone_map: TileMap

func mark_cell(coordinate: Vector2i, pheromone: Pheromone):
	_pheromone_map.mark_cell(coordinate, pheromone)
	

#func get_surrounding_tiles(coordinate):
#	var all_surrounding_tiles = _pheromone_map.get_surrounding_cells(coordinate)
#	var surrounding_tiles = []
#
#	for tile in all_surrounding_tiles:
#		var atlas_coords = _pheromone_map.get_cell_atlas_coords(0, tile)
#		if atlas_coords != Vector2i(-1,-1):
#			surrounding_tiles.append(tile)
#
#	return surrounding_tiles
#

func get_surrounding_pheromone_cells(coordinate: Vector2i):
#	var all_surrounding_tiles = _pheromone_map.get_surrounding_pheromone_cells(coordinate)
#	var surrounding_tiles = []
#
#	for tile in all_surrounding_tiles:
#		var atlas_coords = _pheromone_map.get_cell_atlas_coords(0, tile.coordinates)
#		if atlas_coords != Vector2i(-1,-1):
#			surrounding_tiles.append(tile)
	var surrounding_tiles = _pheromone_map.get_surrounding_pheromone_cells(coordinate)
	return surrounding_tiles

func get_tile_position(coordinate):
	return _pheromone_map.get_tile_position(coordinate)
	

func get_tile_coordinate(position):
	return _pheromone_map.get_tile_coordinate(position)
	

func get_home_tile():
	return _pheromone_map.home_tile


func toggle_terrain():
	_pheromone_map.toggle_terrain()

func toggle_pheromones():
	_pheromone_map.toggle_pheromones()

func toggle_outline():
	_pheromone_map.toggle_outline()
