# ReactiveUI.SourceGenerators
Use source generators to generate ReactiveUI objects.

These Source Generators were designed to work in full with ReactiveUI V19.5.31 and newer supporting all features, currently:
- [Reactive]
- [ObservableAsProperty]
- [ObservableAsProperty(PropertyName = "ReadOnlyPropertyName")]
- [ReactiveCommand]
- [ReactiveCommand(CanExecute = nameof(IObservableBoolName))] with CanExecute
- [ReactiveCommand][property: AttribueToAddToCommand] with Attribute passthrough
- [IViewFor(nameof(ViewModelName))]

Versions older than V19.5.31 to this:
- [Reactive] fully supported, 
- [ObservableAsProperty] fully supported, 
- [ReactiveCommand] all options supported except Cancellation Token asnyc methods.

# Historical ways
## Read-write properties
Typically properties are declared like this:

```csharp
private string _name;
public string Name 
{
    get => _name;
    set => this.RaiseAndSetIfChanged(ref _name, value);
}
```

Before these Source Generators were avaliable we used ReactiveUI.Fody.
With ReactiveUI.Fody the `[Reactive]` Attribute was placed on a Public Property with Auto get / set properties, the generated code from the Source Generator and the Injected code using Fody are very similar with the exception of the Attributes.
```csharp
[Reactive]
public string Name { get; set; }
```

## ObservableAsPropertyHelper properties
Similarly, to declare output properties, the code looks like this:
```csharp
public partial class MyReactiveClass : ReactiveObject
{
    ObservableAsPropertyHelper<string> _firstName;

    public MyReactiveClass()
    {
        _firstName = firstNameObservable
            .ToProperty(this, x => x.FirstName);
    }

    public string FirstName => _firstName.Value;

    private IObservable<string> firstNameObservable() => Observable.Return("Test");
}
```

With ReactiveUI.Fody, you can simply declare a read-only property using the [ObservableAsProperty] attribute, using either option of the two options shown below.
```csharp
[ObservableAsProperty]
public string FirstName { get; }
```

# Welcome to a new way - Source Generators

## Usage Reactive property `[Reactive]`
```csharp
using ReactiveUI.SourceGenerators;

public partial class MyReactiveClass : ReactiveObject
{
    [Reactive]
    private string _myProperty;
}
```

## Usage ObservableAsPropertyHelper `[ObservableAsProperty]`

### Usage ObservableAsPropertyHelper with Field
```csharp
using ReactiveUI.SourceGenerators;

public partial class MyReactiveClass : ReactiveObject
{
    [ObservableAsProperty]
    private string _myProperty = "Default Value";

    public MyReactiveClass()
    {
        _myPrpertyHelper = MyPropertyObservable()
            .ToProperty(this, x => x.MyProperty);
    }

    IObservable<string> MyPropertyObservable() => Observable.Return("Test Value");
}
```

### Usage ObservableAsPropertyHelper with Observable Property
```csharp
using ReactiveUI.SourceGenerators;

public partial class MyReactiveClass : ReactiveObject
{    
    public MyReactiveClass()
    { 
        // Initialize generated _myObservablePropertyHelper
        // for the generated MyObservableProperty
        InitializeOAPH();
    }

    [ObservableAsProperty]
    IObservable<string> MyObservable => Observable.Return("Test Value");
}
```

### Usage ObservableAsPropertyHelper with Observable Property and specific PropertyName
```csharp
using ReactiveUI.SourceGenerators;

public partial class MyReactiveClass : ReactiveObject
{    
    public MyReactiveClass()
    { 
        // Initialize generated _testValuePropertyHelper
        // for the generated TestValueProperty
        InitializeOAPH();
    }

    [ObservableAsProperty(PropertyName = TestValueProperty)]
    IObservable<string> MyObservable => Observable.Return("Test Value");
}
```

### Usage ObservableAsPropertyHelper with Observable Method

NOTE: This does not support methods with parameters
```csharp
using ReactiveUI.SourceGenerators;

public partial class MyReactiveClass : ReactiveObject
{    
    public MyReactiveClass()
    { 
        // Initialize generated _myObservablePropertyHelper
        // for the generated MyObservableProperty
        InitializeOAPH();
    }

    [ObservableAsProperty]
    IObservable<string> MyObservable() => Observable.Return("Test Value");
}
```

### Usage ObservableAsPropertyHelper with Observable Method and specific PropertyName
```csharp
using ReactiveUI.SourceGenerators;

public partial class MyReactiveClass : ReactiveObject
{    
    public MyReactiveClass()
    { 
        // Initialize generated _testValuePropertyHelper
        // for the generated TestValueProperty
        InitializeOAPH();
    }

    [ObservableAsProperty(PropertyName = TestValueProperty)]
    IObservable<string> MyObservable() => Observable.Return("Test Value");
}
```

## Usage ReactiveCommand `[ReactiveCommand]`

