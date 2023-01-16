#version 330 core
out vec4 outputColor;

in vec2 texCoord;
in vec3 normal;
in vec3 fragPos;

uniform float specularStrength;
uniform int shininess;

uniform vec4 colorMod;
uniform vec4 ambientLight;

uniform sampler2D texture0;

struct PointLight {
	vec3 pos;
	
	vec4 color;
	float distance;
};
#define POINT_LIGHT_COUNT 4
uniform PointLight pointLights[POINT_LIGHT_COUNT];


vec CalculatePointLight(PointLight light, vec3 normal, vec3 fragPos, vec3 viewDir);

void main()
{
    outputColor = vec4(0.0, 0.0, 0.0, 1.0);
	
	//for(int i = 0; i < POINT_LIGHT_COUNT; i++)
		//outputColor += CalculatePointLight(pointLights[i]);
		
	outputColor += ambientLight;
	outputColor *= colorMod * texture(texture0, texCoord);
}

vec3 CalculatePointLight(PointLight light)
{
	vec3 lightDir = normalize(light.pos - fragPos);
	float diff = max(dot(normal, lightDir), 0.0);
	vec3 diffuse = diff * light.color.xyz;
	vec3 reflectDir = reflectDir = reflect(-lightDir, normal);
	float spec = pow(max(dot(-fragPos, reflectDir), 0.0), shininess);
	light.color.w = 1;
	vec specular = specularStrength * spec * light.color;
	return specular;
}