#version 330 core
in vec3 aPosition;
in vec2 aTexCoord;
uniform vec4 colorModInput;

out vec2 texCoord;
out vec4 colorMod;

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;

void main()
{
    gl_Position = vec4(aPosition, 1.0) * model * view * projection;
    texCoord = aTexCoord;
    colorMod = colorModInput;
}
