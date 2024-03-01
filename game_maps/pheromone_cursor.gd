class_name PheromoneCursorModule
extends MapModule

@onready var _particles = $PheromoneTrail
@onready var _pheromones = %Pheromones

var _last_clicked_cell: Vector2i
var _active: bool = false
var _current_pheromone_color = Color.HOT_PINK

var current_pheromone: Pheromone
const EXPLORED = preload("res://pheromones/explored.tres")
const TO_FOOD = preload("res://pheromones/to_food.tres")

func _setup():
	current_pheromone = TO_FOOD
	_particles.amount_ratio = 0
	set_process(false)


func _process(delta):
	_particles.amount_ratio = 1
	var mouse_position = game_map.get_global_mouse_position()
	var mouse_cell = game_map.local_to_map(mouse_position)
	_particles.position = mouse_position
	if Input.is_action_just_pressed("primary_action"):
		_pheromones.add_at_location(current_pheromone, mouse_position)
		_last_clicked_cell = mouse_cell
	if Input.is_action_pressed("primary_action") and _last_clicked_cell != mouse_cell:
		_pheromones.add_at_location(current_pheromone, mouse_position)
		_last_clicked_cell = mouse_cell


func _input(event):
	if event.is_action_pressed("pheromone_mode"):
		_active = not _active
	
	set_process(_active)
