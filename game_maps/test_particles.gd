class_name ParticleCloud
extends GPUParticles2D


var _tween: Tween


func _ready():
	_tween = get_tree().create_tween()
	_tween.tween_property(self, "modulate", Color(1, 1, 1, 0), 10).set_trans(Tween.TRANS_SINE)
