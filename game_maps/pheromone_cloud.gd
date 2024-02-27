class_name PheromoneCloud
extends GPUParticles2D

@export var location: Vector2i
@export var pheromone_lifetime: float

var _tween: Tween


func start():
	_tween = get_tree().create_tween()
	_tween.tween_property(self, "modulate", Color(1, 1, 1, 0), pheromone_lifetime).set_trans(Tween.TRANS_SINE)
	_tween.finished.connect(_finish)


func _finish():
	get_parent().remove(location, self)
	queue_free()
