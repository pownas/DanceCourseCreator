# Raspberry Pi Network Binding Fix

## Problem
Applikationen lyssnade endast på `127.0.0.1` (localhost) istället för `0.0.0.0` (alla nätverksinterfaces), vilket gjorde den otillgänglig från andra enheter på nätverket.

## Root Cause
.NET Aspire och Kestrel binder som standard till localhost av säkerhetsskäl. Detta är korrekt för utvecklingsmiljöer men fungerar inte för Raspberry Pi-deployment där vi vill att applikationen ska vara nåbar från nätverket.

## Solution
Implementerade en Raspberry Pi-specifik konfiguration som tvingar alla komponenter att binda till `0.0.0.0`:

### 1. AppHost.cs (Kod-nivå)
```csharp
var isRaspberryPi = Environment.GetEnvironmentVariable("DANCECOURSE_RASPBERRY_PI") == "true";

if (isRaspberryPi)
{
    // Force API to bind to all interfaces on Raspberry Pi
    api.WithEnvironment("ASPNETCORE_URLS", "http://0.0.0.0:7177");
    
    // Force Web to bind to all interfaces on Raspberry Pi
    web.WithEnvironment("ASPNETCORE_URLS", "http://0.0.0.0:5001");
}
```

### 2. Miljövariabler (raspberry-pi-start.sh)
```bash
export DANCECOURSE_RASPBERRY_PI=true
export DOTNET_DASHBOARD_URLS="http://0.0.0.0:15000"
export DOTNET_URLS="http://0.0.0.0:15000"
export ASPNETCORE_HTTP_PORTS=""  # Töm denna så ASPNETCORE_URLS används
```

### 3. SystemD Service
```ini
Environment=DANCECOURSE_RASPBERRY_PI=true
Environment=DOTNET_DASHBOARD_URLS=http://0.0.0.0:15000
Environment=DOTNET_URLS=http://0.0.0.0:15000
Environment=ASPNETCORE_HTTP_PORTS=
```

## Verification
Efter fix ska `ss -lntp` visa:
```
0.0.0.0:15000  (Aspire Dashboard)
0.0.0.0:5001   (Web App)
0.0.0.0:7177   (API)
```

Istället för tidigare:
```
127.0.0.1:15000  ❌ Endast localhost
127.0.0.1:5001   ❌ Endast localhost
127.0.0.1:7177   ❌ Endast localhost
```

## Testing
```bash
# På Raspberry Pi
./raspberry-pi-debug.sh

# Förväntat resultat:
# ✓ Port 15000 lyssnar på ALLA nätverksinterfaces (0.0.0.0:15000)
# ✓ Port 5001 lyssnar på ALLA nätverksinterfaces (0.0.0.0:5001)
# ✓ Port 7177 lyssnar på ALLA nätverksinterfaces (0.0.0.0:7177)
```

## Files Modified
1. `src/DanceCourseCreator.AppHost/AppHost.cs` - Raspberry Pi-specifik binding
2. `raspberry-pi-start.sh` - Miljövariabel-konfiguration
3. `raspberry-pi-install.sh` - SystemD service template
4. `raspberry-pi-update.sh` - SystemD service update

## Security Note
Att binda till `0.0.0.0` exponerar tjänsterna för hela nätverket. Detta är avsiktligt för Raspberry Pi-deployment men ska endast användas i privata nätverk. För produktion på internet, använd Nginx reverse proxy med SSL och brandväggskonfiguration.

## Related Issues
- Port conflict med Privatekonomi (löst med olika portar: 15000, 5001, 7177)
- API-katalognamn (DanceCourseCreator.API med stort API)
- appsettings.Production.json måste finnas för att Urls-konfiguration ska användas
