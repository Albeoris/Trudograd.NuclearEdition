using System;
using System.IO;
using System.Reflection;
using HarmonyLib;
using UnityEngine;

namespace Trudograd.NuclearEdition
{
    // This is the entry point.
    // It should be named ModEntryPoint.
    // It should be inherited from MonoBehaviour.
    // This component will be instantiated while loading mods. You control the lifetime of the object.
    public class ModEntryPoint : MonoBehaviour
    {
        // private Single _recheckTimeSec = 0;

        void Awake()
        {
            try
            {
                var allInstances = FindObjectsOfType<ModEntryPoint>();
                if (allInstances.Length > 1)
                {
                    Debug.Log($"[{nameof(NuclearEdition)}] Another instance of {typeof(ModEntryPoint)} was instantiated. Will destroy this: {this.gameObject.GetInstanceID()}");
                    DestroyImmediate(this);
                    return;
                }
                Harmony harmony = new Harmony("https://github.com/Albeoris/Trudograd.NuclearEdition");
                harmony.PatchAll(Assembly.GetExecutingAssembly());

                Debug.Log($"[{nameof(NuclearEdition)}] Successfully patched via Harmony.");
            }
            catch (Exception ex)
            {
                Debug.Log($"[{nameof(NuclearEdition)}] Failed to patch via Harmony. Error: {ex}");
            }
        }

        // void Update()
        // {
        //     try
        //     {
        //         // Exit to the main menu unloads game scenes and interface objects. We must ensure that our mod remains active.
        //         // Recheck it every 10 seconds
        //         _recheckTimeSec += Time.deltaTime;
        //         if (_recheckTimeSec < 10.0 /*sec*/)
        //             return;
        //
        //         _recheckTimeSec = 0;
        //
        //         // Don't use Awake()
        //         // Now the initialization of mods occurs before the initialization of the environment of the game.
        //         // We have to wait for it to be initialized.
        //         BarterHUD barterHud = Game.World?.HUD?.Barter;
        //         if (barterHud == null) // Not yet initialized
        //             return;
        //
        //         // Initialize our mod
        //         // Initialize(barterHud);
        //
        //         // We can destroy this component after initialization but this mod will be unloaded when the player exit to the main menu. Leave it active in the background.
        //         // Destroy(this.gameObject);
        //     }
        //     catch (Exception ex)
        //     {
        //         Debug.LogError("[Trudograd.NuclearEdition] Something went wrong. Modification will be disabled. Error: " + ex);
        //         Destroy(this.gameObject);
        //     }
        // }
        //
        // private void Initialize(BarterHUD barterHud)
        // {
        //     //foreach (var tabs in FindObjectsOfType<TeammateTabsHUD>())
        //     //{
        //
        //     //}
        //
        //     ExtendedBarterHUD extendedHud = barterHud.gameObject.GetComponent<ExtendedBarterHUD>();
        //     if (extendedHud == null)
        //         InitializeExtendedBarterHUD(barterHud);
        // }
        //
        // private static void InitializeExtendedBarterHUD(BarterHUD barterHud)
        // {
        //     ExtendedBarterHUD extendedHud;
        //     try
        //     {
        //         // Add our own component to an existing interface object.
        //         extendedHud = barterHud.gameObject.AddComponent<ExtendedBarterHUD>();
        //         extendedHud.Initialize(barterHud);
        //
        //         Debug.Log("[Trudograd.NuclearEdition] ExtendedBarterHUD is initialized.");
        //     }
        //     catch (Exception ex)
        //     {
        //         Debug.LogError("[Trudograd.NuclearEdition] Failed to initialize ExtendedBarterHUD: " + ex);
        //     }
        // }
    }
}