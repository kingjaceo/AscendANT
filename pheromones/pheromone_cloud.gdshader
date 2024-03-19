shader_type canvas_item;
 
uniform sampler2D noise_texture: repeat_enable, filter_nearest;
uniform sampler2D curve;
uniform float fog_density: hint_range(0.0, 1.0) = 0.25;
uniform vec2 fog_speed = vec2(0.3, 0.3);
uniform float radius = 1;
//uniform vec2 center = vec2()
void fragment() {
	float distance_from_center = distance(UV.xy, vec2(0.5, 0.5));
	float strength = 1.0 - (distance_from_center - radius) / radius;
	//if (distance(UV.xy, vec2(0.5, 0.5)) < radius) {
	vec2 uv = UV + fog_speed * TIME;
	float noise = texture(noise_texture, uv).r;
	// convert noise from (0, 1) range to (-1, 1) range
	float fog = clamp(noise * 2.0 - 1.0, 0.0, 1.0);
	// fog affect
	COLOR.a *= fog * strength;
	//} else {
		//COLOR.a = 0.0;
	//}
}
