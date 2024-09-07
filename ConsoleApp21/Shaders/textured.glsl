#version 410 core
out vec4 FragColor;

in vec2 texCoord;

struct Material
{
    sampler2D diffuseMap0;
};

uniform Material material;

void main()
{
    FragColor = vec4(texture(material.diffuseMap0, texCoord).rrr, 1);
} 