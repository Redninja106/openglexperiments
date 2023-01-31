#version 410 core

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
	sampler2D diffuse;
	sampler2D specular;
	sampler2D emission;
	float shininess;
};

out vec4 FragColor;

in vec3 Position;
in vec3 Normal;
in vec2 TexCoords;

uniform DirectionalLight directionalLight;

const int POINT_LIGHT_COUNT = 8;
uniform PointLight pointLights[POINT_LIGHT_COUNT];

const int SPOT_LIGHT_COUNT = 8;
uniform SpotLight spotLights[SPOT_LIGHT_COUNT];

uniform Material material;
uniform float time;

uniform vec3 viewPos;

vec3 directionalLightBrightness(DirectionalLight light, vec3 normal, vec3 matDiffuse, vec3 matSpecular);
vec3 pointLightBrightness(PointLight light, vec3 normal, vec3 matDiffuse, vec3 matSpecular);
vec3 spotLightBrightness(SpotLight light, vec3 normal, vec3 matDiffuse, vec3 matSpecular);
vec3 lightBrightness(LightColor color, vec3 lightDirection, vec3 normal, vec3 matDiffuse, vec3 matSpecular);

void main()
{
	vec3 color = vec3(texture(material.diffuse, TexCoords));

//	// ambient
//	vec3 ambient = light.ambient * color;
//	
//	// diffuse
//	vec3 lightDirection = normalize(vec3(light.vector) - Position);
//
//	vec3 normal = normalize(Normal);
//	float diffuseBrightness = max(dot(normal, lightDirection), 0);
//	vec3 diffuse = light.diffuse * color * diffuseBrightness;
//	// specular
//	vec3 viewDirection = normalize(viewPos - Position);
//	vec3 reflectDirection = reflect(-lightDirection, normal);
//
//	vec3 specularColor = vec3(texture(material.specular, TexCoords));
//	float specularBrightness = pow(max(dot(viewDirection, reflectDirection), 0), material.shininess * 128);
//	vec3 specular = light.specular * (specularBrightness * specularColor);
//	
	vec3 normal = normalize(Normal);
	vec3 diffuse = vec3(texture(material.diffuse, TexCoords));
	vec3 specular = vec3(texture(material.specular, TexCoords));

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
	// emission
	vec3 emission = vec3(texture(material.emission, TexCoords));
	incomingLight += emission;
	FragColor = vec4(incomingLight, 1);
}

vec3 directionalLightBrightness(DirectionalLight light, vec3 normal, vec3 matDiffuse, vec3 matSpecular)
{
	vec3 lightDirection = normalize(-light.direction);

	return lightBrightness(light.color, lightDirection, normal, matDiffuse, matSpecular);
}

vec3 pointLightBrightness(PointLight light, vec3 normal, vec3 matDiffuse, vec3 matSpecular)
{
	vec3 lightDirection = normalize(vec3(light.position) - Position);
	
	float dist = distance(light.position, Position);
	float invAtt = (light.constant + light.linear * dist + light.quadratic * (dist * dist));
	float attenuation = invAtt == 0 ? 0 : 1 / invAtt;

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

vec3 lightBrightness(LightColor color, vec3 lightDirection, vec3 normal, vec3 matDiffuse, vec3 matSpecular)
{
	vec3 viewDirection = normalize(viewPos - Position);
	vec3 reflectDirection = reflect(-lightDirection, normal);

	float specularBrightness = pow(max(dot(viewDirection, reflectDirection), 0), material.shininess * 128);
	float diffuseBrightness = max(dot(normal, lightDirection), 0);
	
	vec3 ambient = color.ambient * matDiffuse;
	vec3 diffuse = color.diffuse * diffuseBrightness * matDiffuse;
	vec3 specular = color.specular * specularBrightness * matSpecular;
	return (ambient + diffuse + specular);
}