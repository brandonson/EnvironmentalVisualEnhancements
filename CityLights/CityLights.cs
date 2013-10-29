﻿using CommonUtils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace CityLights
{
    [KSPAddon(KSPAddon.Startup.EveryScene, false)]
    public class CityLights: MonoBehaviour
    {
        static Material lightMaterial;
        static bool setup = false;

        public static void InitTextures()
        {
            Log("Initializing Textures");
            Assembly assembly = Assembly.GetExecutingAssembly();
            StreamReader shaderStreamReader = new StreamReader(assembly.GetManifestResourceStream("CityLights.CompiledCityLightsShader.txt"));
            Log("read stream");
            lightMaterial = new Material(shaderStreamReader.ReadToEnd());
            Texture2D main = GameDatabase.Instance.GetTexture("BoulderCo/CityLights/Textures/main", false);
            Texture2D detail = GameDatabase.Instance.GetTexture("BoulderCo/CityLights/Textures/detail", false);
//            texture.filterMode = FilterMode.Point;
//            texture.Apply();
            lightMaterial.mainTexture = main;
            lightMaterial.mainTextureScale = new Vector2(1f, 1f);
            lightMaterial.mainTextureOffset = new Vector2(-.25f, 0f);
            lightMaterial.SetTexture("_DetailTex", detail);
            lightMaterial.SetTextureScale("_DetailTex", new Vector2(140f, 140f));
            Log("Textures initialized");
        }

        protected void Awake()
        {
            if (HighLogic.LoadedScene == GameScenes.MAINMENU && !setup)
            {
                InitTextures();
                Utils.Overlay.GeneratePlanetOverlay("Kerbin", 1.001f, gameObject, lightMaterial, Utils.OVER_LAYER, true);
                setup = true;
            }
        }

        public static void Log(String message)
        {
            UnityEngine.Debug.Log("CityLights: " + message);
        }

    }
}
