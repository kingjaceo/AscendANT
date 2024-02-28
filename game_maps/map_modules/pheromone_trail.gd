class_name PheromoneTrail
extends GPUParticles2D

var _active: bool = false

func _setup():
	amount_ratio = 0
	set_process(false)


func _process(_delta):
	amount_ratio = 1
	#position = owner.position


func _input(event):
	if event.is_action_pressed("pheromone_mode"):
		_active = not _active
	
	if not _active:
		amount_ratio = 0
	set_process(_active)
