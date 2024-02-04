extends Node2D
@onready var colony_map = %ColonyMap
var _descendANT = preload("res://ants/baseANT.tscn")

# Called when the node enters the scene tree for the first time.
func _ready():
	for i in range(6):
		var instance = _descendANT.instantiate()
		add_child(instance)
		instance.position = Vector2(264, 264)
		instance._colony_map = colony_map
		#instance._pheremone_map = pheromone_map
		instance._current_cell = colony_map.local_to_map(instance.position)

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	pass
