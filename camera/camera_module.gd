class_name CameraModule
extends Node

var camera: Camera2D


func _ready():
	camera = owner as Camera2D
	_setup()


func _setup():
	pass


func get_debug_text() -> String:
	return ""
