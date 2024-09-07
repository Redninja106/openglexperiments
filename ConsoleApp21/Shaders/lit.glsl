#version 450 core

struct LightColor
{
	vec3 ambient;
	vec3 diffuse;
	vec3 specular;
};

struct DirectionalLight
{
	vec3 direction;
	LightColor color;
};

struct PointLight
{
	vec3 position;

	float constant;
	float linear;
	float quadratic;

	LightColor color;
};

struct SpotLight
{
	PointLight pointLight;
	vec3 direction;
	float innerCutoff;
	float outerCutoff;
};

struct Material 
{
	sampler2D diffuseMap0;
	sampler2D specularMap0;
	sampler2D emissionMap0;
	sampler2D normalMap0;
};

out vec4 FragColor;

in vec3 Position;
in vec2 TexCoords;
in vec3 Normal;
in mat3 TangentSpaceMatrix;
in vec4 PositionLightSpace;

uniform DirectionalLight directionalLight;

const int POINT_LIGHT_COUNT = 8;
uniform PointLight pointLights[POINT_LIGHT_COUNT];

const int SPOT_LIGHT_COUNT = 8;
uniform SpotLight spotLights[SPOT_LIGHT_COUNT];

uniform sampler2D depthMap;

uniform Material material;
uniform float time;
uniform vec2 uvScale;
uniform vec3 viewPos;
uniform bool renderNormals;
uniform float depthBias;
uniform bool normalMappingEnable;

vec3 directionalLightBrightness(DirectionalLight light, vec3 normal, vec3 matDiffuse, vec3 matSpecular);
vec3 pointLightBrightness(PointLight light, vec3 normal, vec3 matDiffuse, vec3 matSpecular);
vec3 spotLightBrightness(SpotLight light, vec3 normal, vec3 matDiffuse, vec3 matSpecular);
vec3 lightBrightness(LightColor color, vec3 lightDirection, vec3 normal, vec3 matDiffuse, vec3 matSpecular, float shadow = 0);
float calcShadow(vec4 fragPos);

void main()
{
	vec3 normal;
	
	if (!normalMappingEnable)
	{
		// use vertex normal
		normal = Normal;
	}
	else
	{
	    normal =  vec3(texture(material.normalMap0, TexCoords * uvScale));
		normal = (normal * 2) - vec3(1);
		normal = TangentSpaceMatrix * normal;
	}

	normal = normalize(normal);

	if (renderNormals)
	{
		FragColor = vec4(normal*.5+.5,1);
		return;
	}

	vec3 diffuse = vec3(texture(material.diffuseMap0, TexCoords * uvScale));
	vec3 specular = vec3(texture(material.specularMap0, TexCoords * uvScale));

	vec3 incomingLight = vec3(0);

	incomingLight += directionalLightBrightness(directionalLight, normal, diffuse, specular);

	for (int i = 0; i < POINT_LIGHT_COUNT; i++)
	{
		incomingLight += pointLightBrightness(pointLights[i], normal, diffuse, specular);
	}

	for (int i = 0; i < SPOT_LIGHT_COUNT; i++)
	{
		incomingLight += spotLightBrightness(spotLights[i], normal, diffuse, specular);
	}

	// vec3 emission = vec3(texture(material.emissionMap0, TexCoords * uvScale));
	// incomingLight += emission;
	FragColor = vec4(incomingLight, 1);
}

vec3 directionalLightBrightness(DirectionalLight light, vec3 normal, vec3 matDiffuse, vec3 matSpecular)
{
	vec3 lightDirection = normalize(-light.direction);

	return lightBrightness(light.color, lightDirection, normal, matDiffuse, matSpecular, calcShadow(PositionLightSpace));
}

vec3 pointLightBrightness(PointLight light, vec3 normal, vec3 matDiffuse, vec3 matSpecular)
{
	vec3 lightDirection = normalize(vec3(light.position) - Position);
	
	float dist = distance(light.position, Position);
	float invAtt = light.constant + light.linear * dist + light.quadratic * (dist * dist);
	float attenuation = invAtt == 0 ? 0 : (1 / invAtt);

	return attenuation * lightBrightness(light.color, lightDirection, normal, matDiffuse, matSpecular);
}

vec3 spotLightBrightness(SpotLight light, vec3 normal, vec3 matDiffuse, vec3 matSpecular)
{
	vec3 lightDirection = normalize(vec3(light.pointLight.position) - Position);
	
	float theta = dot(lightDirection, normalize(light.direction));
	float epsilon = light.innerCutoff - light.outerCutoff;
	float intensity = clamp((theta - light.outerCutoff) / epsilon, 0, 1);

	return pointLightBrightness(light.pointLight, normal, matDiffuse, matSpecular) * intensity;
}

vec3 lightBrightness(LightColor color, vec3 lightDirection, vec3 normal, vec3 matDiffuse, vec3 matSpecular, float shadow = 0)
{
	vec3 viewDirection = normalize(viewPos - Position);
	vec3 reflectDirection = reflect(-lightDirection, normal);

	vec3 halfwayDirection = normalize(viewDirection + lightDirection);
	float specularBrightness = pow(max(dot(normal, halfwayDirection), 0), 128);
	float diffuseBrightness = max(dot(normal, lightDirection), 0);
	
	float shadowScalar = (1 - shadow);

	vec3 ambient = color.ambient * matDiffuse;
	vec3 diffuse = color.diffuse * diffuseBrightness * matDiffuse;
	vec3 specular = color.specular * specularBrightness * matSpecular;
	return (ambient + (diffuse * shadowScalar) + (specular * shadowScalar));
}

float calcShadow(vec4 fragPos)
{
	vec3 projCoords = fragPos.xyz / fragPos.w;
	projCoords = projCoords * .5 + .5;

	float closestDepth = texture(depthMap, projCoords.xy).r;
	float currentDepth = projCoords.z;
	return (currentDepth - depthBias) > closestDepth ? 1 : 0;
}