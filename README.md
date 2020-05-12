# Trudograd.NuclearEdition
Modification for the game Atom RPG Trudograd

# Install
1. Unpack [launcher](https://yadi.sk/d/knyeJOGJ4-OMXA) archive to the **game folder**
2. Unpack [mod](https://yadi.sk/d/b0ZM1W-sveuJSw) archive to the %AppData%\..\LocalLow\AtomTeam\Atom_Trudograd\

# Build
1. Open Trudograd.NuclearEdition.csproj via text editor
2. Change "C:\Git\C#\Trudograd.NuclearEdition\" to your own project path
3. Change "..\..\..\..\Users\Admin\AppData\LocalLow\AtomTeam\Atom_Trudograd\Mods\NuclearEdition\" to your own %AppData%\..\LocalLow\AtomTeam\Atom_Trudograd\Mods\NuclearEdition\ path
4. Build in Visual Studio 2019 / JetBrains Rider

# Todo
1. Remove hardcoded paths

# Feautures
1. Better highlight locked containers and doors

Changed highlight color of environment objects.
Locked containers or doors - Violet.

2. Auto-lockpick

Hold Alt (Highlight) and click on a locked container or door to send a character with maximum skill of lock picking.

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