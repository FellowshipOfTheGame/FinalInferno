#if UNITY_EDITOR
using System;
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace FinalInferno {
	public static class WebGLBuilder {
		public static void Build() {
			string[] arguments = Environment.GetCommandLineArgs();
			int index = System.Array.IndexOf(arguments, "FinalInferno.WebGLBuilder.Build");
			if(index < 0 || index >= arguments.Length-1){
				throw new ArgumentException("[WebGLBuilder] Could not determine destination path!");
			}
			string destinationPath = arguments[index+1];
			List<string> scenes = new List<string>();
			foreach(EditorBuildSettingsScene scene in EditorBuildSettings.scenes){
				scenes.Add(scene.ToString());
			}
			BuildPipeline.BuildPlayer(EditorBuildSettings.scenes, destinationPath, BuildTarget.WebGL, BuildOptions.None);
		}
	}
}

#endif
