# CustomLightmapping

By attaching this component to any object, you can customize its behavior during light mapping.

## How to use

You can use it just by attaching this component to any object in the scene. Some settings cannot be changed from Unity's UI, so you need to use this component.

## Property

|Name|Description|
|-|-|
|Falloff Type|The type of light attenuation. InverseSquared is a darker but physically correct attenuation than the default. InverseSquaredNoRangeAttenuation is also physically correct but ignores the range parameter. Linear is a non-physically correct linear attenuation, Legacy is the quadratic attenuation that Unity uses by default.|
|Intensity Multiplier|Multiplier for the light intensity. Does not affect directional lights. This option applies to non-directional lights, as it is intended to be used to adjust darkened scenes by changing the type of attenuation.|
|Range Multiplier|Multiplier for the light range. Does not affect directional lights. This option applies to non-directional lights, as it is intended to be used to adjust darkened scenes by changing the type of attenuation.|

