extends Camera2D

var fast_zoom_multiplier = 4
var zoom_factor = 1
var zoom_margin = 0.3

var _max_zoom: Vector2
var _min_zoom: Vector2
var _zoom_step = Vector2(0.1, 0.1)
var _zoom_ratio: float
#var _zoom_step = 1.1
var _attributes_set: bool
var _position_limit_left: float
var _position_limit_right: float
var _position_limit_top: float
var _position_limit_bottom: float
@export var _tile_map: TileMap
@export var _subviewport: SubViewport

var _camera_speed = 10

# Called when the node enters the scene tree for the first time.
func _ready():
	_attributes_set = false
#
# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	if not _attributes_set:
		set_attributes()
		_attributes_set = true
		
	var fast_zoom = 1
	var position_change = Vector2.ZERO
	
	if Input.is_action_pressed("up"):
		position_change -= Vector2(0, 1) * _camera_speed
		
	if Input.is_action_pressed("down"):
		position_change += Vector2(0, 1) * _camera_speed
		
	if Input.is_action_pressed("left"):
		position_change -= Vector2(1, 0) * _camera_speed
		
	if Input.is_action_pressed("right"):
		position_change += Vector2(1, 0) * _camera_speed
	
	var new_position = position + position_change
	position = new_position
	if new_position.x < _position_limit_left:
		position.x = _position_limit_left
	if new_position.x > _position_limit_right:
		position.x = _position_limit_right
	if new_position.y < _position_limit_top:
		position.y = _position_limit_top
	if new_position.y > _position_limit_bottom:
		position.y = _position_limit_bottom


func _input(event):
	if event is InputEventMouseButton:
		if event.is_pressed() and not event.is_echo():
			var mouse_position = event.position
			#if zoom.x < _max_zoom.x:
			if event.button_index == MOUSE_BUTTON_WHEEL_UP:
				zoom = clamp(zoom + _zoom_step, _min_zoom, _max_zoom)
				update_position_limits()
				#print(zoom)
				#_zoom_at_point(_zoom_step,mouse_position)
			else : if event.button_index == MOUSE_BUTTON_WHEEL_DOWN:
				zoom = clamp(zoom - _zoom_step, _min_zoom, _max_zoom)
				update_position_limits()
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
		print(zoom)
		#global_position = c1


func _on_vertical_view_mouse_entered():
	set_process(true)


func _on_vertical_view_mouse_exited():
	set_process(false)

func set_attributes():
	set_process(true)
	var bounds = _tile_map.get_bounds()
	var viewport_width = _subviewport.size.x
	var viewport_halfwidth = viewport_width / 2
	var viewport_halfheight = _subviewport.size.y / 2
	limit_left = bounds[0]
	limit_right = bounds[1]
	limit_top = bounds[2] - 800
	limit_bottom = bounds[3] + 1000

	var max_zoom = 4
	_max_zoom = Vector2(max_zoom, max_zoom)
	_zoom_ratio =  float(viewport_width) / float(bounds[1] - bounds[0])
	#min_zoom = 0.846
	_min_zoom = Vector2(_zoom_ratio, _zoom_ratio)
	zoom = _min_zoom
	
	_position_limit_left = limit_left + viewport_halfwidth
	_position_limit_right = limit_right - viewport_halfwidth
	_position_limit_top = limit_top + viewport_halfheight
	_position_limit_bottom = limit_bottom - viewport_halfheight
	

func update_position_limits():
	pass
	#_position_limit_left /= zoom.x
	#_position_limit_right /= zoom.x
	#_position_limit_top /= zoom.x
	#_position_limit_bottom /= zoom.x
