using Staxel.Browser;
using Sunbeam;

namespace PaletteName
{
	public class PaletteNameManager : SunbeamMod
    {
        public override string ModIdentifier => "PaletteName";

        private string HTMLAsset { get; set; }
        private string JSAsset { get; set; }
        private string CSSAsset { get; set; }

        /// <summary>
        /// Construct a new PaletteNameManager
        /// </summary>
        public PaletteNameManager()
        {
            this.HTMLAsset = this.AssetLoader.ReadFileContent("Assets/index.min.html");
            this.JSAsset = this.AssetLoader.ReadFileContent("Assets/main.min.js");
            this.CSSAsset = this.AssetLoader.ReadFileContent("Assets/style.min.css");
        }

		/// <summary>
		/// Inject the UI contents
		/// </summary>
		public override void IngameOverlayUILoaded(BrowserRenderSurface surface)
		{
			surface.CallPreparedFunction("(() => { const el = document.createElement('style'); el.type = 'text/css'; el.appendChild(document.createTextNode('" + this.CSSAsset + "')); document.head.appendChild(el); })();");
			surface.CallPreparedFunction("$('#CharacterCreator:not(:has(#CharacterCreator_Info))').append(\"" + this.HTMLAsset + "\");");
			surface.CallPreparedFunction(this.JSAsset);
		}
    }
}
