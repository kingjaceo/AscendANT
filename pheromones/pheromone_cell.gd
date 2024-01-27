class_name PheromoneCell extends Resource

var coordinates
var _pheromones = {}
var strongest_pheromone: Pheromone
var strongest_pheromone_percentage: float
var strongest_pheromone_color: Color

func _init(coordinates):
	self.coordinates = coordinates
	

func add_pheromone(pheromone: Pheromone):
	_pheromones[pheromone] = pheromone.lifetime
	strongest_pheromone = pheromone
	strongest_pheromone_percentage = 100
	

func decay_pheromones(delta):
	for pheromone in _pheromones:
		_pheromones[pheromone] -= delta
		
		if _pheromones[pheromone] <= 0:
			_pheromones.erase(pheromone)
			if strongest_pheromone == pheromone:
				strongest_pheromone = null
			

func update_strongest_pheromone():
	strongest_pheromone_percentage = 0
	for pheromone in _pheromones:
		var pheromone_percentage = _pheromones[pheromone] / pheromone.lifetime * 100
		if pheromone_percentage > strongest_pheromone_percentage:
			strongest_pheromone_percentage = pheromone_percentage
			strongest_pheromone = pheromone
			
		if _pheromones[pheromone] == INF:
			strongest_pheromone_percentage = 100
			strongest_pheromone = pheromone
