#version 450 core

in vec2 UV;
out vec3 color;
flat in int textInd;
in vec4 norm;
uniform sampler2D images[32];

void main()
{
	vec2 flip_textCoord = vec2(UV.x, 1 - UV.y);
	if (textInd == 0) {
		color.rgb = texture(images[0], flip_textCoord).rgb;
	}
	else {
		color.rgb = vec3(0.0,1.0,0.0);
	}
}