using System;
using Game.Sys.GfxData;
using Torque3D;
using Torque3D.Engine;

namespace Game.Sys
{
   [ConsoleClass]
   public class PostFx : PostEffect
   {
      public static void Init()
      {
         PFX_DefaultStateBlock = new GFXStateBlockData("PFX_DefaultStateBlock")
         {
            ZDefined = true,
            ZEnable = false,
            ZWriteEnable = false,
            SamplersDefined = true,
            SamplerStates = {[0] = CommonMaterialData.SamplerClampLinear}
         };
         PFX_DefaultStateBlock.registerObject();

         ShaderData PFX_PassthruShader = new ShaderData("PFX_PassthruShader")
         {
            DXVertexShaderFile = "shaders/common/postFx/postFxV.hlsl",
            DXPixelShaderFile = "shaders/common/postFx/passthruP.hlsl",

            //   OGLVertexShaderFile  = ""shaders/common/postFx/gl//postFxV.glsl"";
            //   OGLPixelShaderFile   = ""shaders/common/postFx/gl/passthruP.glsl"";

            SamplerNames = {[0] = "$inputTex"},
            PixVersion = 2.0f
         };
         PFX_PassthruShader.registerObject();
      }

      public static GFXStateBlockData PFX_DefaultStateBlock { get; set; }

      public void inspectVars()
      {
         string name = getName();
         string globals = $"${name}::*";
         //TODO inspectVars(globals);
      }

      public void viewDisassembly()
      {
         string file = dumpShaderDisassembly();

         if (string.IsNullOrEmpty(file))
         {
            Global.echo("PostEffect::viewDisassembly - no shader disassembly found.");
         }
         else
         {
            Global.echo($"PostEffect::viewDisassembly - shader disassembly file dumped ( {file} ).");
            Global.openFile(file);
         }
      }

      // Return true if we really want the effect enabled.
      // By default this is the case.
      public bool onEnabled()
      {
         return true;
      }
   }
}