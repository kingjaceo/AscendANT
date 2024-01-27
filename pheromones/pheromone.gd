class_name Pheromone extends Resource

var name
var lifetime: float # seconds before pheromone dissipates
var atlas_coord: Vector2i

func _init(name, lifetime, atlas_coord):
	self.name = name
	self.lifetime = lifetime
	self.atlas_coord = atlas_coord
