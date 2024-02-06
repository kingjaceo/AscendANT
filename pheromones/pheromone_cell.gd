class_name PheromoneCell extends Resource

var coordinates: Vector2i
var pheromone_strengths = {}
var pheromone_directions = {}
var strongest_pheromone: Pheromone
var strongest_pheromone_percentage: float

func _init(coordinates: Vector2i):
	self.coordinates = coordinates
	

func add_pheromone(pheromone: Pheromone, from: Vector2i):
	pheromone_strengths[pheromone] = pheromone.lifetime
	pheromone_directions[pheromone] = from
	strongest_pheromone = pheromone
	strongest_pheromone_percentage = 100
	

func decay_pheromones(delta):
	for pheromone in pheromone_strengths:
		pheromone_strengths[pheromone] -= delta
		
		if pheromone_strengths[pheromone] <= 0:
			delete_pheromone(pheromone)
		
	_update_strongest_pheromone()	
	

func _update_strongest_pheromone():
	strongest_pheromone_percentage = 0
	
	for pheromone in pheromone_strengths:
		if pheromone.lifetime == INF:
			strongest_pheromone_percentage = 100
			strongest_pheromone = pheromone
			return
		
		var pheromone_percentage = pheromone_strengths[pheromone] / pheromone.lifetime * 100
		
		if pheromone_percentage > strongest_pheromone_percentage:
			strongest_pheromone_percentage = pheromone_percentage
			strongest_pheromone = pheromone
		

func delete_pheromone(pheromone):
	pheromone_strengths.erase(pheromone)
	pheromone_directions.erase(pheromone)
	if strongest_pheromone == pheromone:
		strongest_pheromone = null
		strongest_pheromone_percentage = 0
