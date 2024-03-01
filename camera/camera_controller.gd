class_name CameraController
extends Camera2D

var _bounds

var _lower_bound: Vector2
var _upper_bound: Vector2

@export var _game_map: GameMap
@export var _viewport_container: SubViewportContainer
@onready var camera_modules = %Modules

var follow: Callable



func _ready() -> void:
	camera_modules = camera_modules.get_children()
	for module in camera_modules:
		if module is CameraFollower:
			follow = module.follow
	_set_attributes()
	_viewport_container.mouse_entered.connect(_on_mouse_entered)
	_viewport_container.mouse_exited.connect(_on_mouse_exited)
	#zoom = _start_zoom
	set_process_input(false)


func _on_mouse_entered():
	set_process_input(true)


func _on_mouse_exited():
	set_process_input(false)


func _set_attributes():
	_bounds = _game_map.get_bounds()
	var viewport = get_viewport()
	
	var visible_half_width = viewport.size.x / (2 * zoom.x)
	var min_x = _bounds[0] + visible_half_width
	var max_x = _bounds[1] - visible_half_width
	
	var visible_half_height = viewport.size.y / (2 * zoom.y)
	var min_y = _bounds[2] + visible_half_height - 800
	var max_y = _bounds[3] - visible_half_height + 1000
	
	_lower_bound = Vector2(min_x, min_y)
	_upper_bound = Vector2(max_x, max_y)
