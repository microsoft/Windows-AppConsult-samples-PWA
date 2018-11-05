## My FullTrust PWA

This demonstrates a Progressive Web App with the follow components:

1. WRCBackgroundTasks (Windows Runtime Compoment)
Contains the following background tasks:
	
   a. Pre-Install (Applies to OEM scenarios only)
    
    b. Timer
    
    c. Connected Session
    
    d. Application
    
2. My PWA - Simple PWA app host local content
This registers the background tasks. See js/main.js

3. LauncherWin32 - This is a full trust app that launches 'MyPWA' from the Preinstall task using the API 
```C#
FullTrustProcessLauncher.LaunchFullTrustProcessForCurrentAppAsync(); 
```
This was necessary since a requirement was to launch from pre-install - before the app protocol is registered at install time.

The app is Full Trust, with a custom protoco and full trust process defined. See package.appxmanifest.

Note addition of <build> section to appxmanifest, this was workaround to a problem with full trust apps require special build handling - this is done if the app is packaged with a Desktop Packaging Project - however, PWAs can't be adding to packaging projects since they require only projects with EXE output to be added.

## To Test Pre Install Task

* Set project architecture configuration to x86
* Create AppxBundle  
  * Right click MyPWA  
  * Store | Associate App with Store  
  * Store | Create App Packages
* Navigate to MyUWP1.Package\AppPackages
  * Note: You appxupload file is here. Used for Dev Center upload.
* Navigate to packages directory just created.
  * You will find the appxbundle for testing here.
* Start Powershell (Admin)
  * The following command will run the pre-install task and install the app
  * ```Add-AppxProvisionedPackage -online -SkipLicense -PackagePath <filename>.appxbundle```
  * It may take up to 15 seconds for the pre-install task to run
 
## PKG cmd file

See the file ```pkg.cmd``` for useful commands for adding, removing and enumerating packages.
