class_name Pile
extends MapObject

@export var location: Vector2i
@export var starting_amount: float
var amount_remaining: float

@onready var pile_sprite: Sprite2D = $Sprite2D


func _setup() -> void:
	amount_remaining = starting_amount


func take_from(amount: float) -> float:
	var amount_available = min(amount, amount_remaining)
	amount_remaining -= amount_available
	_update()
	return amount_available


func _update() -> void:
	if amount_remaining <= 0:
		queue_free()


#func _place_pile() -> void:
	#pile_sprite.position = owner.map_to_local(location)
	#pile_sprite.visible = true
