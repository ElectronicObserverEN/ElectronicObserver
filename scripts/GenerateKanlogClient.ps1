<#
Simple generator for KanlogHttpClient using NSwag as a local dotnet tool.

Usage:
  powershell -ExecutionPolicy Bypass -File .\scripts\GenerateKanlogClient.ps1
  powershell -ExecutionPolicy Bypass -File .\scripts\GenerateKanlogClient.ps1 -OpenApiUrl <url> -ToolVersion 14.7.1 -Force

Behavior:
 - Uses the OpenAPI URL directly (no local temp file) to avoid path/config issues.
 - Ensures a local dotnet tool manifest exists and NSwag.ConsoleCore is installed as a local tool.
 - Invokes: dotnet tool run nswag openapi2csclient /input:<OpenApiUrl> ...
 - Prints NSwag stdout/stderr for diagnostics.

Notes:
 - Run from a normal PowerShell or VS Terminal (not the Package Manager Console).
 - If execution is blocked, run with -ExecutionPolicy Bypass or set CurrentUser RemoteSigned.
#>

param(
	[string]$OpenApiUrl = 'https://raw.githubusercontent.com/yukikuri/koubrowser/master/openapi/kc-intake.yaml',
	[string]$ToolVersion = '14.7.1',
	[string]$ClassName = 'KanlogHttpClient',
	[string]$Namespace = 'ElectronicObserver.Data.KanlogSubmission',
	[string]$OutputRelative = 'ElectronicObserver/Data/KanlogSubmission/KanlogHttpClient.cs',
	[switch]$Force
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

$ScriptDir = Split-Path -Parent $MyInvocation.MyCommand.Definition
$RepoRoot = (Resolve-Path (Join-Path $ScriptDir '..')).Path

Write-Host "Repository root: $RepoRoot"

# Ensure dotnet CLI is available
try 
{
	& dotnet --version > $null
}
catch
{
	Write-Error 'dotnet CLI not found in PATH. Install .NET SDK.';
	exit 2
}

Push-Location $RepoRoot

# Ensure local tool manifest exists (dotnet-tools.json at repo root or .config)
$manifest1 = Join-Path $RepoRoot 'dotnet-tools.json'
$manifest2 = Join-Path $RepoRoot '.config\dotnet-tools.json'
if (-not (Test-Path $manifest1) -and -not (Test-Path $manifest2))
{
	Write-Host 'Creating local tool manifest (dotnet new tool-manifest)'
	if ($Force) { & dotnet new tool-manifest --force } else { & dotnet new tool-manifest }
}

# Ensure NSwag.ConsoleCore installed as local tool
$toolsList = ''
try
{
	$toolsList = & dotnet tool list --local 2>&1 | Out-String
}
catch
{
	$toolsList = ''
}

if ($toolsList -notmatch 'NSwag.ConsoleCore')
{
	Write-Host "Installing NSwag.ConsoleCore version $ToolVersion as a local tool"
	& dotnet tool install NSwag.ConsoleCore --version $ToolVersion
}
else
{
	Write-Host 'NSwag.ConsoleCore already present in tool manifest'
}

# Restore tools
Write-Host 'Restoring local tools (dotnet tool restore)'
& dotnet tool restore

# Prepare output path
$outputPath = Join-Path $RepoRoot $OutputRelative
$outputDir = Split-Path -Parent $outputPath
if (-not (Test-Path $outputDir))
{
	New-Item -ItemType Directory -Path $outputDir | Out-Null
}

$nswagArgs = @(
	'openapi2csclient',
	"/input:$OpenApiUrl",
	"/classname:$ClassName",
	"/namespace:$Namespace",
	"/output:$outputPath",
	"/injectHttpClient:true",
	"/generateDataAnnotations:true",
	"/generateExceptionClasses:true",
	"/generateDtoTypes:true",
	"/disposeHttpClient:false",
	"/generateOptionalParameters:false",
	"/generateSyncMethods:false"
)

Write-Host "Running: dotnet tool run nswag $($nswagArgs -join ' ')"

# Start process and capture output
$psi = New-Object System.Diagnostics.ProcessStartInfo
$psi.FileName = 'dotnet'
$psi.Arguments = 'tool run nswag ' + ($nswagArgs -join ' ')
$psi.RedirectStandardOutput = $true
$psi.RedirectStandardError = $true
$psi.UseShellExecute = $false
$psi.CreateNoWindow = $true

$proc = New-Object System.Diagnostics.Process
$proc.StartInfo = $psi
$proc.Start() | Out-Null
$stdOutTask = $proc.StandardOutput.ReadToEndAsync()
$stdErrTask = $proc.StandardError.ReadToEndAsync()
$proc.WaitForExit()
$stdout = $stdOutTask.Result.TrimEnd()
$stderr = $stdErrTask.Result.TrimEnd()

if ($proc.ExitCode -ne 0)
{
	Write-Error "NSwag failed with exit code $($proc.ExitCode)"
	if ($stdout)
	{
		Write-Host '--- NSwag stdout ---';
		Write-Host $stdout; Write-Host '--- end stdout ---'
	}

	if ($stderr)
	{
		Write-Host '--- NSwag stderr ---';
		Write-Host $stderr;
		Write-Host '--- end stderr ---'
	}
	
	Pop-Location
	exit $proc.ExitCode
}

Write-Host '--- NSwag stdout ---'
if ($stdout)
{
	Write-Host $stdout
}
else
{
	Write-Host '(no stdout)'
}
Write-Host '--- end stdout ---'

if ($stderr)
{
	Write-Host '--- NSwag stderr ---';
	Write-Host $stderr;
	Write-Host '--- end stderr ---'
}

Write-Host "Client generation completed successfully: $outputPath"

Pop-Location
