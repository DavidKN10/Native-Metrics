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
    [switch]$Msvc,
    [switch]$Ninja,
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
    
    if ($Msvc) {
        Set-Location $CoreDir; cmake --preset x64-windows-msvc-debug
        Set-Location $CoreDir; cmake --build --preset x64-windows-msvc-debug     
    }
    elseif ($Ninja) {
        Set-Location $CoreDir; cmake --preset x64-windows-ninja-debug
        Set-Location $CoreDir; cmake --build --preset x64-windows-ninja-debug
    }
    else {
        Set-Location $CoreDir; cmake --preset x64-windows-msvc-debug
        Set-Location $CoreDir; cmake --build --preset x64-windows-msvc-debug
    }
    

    Set-Location "..\"
   
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

    if ($Msvc) {
        Set-Location $CoreDir; cmake --preset x64-windows-msvc-release
        Set-Location $CoreDir; cmake --build --preset x64-windows-msvc-release     
    }
    elseif ($Ninja) {
        Set-Location $CoreDir; cmake --preset x64-windows-ninja-release
        Set-Location $CoreDir; cmake --build --preset x64-windows-ninja-release
    }
    else {
        Set-Location $CoreDir; cmake --preset x64-windows-msvc-release
        Set-Location $CoreDir; cmake --build --preset x64-windows-msvc-release
    }

    Set-Location "..\"
    
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

    Set-Location $CoreDir; cmake --preset x64-windows-msvc-release
    Set-Location $CoreDir; cmake --build --preset x64-windows-msvc-release

    Set-Location "..\"

    Write-Host "Done."
}
