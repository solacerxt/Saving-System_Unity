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
The static class `Saves` allows you to load and save data with appropriate comprehensive methods. Basically, it works with serializable structures 'implementing' `IStorable` interface _(this is an empty interface, made for constraining generic type parameter)_. These structures should have name with 'S' prefix (Storable)

## Usage
### Storing the primitive
