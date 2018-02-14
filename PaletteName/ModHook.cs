using Staxel;
using Staxel.Rendering;
using Sunbeam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PaletteName
{
    public class ModHook : BaseMod
    {
        public override string ModIdentifier => "PaletteName";

        private bool AssetsInjected = false;
        private string DomAsset { get; set; }
        private string ScriptAsset { get; set; }
        private string StyleAsset { get; set; }

        /// <summary>
        /// Load assets
        /// </summary>
        protected override void GameContextInitializeInitOverride()
        {
            this.DomAsset = this.AssetLoader.ReadFileContent("Assets/index.min.html");
            this.ScriptAsset = this.AssetLoader.ReadFileContent("Assets/main.min.js");
            this.StyleAsset = this.AssetLoader.ReadFileContent("Assets/style.min.css");
        }

        /// <summary>
        /// Initialize mod by injecting assets
        /// They need to be injected through UniverseUpdate, when using ClientContextInitializeBefore the
        /// webpage is not ready yet, we could patch a hook to this to have a proper method hook to inject
        /// assets, but this also works for now
        /// </summary>
        protected override void UniverseUpdateAfterOverride()
        {
            this.InjectAssets();
        }

        /// <summary>
        /// Injects the UI assets
        /// </summary>
        private void InjectAssets()
        {
            if (this.AssetsInjected)
            {
                return;
            }

            WebOverlayRenderer overlay = ClientContext.WebOverlayRenderer;
            if (overlay == null)
            {
                return;
            }

            overlay.CallPreparedFunction("(() => { const el = document.createElement('style'); el.type = 'text/css'; el.appendChild(document.createTextNode('" + this.StyleAsset + "')); document.head.appendChild(el); })();");
            overlay.CallPreparedFunction("$('#CharacterCreator:not(:has(#CharacterCreator_Info))').prepend(\"" + this.DomAsset + "\");");
            overlay.CallPreparedFunction(this.ScriptAsset);

            this.AssetsInjected = true;
        }
    }
}
