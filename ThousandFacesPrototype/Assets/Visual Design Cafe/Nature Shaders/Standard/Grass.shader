Shader "Nature Shaders/Grass"
{
    Properties
    {
        // Transparency
        _Cutoff("Cutoff", Range(0, 1)) = 0.5

        // Color
        _HSL("Hue, Saturation, Lightness", Vector) = (0.02, 0.05, 0.1, 0)
        _HSLVariation("Hue, Saturation, Lightness Variation", Vector) = (-0.02, -0.05, -0.1, 0)
        _Tint("Tint", Color) = (1, 1, 1, 1)
        _TintVariation("Tint Variation", Color) = (1, 1, 1, 1)
        _ColorVariationSpread("Color Variation Spread", Float) = 0.2

        // Surface
        [NoScaleOffset]_MainTex("MainTex", 2D) = "white" {}
        [NoScaleOffset][MainTexture]_Albedo("Albedo", 2D) = "white" {}
        [NoScaleOffset]_BumpMap("Normal Map", 2D) = "bump" {}
        [NoScaleOffset]_MaskMap("Mask Map", 2D) = "white" {}
        [NoScaleOffset]_MetallicGlossMap("Metallic Gloss Map", 2D) = "black" {}
        [NoScaleOffset]_OcclusionMap("Occlusion Map", 2D) = "white" {}
        _GlossRemap("Remap Smoothness", Vector) = (0, 1, 0, 0)
        _OcclusionRemap("Remap Occlusion", Vector) = (0, 1, 0, 0)
        _BumpScale("Normal Map Strength", Range(0, 1)) = 1
        _Glossiness("Smoothness", Range(0, 1)) = 0.2
        _Metallic("Metallic", Range(0, 1)) = 0
        _VertexNormalStrength("Vertex Normal Strength", Range(0, 1)) = 1

        // Wind
        _ObjectHeight("Grass Height", Float) = 0.5
        _ObjectRadius("Grass Radius", Float) = 0.5
        _WindVariation("Wind Variation", Range(0, 1)) = 0.3
        _WindStrength("Wind Strength", Range(0, 2)) = 1
        _TurbulenceStrength("Turbulence Strength", Range(0, 2)) = 1
        _RecalculateWindNormals("Recalculate Normals", Range(0,1)) = 0.5
        
        // Interaction
        _InteractionDuration("Interaction Duration", Range(0, 5)) = 1
        _InteractionStrength("Interaction Strength", Range(0, 1)) = 1
        _InteractionPushDown("Interaction PushDown", Range(0,1)) = 1

        // Fade
        _WindFade("Wind Fade", Vector) = (50, 20, 0, 0)
        _ScaleFade("Scale Fade", Vector) = (50, 20, 0, 0)
        [HideInInspector]_NatureRendererDistanceControl("", Float) = 1

        // Translucency
        [NoScaleOffset]_ThicknessMap("Thickness Map", 2D) = "black" {}
        _Translucency("Translucency", Range(0, 2)) = 1
        _TranslucencyDistortion("Translucency Distortion", Range(0, 1)) = 0.5
        _TranslucencyScattering("Translucency Scattering", Range(0, 3)) = 2
        _TranslucencyColor("Translucency Color", Color) = (1, 1, 1, 1)
        _ThicknessRemap("Thickness Remap", Vector) = (0, 1, 0, 0)

        // Emission
        _EmissionMap("Emission Map", 2D) = "black" {}
        [HDR]_EmissionColor("Emission Color", Color) = (1,1,1,1)

        // Keywords
        [KeywordEnum(Baked, UV, Vertex)]_WIND_CONTROL("Wind Control", Float) = 2
        [KeywordEnum(Ambient, Gust, Turbulence, None)]_DEBUG("Debug Wind", Float) = 3
        [Enum(On, 0, Off, 2)] _DoubleSidedMode ("Double Sided", Float) = 0
        [KeywordEnum(Off, MetallicGloss, Mask)]_SURFACE_MAP("Surface Map Method", Float) = 0
        [Enum(None, 0, Flip, 1)] _DoubleSidedNormalMode("Double sided normal mode", Float) = 0
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
        Cull [_DoubleSidedMode]
        ZTest LEqual
        ZWrite On
        // ColorMask: <None>

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface NatureShaderSurface StandardCustom vertex:NatureShaderVertex addshadow fullforwardshadows nolightmap nometa nolppv 
        
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
        #pragma shader_feature_local _ALPHA_CLIP_DISABLED
        #pragma shader_feature_local _EMISSION
        #pragma shader_feature_local _INTERACTION_OFF _INTERACTION_ON _INTERACTION_ON_VERTEX

        // Bump map is enabled by default. Enable this pragma, or remove the define to disable bump mapping for materials that don't use a normal map.
        // #pragma shader_feature_local _BUMP_MAP_ON
        #define _BUMP_MAP_ON

        // Enable this pragma to completely disable translucency for materials. This improves performance.
        // #pragma shader_feature_local _TRANSLUCENCY_OFF

        // Enable these pragmas to allow specular highlights to be enabled/disabled per material.
        // #pragma shader_feature _SPECULARHIGHLIGHTS_OFF
        // #pragma shader_feature _GLOSSYREFLECTIONS_OFF

        // Defines
        #define NATURE_SHADERS
        #define STANDARD_RENDER_PIPELINE
        #define _TYPE_GRASS
        #define TRANSLUCENT
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
        #define PROPERTY_ThicknessMap
        #define PROPERTY_Translucency
        #define PROPERTY_TranslucencyDistortion
        #define PROPERTY_TranslucencyScattering
        #define PROPERTY_TranslucencyColor
        #define PROPERTY_ThicknessRemap
        #define PROPERTY_ColorVariationSpread
        #define PROPERTY_WindFade
        #define PROPERTY_ScaleFade
        #define PROPERTY_EmissionMap
        #define PROPERTY_EmissionColor
        #define PROPERTY_Glossiness
        #define PROPERTY_Metallic
        #define PROPERTY_DoubleSidedNormalMode
        #define PROPERTY_RecalculateWindNormals

        float _RecalculateWindNormals;

        // Includes
        #include "../Common/Surface Shader.cginc"
        ENDCG
    }

    Fallback Off
	CustomEditor "VisualDesignCafe.Nature.Editor.NatureMaterialEditor"
}
