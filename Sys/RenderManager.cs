using System;
using System.Diagnostics;
using Torque3D;
using Torque3D.Engine;
using Torque3D.Util;

namespace Game.Sys
{
   public class RenderManager
   {
      private static PostEffect AL_FormatCopy;

      public static void Init()
      {
         // This post effect is used to copy data from the non-MSAA back-buffer to the
         // device back buffer (which could be MSAA). It must be declared here so that
         // it is initialized when 'AL_FormatToken' is initialzed.

         GFXStateBlockData AL_FormatTokenState = new GFXStateBlockData("AL_FormatTokenState", PostFx.PFX_DefaultStateBlock)
         {
            SamplersDefined = true,
            SamplerStates = {[0] = Sim.FindObjectByName<GFXSamplerStateData>("SamplerClampPoint")}
         };
         AL_FormatTokenState.registerObject();

         AL_FormatCopy = new PostEffect("AL_FormatCopy")
         {
            // This PostEffect is used by 'AL_FormatToken' directly. It is never added to
            // the PostEffectManager. Do not call enable() on it.
            IsEnabled = false,
            AllowReflectPass = true,
            Shader = "PFX_PassthruShader",
            StateBlock = AL_FormatTokenState,
            Texture = {[0] = "$inTex"},
            Target = "$backbuffer"
         };
         AL_FormatCopy.registerObject();
      }

      [ConsoleFunction]
      public static void initRenderManager()
      {
         Debug.Assert(!Global.isObject("DiffuseRenderPassManager"),
            "initRenderManager() - DiffuseRenderPassManager already initialized!");

         RenderPassManager diffuseRenderPassManager = new RenderPassManager("DiffuseRenderPassManager");
         diffuseRenderPassManager.registerObject();

         // This token, and the associated render managers, ensure that driver MSAA
         // does not get used for Advanced Lighting renders.  The 'AL_FormatResolve'
         // PostEffect copies the result to the backbuffer.
         RenderFormatToken AL_FormatToken = new RenderFormatToken("AL_FormatToken")
         {
            Enabled = false,
            Format = GFXFormat.R8G8B8A8,
            DepthFormat = GFXFormat.D24S8,
            AaLevel = 0, // -1 = match backbuffer

            // The contents of the back buffer before this format token is executed
            // is provided in $inTex
            CopyEffect = AL_FormatCopy,

            // The contents of the render target created by this format token is
            // provided in $inTex
            ResolveEffect = AL_FormatCopy
         };
         AL_FormatToken.registerObject();

         RenderPassStateBin stateBin = new RenderPassStateBin
         {
            RenderOrder = 0.001f,
            StateToken = AL_FormatToken
         };
         stateBin.registerObject();
         diffuseRenderPassManager.addManager(stateBin);

         // We really need to fix the sky to render after all the
         // meshes... but that causes issues in reflections.
         diffuseRenderPassManager.addManager(new RenderObjectMgr(true)
         {
            BinType = "Sky",
            RenderOrder = 0.1f,
            ProcessAddOrder = 0.1f
         });

         //DiffuseRenderPassManager.addManager( new RenderVistaMgr()               { bintype = "Vista"; renderOrder = 0.15; processAddOrder = 0.15; } );

         diffuseRenderPassManager.addManager(new RenderObjectMgr(true)
         {
            BinType = "Begin",
            RenderOrder = 0.2f,
            ProcessAddOrder = 0.2f
         });

         // Normal mesh rendering.
         diffuseRenderPassManager.addManager(new RenderMeshMgr(true)
         {
            BinType = "Interior",
            RenderOrder = 0.3f,
            ProcessAddOrder = 0.3f
         });

         diffuseRenderPassManager.addManager(new RenderTerrainMgr(true) {RenderOrder = 0.4f, ProcessAddOrder = 0.4f});

         diffuseRenderPassManager.addManager(new RenderMeshMgr(true)
         {
            BinType = "Mesh",
            RenderOrder = 0.5f,
            ProcessAddOrder = 0.5f
         });

         diffuseRenderPassManager.addManager(new RenderImposterMgr(true) {RenderOrder = 0.56f, ProcessAddOrder = 0.56f});

         diffuseRenderPassManager.addManager(new RenderObjectMgr(true)
         {
            BinType = "Object",
            RenderOrder = 0.6f,
            ProcessAddOrder = 0.6f
         });

         diffuseRenderPassManager.addManager(new RenderObjectMgr(true)
         {
            BinType = "Shadow",
            RenderOrder = 0.7f,
            ProcessAddOrder = 0.7f
         });
         diffuseRenderPassManager.addManager(new RenderMeshMgr(true)
         {
            BinType = "Decal",
            RenderOrder = 0.8f,
            ProcessAddOrder = 0.8f
         });
         diffuseRenderPassManager.addManager(new RenderOcclusionMgr(true)
         {
            BinType = "Occluder",
            RenderOrder = 0.9f,
            ProcessAddOrder = 0.9f
         });

         // We now render translucent objects that should handle
         // their own fogging and lighting.

         // Note that the fog effect is triggered before this bin.
         diffuseRenderPassManager.addManager(new RenderObjectMgr("ObjTranslucentBin", true)
         {
            BinType = "ObjectTranslucent",
            RenderOrder = 1.0f,
            ProcessAddOrder = 1.0f
         });

         diffuseRenderPassManager.addManager(new RenderObjectMgr(true)
         {
            BinType = "Water",
            RenderOrder = 1.2f,
            ProcessAddOrder = 1.2f
         });
         diffuseRenderPassManager.addManager(new RenderObjectMgr(true)
         {
            BinType = "Foliage",
            RenderOrder = 1.3f,
            ProcessAddOrder = 1.3f
         });
         diffuseRenderPassManager.addManager(new RenderParticleMgr(true) {RenderOrder = 1.35f, ProcessAddOrder = 1.35f});
         diffuseRenderPassManager.addManager(new RenderTranslucentMgr(true) {RenderOrder = 1.4f, ProcessAddOrder = 1.4f});

         // Note that the GlowPostFx is triggered after this bin.
         diffuseRenderPassManager.addManager(new RenderGlowMgr("GlowBin", true)
         {
            RenderOrder = 1.5f,
            ProcessAddOrder = 1.5f
         });

         // We render any editor stuff from this bin.  Note that the HDR is
         // completed before this bin to keep editor elements from tone mapping.
         diffuseRenderPassManager.addManager(new RenderObjectMgr("EditorBin", true)
         {
            BinType = "Editor",
            RenderOrder = 1.6f,
            ProcessAddOrder = 1.6f
         });

         // Resolve format change token last.
         diffuseRenderPassManager.addManager(new RenderPassStateBin(true)
         {
            RenderOrder = 1.7f,
            StateToken = AL_FormatToken
         });
      }
   }
}