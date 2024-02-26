extends Node2D

@onready var label = $Label
var camera: Camera2D

# Called when the node enters the scene tree for the first time.
func _ready():
	camera = get_parent()


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(_delta):
	label.text = ""
	for module in camera.camera_modules:
		label.text += module.get_debug_text() + "\n"
	queue_redraw()


func _draw():
	draw_circle(Vector2.ZERO, 5, Color.WHITE)
