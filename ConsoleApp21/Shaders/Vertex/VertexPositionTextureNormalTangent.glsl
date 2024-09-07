#version 410 core
layout (location = 0) in vec3 vertexPosition;
layout (location = 1) in vec2 vertexTexCoords;
layout (location = 2) in vec3 vertexNormal;
layout (location = 3) in vec3 vertexTangent;
layout (location = 4) in vec3 vertexBitangent;

out vec3 Position;
out vec2 TexCoords;
out vec3 Normal;
out mat3 TangentSpaceMatrix;
out vec4 PositionLightSpace;

uniform mat4 model;
uniform mat4 view;
uniform mat4 proj;
uniform mat4 lightSpaceMatrix;

void main()
{
	gl_Position = ((proj * view) * model) * vec4(vertexPosition, 1);
	Position = vec3(model * vec4(vertexPosition, 1));
	TexCoords = vertexTexCoords;
	
	mat3 normalMatrix = mat3(transpose(inverse(model)));
	
	Normal = normalize(normalMatrix * vertexNormal);
	vec3 tangent = normalize(normalMatrix * vertexTangent);
	vec3 bitangent = normalize(normalMatrix * vertexBitangent); // cross(tangent, Normal);
	//vec3 normal = normalize(vertexNormal);
    //vec3 tangent = normalize(vertexNormal);

	if (dot(cross(Normal, tangent), bitangent) < 0.0)
	{
		tangent = -tangent;
	}
	
	if (dot(cross(Normal, bitangent), tangent) > 0.0)
	{
		bitangent = -bitangent;
	}

	TangentSpaceMatrix = mat3(tangent, bitangent, Normal);

	PositionLightSpace = lightSpaceMatrix * vec4(Position, 1);
}