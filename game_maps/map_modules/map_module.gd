class_name MapModule
extends Node

var game_map: GameMap
@export var layer: int


func _ready():
	game_map = owner as GameMap
	_setup()


func _setup():
	pass


func _draw():
	pass
