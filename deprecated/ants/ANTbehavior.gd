extends Behavior

#var _scout_state: ScoutState
#var _world: BaseANT.World
#var _surrounded_index: int
#var _mark_pheromone: Pheromone
#var _food_held: float
#var _food_capacity: float
#
#var _current_cell: Vector2i
#var _previous_cell: Vector2i
#
#enum ScoutState {HOME, SCOUTING, TO_HOME}


#func _ready():
	#_food_held = 0
	#_food_capacity = 10
	#_surrounded_index = randi() % 6
	#_mark_pheromone = null
	#super()
	#
	#
#func _process(delta):
	#pass
	##var current_cell = _pheromone_map.local_to_map(get_parent().position)
	##_pheromone_map.mark_cell(current_cell, _mark_pheromone, _previous_cell)
	##if _world == BaseANT.World.OVERWORLD:
		##if _scout_state == ScoutState.SCOUTING:
			##_pheromone_map.mark_cell(current_cell, Pheromones.EXPLORED, current_cell)
		##if _scout_state == ScoutState.TO_HOME:
			##_pheromone_map.mark_cell(current_cell, Pheromones.TO_FOOD, current_cell)
		#
#
#func get_world_target_cell(world: BaseANT.World, current_cell: Vector2i) -> Vector2i:
	#var choice
	#if _world != world and world == BaseANT.World.COLONY: # ant moves from overworld to colony
		#_change_scout_state(ScoutState.HOME)
		#choice = _colony_map.get_food_cell()
	#elif _world != world and world == BaseANT.World.OVERWORLD: # ant moves from colony to overworld
		#_change_scout_state(ScoutState.SCOUTING)
		#choice = _get_overworld_target_cell(current_cell)
	#elif _world == world and world == BaseANT.World.COLONY: # ant needs new colony target
		#_dump_food()
		#choice = _colony_map.entrance	
	#elif _world == world and world == BaseANT.World.OVERWORLD: # ant needs new overworld target
		#choice = _get_overworld_target_cell(current_cell)	
		#
	#_world = world
	#return choice
#
#
#func get_world_point_path(world: BaseANT.World, current_cell: Vector2i, target_cell: Vector2i):
	#return super.get_world_point_path(world, current_cell, target_cell)
	#
#
#func get_food_target_cell(map: TileMap) -> Vector2i:
	#_change_scout_state(ScoutState.HOME)
	#return super.get_food_target_cell(map)
#
#
#func move_to_new_cell(world: BaseANT.World, cell: Vector2i) -> void:
	#_pheromone_map.mark_cell(cell, _mark_pheromone, _previous_cell)
	#_previous_cell = _current_cell
	#_current_cell = cell
	##if world == BaseANT.World.COLONY:
		##return
	##if world == BaseANT.World.OVERWORLD and _scout_state == ScoutState.TO_HOME:
		##_pheromone_map.mark_cell(cell, Pheromones.TO_FOOD, cell)
	#
#
#func _get_overworld_target_cell(current_cell: Vector2i):
	#if _scout_state == ScoutState.SCOUTING:
		#return _get_scout_cell(current_cell)
	#if _scout_state == ScoutState.TO_HOME:
		#return _pheromone_map.get_next_home_cell(current_cell)
#
#
#func _change_scout_state(new_state: ScoutState) -> void:
	#if new_state == ScoutState.HOME:
		#_mark_pheromone = null
	#if new_state == ScoutState.TO_HOME:
		#_mark_pheromone = Pheromones.TO_FOOD
	#if new_state == ScoutState.SCOUTING:
		#_mark_pheromone = Pheromones.EXPLORED
		#
	#_scout_state = new_state
#
#
#func _get_scout_cell(current_cell: Vector2i) -> Vector2i:
	#var pheromone_cells = _pheromone_map.get_surrounding_pheromone_cells(current_cell)
	#var food_choices = []
	#var to_food_choices = []
	#var empty_choices = []
	#var remaining_cells = []
	#
	#for pheromone_cell in pheromone_cells:
		## check if there's food
		#if Pheromones.FOOD in pheromone_cell.pheromone_directions:
			#food_choices.append(pheromone_cell.coordinates)
		## check if there is a trail to food
		#elif Pheromones.TO_FOOD in pheromone_cell.pheromone_directions and pheromone_cell.coordinates != _previous_cell:
			#to_food_choices.append(pheromone_cell.coordinates)
		## check if there is an unscouted cell
		#elif not Pheromones.EXPLORED in pheromone_cell.pheromone_directions:
			#empty_choices.append(pheromone_cell.coordinates)
		## otherwise the cell is uninteresting
		#else:
			#remaining_cells.append(pheromone_cell.coordinates)
		#
	#var current_pheromone_cell = _pheromone_map.get_pheromone_cell(current_cell)
	#
	#if food_choices:
		#_take_food(food_choices)
		#_change_scout_state(ScoutState.TO_HOME)
		#return _pheromone_map.get_next_home_cell(current_cell)
	#if Pheromones.TO_FOOD in current_pheromone_cell.pheromone_directions:
		#return current_pheromone_cell.pheromone_directions[Pheromones.TO_FOOD]
	#if to_food_choices:
		#return to_food_choices[randi() % len(to_food_choices)]
	#if empty_choices:
		#return empty_choices[randi() % len(empty_choices)]
	#else:
		#return remaining_cells[_surrounded_index % len(remaining_cells)]
#
#func _take_food(cells):
	#var cell = cells[randi() % len(cells)]
	#get_parent().eat_food()
	#_food_held = Messenger.take_food_from_cell(cell, _food_capacity)
#
#func _dump_food():
	#Messenger.add_food_to_colony(_food_held)
	#_food_held = 0
	#
#
#func _get_colony_target_cell(_map: TileMap) -> Vector2i: # should be overwridden
	#return _colony_map.get_food_cell()
