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

uniform mat4 model;
uniform mat4 view;
uniform mat4 proj;

void main()
{
	gl_Position = proj * view * model * vec4(vertexPosition, 1);
	Position = vec3(model * vec4(vertexPosition, 1));
	TexCoords = vertexTexCoords;
	
	mat3 normalMatrix = mat3(transpose(inverse(model)));
	
	Normal = normalize(vec3(model * vec4(vertexNormal, 0)));
	vec3 tangent = normalize(vec3(model * vec4(vertexTangent, 0)));
	vec3 bitangent = normalize(vec3(model * vec4(vertexBitangent, 0))); // cross(tangent, Normal);
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
}