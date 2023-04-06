[CmdletBinding()]
param (
    [Parameter()]
    [string]
    $serverAddress="https://localhost:7287",
    [Parameter()]
    [char]
    $gameId="a",
    [Parameter(Mandatory=$true)]
    [string]
    $playerName
)

"Joining game at $serverAddress"

$url = "$serverAddress/Game/Join?gameId=$gameId&name=$playerName"

$joinResponse = Invoke-RestMethod $url
$joinResponse | Add-Member -NotePropertyName "ServerAddress" -NotePropertyValue $serverAddress
$joinResponse | Add-Member -NotePropertyName "GameId" -NotePropertyValue $gameId

$joinResponse | ConvertTo-Json | out-file "joinInfo.json"