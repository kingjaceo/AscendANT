extends Node2D

@export var ant = preload("res://ants/ant.tscn")
@export var _view_toggles: Control

# Called when the node enters the scene tree for the first time.
func _ready():
	pass
	#_view_toggles.get_node("Pheromones/PheromoneToggle")._messenger = _messenger
	#_view_toggles.get_node("Outline/OutlineToggle")._messenger = _messenger
	#_view_toggles.get_node("Terrain/TerrainToggle")._messenger = _messenger
		
# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	pass
