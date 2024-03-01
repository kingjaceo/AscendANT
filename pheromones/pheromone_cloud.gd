class_name PheromoneCloud
extends Node2D

var location: Vector2i
var pheromone: Pheromone

var top_tween: Tween


func start():
	modulate = pheromone.color
	var tween = get_tree().create_tween()
	top_tween = tween
	tween.tween_property(self, "modulate", Color(pheromone.color, 0), pheromone.lifetime).set_trans(Tween.TRANS_SINE)
	await tween.finished
	get_parent().remove(location, self)
	queue_free()


#func finish():
	##if _tween:
	#_tween.tween_property(self, "modulate", Color(color, 0), 0.5).set_trans(Tween.TRANS_SINE)
	##get_parent().remove(location, self)
	#queue_free()


#func reset(time: float) -> void:
	#_tween.kill()
	#modulate = color
	#start()
	#_tween = get_tree().create_tween()
	#_tween.tween_property(self, "modulate", Color(1, 1, 1, 0), time).set_trans(Tween.TRANS_SINE)
	#_tween.finished.connect(_finish)
