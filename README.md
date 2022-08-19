# Saving System for Unity

* Easily save and load primitive objects, Dictionary and other [JsonUtility supported types](https://docs.unity3d.com/2020.1/Documentation/Manual/JSONSerialization.html)
* Save and load complex data consiting of the supported types described above
* Bind saving data to specific _game save_. This allows you to have unified structure of data storing for the different game saves, which are in the separate directories

## Installation

Go to `Window > Package Manager`, click `+` and select `Add package from git URL...` where paste the according url (which can also be found in __Code__ option in the [main github page](https://github.com/solacerxt/Saving-System_Unity)):
```
https://github.com/solacerxt/Saving-System_Unity.git
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
using solacerxt.Saving;

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
Saves.Save(ref videoSettings, "video"); // pass the id (string) as second argument
```

#### Loading
```csharp
var videoSettings = Saves.Load<VideoSettings>("id") ?? new VideoSettings(); // nullable version

Saves.TryLoad("id", ref videoSettings);
```

### Storing the primitive
