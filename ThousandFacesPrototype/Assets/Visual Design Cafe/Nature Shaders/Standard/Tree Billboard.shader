Shader "Nature Shaders/Tree Billboard"
{
    Properties
    {
        // Transparency
        _Cutoff("Cutoff", Range(0, 1)) = 0.5

        // Color
        _HSL("Hue, Saturation, Lightness", Vector) = (0, 0, 0, 0)
        _HSLVariation("Hue, Saturation, Lightness Variation", Vector) = (0, 0, 0, 0)
        _Tint("Tint", Color) = (1, 1, 1, 1)
        _TintVariation("Tint Variation", Color) = (1, 1, 1, 1)
        _ColorVariationSpread("Color Variation Spread", Float) = 0.2

        // Surface
        [NoScaleOffset][MainTexture]_Albedo("Albedo", 2D) = "white" {}
        [NoScaleOffset]_BumpMap("Normal Map", 2D) = "bump" {}
        _BumpScale("Normal Map Strength", Range(0, 1)) = 1
        _Glossiness("Smoothness", Range(0, 1)) = 0.2

        // Wind
        _ObjectHeight("Grass Height", Float) = 0.5
        _ObjectRadius("Grass Radius", Float) = 0.5
        _WindVariation("Wind Variation", Range(0, 1)) = 0.3
        _WindStrength("Wind Strength", Range(0, 2)) = 1
        _TurbulenceStrength("Turbulence Strength", Range(0, 2)) = 1
        
        // Translucency
        _Translucency("Translucency", Range(0, 2)) = 1
        _TranslucencyDistortion("Translucency Distortion", Range(0, 1)) = 0.5
        _TranslucencyScattering("Translucency Scattering", Range(0, 3)) = 2
        _TranslucencyColor("Translucency Color", Color) = (0.7568, 0.7019, 0.2431, 1)

        // Keywords
        [KeywordEnum(Baked, UV, Vertex)]_WIND_CONTROL("Wind Control", Float) = 2
        //[ToggleOff] _SpecularHighlights("Specular Highlights", Float) = 1.0
        //[ToggleOff] _GlossyReflections("Glossy Reflections", Float) = 1.0
    }
    SubShader
    {
        Tags
		{ 
			"RenderType" = "TransparentCutout"  
			"Queue" = "AlphaTest+0" 
			"DisableBatching" = "True"
			"NatureRendererInstancing" = "True"
		}
        LOD 200

        // Render State
        Blend One Zero, One Zero
        Cull Back
        ZTest LEqual
        ZWrite On
        // ColorMask: <None>

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface NatureShaderSurface StandardCustom vertex:NatureShaderVertex addshadow fullforwardshadows nolightmap dithercrossfade nometa

        // Pragmas
        #pragma multi_compile_instancing
        #pragma instancing_options procedural:SetupNatureRenderer
        #pragma target 4.0
        #pragma multi_compile_fog
    
        // Keywords
        #pragma shader_feature_local _COLOR_HSL
        #define _BUMP_MAP_ON

        //#pragma shader_feature _SPECULARHIGHLIGHTS_OFF
        //#pragma shader_feature _GLOSSYREFLECTIONS_OFF

        // Defines
        #define NATURE_SHADERS
        #define STANDARD_RENDER_PIPELINE
        #define _TYPE_TREE_BILLBOARD
        #define TRANSLUCENT
        #define PROPERTY_Albedo
        #define PROPERTY_BumpMap
        #define PROPERTY_ObjectHeight
        #define PROPERTY_ObjectRadius
        #define PROPERTY_WindVariation
        #define PROPERTY_WindStrength
        #define PROPERTY_TurbulenceStrength
        #define PROPERTY_HSL
        #define PROPERTY_HSLVariation
        #define PROPERTY_BumpScale
        #define PROPERTY_Tint
        #define PROPERTY_TintVariation
        #define PROPERTY_Cutoff
        #define PROPERTY_TrunkBendFactor
        #define PROPERTY_Translucency
        #define PROPERTY_TranslucencyDistortion
        #define PROPERTY_TranslucencyScattering
        #define PROPERTY_TranslucencyColor
        #define PROPERTY_ColorVariationSpread

        // Includes
        #include "../Common/Surface Shader.cginc"
        ENDCG
    }

    Fallback Off
	CustomEditor "VisualDesignCafe.Nature.Editor.NatureMaterialEditor"
}
