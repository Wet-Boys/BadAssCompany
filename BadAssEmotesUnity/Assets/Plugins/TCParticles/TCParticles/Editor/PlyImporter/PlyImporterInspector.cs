using UnityEditor;


namespace Pcx {
	[CustomEditor(typeof(PlyImporter))]
	class PlyImporterInspector : UnityEditor.AssetImporters.ScriptedImporterEditor {
		protected override bool useAssetDrawPreview => false;
	}
}