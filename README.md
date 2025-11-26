# WpfLoggingApp

Applicazione WPF in C# con console di logging integrata.

## Caratteristiche

- .NET Framework 4.5.2
- NLog per il logging (v4.7.15)
- Dependency Injection con ServiceLocator
- Console di logging integrata con stile terminale
- Pattern MVVM

## Struttura del Progetto

```
WpfLoggingApp/
├── WpfLoggingApp.sln                    # Solution file
├── WpfLoggingApp/
│   ├── WpfLoggingApp.csproj             # Project file
│   ├── App.xaml                         # Application definition
│   ├── App.xaml.cs                      # Application code-behind with DI setup
│   ├── MainWindow.xaml                  # Main window UI
│   ├── MainWindow.xaml.cs               # Main window code-behind
│   ├── NLog.config                      # NLog configuration
│   ├── packages.config                  # NuGet packages
│   ├── ViewModels/
│   │   └── MainViewModel.cs             # ViewModel with business logic
│   ├── Services/
│   │   ├── ILoggerService.cs            # Logger service interface
│   │   ├── LoggerService.cs             # Logger service implementation
│   │   └── ServiceLocator.cs            # Simple DI container
│   ├── Logging/
│   │   └── WpfTarget.cs                 # Custom NLog target for WPF UI
│   └── Properties/
│       └── AssemblyInfo.cs              # Assembly information
```

## Requisiti

- Windows OS
- .NET Framework 4.5.2 o superiore
- Visual Studio 2015 o superiore (consigliato Visual Studio 2017/2019/2022)

## Come Compilare

### Metodo 1: Visual Studio

1. Clona il repository
2. Apri `WpfLoggingApp.sln` in Visual Studio
3. Ripristina i pacchetti NuGet (click destro sulla soluzione → "Restore NuGet Packages")
4. Compila la soluzione (Build → Build Solution o F6)
5. Esegui l'applicazione (F5 o Debug → Start Debugging)

### Metodo 2: Command Line con MSBuild

```bash
# Ripristina i pacchetti NuGet
nuget restore WpfLoggingApp.sln

# Compila la soluzione
msbuild WpfLoggingApp.sln /p:Configuration=Release
```

## Funzionalità

### Interfaccia Utente

- **Bottone Start**: Quando premuto, logga un messaggio che indica l'avvio
- **Bottone Stop**: Quando premuto, logga un messaggio che indica l'arresto
- **Area Console**: 
  - Sfondo: grigio scuro (#1E1E1E)
  - Testo: verde (#00FF00)
  - Font: Courier New (monospace)
  - Auto-scroll automatico verso il basso
  - Mantiene le ultime 10,000 righe in memoria
  - Si ridimensiona automaticamente con la finestra

### Logging

I log vengono scritti in due destinazioni:

1. **File**: `logs/logfile_YYYY-MM-DD.log` (nella directory dell'applicazione)
2. **Console UI**: Area console integrata nella finestra principale

#### Livelli di Log

- **Debug**: Solo su file
- **Info**: File e console UI
- **Warning**: File e console UI  
- **Error**: File e console UI

### Architettura

#### Dependency Injection

L'applicazione utilizza un ServiceLocator personalizzato per la dependency injection:

```csharp
// Registrazione dei servizi (in App.xaml.cs)
ServiceLocator.Instance.RegisterSingleton<ILoggerService, LoggerService>();

// Risoluzione dei servizi
var loggerService = ServiceLocator.Instance.Resolve<ILoggerService>();
```

#### Pattern MVVM

- **Model**: Gestito da NLog e servizi
- **View**: MainWindow.xaml
- **ViewModel**: MainViewModel.cs

#### Custom NLog Target

`WpfTarget` è un custom target di NLog che invia i messaggi di log all'interfaccia utente:

```xml
<target name="wpf" 
        xsi:type="WpfTarget"
        layout="${time} | ${level:uppercase=true} | ${message}" />
```

## Stile del Codice

Il codice segue queste linee guida:

- **Naming Convention**: 
  - PascalCase per tipi pubblici
  - camelCase per variabili locali e parametri
  - NO underscore prefixes
- **Uso di `var`** quando il tipo è chiaro dal contesto
- **Null coalescing operator** (`??`) quando appropriato
- **Braces `{}`** sempre utilizzate, anche per blocchi su una sola riga
- Thread-safe singleton pattern con `Lazy<T>`

## Note di Sicurezza

- Il codice è stato analizzato con CodeQL e non presenta vulnerabilità note
- ServiceLocator implementa lock per thread safety
- Gestione corretta degli eventi con unsubscribe nel cleanup

## Licenza

Copyright © 2025
