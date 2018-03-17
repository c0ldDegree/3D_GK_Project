//#version 450 core
//
//layout(location = 20) uniform  mat4 model;
//layout(location = 21) uniform  mat4 view;//camera
//layout(location = 22) uniform  mat4 projection;
//layout(location = 23) uniform  vec3 LightPosition_worldspace;
//
//in vec3 position_modelspace;//vert
//in vec3 color;
//in vec2 textCoord;//vertTexCoord
//in vec3 normal_modelspace;//vertNormal
//
//out vec3 fragVert;
//out vec2 fragTexCoord;
//out vec3 fragNormal;
//
//void main() {
//	// Pass some variables to the fragment shader
//	fragTexCoord = textCoord;
//	fragNormal = normal_modelspace;
//	fragVert = position_modelspace;
//
//	// Apply all matrix transformations to vert
//	gl_Position = projection*view* model * vec4(position_modelspace, 1.0);
//}
#version 450 core

in vec3 position_modelspace;
in vec3 color;
in vec2 textCoord;
in vec3 normal_modelspace;

layout(location = 20) uniform  mat4 model;
layout(location = 21) uniform  mat4 view;
layout(location = 22) uniform  mat4 projection;


out vec2 UV;
out vec3 Position_worldspace;
out vec3 Normal_cameraspace;
out vec3 EyeDirection_cameraspace;


void main()
{
	// Output position of the vertex, in clip space : MVP * position
	gl_Position = projection * view * model * vec4(position_modelspace, 1.0);

	// Position of the vertex, in worldspace : M * position
	Position_worldspace = (model * vec4(position_modelspace, 1)).xyz;

	// Vector that goes from the vertex to the camera, in camera space.
	// In camera space, the camera is at the origin (0,0,0).
	vec3 vertexPosition_cameraspace = (view * model * vec4(position_modelspace, 1)).xyz;
	EyeDirection_cameraspace = vec3(0, 0, 0) - vertexPosition_cameraspace;

	// Vector that goes from the vertex to the light, in camera space. M is ommited because it's identity.
	//vec3 LightPosition_cameraspace = (view * vec4(LightPosition_worldspace, 1)).xyz;
	//LightDirection_cameraspace = LightPosition_cameraspace + EyeDirection_cameraspace;

	// Normal of the the vertex, in camera space
	Normal_cameraspace = (view * model * vec4(normal_modelspace, 0)).xyz; // Only correct if ModelMatrix does not scale the model ! Use its inverse transpose if not.

																		  // UV of the vertex. No special space for this one.
	UV = textCoord;
}