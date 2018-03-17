//#version 450 core
//
//layout(location = 20) uniform  mat4 model;
//uniform vec3 cameraPosition;//camera
//
//uniform sampler2D materialTex;
//float materialShininess=5.0f;
//vec3 materialSpecularColor=vec3(0.3,0.3,0.3);
//
//#define MAX_LIGHTS 10
//uniform int numLights;
//uniform struct Light {
//	vec4 position;
//	vec3 intensities; //a.k.a the color of the light
//	float attenuation;
//	float ambientCoefficient;
//	float coneAngle;
//	vec3 coneDirection;
//} allLights[MAX_LIGHTS];
//
//in vec2 fragTexCoord;
//in vec3 fragNormal;
//in vec3 fragVert;
//
//out vec4 finalColor;
//
//vec3 ApplyLight(Light light, vec3 surfaceColor, vec3 normal, vec3 surfacePos, vec3 surfaceToCamera) {
//	vec3 surfaceToLight;
//	float attenuation = 1.0;
//	if (light.position.w == 0.0) {
//		//directional light
//		surfaceToLight = normalize(light.position.xyz);
//		attenuation = 1.0; //no attenuation for directional lights
//	}
//	else {
//		//point light
//		surfaceToLight = normalize(light.position.xyz - surfacePos);
//		float distanceToLight = length(light.position.xyz - surfacePos);
//		attenuation = 1.0 / (1.0 + light.attenuation * pow(distanceToLight, 2));
//
//		//cone restrictions (affects attenuation)
//		float lightToSurfaceAngle = degrees(acos(dot(-surfaceToLight, normalize(light.coneDirection))));
//		if (lightToSurfaceAngle > light.coneAngle) {
//			attenuation = 0.0;
//		}
//	}
//
//	//ambient
//	vec3 ambient = light.ambientCoefficient * surfaceColor.rgb * light.intensities;
//
//	//diffuse
//	float diffuseCoefficient = max(0.0, dot(normal, surfaceToLight));
//	vec3 diffuse = diffuseCoefficient * surfaceColor.rgb * light.intensities;
//
//	//specular
//	float specularCoefficient = 0.0;
//	if (diffuseCoefficient > 0.0)
//		specularCoefficient = pow(max(0.0, dot(surfaceToCamera, reflect(-surfaceToLight, normal))), materialShininess);
//	vec3 specular = specularCoefficient * materialSpecularColor * light.intensities;
//
//	//linear color (color before gamma correction)
//	return ambient + attenuation*(diffuse + specular);
//}
//
//void main() {
//	vec3 normal = normalize(transpose(inverse(mat3(model))) * fragNormal);
//	vec3 surfacePos = vec3(model * vec4(fragVert, 1));
//	vec2 flip_textCoord = vec2(fragTexCoord.x, 1 - fragTexCoord.y);
//	vec4 surfaceColor = texture(materialTex, flip_textCoord);
//	vec3 surfaceToCamera = normalize(cameraPosition - surfacePos);
//
//	//combine color from all the lights
//	vec3 linearColor = vec3(0);
//	for (int i = 0; i < numLights; ++i) {
//		linearColor += ApplyLight(allLights[i], surfaceColor.rgb, normal, surfacePos, surfaceToCamera);
//	}
//
//	//final color (after gamma correction)
//	vec3 gamma = vec3(1.0 / 2.2);
//	finalColor = vec4(pow(linearColor, gamma), surfaceColor.a);
//}
#version 450 core

// Interpolated values from the vertex shaders
in vec2 UV;
in vec3 Position_worldspace;
in vec3 Normal_cameraspace;
in vec3 EyeDirection_cameraspace;

// Ouput data
out vec3 color;

uniform sampler2D tex;
layout(location = 20) uniform  mat4 model;
layout(location = 21) uniform  mat4 view;
#define MAX_LIGHTS 10
uniform int numLights;
uniform struct Light {
	vec4 position;
	vec3 intensities; //a.k.a the color of the light
	float attenuation;
	float ambientCoefficient;
	float coneAngle;
	vec3 coneDirection;
} allLights[MAX_LIGHTS];



void main()
{
	//		vec2 flip_textCoord = vec2(TextCoord.x,1-TextCoord.y);
	//		outColor = texture(tex,flip_textCoord);

	// Light emission properties
	// You probably want to put them as uniforms
	vec3 LightColor = vec3(1, 1, 1);
	float LightPower = 10000.0f;

	// Material properties
	vec2 flip_textCoord = vec2(UV.x, 1 - UV.y);
	vec3 MaterialDiffuseColor = texture(tex, flip_textCoord).rgb;
	vec3 MaterialAmbientColor = vec3(0.1, 0.1, 0.1) * MaterialDiffuseColor;
	vec3 MaterialSpecularColor = vec3(0.3, 0.3, 0.3);

	// Normal of the computed fragment, in camera space
	vec3 n = normalize(Normal_cameraspace);
	// Eye vector (towards the camera)
	vec3 E = normalize(EyeDirection_cameraspace);
	color = vec3(0,0,0);
	for (int i = 0; i < numLights; i++) {
		// Distance to the light
		float distance = length(allLights[i].position.xyz - Position_worldspace);

		// Direction of the light (from the fragment to the light)
		vec3 LightPosition_cameraspace = (view * vec4(allLights[i].position.xyz, 1)).xyz;
		vec3 LightDirection_cameraspace = LightPosition_cameraspace + EyeDirection_cameraspace;
		vec3 l = normalize(LightDirection_cameraspace);
		// Cosine of the angle between the normal and the light direction, 
		// clamped above 0
		//  - light is at the vertical of the triangle -> 1
		//  - light is perpendicular to the triangle -> 0
		//  - light is behind the triangle -> 0
		float cosTheta = clamp(dot(n, l), 0, 1);

		// Direction in which the triangle reflects the light
		vec3 R = reflect(-l, n);
		// Cosine of the angle between the Eye vector and the Reflect vector,
		// clamped to 0
		//  - Looking into the reflection -> 1
		//  - Looking elsewhere -> < 1
		float cosAlpha = clamp(dot(E, R), 0, 1);

		color +=
			// Ambient : simulates indirect lighting
			MaterialAmbientColor +
			// Diffuse : "color" of the object
			MaterialDiffuseColor * LightColor * LightPower * cosTheta / (distance*distance) +
			// Specular : reflective highlight, like a mirror
			MaterialSpecularColor * LightColor * LightPower * pow(cosAlpha, 5) / (distance*distance);
	}

}