<#  
    Build core library:
        ./build.ps1         # creates a MSVC release build

        ./build.ps1 -{build type} -{build system} 
#>

param(
    [switch]$Debug,
    [switch]$Release,
    [switch]$Msvc,
    [switch]$Ninja
)

# root directory
$RootDir = Resolve-Path (Get-Item "$PSScriptRoot\..").FullName

# core directory
$CoreDir = Join-Path $RootDir "NativeMetrics_core"

if ($Debug) {
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

    Set-Location "..\Scripts"
   
    Write-Host "Done."
}
elseif ($Release) {
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

    Set-Location "..\Scripts"
    
    Write-Host "Done."
}
else {
    Write-Host "Build type not specified."
    Write-Host "Creating release build with CMake..."

    Set-Location $CoreDir; cmake --preset x64-windows-msvc-release
    Set-Location $CoreDir; cmake --build --preset x64-windows-msvc-release

    Set-Location "..\Scripts"

    Write-Host "Done."
}
