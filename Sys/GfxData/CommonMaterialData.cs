using Torque3D;
using Torque3D.Util;

namespace Game.Sys.GfxData
{
   class CommonMaterialData
   {
      public static void Init()
      {
         //-----------------------------------------------------------------------------
         // Anim flag settings - must match material.h
         // These cannot be enumed through script becuase it cannot
         // handle the "|" operation for combining them together
         // ie. Scroll | Wave does not work.
         //-----------------------------------------------------------------------------
         Globals.SetInt("scroll", 1);
         Globals.SetInt("rotate", 2);
         Globals.SetInt("wave", 4);
         Globals.SetInt("scale", 8);
         Globals.SetInt("sequence", 16);

         // Common stateblock definitions
         SamplerClampLinear = new GFXSamplerStateData("SamplerClampLinear")
         {
            TextureColorOp = GFXTextureOp.Modulate,
            AddressModeU = GFXTextureAddressMode.Clamp,
            AddressModeV = GFXTextureAddressMode.Clamp,
            AddressModeW = GFXTextureAddressMode.Clamp,
            MagFilter = GFXTextureFilterType.Linear,
            MinFilter = GFXTextureFilterType.Linear,
            MipFilter = GFXTextureFilterType.Linear
         };
         SamplerClampLinear.registerObject();

         SamplerClampPoint = new GFXSamplerStateData("SamplerClampPoint")
         {
            TextureColorOp = GFXTextureOp.Modulate,
            AddressModeU = GFXTextureAddressMode.Clamp,
            AddressModeV = GFXTextureAddressMode.Clamp,
            AddressModeW = GFXTextureAddressMode.Clamp,
            MagFilter = GFXTextureFilterType.Point,
            MinFilter = GFXTextureFilterType.Point,
            MipFilter = GFXTextureFilterType.Point
         };
         SamplerClampPoint.registerObject();

         SamplerWrapLinear = new GFXSamplerStateData("SamplerWrapLinear")
         {
            TextureColorOp = GFXTextureOp.Modulate,
            AddressModeU = GFXTextureAddressMode.Wrap,
            AddressModeV = GFXTextureAddressMode.Wrap,
            AddressModeW = GFXTextureAddressMode.Wrap,
            MagFilter = GFXTextureFilterType.Linear,
            MinFilter = GFXTextureFilterType.Linear,
            MipFilter = GFXTextureFilterType.Linear
         };
         SamplerWrapLinear.registerObject();

         SamplerWrapPoint = new GFXSamplerStateData("SamplerWrapPoint")
         {
            TextureColorOp = GFXTextureOp.Modulate,
            AddressModeU = GFXTextureAddressMode.Wrap,
            AddressModeV = GFXTextureAddressMode.Wrap,
            AddressModeW = GFXTextureAddressMode.Wrap,
            MagFilter = GFXTextureFilterType.Point,
            MinFilter = GFXTextureFilterType.Point,
            MipFilter = GFXTextureFilterType.Point
         };
         SamplerWrapPoint.registerObject();
      }

      public static GFXSamplerStateData SamplerWrapPoint { get; set; }

      public static GFXSamplerStateData SamplerWrapLinear { get; set; }

      public static GFXSamplerStateData SamplerClampPoint { get; set; }

      public static GFXSamplerStateData SamplerClampLinear { get; set; }
   }
}
