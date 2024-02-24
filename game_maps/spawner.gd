class_name Spawner
extends MapModule

@export var spawn_locations: Array[Vector2i]


# Called every frame. 'delta' is the elapsed time since the previous frame.
func spawn(entity: Entity):
	owner.entities.add_child(entity)
