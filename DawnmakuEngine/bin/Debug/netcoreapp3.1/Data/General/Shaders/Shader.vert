#version 330 core
in vec3 position;
in vec2 texCoordAttrib;
in vec3 normalAttrib;

out vec2 texCoord;
out vec3 normal;
out vec3 fragPos;

uniform mat4 model;
uniform mat3 normalMatrix;
uniform mat4 view;
uniform mat4 projection;

void main()
{
    gl_Position = vec4(position, 1.0) * model * view * projection;
	//normal = normalAttrib * (normalMatrix * mat3(view));
	fragPos = vec3(model * vec4(position, 1.0));
	texCoord = texCoordAttrib;
}
