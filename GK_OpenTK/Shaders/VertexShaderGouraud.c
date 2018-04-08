#version 450 core

in vec3 position_modelspace;
//in vec3 color;
in vec2 textCoord;
in vec3 normal_modelspace;

layout(location = 20) uniform  mat4 model;
layout(location = 21) uniform  mat4 view;
layout(location = 22) uniform  mat4 projection;
layout(location = 23) uniform  vec3 LightPosition_worldspace;

out vec2 UV;
out vec3 Position_worldspace;
out vec3 Normal_cameraspace;
out vec3 EyeDirection_cameraspace;
out vec3 LightDirection_cameraspace;
out vec4 Color;         // This will be passed into the fragment shader.
out float dif;

uniform sampler2D tex;							  
void main()
{
	mat4 u_MVMatrix = view*model;
	// Transform the vertex into eye space.
	vec3 modelViewVertex = vec3(u_MVMatrix * vec4(position_modelspace, 1.0));

	// Transform the normal's orientation into eye space.
	vec3 modelViewNormal = vec3(u_MVMatrix * vec4(normal_modelspace, 0.0));


	// Vector that goes from the vertex to the light, in camera space. M is ommited because it's identity.
	vec3 u_LightPos = (view*vec4(LightPosition_worldspace, 1.0)).xyz;

	// Will be used for attenuation.
	float distance = length(u_LightPos - modelViewVertex);

	// Get a lighting direction vector from the light to the vertex.
	vec3 lightVector = normalize(u_LightPos - modelViewVertex);

	// Calculate the dot product of the light vector and vertex normal. If the normal and light vector are
	// pointing in the same direction then it will get max illumination.
	float diffuse = max(dot(modelViewNormal, lightVector), 0.1);

	// Attenuate the light based on distance.
	diffuse = diffuse * (1.0 / (1.0 + (0.25 * distance * distance)));

	//vec2 flip_textCoord = vec2(UV.x,1-UV.y);
	//vec4 color = texture(tex, flip_textCoord);
	// Multiply the color by the illumination level. It will be interpolated across the triangle.
	//Color = color * diffuse;

	UV = textCoord;
	dif = diffuse;
	// gl_Position is a special variable used to store the final position.
	// Multiply the vertex by the matrix to get the final point in normalized screen coordinates.
	gl_Position = projection * view *model * vec4(position_modelspace,1.0);
}