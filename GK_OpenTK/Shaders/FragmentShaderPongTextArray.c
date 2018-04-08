#version 450 core
#extension GL_EXT_texture_array : enable

smooth in vec3 UV;
out vec4 color;
in vec4 norm;
uniform sampler2DArray images;

void main()
{
	vec3 tcoord;
	vec2 flip_textCoord = vec2(UV.s, 1 - UV.t);
	tcoord.st = flip_textCoord.st;
	tcoord.p = UV.p;
	color = texture2DArray(images, tcoord);
}