class_name SpawnerModule
extends MapModule

@export var spawn_locations: Array[Vector2i]

signal entity_spawned


func _setup():
	entity_spawned.connect(game_map._update)


func spawn(entity: Entity):
	entity.current_map = game_map
	game_map.entities.add_child(entity)
	entity_spawned.emit(entity)
