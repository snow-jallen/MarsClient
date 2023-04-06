[CmdletBinding()]
param (
    [Parameter()]
    [string]
    $direction = "forward",
    [Parameter()]
    [int]
    $times = 1
)

$joinInfo = get-content "joinInfo.json" | convertfrom-json
$url = "$($joinInfo.ServerAddress)/Game/MovePerseverance?token=$($joinInfo.token)&direction=$direction"
for ($i = 0; $i -lt $times; $i++) {
    $response = Invoke-RestMethod $url
    $response.message
    "You are currently at ($($response.x),$($response.y)) facing $($response.orientation) with $($response.batteryLevel) battery"
}
"The targets are at:"
$($joinInfo.targets) | ft