class_name PheromoneCloud
extends Node2D

var location: Vector2i
var pheromone: Pheromone
var index : int # position in stack of pheromeon 
#var top_tween: Tween

signal dispersed

func _ready():
	modulate = pheromone.color


func start():
	if pheromone.lifetime > 0:
		var tween = get_tree().create_tween()
		#top_tween = tween
		tween.tween_property(self, "modulate", Color(pheromone.color, 0), pheromone.lifetime).set_trans(Tween.TRANS_SINE)
		await tween.finished
		_disperse()


func _disperse():
	dispersed.emit(location, index)
	queue_free()
