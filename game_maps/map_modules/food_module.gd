class_name FoodModule
extends MapModule

var food_piles_by_cell: Dictionary # {Vector2i cell: FoodPile food_pile}

signal food_taken


func _setup() -> void:
	for food_pile in get_children():
		food_piles_by_cell[food_pile.location] = food_pile


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
