using Game.Sys.GfxData;
using Torque3D;
using Torque3D.Engine;
using Torque3D.Util;

namespace Game.Sys.LightingSystems.Basic
{
   class Basic
   {
      public static RenderPassManager BL_ProjectedShadowRPM { get; set; }

      public static void Init()
      {
         ShadowFilter.Init();

         GFXStateBlockData BL_ProjectedShadowSBData = new GFXStateBlockData("BL_ProjectedShadowSBData")
         {
            BlendDefined = true,
            BlendEnable = true,
            BlendSrc = GFXBlend.DestColor,
            BlendDest = GFXBlend.Zero,
            ZDefined = true,
            ZEnable = true,
            ZWriteEnable = false,
            SamplersDefined = true,
            SamplerStates = {[0] = CommonMaterialData.SamplerClampLinear},
            VertexColorEnable = true
         };
         BL_ProjectedShadowSBData.registerObject();

         new ShaderData("BL_ProjectedShadowShaderData")
         {
            DXVertexShaderFile = "shaders/common/projectedShadowV.hlsl",
            DXPixelShaderFile = "shaders/common/projectedShadowP.hlsl",
            OGLVertexShaderFile = "shaders/common/gl/projectedShadowV.glsl",
            OGLPixelShaderFile = "shaders/common/gl/projectedShadowP.glsl",
            PixVersion = 2.0f
         };

         CustomMaterial BL_ProjectedShadowMaterial = new CustomMaterial("BL_ProjectedShadowMaterial")
         {
            Shader = "BL_ProjectedShadowShaderData",
            StateBlock = BL_ProjectedShadowSBData,
            Version = 2.0f,
            ForwardLit = true
         };
         BL_ProjectedShadowMaterial.setFieldValue("sampler[\"inputTex\"]", "$miscbuff");
      }


      public static void onActivateBasicLM()
      {
         PostEffect HDRPostFx = Sim.FindObjectByName<PostEffect>("HDRPostFx");
         RenderFormatToken AL_FormatToken = Sim.FindObjectByName<RenderFormatToken>("AL_FormatToken");
         // If HDR is enabled... enable the special format token.
         if (!Globals.GetString("platform").Equals("macos") && HDRPostFx.isEnabled())
            AL_FormatToken.enable();

         // Create render pass for projected shadow.
         BL_ProjectedShadowRPM = new RenderPassManager("BL_ProjectedShadowRPM");
         BL_ProjectedShadowRPM.registerObject();

         // Create the mesh bin and add it to the manager.
         RenderMeshMgr meshBin = new RenderMeshMgr();
         meshBin.registerObject();
         BL_ProjectedShadowRPM.addManager(meshBin);

         SimGroup RootGroup = Sim.FindObjectByName<SimGroup>("RootGroup");

         // Add both to the root group so that it doesn't
         // end up in the MissionCleanup instant group.
         RootGroup.add(BL_ProjectedShadowRPM);
         RootGroup.add(meshBin);
      }

      public static void onDeactivateBasicLM()
      {
         // Delete the pass manager which also deletes the bin.
         BL_ProjectedShadowRPM.delete();
      }

      public static void setBasicLighting()
      {
         Global.setLightManager("Basic Lighting");
      }
   }
}