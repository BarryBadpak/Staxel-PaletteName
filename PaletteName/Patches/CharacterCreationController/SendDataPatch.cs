using Harmony;
using Plukit.Base;
using Staxel;
using Staxel.Characters;
using Staxel.Client;
using Staxel.Rendering.Palettes;
using System;
using System.Linq;
using System.Reflection;

namespace PaletteName.Patches.CharacterCreationController
{
    [HarmonyPatch(typeof(CharacterCreatorController), "SendData")]
    class SendDataPatch
    {
        [HarmonyPrefix]
        static bool BeforeSendData(CharacterCreatorController __instance)
        {
            MethodInfo PrepareDataBlobMethod = __instance.GetType().GetMethod("PrepareDataBlob", BindingFlags.NonPublic | BindingFlags.Instance);
            PrepareDataBlobMethod.Invoke(__instance, new object[] { });

            FieldInfo DataBlobField = __instance.GetType().GetField("DataBlob", BindingFlags.NonPublic | BindingFlags.Instance);
            Blob CategoryDataBlob = (SendDataPatch.GetFieldValue(__instance, "DataBlob") as Blob);
            SendDataPatch.AddExtraCategoryDataToBlob(__instance, CategoryDataBlob);
       
            ClientContext.WebOverlayRenderer.Call("setData", CategoryDataBlob.ToString(), null, null, null, null, null);
            return false; // Do not execute original
        }

        /// <summary>
        /// Add the labels to the original blob
        /// </summary>
        /// <param name="__instance"></param>
        /// <param name="catData"></param>
        static void AddExtraCategoryDataToBlob(CharacterCreatorController __instance, Blob catData)
        {
            CharacterDesignDatabase dd = GameContext.CharacterDesignDatabase;
            CharacterSpeciesBody body = SendDataPatch.GetFieldValue(__instance, "_body") as CharacterSpeciesBody;
            Blob setupData = catData.FetchBlob("Setup_Data");

            for (int i6 = 0; i6 < body.Palettes.Count; i6++)
            {
                Blob skinColourBlob = setupData.FetchBlob("Char_3").FetchBlob(IntegerStrings.ToString(0)).FetchBlob("palettes")
                    .FetchBlob(IntegerStrings.ToString(i6));
                Palette palette = default(Palette);
                GameContext.PaletteDatabase.TryGetPalette(body.Palettes[i6], out palette);
                
                skinColourBlob.SetString("label", SendDataPatch.LocalisePaletteCode(palette.Code));
            }

            for (int i5 = 0; i5 < dd.Hairs.Count; i5++)
            {
                Blob hair = setupData.FetchBlob("Hair_1").FetchBlob(IntegerStrings.ToString(i5));
                CharacterAccessory hairItem = dd.Hairs[i5];
                for (int i = 0; i < hairItem.Palettes.Count; i++)
                {
                    Blob hairColourBlob = hair.FetchBlob("palettes").FetchBlob(IntegerStrings.ToString(i));
                    Palette palette2 = default(Palette);
                    GameContext.PaletteDatabase.TryGetPalette(hairItem.Palettes[i], out palette2);

                    hairColourBlob.SetString("label", SendDataPatch.LocalisePaletteCode(palette2.Code));
                }
            }

            for (int i4 = 0; i4 < dd.Eyes.Count; i4++)
            {
                Blob eyes = setupData.FetchBlob("Eyes_1").FetchBlob(IntegerStrings.ToString(i4));
                CharacterAccessory eyeItem = dd.Eyes[i4];
                for (int j = 0; j < eyeItem.Palettes.Count; j++)
                {
                    Blob hairColourBlob2 = eyes.FetchBlob("palettes").FetchBlob(IntegerStrings.ToString(j));
                    Palette palette3 = default(Palette);
                    GameContext.PaletteDatabase.TryGetPalette(eyeItem.Palettes[j], out palette3);

                    hairColourBlob2.SetString("label", SendDataPatch.LocalisePaletteCode(palette3.Code));
                }
            }

            for (int i3 = 0; i3 < dd.StarterShirts.Count; i3++)
            {
                Blob shirt = setupData.FetchBlob("Clothing_1").FetchBlob(IntegerStrings.ToString(i3));
                CharacterAccessory shirtItem = dd.StarterShirts[i3];
                for (int k = 0; k < shirtItem.Palettes.Count; k++)
                {
                    Blob shirtColourBlob = shirt.FetchBlob("palettes").FetchBlob(IntegerStrings.ToString(k));
                    Palette palette4 = default(Palette);
                    GameContext.PaletteDatabase.TryGetPalette(shirtItem.Palettes[k], out palette4);

                    shirtColourBlob.SetString("label", SendDataPatch.LocalisePaletteCode(palette4.Code));
                }
            }

            for (int i2 = 0; i2 < dd.StarterTrousers.Count; i2++)
            {
                Blob trousers = setupData.FetchBlob("Clothing_3").FetchBlob(IntegerStrings.ToString(i2));
                CharacterAccessory trouserItem = dd.StarterTrousers[i2];
                for (int l = 0; l < trouserItem.Palettes.Count; l++)
                {
                    Blob trouserColourBlob = trousers.FetchBlob("palettes").FetchBlob(IntegerStrings.ToString(l));
                    Palette palette5 = default(Palette);
                    GameContext.PaletteDatabase.TryGetPalette(trouserItem.Palettes[l], out palette5);

                    trouserColourBlob.SetString("label", SendDataPatch.LocalisePaletteCode(palette5.Code));
                }
            }

            for (int n = 0; n < dd.StarterShoes.Count; n++)
            {
                Blob shoes = setupData.FetchBlob("Clothing_5").FetchBlob(IntegerStrings.ToString(n));
                CharacterAccessory shoeItem = dd.StarterShoes[n];
                for (int m = 0; m < shoeItem.Palettes.Count; m++)
                {
                    Blob shoeColourBlob = shoes.FetchBlob("palettes").FetchBlob(IntegerStrings.ToString(m));
                    Palette palette6 = default(Palette);
                    GameContext.PaletteDatabase.TryGetPalette(shoeItem.Palettes[m], out palette6);

                    shoeColourBlob.SetString("label", SendDataPatch.LocalisePaletteCode(palette6.Code));
                }
            }
        }

        /// <summary>
        /// Retrieve private field value through reflection
        /// </summary>
        /// <param name="__instance"></param>
        /// <param name="FieldName"></param>
        /// <returns></returns>
        static object GetFieldValue(CharacterCreatorController __instance, string FieldName)
        {
            FieldInfo Field = __instance.GetType().GetField(FieldName, BindingFlags.NonPublic | BindingFlags.Instance);
            Object FieldValue = Field.GetValue(__instance);

            return FieldValue;
        }

        /// <summary>
        /// Attempt to localise a palette code
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        static string LocalisePaletteCode(string code)
        {
            string localisedLabel = ClientContext.LanguageDatabase.GetTranslationString(code);
            return localisedLabel.First().ToString().ToUpper() + localisedLabel.Substring(1);
        }
    }
}
