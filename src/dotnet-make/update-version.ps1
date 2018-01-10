Function Get-ScriptDirectory { Split-Path -parent $PSCommandPath }

$attribute = Get-Content "$(Get-ScriptDirectory)\Properties\AssemblyInfo.cs" |? { $_ -match "AssemblyFileVersion" } | Select-Object -First 1
$attribute -match '[0-9]+(\.[0-9]+){2,3}'
$version = $matches[0]
Set-Content -Path "$(Get-ScriptDirectory)\dotnet-make.csproj" -Value (Get-Content "$(Get-ScriptDirectory)\dotnet-make.csproj").Replace("<VersionPrefix>42.43.44.45</VersionPrefix>", "<VersionPrefix>$($version)</VersionPrefix>")