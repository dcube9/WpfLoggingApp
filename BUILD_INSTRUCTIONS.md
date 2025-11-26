# Build Instructions - WpfLoggingApp

## Prerequisites

- **Operating System**: Windows (WPF requires Windows)
- **.NET Framework**: 4.5.2 or higher
- **IDE**: Visual Studio 2015 or later (recommended: Visual Studio 2017, 2019, or 2022)
  - Community Edition is free and sufficient
  - Download from: https://visualstudio.microsoft.com/

## Method 1: Build with Visual Studio (Recommended)

### Step 1: Clone the Repository
```bash
git clone https://github.com/dcube9/WpfLoggingApp.git
cd WpfLoggingApp
```

### Step 2: Open the Solution
1. Navigate to the cloned directory
2. Double-click `WpfLoggingApp.sln`
3. Visual Studio will open the solution

### Step 3: Restore NuGet Packages
Visual Studio should automatically restore NuGet packages. If not:
1. Right-click on the solution in Solution Explorer
2. Select "Restore NuGet Packages"
3. Wait for the restore to complete

### Step 4: Build the Solution
1. Press `F6` or
2. Go to `Build` → `Build Solution`
3. Wait for the build to complete

### Step 5: Run the Application
1. Press `F5` to run with debugging, or
2. Press `Ctrl+F5` to run without debugging, or
3. Go to `Debug` → `Start Debugging`

The application window should appear with:
- Start and Stop buttons at the top
- Console logging area below with green text on dark background

## Method 2: Build from Command Line

### Prerequisites
- MSBuild (comes with Visual Studio or .NET Framework SDK)
- NuGet command-line tool

### Step 1: Restore NuGet Packages
```bash
nuget restore WpfLoggingApp.sln
```

### Step 2: Build with MSBuild
```bash
msbuild WpfLoggingApp.sln /p:Configuration=Release
```

### Step 3: Run the Application
```bash
cd WpfLoggingApp\bin\Release
WpfLoggingApp.exe
```

## Troubleshooting

### NuGet Package Restore Fails
**Solution**: 
- Check internet connection
- Clear NuGet cache: `nuget locals all -clear`
- Try restoring again

### Build Errors about .NET Framework 4.5.2
**Solution**:
- Install .NET Framework 4.5.2 Developer Pack
- Download from: https://dotnet.microsoft.com/download/dotnet-framework

### "The type or namespace name 'NLog' could not be found"
**Solution**:
- Ensure NuGet packages are restored
- Check that `packages.config` exists
- Try cleaning and rebuilding: `Build` → `Clean Solution`, then `Build` → `Build Solution`

### Application Starts but Console Area is Empty
**Solution**:
- Click the "Start" or "Stop" buttons to generate log messages
- Check that `NLog.config` is being copied to output directory (it should be set to "Copy if newer")

## File Structure After Build

```
WpfLoggingApp/
├── bin/
│   ├── Debug/          or    Release/
│   │   ├── WpfLoggingApp.exe      # The application executable
│   │   ├── WpfLoggingApp.exe.config
│   │   ├── NLog.dll               # NLog dependency
│   │   ├── NLog.config            # NLog configuration
│   │   └── logs/                  # Log files will be created here
│   │       └── logfile_YYYY-MM-DD.log
```

## Running the Application

Once built, you can run the application by:
1. Using the executable directly from the `bin` folder
2. Creating a shortcut to the executable
3. Running from Visual Studio (F5)

## Logs

The application creates two types of logs:
1. **UI Console**: Visible in the application window (green text on dark background)
2. **File Logs**: Located in `bin/Debug/logs/` or `bin/Release/logs/`
   - File naming: `logfile_YYYY-MM-DD.log` (one file per day)
   - Example: `logfile_2025-11-26.log`

## Testing the Application

1. Run the application
2. Click the "Start" button
   - Should see: `HH:MM:SS | INFO | Start button pressed` in the console
3. Click the "Stop" button
   - Should see: `HH:MM:SS | INFO | Stop button pressed` in the console
4. Check the log file in the `logs` folder
   - Should contain the same messages with more details

## Clean Build

If you encounter issues, try a clean build:

**Visual Studio:**
1. `Build` → `Clean Solution`
2. Close Visual Studio
3. Delete `bin` and `obj` folders from the project directory
4. Reopen solution
5. `Build` → `Rebuild Solution`

**Command Line:**
```bash
msbuild WpfLoggingApp.sln /t:Clean
msbuild WpfLoggingApp.sln /t:Rebuild /p:Configuration=Release
```

## Support

For issues or questions:
- Check the main README.md for architecture details
- Review the code comments in the source files
- Check NLog documentation: https://nlog-project.org/
