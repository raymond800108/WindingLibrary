# WindingLibrary

## Contributors
Tech Support (Nicolas Kubail Kalousdian) </br >
Senpai (Shu Chuan Yao) </br >
The Polish Bear (Grzesiek Łochnicki) </br >

## Contents
This repository contains code for the ITECH 2020 Pavillion's toolpath generation system. </br >
The Winding Libary is split into three parts: behaviours, utilities, and class objects.

### Behaviours
WindingBehaviour </br >
TravelBehaviour </br >
AttackAngleBehaviour </br >
PositionerBehaviour </br >
ToolPriority </br >

### Utilities
WindingClassConstructor </br >
SimulationCompiler </br >
KRLCodeGenerator </br >
E6PosGenerator </br >
AxisAngleMonitor </br >
SingularityMonitor </br >

### Objects
WindingClass </br >

## Rhino and Grasshopper Files
The latest version of the Rhino model and Grasshopper file can be found in Google Drive within the Fabrication folder of the current design phase.<br />
[Fabrication Folder](https://drive.google.com/open?id=1-NvEibNWGSiWkUpBPFH5GkPmLzh_SPTq)

## Workflow
In order to keep all the files organized and correctly updated please follow the suggested workflow:
1. Download the current Rhino and GH file from the Google Drive folder: [Fabrication Folder](https://drive.google.com/open?id=1-NvEibNWGSiWkUpBPFH5GkPmLzh_SPTq)
2. Copy this github repository to your computer by using the following command: git clone https://github.com/t3ch-support/WindingLibrary.git
3. Inside of the Fabrication folder you will find a folder called Workspace_VS_File. Download this folder locally and use the Visual Studio Solution provided within as your development environment.
4. Open the VS Solution, and add the .cs files from the github to your project. Make sure you add them as links so that you can push the changes back to the github repository. You can do this by right clicking the solution name -> Add -> Existing Item... -> Add as link..
5. Once you are ready to upload changed code to the master repository, navigate to your local repository, commit your changes, and push. Make sure to provide a description of the changes in your commit.
6. If the components inside of the grasshopper file give you errors, make sure you have referenced the Objects .dll library. You can download this from the Google Drive folder, and add it by right clicking the component -> Manage Assemblies -> Add..
7. Also make sure that the ScriptSync components are pointing to the files inside of your local github repository so they are automatically updated when you save your changes in VS.
8. If there are any questions please email Tech Support