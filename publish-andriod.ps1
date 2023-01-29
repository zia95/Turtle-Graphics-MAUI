Param(
        [Parameter(Mandatory=$true)]
        [string]$KeyStoreFile,

        [Parameter(Mandatory=$true)]
        [string]$KeyAlias,

        [Parameter(Mandatory=$true)]
        [string]$KeyStorePass,

        [Parameter(Mandatory=$true)]
        [string]$KeyPass
    )

    Write-Output "-> KeyStoreFile='$KeyStoreFile'"
    Write-Output "-> KeyAlias='$KeyAlias'"
    Write-Output "-> KeyStorePass='$KeyStorePass'"
    Write-Output "-> KeyPass='$KeyPass'"

    $cont = Read-Host "Continue? Type 'yes'"
    If ($cont -eq "yes")
    {
        dotnet publish -f:net6.0-android -c:Release /p:AndroidSigningKeyStore=$KeyStoreFile /p:AndroidSigningKeyAlias=$KeyAlias /p:AndroidSigningKeyPass=$KeyPass /p:AndroidSigningStorePass=$KeyStorePass
    }
