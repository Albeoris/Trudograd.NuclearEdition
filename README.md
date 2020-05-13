# Trudograd.NuclearEdition
Modification for the game Atom RPG Trudograd

# Install
1. Unpack launcher archive to the **game folder**
2. Unpack mod archive to the %AppData%\..\LocalLow\AtomTeam\Atom_Trudograd\

# Build
1. Open Trudograd.NuclearEdition.csproj via text editor
2. Change hardcoded paths to your own
3. Build in Visual Studio 2019 / JetBrains Rider

# Todo
1. Remove hardcoded paths

# Features
- [Better Bombagan](https://github.com/Albeoris/Trudograd.NuclearEdition/wiki/Features-Bombagan) controls
- [Better highlight](https://github.com/Albeoris/Trudograd.NuclearEdition/wiki/Features-HUD#better-highlight-locked-containers-and-doors) locked containers and doors
- [Auto-lockpick](https://github.com/Albeoris/Trudograd.NuclearEdition/wiki/Features-HUD#auto-lockpick)

Details [here](https://github.com/Albeoris/Trudograd.NuclearEdition/wiki)

# Current loader

```csharp
        private static void LoadMods()
		{
			if (Logger._modsLoaded)
			{
				return;
			}
			Logger._modsLoaded = true;
			try
			{
				string path = Application.persistentDataPath + "/Mods";
				if (Directory.Exists(path))
				{
					foreach (string text in Directory.GetFiles(path, "*.dll", SearchOption.AllDirectories))
					{
						Debug.Log("Load Assembly (" + text + ")");
						try
						{
							foreach (Type type in Assembly.LoadFrom(text).GetTypes())
							{
								if (type.Name == "ModEntryPoint")
								{
									Debug.Log("Found entry point (" + type.FullName + "). Initializing...");
									GameObject gameObject = new GameObject(type.FullName);
									gameObject.AddComponent(type);
									Object.DontDestroyOnLoad(gameObject);
								}
							}
						}
						catch (Exception arg)
						{
							Debug.LogError(string.Format("Failed to load assembly {0}. Error: {1}", text, arg));
						}
					}
				}
			}
			catch (Exception message)
			{
				Debug.LogError(message);
			}
		}
```