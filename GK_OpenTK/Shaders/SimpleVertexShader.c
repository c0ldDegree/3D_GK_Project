#version 450 core

in vec3 position_modelspace;
in vec3 color;
in vec2 textCoord;
in vec3 normal_modelspace;
in int texture_index;


layout(location = 20) uniform  mat4 model;
layout(location = 21) uniform  mat4 view;
layout(location = 22) uniform  mat4 projection;
layout(location = 23) uniform  vec3 LightPosition_worldspace;

out vec2 UV;
out int textInd;
out vec3 norm;
void main()
{
	gl_Position = projection*view*model * vec4(position_modelspace, 1.0);
	textInd = texture_index;
	UV = textCoord;
}