### Usage ReactiveCommand without parameter
```csharp
using ReactiveUI.SourceGenerators;

public partial class MyReactiveClass
{
    public MyReactiveClass()
    {
        InitializeCommands();
    }

    [ReactiveCommand]
    private void Execute() { }
}
```

### Usage ReactiveCommand with parameter
```csharp
using ReactiveUI.SourceGenerators;

public partial class MyReactiveClass
{
    public MyReactiveClass()
    {
        InitializeCommands();
    }

    [ReactiveCommand]
    private void Execute(string parameter) { }
}
```

### Usage ReactiveCommand with parameter and return value
```csharp
using ReactiveUI.SourceGenerators;

public partial class MyReactiveClass
{
    public MyReactiveClass()
    {
        InitializeCommands();
    }

    [ReactiveCommand]
    private string Execute(string parameter) => parameter;
}
```

### Usage ReactiveCommand with parameter and async return value
```csharp
using ReactiveUI.SourceGenerators;

public partial class MyReactiveClass
{
    public MyReactiveClass()
    {
        InitializeCommands();
    }

    [ReactiveCommand]
    private async Task<string> Execute(string parameter) => await Task.FromResult(parameter);
}
```

### Usage ReactiveCommand with IObservable return value
```csharp
using ReactiveUI.SourceGenerators;

public partial class MyReactiveClass
{
    public MyReactiveClass()
    {
        InitializeCommands();
    }

    [ReactiveCommand]
    private IObservable<string> Execute(string parameter) => Observable.Return(parameter);
}
```

### Usage ReactiveCommand with CancellationToken
```csharp
using ReactiveUI.SourceGenerators;

public partial class MyReactiveClass
{
    public MyReactiveClass()
    {
        InitializeCommands();
    }

    [ReactiveCommand]
    private async Task Execute(CancellationToken token) => await Task.Delay(1000, token);
}
```

### Usage ReactiveCommand with CancellationToken and parameter
```csharp
using ReactiveUI.SourceGenerators;

public partial class MyReactiveClass
{
    public MyReactiveClass()
    {
        InitializeCommands();
    }

    [ReactiveCommand]
    private async Task<string> Execute(string parameter, CancellationToken token)
    {
        await Task.Delay(1000, token);
        return parameter;
    }
}
```

### Usage ReactiveCommand with CanExecute
```csharp
using ReactiveUI.SourceGenerators;

public partial class MyReactiveClass
{
    private IObservable<bool> _canExecute;

    [Reactive]
    private string _myProperty1;

    [Reactive]
    private string _myProperty2;

    public MyReactiveClass()
    {
        InitializeCommands();
        _canExecute = this.WhenAnyValue(x => x.MyProperty1, x => x.MyProperty2, (x, y) => !string.IsNullOrEmpty(x) && !string.IsNullOrEmpty(y));
    }

    [ReactiveCommand(CanExecute = nameof(_canExecute))]
    private void Search() { }
}
```

### Usage ReactiveCommand with property Attribute pass through
```csharp
using ReactiveUI.SourceGenerators;

public partial class MyReactiveClass
{
    private IObservable<bool> _canExecute;

    [Reactive]
    private string _myProperty1;

    [Reactive]
    private string _myProperty2;

    public MyReactiveClass()
    {
        InitializeCommands();
        _canExecute = this.WhenAnyValue(x => x.MyProperty1, x => x.MyProperty2, (x, y) => !string.IsNullOrEmpty(x) && !string.IsNullOrEmpty(y));
    }

    [ReactiveCommand(CanExecute = nameof(_canExecute))]
    [property: JsonIgnore]
    private void Search() { }
}
```

## Usage IViewFor `[IViewFor(nameof(ViewModelName))]`

### IViewFor usage

IVIewFor is used to link a View to a ViewModel, this is used to link the ViewModel to the View in a way that ReactiveUI can use it to bind the ViewModel to the View.
The ViewModel is passed as a string to the IViewFor Attribute.
The class must inherit from a UI Control from any of the following platforms and namespaces:
- Maui (Microsoft.Maui)
- WinUI (Microsoft.UI.Xaml)
- WPF (System.Windows or System.Windows.Controls)
- WinForms (System.Windows.Forms)
- Avalonia (Avalonia)
- Uno (Windows.UI.Xaml).

```csharp
using ReactiveUI.SourceGenerators;

[IViewFor(nameof(MyReactiveClass))]
public partial class MyReactiveControl : UserControl
{
    public MyReactiveControl()
    {
        InitializeComponent();
        MyReactiveClass = new MyReactiveClass();
    }
}
```

## Platform specific Attributes

### WinForms

#### RoutedControlHost
```csharp
using ReactiveUI.SourceGenerators.WinForms;

[RoutedControlHost("YourNameSpace.CustomControl")]
public partial class MyCustomRoutedControlHost;
```

#### ViewModelControlHost
```csharp
using ReactiveUI.SourceGenerators.WinForms;

[ViewModelControlHost("YourNameSpace.CustomControl")]
public partial class MyCustomViewModelControlHost;
```


### TODO:
- Add ObservableAsProperty to generate from a IObservable method with parameters.
