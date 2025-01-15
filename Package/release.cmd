del "*.nupkg"
"..\..\oqtane.framework\oqtane.package\nuget.exe" pack Devessence.Module.OrgChart.nuspec 
XCOPY "*.nupkg" "..\..\oqtane.framework\Oqtane.Server\Packages\" /Y

