class_name State
extends Node

signal entered
signal exited

var state_machine = null

func enter(data: Dictionary = {}):
	entered.emit()


func exit():
	exited.emit()
