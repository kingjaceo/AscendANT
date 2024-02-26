extends Node2D

@onready var label = $Label
var camera: Camera2D

# Called when the node enters the scene tree for the first time.
func _ready():
	camera = get_parent()


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(_delta):
	label.text = ""
	label.text += "Zoom: " + str(camera.zoom) + "\n"
	label.text += "Position: " + str(camera._position_change) + "\n"
	label.text += "Move Direction: " + str(camera._move_direction) + "\n"
	queue_redraw()


func _draw():
	draw_circle(Vector2.ZERO, 5, Color.WHITE)
