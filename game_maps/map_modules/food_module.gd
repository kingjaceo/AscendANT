class_name FoodModule
extends MapModule

var food_piles_by_cell: Dictionary # {Vector2i cell: FoodPile food_pile}
const FOOD_PILE = preload("res://game_maps/map_modules/food_pile.tscn")
const FOOD_PHEROMONE = preload("res://pheromones/food.tres")
@export var _pheromone_module: PheromoneModule
@export var _min_food_spawn_time: float = 10
@export var _max_food_spawn_time: float = 30

signal food_taken
signal food_spawned
signal food_removed


func _setup() -> void:
	_spawn_food_randomly()


func has_food() -> bool:
	return len(food_piles_by_cell)


func get_nearest_pile(position: Vector2) -> Vector2:
	var nearest_pile = Vector2.ZERO
	var nearest_distance = INF
	for cell in food_piles_by_cell:
		var pile_position = owner.map_to_local(cell)
		var distance = (position - pile_position).length()
		if distance < nearest_distance:
			nearest_pile = pile_position
			nearest_distance = distance
	return nearest_pile


func food_at(position: Vector2) -> bool:
	var cell = owner.local_to_map(position)
	if food_piles_by_cell.has(cell):
		return food_piles_by_cell[cell].amount_remaining > 0
	return false


func take_food_from(position: Vector2, amount: float) -> float:
	var cell = owner.local_to_map(position)
	if food_piles_by_cell.has(cell):
		return food_piles_by_cell[cell].take_from(amount)
	return 0


func remove(cell: Vector2i) -> void:
	food_piles_by_cell.erase(cell)
	food_removed.emit(cell)


func _spawn_food_randomly() -> void:
	while true:
		var time_til_next_food = randi_range(_min_food_spawn_time, _max_food_spawn_time)
		var timer = get_tree().create_timer(time_til_next_food)
		await timer.timeout
		var new_pile = FOOD_PILE.instantiate()
		var new_location = game_map.get_random_walkable_cell()
		new_pile.location = new_location
		new_pile.position = game_map.map_to_local(new_location)
		add_child(new_pile)
		food_piles_by_cell[new_location] = new_pile
		food_spawned.emit(FOOD_PHEROMONE, new_location)
