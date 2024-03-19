class_name ShaderCloud
extends PheromoneCloud


# Called when the node enters the scene tree for the first time.
func _ready():
	visible = GameController.pheromones_on
	modulate = pheromone.color
