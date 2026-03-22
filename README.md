# AIS4CH-Model-Tracking
A Unity XR application build for Meta Quest 3 platform for scanning QR codes and rendering 3D objects at runtime.

## Unity setup instructions
Install Unity Editor 2022 or 2023. Do not use Unity 6 as it has compatability issues with most of the packages that are used in the project.

Be sure to create a Unity account and have a Unity ID. Follow the instruction here: https://support.unity.com/hc/en-us/articles/208626336-How-do-I-create-a-Unity-ID-account

Be sure to have a Meta account as it's necessary to use the Meta Quest 3 device.

After cloning the repository and opening it in the Unity Editor be sure to have the following packages installed:
- Meta XR Core SDK v83 or higher (from the Unity Asset Store)
- Meta XR Utility Kit v83 or higher (from the Unity Asset Store)
- Unity glTFast v6.14.1 or higher (from the Unity Package Manager by searching the name "com.unity.cloud.gltfast")

### --NOTE--
As of version 83 of the Meta SDK the QR code tracking feature is no longer an experimental feature so there's no need to enable any special developer feature on the device. If you're on older versions make sure to go into the Meta quest settings and Meta Horizon Link settings and enable the necessary features to run the application.
### --------

If all the packages are correctly imported, be sure to fix all the build validation error by going Edit > Project Settings > Project Validation and clicking Fix All.

In the same window, go to the XR Plug-in Management. Check the `Oculus` option for all platforms. \
**DO NOT USE OPEN XR** \
Finally make sure to switch the build platform from PC to Android.


## Package versions
- Meta XR Core SDK v83.0.0
- Meta XR Utility Kit v83.0.0
- Meta XR Audio SDK v83.0.0
- Meta XR Void SDK v83.0.0
- Meta XR Haptics SDK v83.0.0
- Meta XR Interaction SDK v83.0.0
- Meta XR Platform SDK v83.0.0
- Unity glTFast v6.14.1

## Running the application
The standard way of running the application is by building from File > Build Settings > Build (make sure to select the scene called "MainScene") and installing the .apk result file in the device.

A more convinient way is to run the application using Meta Quest Link but to do that you will need two additional applications:
- Meta Quest Developer Hub
- Meta Quest Link

Connect you Meta account to both and connect the Meta Quest 3 device.

In the Meta Quest Developer Hub go to Settings > Beta Version and make sure to enable developer settings and the rest of the options (they are required for the passthrough and other important features)

Once all is setup, from the Meta Quest 3 settings, activate Meta Link (either by wire or wireless communication). This will take you to a dedicated virtual area where you can check you PC monitors. From there run the Unity application by clikcing the run button in the editor

