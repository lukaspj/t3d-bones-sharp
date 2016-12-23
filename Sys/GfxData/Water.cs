using Torque3D;
using Torque3D.Util;

namespace Game.Sys.GfxData
{
   class Water
   {
      public static void Init()
      {
         //-----------------------------------------------------------------------------
         // Water
         //-----------------------------------------------------------------------------

         ShaderData WaterShader = new ShaderData("WaterShader")
         {
            DXVertexShaderFile = "shaders/common/water/waterV.hlsl",
            DXPixelShaderFile = "shaders/common/water/waterP.hlsl",
            OGLVertexShaderFile = "shaders/common/water/gl/waterV.glsl",
            OGLPixelShaderFile = "shaders/common/water/gl/waterP.glsl",
            SamplerNames =
            {
               [0] = "$bumpMap", // noise
               [1] = "$prepassTex", // #prepass
               [2] = "$reflectMap", // $reflectbuff
               [3] = "$refractBuff", // $backbuff
               [4] = "$skyMap", // $cubemap   
               [5] = "$foamMap", // foam     
               [6] = "$depthGradMap"
            }, // depthMap ( color gradient ) 

            PixVersion = 3.0f
         };
         WaterShader.registerObject();

         GFXSamplerStateData WaterSampler = new GFXSamplerStateData("WaterSampler")
         {
            TextureColorOp = GFXTextureOp.Modulate,
            AddressModeU = GFXTextureAddressMode.Wrap,
            AddressModeV = GFXTextureAddressMode.Wrap,
            AddressModeW = GFXTextureAddressMode.Wrap,
            MagFilter = GFXTextureFilterType.Linear,
            MinFilter = GFXTextureFilterType.Anisotropic,
            MipFilter = GFXTextureFilterType.Linear,
            MaxAnisotropy = 4
         };
         WaterSampler.registerObject();

         GFXStateBlockData WaterStateBlock = new GFXStateBlockData("WaterStateBlock")
         {
            SamplersDefined = true,
            SamplerStates =
            {
               [0] = WaterSampler, // noise
               [1] = CommonMaterialData.SamplerClampPoint, // #prepass
               [2] = CommonMaterialData.SamplerClampLinear, // $reflectbuff
               [3] = CommonMaterialData.SamplerClampPoint, // $backbuff
               [4] = CommonMaterialData.SamplerWrapLinear, // $cubemap   
               [5] = CommonMaterialData.SamplerWrapLinear, // foam     
               [6] = CommonMaterialData.SamplerClampLinear // depthMap ( color gradient ) 
            },
            CullDefined = true,
            CullMode = GFXCullMode.CullCCW
         };
         WaterStateBlock.registerObject();

         GFXStateBlockData UnderWaterStateBlock = new GFXStateBlockData("UnderWaterStateBlock", WaterStateBlock)
         {
            CullMode = GFXCullMode.CullCCW
         };
         UnderWaterStateBlock.registerObject();

         CustomMaterial WaterMat = new CustomMaterial("WaterMat")
         {
            Shader = "WaterShader",
            StateBlock = WaterStateBlock,
            Version = 3.0f,
            UseAnisotropic = {[0] = true}
         };
         WaterMat.setFieldValue("sampler[prepassTex]", "#prepass");
         WaterMat.setFieldValue("sampler[reflectMap]", "$reflectbuff");
         WaterMat.setFieldValue("sampler[refractBuff]", "$backbuff");
         WaterMat.registerObject();

         //-----------------------------------------------------------------------------
         // Underwater
         //-----------------------------------------------------------------------------

         new ShaderData("UnderWaterShader", WaterShader)
         {
            Defines = "UNDERWATER"
         }.registerObject();

         CustomMaterial UnderwaterMat = new CustomMaterial("UnderwaterMat")
         {
            // These samplers are set in code not here.
            // This is to allow different WaterObject instances
            // to use this same material but override these textures
            // per instance.   
            //sampler["bumpMap"] = "art/images/water/noise02";
            //sampler["foamMap"] = "art/images/water/foam";

            Shader = "UnderWaterShader",
            StateBlock = UnderWaterStateBlock,
            Specular = {[0] = new ColorF(0.75f, 0.75f, 0.75f, 1.0f)},
            SpecularPower = {[0] = 48.0f},
            Version = 3.0f
         };
         UnderwaterMat.setFieldValue("sampler[prepassTex]", "#prepass");
         UnderwaterMat.setFieldValue("sampler[refractBuff]", "$backbuff");
         UnderwaterMat.registerObject();

         //-----------------------------------------------------------------------------
         // Basic Water
         //-----------------------------------------------------------------------------

         ShaderData WaterBasicShader = new ShaderData("WaterBasicShader")
         {
            DXVertexShaderFile = "shaders/common/water/waterBasicV.hlsl",
            DXPixelShaderFile = "shaders/common/water/waterBasicP.hlsl",
            OGLVertexShaderFile = "shaders/common/water/gl/waterBasicV.glsl",
            OGLPixelShaderFile = "shaders/common/water/gl/waterBasicP.glsl",
            SamplerNames =
            {
               [0] = "$bumpMap",
               [2] = "$reflectMap",
               [3] = "$refractBuff",
               [4] = "$skyMap",
               [5] = "$depthGradMap"
            },
            PixVersion = 2.0f
         };
         WaterBasicShader.registerObject();

         GFXStateBlockData WaterBasicStateBlock = new GFXStateBlockData("WaterBasicStateBlock")
         {
            SamplersDefined = true,
            SamplerStates =
            {
               [0] = WaterSampler, // noise
               [2] = CommonMaterialData.SamplerClampLinear, // $reflectbuff
               [3] = CommonMaterialData.SamplerClampPoint, // $backbuff
               [4] = CommonMaterialData.SamplerWrapLinear // $cubemap
            },
            CullDefined = true,
            CullMode = GFXCullMode.CullCCW
         };
         WaterBasicStateBlock.registerObject();

         GFXStateBlockData UnderWaterBasicStateBlock = new GFXStateBlockData("UnderWaterBasicStateBlock",
            WaterBasicStateBlock)
         {
            CullMode = GFXCullMode.CullCCW
         };
         UnderWaterBasicStateBlock.registerObject();

         CustomMaterial WaterBasicMat = new CustomMaterial("WaterBasicMat")
         {
            // These samplers are set in code not here.
            // This is to allow different WaterObject instances
            // to use this same material but override these textures
            // per instance.     
            //sampler["bumpMap"] = "art/images/water/noise02";
            //sampler["skyMap"] = "$cubemap";   

            //sampler["prepassTex"] = "#prepass";
         
            Cubemap = "NewLevelSkyCubemap",
            Shader = "WaterBasicShader",
            StateBlock = WaterBasicStateBlock,
            Version = 2.0f
         };
         WaterBasicMat.setFieldValue("sampler[reflectMap]", "$reflectbuff");
         WaterBasicMat.setFieldValue("sampler[refractBuff]", "$backbuff");
         WaterBasicMat.registerObject();

         //-----------------------------------------------------------------------------
         // Basic UnderWater
         //-----------------------------------------------------------------------------

         new ShaderData("UnderWaterBasicShader", WaterBasicShader)
         {
            Defines = "UNDERWATER"
         }.registerObject();

         CustomMaterial UnderwaterBasicMat = new CustomMaterial("UnderwaterBasicMat")
         {
            // These samplers are set in code not here.
            // This is to allow different WaterObject instances
            // to use this same material but override these textures
            // per instance.  
            //sampler["bumpMap"] = "art/images/water/noise02";
            //samplers["skyMap"] = "$cubemap";  

            //sampler["prepassTex"] = "#prepass";

            Shader = "UnderWaterBasicShader",
            StateBlock = UnderWaterBasicStateBlock,
            Version = 2.0f
         };
         UnderwaterBasicMat.setFieldValue("sampler[refractBuff]", "$backbuff");
         UnderwaterBasicMat.registerObject();
      }
   }
}