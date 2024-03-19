extends Node

var pheromones_on: bool = false

func _input(event):
	if event.is_action_pressed("pheromone_mode"):
		pheromones_on = not pheromones_on
		var pheromone_effects = get_tree().get_nodes_in_group("pheromone_effects")
		for effect in pheromone_effects:
			effect.set_visible(pheromones_on)
			 
