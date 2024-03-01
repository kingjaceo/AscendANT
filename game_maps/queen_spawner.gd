class_name QueenSpawner
extends SpawnerModule

@export var spawn_location: Vector2i
const QUEEN = preload("res://entities/queen.tscn")


func _process(_delta):
	var queen = QUEEN.instantiate()
	spawn(queen)
	game_map.remove(self)
	queue_free()
