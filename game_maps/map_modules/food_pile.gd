class_name FoodPile
extends Pile


# TODO: should only be on food taken
func _update() -> void:
	if amount_remaining <= 0:
		_remove("eaten")
		return
	if amount_remaining <= 0.25 * starting_amount:
		pile_sprite.region_rect = Rect2(384, 0, 128, 128)
		return
	if amount_remaining <= 0.5 * starting_amount:
		pile_sprite.region_rect = Rect2(256, 0, 128, 128)
		return
	if amount_remaining <= 0.75 * starting_amount:
		pile_sprite.region_rect = Rect2(128, 0, 128, 128)
		return
