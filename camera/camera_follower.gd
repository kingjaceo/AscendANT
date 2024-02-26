class_name CameraFollower
extends CameraModule

@export var mover: CameraMover
var _follow: Node2D


func _setup():
	mover.input_pressed.connect(_unfollow)
	set_process(false)


func _process(delta):
	camera.position = camera.position.lerp(_follow.position, delta)


func follow(node: Node2D):
	set_process(true)
	node.died.connect(_unfollow)
	_follow = node


func _unfollow() -> void:
	set_process(false)
	if _follow:
		_follow.died.disconnect(_unfollow)
	_follow = null


func get_debug_text() -> String:
	var text = ""
	text += "Following: " + str(_follow) + "\n"
	if _follow:
		text += "Follow: " + str(snapped(_follow.position,  Vector2(0.1, 0.1)))
	return text
