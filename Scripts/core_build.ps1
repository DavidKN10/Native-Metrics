<#  
    Build core dll:
        ./core_build.ps1 -Debug 
        ./core_build.ps1 -Release 
        ./core_build.ps1 -Debug -Clean
        ./core_build.ps1 -Release -Clean
#>

param(
    [switch]$Debug,
    [switch]$Release,
    [switch]$Clean
)

# root directory
$RootDir = Resolve-Path (Get-Item "$PSScriptRoot\..").FullName

# core directory
$CoreDir = Join-Path $RootDir "NativeMetrics_core"

# CMake build folders
$BuildDir = Join-Path $RootDir "NativeMetrics_core\build"
$InstallDir = Join-Path $RootDir "NativeMetrics_core\install"

if ($Debug) {
    if ($Clean) {
        Write-Host "Cleaning..."
        if (Test-Path $BuildDir) {
            Remove-Item -Recurse -Force $BuildDir
        }
        if (Test-Path $InstallDir) {
            Remove-Item -Recurse -Force $InstallDir 
        }

        Write-Host "Cleaned build/ and install/."
    }

    Write-Host "Creating debug build with CMake..."

    cmake -S $CoreDir -B $BuildDir
    cmake --build $BuildDir --config Debug

    Write-Host "Done."
}
elseif ($Release) {
    if ($Clean) {
        Write-Host "Cleaning..."
        if (Test-Path $BuildDir) {
            Remove-Item -Recurse -Force $BuildDir
        }
        if (Test-Path $InstallDir) {
            Remove-Item -Recurse -Force $InstallDir 
        }

        Write-Host "Cleaned build/ and install/."
    }

    Write-Host "Creating release build with CMake..."

    cmake -S $CoreDir -B $BuildDir
    cmake --build $BuildDir --config Release

    Write-Host "Done."
}
else {
    if ($Clean) {
        Write-Host "Cleaning..."
        if (Test-Path $BuildDir) {
            Remove-Item -Recurse -Force $BuildDir
        }
        if (Test-Path $InstallDir) {
            Remove-Item -Recurse -Force $InstallDir 
        }

        Write-Host "Cleaned build/ and install/."
    }
    
    Write-Host "Build type not specified."
    Write-Host "Creating release build with CMake..."
     
    cmake -S $CoreDir -B $BuildDir
    cmake --build $BuildDir --config Release

    Write-Host "Done."
}
