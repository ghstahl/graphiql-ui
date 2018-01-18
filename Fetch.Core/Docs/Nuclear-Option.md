[electron-edge-js issue](https://github.com/agracio/electron-edge-js/issues/4)  

I have found some issues that I have yet been able to explain.
1. Loading Fetch.Core.dll from the Fetch.Core project bin folder works, but when I attach to electron from the debugger I can't set breakpoints that get hit. 
2. Loading Fetch.Core.dll from the Hello.Console PublishOutput works, but this time when I attatch the debugger my breakpoints get hit. 
3. When I use System.Composition, i.e. MEF, it works with all my unit test.  However when Fetch.Core.dll is loaded from electron it can't find any objects.  I had to roll my own Reflection find to get over this hump.  I still want to use MEF, so I will visit this later.  


# Nuclear Option: Everything in one directory experiment

1. I created a Hello.Console, just so I could use "Publish to Folder"
2. Referenced Fetch.Core.  NOTE: my Fetch.Core.csproj is as follows;  Fetch.Core is really the C# app here, i.e the main entry point.   
```
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>netcoreapp2.0</TargetFramework>
    <PreserveCompilationContext>true</PreserveCompilationContext>
  </PropertyGroup>

  <ItemGroup>
    <ProjectReference Include="..\Fetch.Core\Fetch.Core.csproj" />
  </ItemGroup>

  <ItemGroup>
    <Reference Include="EdgeJs">
      <HintPath>..\..\node_modules\electron-edge-js\lib\bootstrap\bin\Release\netcoreapp2.0\EdgeJs.dll</HintPath>
    </Reference>
  </ItemGroup>
  
</Project>
```
### Fetch.Core.csproj is our main entry point, so treate it as an app.  
```
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <TargetFramework>netcoreapp2.0</TargetFramework>
    <PreserveCompilationContext>true</PreserveCompilationContext>
    <CopyLocalLockFileAssemblies>true</CopyLocalLockFileAssemblies>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="Newtonsoft.Json" Version="10.0.3" />
    <PackageReference Include="Microsoft.CodeAnalysis" Version="2.6.1" />
    <PackageReference Include="Microsoft.CSharp" Version="4.4.1" />
    <PackageReference Include="Microsoft.DotNet.InternalAbstractions" Version="1.0.0" />
    <PackageReference Include="Microsoft.Extensions.DependencyModel" Version="2.0.4" />
  </ItemGroup>
  <ItemGroup>
    <ProjectReference Include="..\Command.Common\Command.Common.csproj" />
    <ProjectReference Include="..\Command.FileLoader\Command.FileLoader.csproj" />
    <ProjectReference Include="..\Command.POC.Callbacks\Command.POC.Callbacks.csproj" />
    <ProjectReference Include="..\Programs.Models\Programs.Models.csproj" />
    <ProjectReference Include="..\Programs.Repository\Programs.Repository.csproj" />
    <ProjectReference Include="..\ProgramsCommand\ProgramsCommand.csproj" />
    <ProjectReference Include="..\SimpleCommands\SimpleCommands.csproj" />
    <ProjectReference Include="..\Synoptic.CommandAction\Synoptic.CommandAction.csproj" />
  </ItemGroup>
  <ItemGroup>
    <Reference Include="EdgeJs">
      <HintPath>..\..\node_modules\electron-edge-js\lib\bootstrap\bin\Release\netcoreapp2.0\EdgeJs.dll</HintPath>
      <Private>true</Private>
    </Reference>
  </ItemGroup>
</Project>
```  

3. Publish to folder, which brought in 170 dlls (WOW, just for my little ole app).  All my dlls are there, including Newtonsoft.Json.dll, etc. 

4. Modified my main.js to look like this;  
```
const electron = require('electron')
const path = require('path')
const url = require('url')
const baseNetAppPath = path.join(__dirname, '\\LocalFetch\\Hello.Console\\bin\\Debug\\PublishOutput');

process.env.EDGE_USE_CORECLR = 1;
process.env.EDGE_APP_ROOT = baseNetAppPath;

var edge = require('electron-edge-js');
// rest of file

var localFetch = edge.func({
    assemblyFile: path.join(baseNetAppPath, 'Fetch.Core.dll'),
    typeName: 'Fetch.Core.Local',
    methodName: 'Fetch'
  });


  localFetch({
      url: 'local://v1/programs/is-installed',
      method: 'GET',
      headers: {
          'Content-Type': 'application/json',
          'X-Symc-Fetch-App-Version': '1.0'
      },
      body: {
          displayName: 'Norton Internet Security'
      }
  }, function(error, result) {
      if (error) throw error;
      console.log(result);
  });
  
```
5. Modified .\node_modules\electron-edge-js\lib\edge.js, because I am a netcoreapp2.0 project.  
NOTE: I was told this wasn not necessary as 1.1 is compatible with 2.0, but I like accuracy.
```
if (process.env.EDGE_USE_CORECLR && !process.env.EDGE_BOOTSTRAP_DIR && fs.existsSync(path.join(__dirname, 'bootstrap', 'bin', 'Release', 'netcoreapp2.0', 'bootstrap.dll'))) {
    process.env.EDGE_BOOTSTRAP_DIR = path.join(__dirname, 'bootstrap', 'bin', 'Release', 'netcoreapp2.0');
}
```

# And it works.

This is actually how I would want to have it work when I publish/install the app in its final form.  Everything side-by-side, with no risk of magic dlls anywhere.  My dlls, Microsofts dlls, third party dlls, edge dlls, etc.
