﻿#version 330 core
out vec4 outputColor;

in vec2 texCoord;
uniform vec4 colorMod;


uniform sampler2D texture0;

void main()
{
    outputColor = colorMod * texture(texture0, texCoord);
}