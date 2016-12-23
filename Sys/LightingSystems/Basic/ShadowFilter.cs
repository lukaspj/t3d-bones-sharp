using Torque3D;
using Torque3D.Util;

namespace Game.Sys.LightingSystems.Basic
{
   class ShadowFilter
   {
      public static void Init()
      {
         ShaderData ShadowFilterShaderV = new ShaderData("BL_ShadowFilterShaderV")
         {
            DXVertexShaderFile = "shaders/common/lighting/basic/shadowFilterV.hlsl",
            DXPixelShaderFile = "shaders/common/lighting/basic/shadowFilterP.hlsl",
            OGLVertexShaderFile = "shaders/common/lighting/basic/gl/shadowFilterV.glsl",
            OGLPixelShaderFile = "shaders/common/lighting/basic/gl/shadowFilterP.glsl",
            SamplerNames = {[0] = "$diffuseMap"},
            Defines = "BLUR_DIR=float2(1.0,0.0)",
            PixVersion = 2.0f
         };
         ShadowFilterShaderV.registerObject();

         new ShaderData("BL_ShadowFilterShaderH", ShadowFilterShaderV)
         {
            Defines = "BLUR_DIR=float2(0.0,1.0)"
         }.registerObject();


         GFXStateBlockData BL_ShadowFilterSB = new GFXStateBlockData("BL_ShadowFilterSB", PostFx.PFX_DefaultStateBlock)
         {
            ColorWriteDefined = true,
            ColorWriteRed = false,
            ColorWriteGreen = false,
            ColorWriteBlue = false,
            BlendDefined = true,
            BlendEnable = true
         };
         BL_ShadowFilterSB.registerObject();

         // NOTE: This is ONLY used in Basic Lighting, and 
         // only directly by the ProjectedShadow.  It is not 
         // meant to be manually enabled like other PostEffects.
         PostEffect BL_ShadowFilterPostFx = new PostEffect("BL_ShadowFilterPostFx")
         {
            // Blur vertically
            Shader = "BL_ShadowFilterShaderV",
            StateBlock = PostFx.PFX_DefaultStateBlock,
            TargetClear = PFXTargetClear.OnDraw,
            TargetClearColor = new ColorF(0.0f, 0.0f, 0.0f, 0.0f),
            Texture = {[0] = "$inTex"},
            Target = "$outTex"
         };

         PostEffect blurEffect = new PostEffect
         {
            Shader = "BL_ShadowFilterShaderH",
            StateBlock = PostFx.PFX_DefaultStateBlock,
            Texture = {[0] = "$inTex"},
            Target = "$outTex"
         };
         blurEffect.registerObject();
         BL_ShadowFilterPostFx.add(blurEffect);

         BL_ShadowFilterPostFx.registerObject();
      }
   }
}