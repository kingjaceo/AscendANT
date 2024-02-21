class_name ColonyMap 
extends GameMap

@export var pheromone_map: PheromoneMap
@export var spawn_location: Vector2i = Vector2i(15, 11)
@export var exit: Vector2i = Vector2i(10, -7)
@export var entrance: Vector2i = Vector2i(10, -6)
@export var cell_size: Vector2 = Vector2(64, 64)
@export var adjustment: Vector2 = cell_size / 2
@export var temp_food_cell = Vector2i(7, 11)

var diggable_from_choices: Array[Vector2i] = []
var colony: Colony
var dump_sites: Dictionary = {"dirt": Vector2i(10, -6)}
var dump_amounts: Dictionary = {"dirt": 1000}

var _valid_cells: Array[Vector2i]
var _colony_cells = {}
var _food_by_cell = {}

const BACKGROUND_LAYER = 0
const WALKABLE_LAYER = 1
const DIGGABLE_LAYER = 2
const NAV_TERRAIN_SET = 0
const IMPASSABLE_ATLAS_COORDS = Vector2i(10, 1)

signal cell_excavated


func _ready() -> void:
	spawn_locations = [spawn_location]
	_exits_at = {exit: pheromone_map}
	_entrances_from = {pheromone_map: entrance}
	
	_setup_astar_grid()
	_place_sprites()
	
	camera.position = map_to_local(spawn_location)
		
	_food_by_cell[temp_food_cell] = 100
	_create_colony_cells()


func get_random_cell() -> Vector2i:
	var choice = _valid_cells[randi() % len(_valid_cells)]
	return choice


func get_nearest_food_cell(cell: Vector2i) -> Vector2i:
	return temp_food_cell


func get_food_cell() -> Vector2i:
	return temp_food_cell


func get_point_path(start: Vector2i, end: Vector2i) -> PackedVector2Array:
	var point_path = astar_grid.get_point_path(start, end)
	for i in range(len(point_path)):
		point_path[i] += adjustment
	return point_path


func has_food(cell: Vector2i) -> bool:
	return cell == temp_food_cell


func get_diggable_from_cell(cell: Vector2i) -> Vector2i:
	return diggable_from_choices[randi() % len(diggable_from_choices)]


#func __create_colony_cells():
	##var seeds = [Vector2i(10, 11), Vector2i(6, 11)]
	#var top = Vector2i(10, 11)
	#var depth = 34
	#var bottom = top + Vector2i(0, depth)
	#
	#var next = top + Vector2i(0, 1)
	#var current = top
	#var prev = null
	#
	##expand_choices = [next]
	##expand_choice = top
	#
	#for i in range(1, depth - 1):
		#next = top + Vector2i(0, 1)
		#var colony_cell = ColonyCell.new(current, [next], [prev])
		#_colony_cells[current] = colony_cell
		#prev = current
		#current = next


func take_food_from(cell: Vector2i, amount: float) -> float:
	var amount_taken = min(_food_by_cell[cell], amount)
	_food_by_cell[cell] -= amount_taken
	return amount_taken


func excavate_from(from_cell: Vector2i, excavated_cell: Vector2i, dirt_moved: float) -> float:
	#excavated_cell = _colony_cells[from_cell].accesses[0]
	var actual_dirt_moved = min(dirt_moved, _colony_cells[excavated_cell].dirt_left)
	_colony_cells[excavated_cell].dirt_left -= actual_dirt_moved
	if _colony_cells[excavated_cell].dirt_left <= 0:
		_update_colony_cells(from_cell, excavated_cell)
		set_cells_terrain_connect(WALKABLE_LAYER, [excavated_cell], 0, 0)
		set_cell(DIGGABLE_LAYER, excavated_cell)
		astar_grid.set_point_solid(excavated_cell, false)
	return actual_dirt_moved


func dirt_left(at_cell: Vector2i) -> float:
	return _colony_cells[at_cell].dirt_left


func get_dump_site(resource: String) -> Vector2i:
	return dump_sites[resource]


func dump(resource: String, amount: float) -> void:
	dump_amounts[resource] += amount


func _setup_astar_grid() -> void:
	astar_grid = AStarGrid2D.new()
	astar_grid.region = Rect2i(0, -6, 33, 65)
	astar_grid.cell_size = cell_size
	astar_grid.diagonal_mode = 3
	astar_grid.update()
	
	for cell in get_used_cells(BACKGROUND_LAYER):
		astar_grid.set_point_solid(cell)
		
	for cell in get_used_cells(WALKABLE_LAYER):
		astar_grid.set_point_solid(cell, false)
		_valid_cells.append(cell)


func _place_sprites() -> void:
	_place_sprite("Colony", spawn_location)
	_place_sprite("Food", temp_food_cell)


func _place_sprite(node_name: String, cell: Vector2i):
	var sprite = get_node(node_name)
	sprite.position = map_to_local(cell) + Vector2(0, -6)


func _create_colony_cells() -> void:
	var walkable_cells = get_used_cells(1)
	var diggable_cells = get_used_cells(2)
	
	for cell in walkable_cells:
		var accesses = []
		var neighbors = get_surrounding_cells(cell)
		for neighbor in neighbors:
			if neighbor in diggable_cells:
				accesses.append(neighbor)
		if len(accesses) > 0:
			diggable_from_choices.append(cell)
		_colony_cells[cell] = ColonyCell.new(cell, accesses, [])
	
	for cell in diggable_cells:
		var accesses = []
		var accessed_by = []
		var neighbors = get_surrounding_cells(cell)
		for neighbor in neighbors:
			if neighbor in walkable_cells:
				accessed_by.append(neighbor)
			if neighbor in diggable_cells:
				accesses.append(neighbor)
		_colony_cells[cell] = ColonyCell.new(cell, accesses, accessed_by)


func get_colony_cell(cell: Vector2i) -> ColonyCell:
	return _colony_cells[cell]


func _update_colony_cells(from_cell: Vector2i, excavated_cell: Vector2i) -> void:
	_colony_cells[from_cell].accesses.erase(excavated_cell)
	if len(_colony_cells[from_cell].accesses) == 0:
		diggable_from_choices.erase(from_cell)
	_colony_cells[excavated_cell].accesses = _colony_cells[excavated_cell].accessed_by
	diggable_from_choices.append(excavated_cell)
