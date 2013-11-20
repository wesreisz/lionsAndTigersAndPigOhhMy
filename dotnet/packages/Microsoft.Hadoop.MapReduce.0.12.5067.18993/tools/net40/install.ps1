param($installPath, $toolsPath, $package, $project)

$dir = split-Path $MyInvocation.MyCommand.Path;
& "$dir\a56ec341687e41e7ba79bcbab69396f3.ps1" $installPath $toolsPath $package $project;

