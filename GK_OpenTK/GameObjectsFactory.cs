﻿using GK_OpenTK.GameObjects;
using GK_OpenTK.ModelsFactory;
using GK_OpenTK.Renderable;
using GK_OpenTK.Renderable2;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GK_OpenTK
{
    class GameObjectsFactory
    {
        public static AGameObject bus(ShaderProgram program,Vector4 position,bool IsTextured)
        {
            MyComplexObjectFactory2 objBus = MyComplexObjectFactory2.LoadFromFile(@"Models\bus.obj", 1);
            ARenderable2 busRend;
            if (IsTextured)
                busRend = new TextureRender2(program.program, objBus.GetVerts(), objBus.GetTextureCoords(), objBus.GetNormals(), objBus.GetIndices(), @"Textures\bus.png");
            else
                busRend = new TextureRender2(program.program, objBus.GetVerts(), objBus.GetIndices());
            AGameObject bus = new CowObject(busRend, new Vector4(0f, 0f, 0f, 0f), position,objBus.CalculateObjectCenter(),100);
            bus.Rotate(150,null);
            bus.rot_dir = true;
            return bus;
        }
        public static AGameObject f16(ShaderProgram program, Vector4 position, bool IsTextured)
        {
            MyComplexObjectFactory2 objF16 = MyComplexObjectFactory2.LoadFromFile(@"Models\f16.obj", 0);
            ARenderable2 f16Rend;
            if (IsTextured)
                f16Rend = new TextureRender2(program.program, objF16.GetVerts(), objF16.GetTextureCoords(), objF16.GetNormals(), objF16.GetIndices(), @"Textures\f16.bmp");
            else
                f16Rend = new TextureRender2(program.program, objF16.GetVerts(), objF16.GetIndices());
            AGameObject f16 = new CowObject(f16Rend, new Vector4(0f, 0f, 0f, 0f), position,objF16.CalculateObjectCenter(),100);
            f16.Rotate(0,null);
            return f16;
        }
        public static AGameObject earth(ShaderProgram program, Vector4 position, bool IsTextured)
        {
            MyComplexObjectFactory2 modelObj = MyComplexObjectFactory2.LoadFromFile(@"Models\earth.obj", 2);
            ARenderable2 modelRend;
            if (IsTextured)
                modelRend = new TextureRender2(program.program, modelObj.GetVerts(), modelObj.GetTextureCoords(), modelObj.GetNormals(), modelObj.GetIndices(), @"Textures\earth.jpg");
            else
                modelRend = new TextureRender2(program.program, modelObj.GetVerts(), modelObj.GetIndices());
            AGameObject model = new CowObject(modelRend, new Vector4(0f, 0f, 0f, 0f), position,Vector3.Zero);
            return model;
        }
        public static AGameObject city(ShaderProgram program, Vector4 position, bool IsTextured)
        {
            Dictionary<String, Material> materials = Material.LoadFromFile(@"Materials\TheCity.mtl");
            MyComplexObjectFactory3 modelObj = MyComplexObjectFactory3.LoadFromFile(@"Models\TheCity.obj");
            Tuple<int[], Dictionary<String, int>> textInd = modelObj.GetTexturesInd(materials);
            float[] textIndices = new float[textInd.Item1.Length];
            for (int i = 0; i < textIndices.Length; i++)
            {
                textIndices[i] = textInd.Item1[i];
            }
            Dictionary<String, int> mapInd = textInd.Item2;
            List<String> texturesPath = new List<string>();
            for (int i = 0; i < mapInd.Count; i++)
            {
                String mat = mapInd.FirstOrDefault(x => x.Value == i).Key;
                texturesPath.Add(materials.FirstOrDefault(x => x.Key == mat).Value.DiffuseMap);
            }
            ARenderable2 modelRend;
          
            modelRend = new TextureRender2(program.program, modelObj.GetVerts(), modelObj.GetTextureCoords(), modelObj.GetNormals(), modelObj.GetIndices(), textIndices, texturesPath.ToArray());
            AGameObject model = new CowObject(modelRend, new Vector4(0f, 0f, 0f, 0), position,Vector3.Zero);

            return model;
        }

    }
}
