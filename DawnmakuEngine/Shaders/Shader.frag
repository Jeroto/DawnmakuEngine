#version 330 core
out vec4 outputColor;

in vec2 texCoord;
in vec4 colorMod;


uniform sampler2D texture0;
uniform sampler2D texture1;

void main()
{
    outputColor = colorMod * mix(texture(texture0, texCoord), texture(texture1, texCoord), 0.5);
}