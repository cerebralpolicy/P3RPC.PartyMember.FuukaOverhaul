# Set Working Directory
Split-Path $MyInvocation.MyCommand.Path | Push-Location
[Environment]::CurrentDirectory = $PWD

Remove-Item "$env:RELOADEDIIMODS/P3RPC.PartyMember.FuukaOverhaul/*" -Force -Recurse
dotnet publish "./P3RPC.PartyMember.FuukaOverhaul.csproj" -c Release -o "$env:RELOADEDIIMODS/P3RPC.PartyMember.FuukaOverhaul" /p:OutputPath="./bin/Release" /p:ReloadedILLink="true"

# Restore Working Directory
Pop-Location