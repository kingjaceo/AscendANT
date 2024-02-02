class_name Pheromone extends Resource

var name: Pheromones.Names
var lifetime: float # seconds before pheromone dissipates
var atlas_coord: Vector2i

func _init(name, lifetime, atlas_coord):
	self.name = name
	self.lifetime = lifetime
	self.atlas_coord = atlas_coord

func _to_string():
	return Pheromones.Names.keys()[name]
