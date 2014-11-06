Modularity for UniversalApps
=======================

A modularity sample for UniversalApps.
Feel free to reuse in your application as-is.

Microsoft.TED.WinRT.Modules
-------------------------------
Adds a ModuleManager class that loads dependencies in a definable order.

    ModuleManager manager = new ModuleManager();
    manager.AddModule(name: "Module1", entryPoint: "Module1.Module, Module1", dependsOn: new[] { "Module2", "Module3" });
    manager.AddModule(name: "Module2", entryPoint: "Module2.Module, Module2");
    manager.AddModule(name: "Module2", entryPoint: "Module3.Module, Module3");
    await manager.InitializeAsync();

Each Module has an EntryPoint class, that is being called on initialization:

    public class Module : IModule
    {
        public async Task InitializeAsync()
        {
           // ...
        }
    }
    
![modules](https://cloud.githubusercontent.com/assets/3861846/4940121/bdb1fb44-65d7-11e4-9d47-ed0585fef583.png)

Microsoft.TED.WinRT.ModularHub
-------------------------------
A WinRT Hub derived control, that can be extended from losely coupled modules.

  1) Each Module can add content to a global hub, via an IHubMetadataService singleton:

    _hubMetadataService.AddHubSection("M2", "Cats", typeof(CatListView).AssemblyQualifiedName);
    
  2) Place the hub in a page & bind it:

    <hub:ModularHub ItemsSource="{Binding HubSections}"/>
    
  3) Use the IHubMetadataService singleton to fetch the hub sections from modules in the ViewModel

    this.HubSections = new ObservableCollection<HubMetadata>(_hubMetadataService.GetSections());
