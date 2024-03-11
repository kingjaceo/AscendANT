extends Sprite2D


# Called when the node enters the scene tree for the first time.
func _ready():
	_change()
	pass




func _change():
	while true:
		# pick a random starting sprite
		region_rect = Rect2i(64 * randi_range(0, 6), 0, 64, 64)
		# cycle through different rect values
		await get_tree().create_timer(0.4).timeout
		#tween.tween_property(self, "modulate", Color(pheromone.color, 0), 0.1)
		#await tween.finished
