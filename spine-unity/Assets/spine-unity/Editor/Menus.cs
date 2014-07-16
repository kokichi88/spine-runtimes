/******************************************************************************
 * Spine Runtimes Software License
 * Version 2.1
 * 
 * Copyright (c) 2013, Esoteric Software
 * All rights reserved.
 * 
 * You are granted a perpetual, non-exclusive, non-sublicensable and
 * non-transferable license to install, execute and perform the Spine Runtimes
 * Software (the "Software") solely for internal use. Without the written
 * permission of Esoteric Software (typically granted by licensing Spine), you
 * may not (a) modify, translate, adapt or otherwise create derivative works,
 * improvements of the Software or develop new applications using the Software
 * or (b) remove, delete, alter or obscure any trademarks or any copyright,
 * trademark, patent or other intellectual property or proprietary rights
 * notices on or in the Software, including any copy thereof. Redistributions
 * in binary or source form must include this license and terms.
 * 
 * THIS SOFTWARE IS PROVIDED BY ESOTERIC SOFTWARE "AS IS" AND ANY EXPRESS OR
 * IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF
 * MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO
 * EVENT SHALL ESOTERIC SOFTARE BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL,
 * SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO,
 * PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS;
 * OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY,
 * WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR
 * OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF
 * ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 *****************************************************************************/

using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using Spine;

public class Menus {
	[MenuItem("Assets/Create/Spine Atlas")]
	static public void CreateAtlas () {
		CreateAsset<AtlasAsset>("New Atlas");
	}
	
	[MenuItem("Assets/Create/Spine SkeletonData")]
	static public void CreateSkeletonData () {
		CreateAsset<SkeletonDataAsset>("New SkeletonData");
	}
	
	static private T CreateAsset <T> (String name) where T : ScriptableObject {
		var dir = "Assets/";
		var selected = Selection.activeObject;
		if (selected != null) {
			var assetDir = AssetDatabase.GetAssetPath(selected.GetInstanceID());
			if (assetDir.Length > 0 && Directory.Exists(assetDir)) dir = assetDir + "/";
		}
		ScriptableObject asset = ScriptableObject.CreateInstance<T>();
		
		AssetDatabase.CreateAsset(asset, dir + name + ".asset");
		AssetDatabase.SaveAssets();
		EditorUtility.FocusProjectWindow();
		return asset as T;
	}
	
	[MenuItem("GameObject/Create Other/Spine SkeletonRenderer")]
	static public void CreateSkeletonRendererGameObject () {
		GameObject gameObject = new GameObject("New SkeletonRenderer", typeof(SkeletonRenderer));
		EditorUtility.FocusProjectWindow();
		Selection.activeObject = gameObject;
	}
	
	[MenuItem("GameObject/Create Other/Spine SkeletonAnimation")]
	static public void CreateSkeletonAnimationGameObject () {
		GameObject gameObject = new GameObject("New SkeletonAnimation", typeof(SkeletonAnimation));
		EditorUtility.FocusProjectWindow();
		Selection.activeObject = gameObject;
	}
	
	[MenuItem("Assets/Create/SpineData")]
	static public void CreateSpineData() {
		UnityEngine.Object selected = Selection.activeObject;
		string skeStr = "skeleton";
		string pngExt = ".png";
		string atlasExt = ".atlas.txt";
		string jsonExt = ".json.txt";
		string path = "/";
		if(selected != null)
		{
			var material = new Material (Shader.Find("Spine/Skeleton"));
			string assetDir = AssetDatabase.GetAssetPath(selected.GetInstanceID());
			string[] arr = assetDir.Split('/');
			string name = arr[arr.Length - 1];
			AssetDatabase.CreateAsset(material, assetDir + path +  name + ".mat");
			AssetDatabase.SaveAssets();
			
			material.mainTexture = Resources.LoadAssetAtPath(assetDir + path + skeStr + pngExt, typeof(Texture)) as Texture;
			AtlasAsset atlasAsset = CreateAsset<AtlasAsset>(name + ".atlas");
			atlasAsset.atlasFile = Resources.LoadAssetAtPath(assetDir + path + skeStr + atlasExt, typeof(TextAsset)) as TextAsset;
			atlasAsset.materials = new Material[1];
			atlasAsset.materials[0] = material;
			
			SkeletonDataAsset dataAsset = CreateAsset<SkeletonDataAsset>(name + ".ske");
			dataAsset.skeletonJSON = Resources.LoadAssetAtPath(assetDir + path + skeStr + jsonExt,typeof(TextAsset)) as TextAsset;
			dataAsset.atlasAsset= atlasAsset;
		}
	}
}
