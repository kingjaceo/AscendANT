extends Node2D

var fast_zoom_multiplier = 4
var zoom_amount = 0.1
var zoom_max = 10
var zoom_min = 0.25
var zoom_factor = 1
var zoom_margin = 0.3

var _camera_speed = 10
@export var _camera: Camera2D

# Called when the node enters the scene tree for the first time.
func _ready():
	pass # Replace with function body.

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	var fast_zoom = 1
	
	if Input.is_action_pressed("up"):
		_camera.position -= Vector2(0, 1) * _camera_speed
		
	if Input.is_action_pressed("down"):
		_camera.position += Vector2(0, 1) * _camera_speed
		
	if Input.is_action_pressed("left"):
		_camera.position -= Vector2(1, 0) * _camera_speed
		
	if Input.is_action_pressed("right"):
		_camera.position += Vector2(1, 0) * _camera_speed
		
	if Input.is_action_pressed("scroll up"):
		print("scrolling up")
		_camera.zoom.x = min(_camera.zoom.x + zoom_amount, zoom_max)
		_camera.zoom.y = min(_camera.zoom.y + zoom_amount, zoom_max)
		
	if Input.is_action_pressed("scroll down"):	
		print("scrolling down")	
		_camera.zoom.x = max(_camera.zoom.x - zoom_amount, zoom_min)
		_camera.zoom.y = max(_camera.zoom.y - zoom_amount, zoom_min)
#	if Input.is_action_pressed("fast zoom"):
#		fast_zoom = fast_zoom_multiplier

func _input(event):

	if event is InputEventMouseButton:
		print("input pressed")
		if event.button_index == MOUSE_BUTTON_WHEEL_UP:
			_camera.zoom.x = min(_camera.zoom.x + zoom_amount, zoom_max)
			_camera.zoom.y = min(_camera.zoom.y + zoom_amount, zoom_max)
			print("camera zoomed up:")
			print(_camera.zoom)
		if event.button_index == MOUSE_BUTTON_WHEEL_DOWN and event.pressed:
			_camera.zoom.x = max(_camera.zoom.x - zoom_amount, zoom_min)
			_camera.zoom.y = max(_camera.zoom.y - zoom_amount, zoom_min)
			print("camera zoomed down:")
			print(_camera.zoom)
			
	
