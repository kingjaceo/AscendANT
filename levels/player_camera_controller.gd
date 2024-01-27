extends Camera2D

var fast_zoom_multiplier = 4
var zoom_amount = 0.1
var zoom_max = 10
var zoom_min = 0.25
var zoom_factor = 1
var zoom_margin = 0.3

var _camera_speed = 10

# Called when the node enters the scene tree for the first time.
func _ready():
	pass # Replace with function body.

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	var fast_zoom = 1
	
	if Input.is_action_pressed("up"):
		position -= Vector2(0, 1) * _camera_speed
		
	if Input.is_action_pressed("down"):
		position += Vector2(0, 1) * _camera_speed
		
	if Input.is_action_pressed("left"):
		position -= Vector2(1, 0) * _camera_speed
		
	if Input.is_action_pressed("right"):
		position += Vector2(1, 0) * _camera_speed
		
#	if Input.is_action_pressed("fast zoom"):
#		fast_zoom = fast_zoom_multiplier

func _input(event):
	if event is InputEventMouseButton:
		if event.button_index == MOUSE_BUTTON_WHEEL_UP:
			zoom.x = min(zoom.x + zoom_amount, zoom_max)
			zoom.y = min(zoom.y + zoom_amount, zoom_max)
		if event.button_index == MOUSE_BUTTON_WHEEL_DOWN and event.pressed:
			zoom.x = max(zoom.x - zoom_amount, zoom_min)
			zoom.y = max(zoom.y - zoom_amount, zoom_min)
