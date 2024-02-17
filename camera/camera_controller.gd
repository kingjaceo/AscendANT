extends Camera2D

var fast_zoom_multiplier = 4
var zoom_factor = 1
var zoom_margin = 0.3

var _bounds
var _max_zoom: Vector2
var _min_zoom: Vector2
var _zoom_step = Vector2(0.05, 0.05)
var _zoom_ratio: float
@export var _game_map: GameMap
@export var _subviewport: SubViewport
@export var _start_zoom: Vector2

var _camera_speed = 5

var _follow: Node2D

# Called when the node enters the scene tree for the first time.
func _ready():
	_set_attributes()


func _process(_delta):
	var fast_zoom = 1
	var position_change = Vector2.ZERO
	
	if Input.is_action_pressed("up"):
		position_change -= Vector2(0, 1) * _camera_speed
		_follow = null
	if Input.is_action_pressed("down"):
		position_change += Vector2(0, 1) * _camera_speed
		_follow = null
	if Input.is_action_pressed("left"):
		position_change -= Vector2(1, 0) * _camera_speed
		_follow = null
	if Input.is_action_pressed("right"):
		position_change += Vector2(1, 0) * _camera_speed
		_follow = null
	
	_set_position(position_change)
	if _follow:
		position = _follow.position



func _input(event):
	if event is InputEventMouseButton:
		if event.is_pressed() and not event.is_echo():
			var mouse_position = event.position
			#if zoom.x < _max_zoom.x:
			if event.button_index == MOUSE_BUTTON_WHEEL_UP:
				zoom = clamp(zoom + _zoom_step, _min_zoom, _max_zoom)
				#set_position(Vector2.ZERO)
				#print(zoom)
				#_zoom_at_point(_zoom_step,mouse_position)
			else : if event.button_index == MOUSE_BUTTON_WHEEL_DOWN:
				zoom = clamp(zoom - _zoom_step, _min_zoom, _max_zoom)
				#set_position(Vector2.ZERO)
				#print(zoom, " ", _min_zoom, " ", _max_zoom)
					#_zoom_at_point(1/_zoom_step,mouse_position)
			#position = event.position

func _zoom_at_point(zoom_change, point):
		var c0 = global_position # camera position
		var v0 = get_viewport().size # vieport size
		var c1 # next camera position
		var z0 = zoom # current zoom value
		var z1 = z0 * zoom_change # next zoom value

		c1 = c0 + (-0.5*v0 + point)*(z0 - z1)
		zoom = clamp(z1, _min_zoom, _max_zoom)
		#global_position = c1


func _on_vertical_view_mouse_entered():
	set_process(true)


func _on_vertical_view_mouse_exited():
	set_process(false)


func _set_attributes():
	set_process(false)
	_bounds = _game_map.get_bounds()
	var viewport_width = _subviewport.size.x

	var max_zoom = 8
	_max_zoom = Vector2(max_zoom, max_zoom)
	_zoom_ratio =  float(viewport_width) / float(_bounds[1] - _bounds[0])
	_min_zoom = Vector2(_zoom_ratio, _zoom_ratio)
	
	limit_left = _bounds[0]
	limit_right = _bounds[1]
	limit_top = _bounds[2] - 800
	limit_bottom = _bounds[3] + 1000
	#position = Vector2((_bounds[1] - _bounds[0]) / 2, (_bounds[1] - _bounds[0]) / 2)
	zoom = _start_zoom
	
	
func _set_position(position_change: Vector2):
	var new_position = position + position_change
	var visible_half_width = _subviewport.size.x / (2 * zoom.x)
	var min_x = _bounds[0] + visible_half_width
	var max_x = _bounds[1] - visible_half_width

	var visible_half_height = _subviewport.size.y / (2 * zoom.y)
	var min_y = _bounds[2] + visible_half_height - 800
	var max_y = _bounds[3] - visible_half_height + 1000
	var y = zoom.y*_subviewport.size.y/2
	position.x = clamp(new_position.x, min_x, max_x)
	position.y = clamp(new_position.y, min_y, max_y)

func follow(node: Node2D):
	_follow = node
