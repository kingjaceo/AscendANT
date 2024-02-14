class_name Pheromone extends Resource

var name: Pheromones.Names
var lifetime: float # seconds before pheromone dissipates
var atlas_coord: Vector2i

func _init(_name, _lifetime, _atlas_coord):
	self.name = _name
	self.lifetime = _lifetime
	self.atlas_coord = _atlas_coord

func _to_string():
	return Pheromones.Names.keys()[name]
