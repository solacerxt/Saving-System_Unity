# Save System for Unity

* Easily save and load primitive objects, Dictionary and other [JsonUtility supported types](https://docs.unity3d.com/2020.1/Documentation/Manual/JSONSerialization.html)
* Save and load complex data consiting of the supported types described above
* Bind saving data to specific _game save_. This allows you to have unified structure of data storing for the different game saves, which are in the separate directories

## Installation

Go to `Window > Package Manager`, click `+` and select `Add package from git URL...` where paste the according url (which can also be found in __Code__ option in the [main github page](https://github.com/solacerxt/Saving-System_Unity)):
```
https://github.com/solacerxt/Save-System_Unity.git
```

_Make sure that your Unity version is >= 2021.3.8f1_

## Overview
The static class `Saves` allows you to load and save data with appropriate comprehensive methods. Basically, it works with serializable structures 'implementing' `IStorable` interface _(this is an empty interface, made for constraining generic type parameter)_

## Usage
### General case
Let's say we need to save video settings:
```
screenSize: uint | Fullscreen
vSync: bool
particleEffects: None | Low | Normal | High
```

To store them, we define an IStorable structure with [System.Serializable] attribute:
```csharp
using solacerxt.SaveSystem;

[System.Serializable]
public struct VideoSettings : IStorable
{
    public ScreenSize screenSize;
    public bool vSync;
    public ParticleEffects particleEffects;
    
    [System.Serializable]
    public struct ScreenSize
    {
        public uint scale;
        public bool fullScreen;
    }
    
    public enum ParticleEffects { None, Low, Normal, High }
}
```

#### Saving
```csharp
var videoSettings = new VideoSettings();
...
Saves.Save("video", videoSettings); // pass the id (string) as a first parameter
```

#### Loading
```csharp
var videoSettings = Saves.Load<VideoSettings>("id") ?? new VideoSettings(); // nullable version

Saves.TryLoad("id", ref videoSettings); // returns true and rewrites videoSettings if success, false otherwise
```

### Storing the primitive
Imagine we need to save app language:
```csharp
public enum Language { English, Russian, Japanese ... }
```
We can define this structure
```csharp
using solacerxt.SaveSystem;

[System.Serializable]
public struct StorableLanguage : IStorable
{
    public Language value;
}
```
But the better solution is to create a generic version:
```csharp
using solacerxt.SaveSystem;

[System.Serializable]
public struct StorableBox<T> : IStorable
{
    public T value;
}
```
and use `StorableBox<Language>` for this purpose, so we don't have to define new stuctures to store other primitives

However, you don't need to define this structure manually, there is already a built-in `SBox<T>`. 

Moreover, you can use special methods for this, such as:
#### Saving
```csharp
var lang = Language.English;
Saves.SaveBoxed("lang", lang);
Saves.SaveBoxed("lang", Language.Japanese);
```
#### Loading
```csharp
Saves.TryLoadFromBox("lang", ref lang);
lang = Saves.LoadFromBoxOrDefault("lang", Language.English);
```
### Binding data to the game save
For games, where player can have multiple game saves (e.g. Hollow Knight), it's important to save all game data for specific game save in separate place. 

Your may have some type of this class for game entity in your app:
```csharp
public class Game
{
    ...
    public static Game? Current { get; private set; }
    ...
}
```

`Saves` have methods for loading and saving that requires the parameter `ISavedGame savedGame`, where `ISavedGame` requires to have `int Index { get; }` property, so game saves are identified by its Index:
```csharp
public class Game : ISavedGame
{
    ...
    public static Game? Current { get; private set; }

    public int Index { get; private set; }
    ...
}
```
Now we can load and save data for current game:
```csharp
var inventoryData = new InventoryData(...);
Saves.Save("inventory", Game!.Current, inventoryData);
Saves.TryLoad("inventory", Game!.Current, ref inventoryData);
```
