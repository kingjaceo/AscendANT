class_name PheromoneCursorModule
extends MapModule

@onready var _particles = $GPUParticles2D
@onready var _pheromones = %Pheromones

var _active: bool = false
var _current_pheromone_color = Color.HOT_PINK

# Called when the node enters the scene tree for the first time.
func _ready():
	_particles.amount_ratio = 0
	set_process(false)


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	_particles.amount_ratio = 0.1
	var mouse_position = owner.get_global_mouse_position()
	_particles.position = mouse_position
	if Input.is_action_pressed("primary_action"):
		#_particles.process_material.spread = 180.0
		_particles.amount_ratio = 5
		_pheromones.mark_position(mouse_position, _current_pheromone_color)
	#if Input.is_action_just_released("primary_action"):
		#_particles.process_material.spread = 0



func _input(event):
	if event.is_action_pressed("pheromone_cursor"):
		_active = not _active
	
	set_process(_active)
