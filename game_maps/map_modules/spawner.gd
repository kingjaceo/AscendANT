class_name SpawnerModule
extends MapModule

@export var spawn_locations: Array[Vector2i]

signal entity_spawned


func _ready():
	entity_spawned.connect(owner._update)


func spawn(entity: Entity):
	entity.current_map = owner
	owner.entities.add_child(entity)
	entity_spawned.emit(entity)
