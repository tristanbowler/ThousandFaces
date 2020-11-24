Shader "Nature Shaders/Tree Bark"
{
    Properties
    {
        _HSL("Hue, Saturation, Lightness", Vector) = (0, 0, 0, 0)
        _HSLVariation("Hue, Saturation, Lightness Variation", Vector) = (0, 0, 0, 0)
        _Tint("Tint", Color) = (1, 1, 1, 1)
        _TintVariation("Tint Variation", Color) = (1, 1, 1, 1)
        _ColorVariationSpread("Color Variation Spread", Float) = 0.2
        
        [NoScaleOffset]_MainTex("Main Texture", 2D) = "white" {}
        [NoScaleOffset]_Albedo("Albedo", 2D) = "white" {}
        [NoScaleOffset]_BumpMap("Normal Map", 2D) = "bump" {}
        [NoScaleOffset]_MaskMap("Mask Map", 2D) = "white" {}
        [NoScaleOffset]_MetallicGlossMap("Metallic Gloss Map", 2D) = "black" {}
        [NoScaleOffset]_OcclusionMap("Occlusion Map", 2D) = "white" {}
        _ObjectHeight("Grass Height", Float) = 0.5
        _ObjectRadius("Tree Radius", Float) = 0.5
        _VertexNormalStrength("Vertex Normal Strength", Range(0, 1)) = 1
        _TrunkBendFactor("Trunk Bending", Vector) = (2, 0.3, 0, 0)
        _WindVariation("Wind Variation", Range(0, 1)) = 0.3
        _WindStrength("Wind Strength", Range(0, 2)) = 1
        _TurbulenceStrength("Turbulence Strength", Range(0, 2)) = 1
        _GlossRemap("Remap Smoothness", Vector) = (0, 1, 0, 0)
        _OcclusionRemap("Remap Occlusion", Vector) = (0, 1, 0, 0)
        _BumpScale("Normal Map Strength", Range(0, 1)) = 1
        _Glossiness("Smoothness", Range(0, 1)) = 0.2
        _Metallic("Metallic", Range(0, 1)) = 0

        // Emission
        _EmissionMap("Emission Map", 2D) = "black" {}
        [HDR]_EmissionColor("Emission Color", Color) = (1,1,1,1)

        [KeywordEnum(Baked, UV, Vertex)]_WIND_CONTROL("Wind Control", Float) = 2
        [KeywordEnum(Ambient, Gust, Turbulence, None)]_DEBUG("Debug Wind", Float) = 3
        //[ToggleOff] _SpecularHighlights("Specular Highlights", Float) = 1.0
        //[ToggleOff] _GlossyReflections("Glossy Reflections", Float) = 1.0
        [KeywordEnum(Off, MetallicGloss, Mask)]_SURFACE_MAP("Surface Map Method", Float) = 0
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
        //Cull Off
        ZTest LEqual
        ZWrite On
        // ColorMask: <None>

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface NatureShaderSurface Standard vertex:NatureShaderVertex addshadow fullforwardshadows nolightmap dithercrossfade
        
        // Pragmas
        #pragma multi_compile_instancing
        #pragma instancing_options procedural:SetupNatureRenderer
        #pragma target 4.0
        #pragma multi_compile_fog
    
        // Keywords
        // There are too many shader variants for the Surface Shader compiler.
        // So a few are disabled or set using a define.
        #pragma shader_feature_local _WIND_OFF _WIND_CONTROL_AUTO _WIND_CONTROL_BAKED
        #pragma shader_feature_local _COLOR_HSL
        #pragma shader_feature_local _SURFACE_MAP_OFF _SURFACE_MAP_MASK _SURFACE_MAP_METALLIC_GLOSS
        #pragma shader_feature_local _EMISSION

        // Bump map is enabled by default. Enable this pragma, or remove the define to disable bump mapping for materials that don't use a normal map.
        // #pragma shader_feature_local _BUMP_MAP_ON
        #define _BUMP_MAP_ON

        // Enable these pragmas to allow specular highlights to be enabled/disabled per material.
        // #pragma shader_feature _SPECULARHIGHLIGHTS_OFF
        // #pragma shader_feature _GLOSSYREFLECTIONS_OFF

        // Defines
        #define NATURE_SHADERS
        #define STANDARD_RENDER_PIPELINE
        #define _TYPE_TREE_BARK
        #define PROPERTY_Albedo
        #define PROPERTY_BumpMap
        #define PROPERTY_MaskMap
        #define PROPERTY_ObjectHeight
        #define PROPERTY_VertexNormalStrength
        #define PROPERTY_WindVariation
        #define PROPERTY_WindStrength
        #define PROPERTY_TurbulenceStrength
        #define PROPERTY_HSL
        #define PROPERTY_HSLVariation
        #define PROPERTY_GlossRemap
        #define PROPERTY_OcclusionMap
        #define PROPERTY_OcclusionRemap
        #define PROPERTY_BumpScale
        #define PROPERTY_Tint
        #define PROPERTY_TintVariation
        #define PROPERTY_Cutoff
        #define PROPERTY_TrunkBendFactor
        #define PROPERTY_ColorVariationSpread
        #define PROPERTY_EmissionMap
        #define PROPERTY_EmissionColor
        
        // Includes
        #include "../Common/Surface Shader.cginc"
        ENDCG
    }

    Dependency "OptimizedShader" = "Nature Shaders/Tree Bark" 
    Fallback Off
	CustomEditor "VisualDesignCafe.Nature.Editor.NatureMaterialEditor"
}
