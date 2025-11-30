# DanceCourseCreator - Raspberry Pi Installation Guide

Detta är en guide för att installera och köra DanceCourseCreator på Raspberry Pi OS.

## 🎯 Översikt

DanceCourseCreator kan köras på Raspberry Pi för att skapa en lokal server för att planera och hantera West Coast Swing-kurser. Perfekt för dansskolor och instruktörer som vill ha en dedikerad server.

## 📋 Förutsättningar

- **Raspberry Pi**: Modell 3B+ eller nyare rekommenderas (Pi 4 eller Pi 5 för bästa prestanda)
- **Operativsystem**: Raspberry Pi OS (64-bit rekommenderas)
- **Minne**: Minst 2GB RAM (4GB+ rekommenderas)
- **Lagring**: Minst 8GB ledigt utrymme
- **Nätverk**: Ethernet eller WiFi-anslutning

## 🚀 Snabbstart

### Automatisk Installation

Den enklaste metoden är att använda installationsskriptet:

```bash
# Ladda ner och kör installationsskriptet
curl -sSL https://raw.githubusercontent.com/pownas/DanceCourseCreator/main/raspberry-pi-install.sh | bash
```

Detta kommer att:
- ✅ Installera .NET 10 SDK
- ✅ Klona DanceCourseCreator-repositoryt
- ✅ Konfigurera databasen (SQLite)
- ✅ Bygga och publicera applikationen
- ✅ Konfigurera Nginx som reverse proxy (valfritt)
- ✅ Sätta upp systemd-tjänst för automatisk start (valfritt)
- ✅ Konfigurera brandväggen (valfritt)
- ✅ Konfigurera automatiska backuper (valfritt)

### Manuell Installation

Om du föredrar att klona repositoryt först:

```bash
# Klona repositoryt
git clone https://github.com/pownas/DanceCourseCreator.git
cd DanceCourseCreator

# Gör skripten körbara
chmod +x raspberry-pi-*.sh

# Kör installationen
./raspberry-pi-install.sh
```

## 🎮 Användning

### Starta Applikationen

Efter installation kan du starta applikationen på flera sätt:

**1. Med start-skriptet (rekommenderat för utveckling):**
```bash
cd ~/DanceCourseCreator
./raspberry-pi-start.sh
```

**2. Med systemd (automatisk start vid uppstart):**
```bash
sudo systemctl start dancecourse
sudo systemctl status dancecourse
```

**3. Aktivera automatisk start vid uppstart:**
```bash
sudo systemctl enable dancecourse
```

### Åtkomst till Applikationen

Efter start är applikationen tillgänglig på:

- **Web App (Blazor)**: `http://[raspberry-pi-ip]:5001`
- **API Backend**: `http://[raspberry-pi-ip]:7177`
- **Aspire Dashboard**: `http://[raspberry-pi-ip]:15000`

För att hitta din Raspberry Pi:s IP-adress:
```bash
hostname -I
```

### Stoppa Applikationen

**Med systemd:**
```bash
sudo systemctl stop dancecourse
```

**Manuellt (om du startade med start-skriptet):**
```
Ctrl+C i terminalen
```

## 🔧 Konfiguration

### Databas

Applikationen använder SQLite som standard. Databasen lagras i:
```
~/dancecourse-data/dancecourse.db
```

### Appsettings

Konfigurationsfiler skapas automatiskt under installationen:
- `~/DanceCourseCreator/src/DanceCourseCreator.API/appsettings.Production.json`
- `~/DanceCourseCreator/src/DanceCourseCreator.Web/appsettings.Production.json`
- `~/DanceCourseCreator/src/DanceCourseCreator.AppHost/appsettings.Production.json`

### Portar

Standard-portar:
- **15000** - Aspire Dashboard
- **5001** - Web App (Blazor) - HTTP
- **5001** - Web App (Blazor) - HTTPS (om SSL är konfigurerat)
- **7177** - API Backend - HTTP
- **7177** - API Backend - HTTPS (om SSL är konfigurerat)

För att ändra portar, redigera `appsettings.Production.json`-filerna.

## 🔄 Uppdatering

För att uppdatera till senaste versionen:

```bash
cd ~/DanceCourseCreator
./raspberry-pi-update.sh
```

Detta kommer att:
1. Stoppa körande tjänster
2. Skapa backup av data
3. Hämta senaste ändringarna från GitHub
4. Bygga om applikationen
5. Starta om tjänsterna

## 🔍 Felsökning

### Debug-skript

Om du har problem med nätverksåtkomst, använd debug-skriptet:

```bash
cd ~/DanceCourseCreator
./raspberry-pi-debug.sh
```

Detta kommer att kontrollera:
- IP-adresser
- Körande processer
- Öppna portar
- Brandväggsinställningar
- Konfigurationsfiler
- Nginx-konfiguration (om installerat)
- SSL-certifikat (om installerat)

### Vanliga Problem

**Problem: Applikationen startar inte**
```bash
# Kontrollera loggar
journalctl -u dancecourse -n 50 --no-pager

# Kontrollera om portar är upptagna
ss -lntp | grep '15000\|5001\|7177'
```

**Problem: Kan inte nå applikationen från andra enheter**
```bash
# Kontrollera att appsettings använder 0.0.0.0
grep -r "0.0.0.0" ~/DanceCourseCreator/src/*/appsettings.Production.json

# Kontrollera brandväggen
sudo ufw status

# Öppna portar om nödvändigt
sudo ufw allow 15000/tcp
sudo ufw allow 5001/tcp
sudo ufw allow 7177/tcp
```

**Problem: Databas-fel**
```bash
# Kontrollera databas-sökväg
ls -lh ~/dancecourse-data/

# Återskapa databas
cd ~/DanceCourseCreator/src/DanceCourseCreator.API
dotnet ef database drop --force
dotnet ef database update
```

## 📦 Backup och Återställning

### Automatiska Backuper

Om du valde att konfigurera automatiska backuper under installationen, körs de dagligen kl 02:00.

Backuper sparas i:
```
~/dancecourse-backups/
```

### Manuell Backup

```bash
# Kör backup-skriptet
~/scripts/backup-DanceCourseCreator.sh

# Eller kopiera databasen manuellt
cp ~/dancecourse-data/dancecourse.db ~/dancecourse-backups/manual-backup-$(date +%Y%m%d).db
```

### Återställa från Backup

```bash
# Stoppa applikationen
sudo systemctl stop dancecourse

# Återställ databasen
cp ~/dancecourse-backups/dancecourse_20250130_020000.db ~/dancecourse-data/dancecourse.db

# Starta applikationen
sudo systemctl start dancecourse
```

## 🌐 Nginx och SSL (Valfritt)

Om du valde att installera Nginx under installationen:

### Åtkomst via Nginx
- **HTTP**: `http://[raspberry-pi-ip]`
- **HTTPS**: `https://[raspberry-pi-ip]` (om SSL är konfigurerat)

### SSL-konfiguration

**Let's Encrypt (rekommenderat för publika domäner):**
```bash
cd ~/DanceCourseCreator
./raspberry-pi-install.sh --configure-ssl
```

**Self-signed certifikat (för lokal användning):**
```bash
cd ~/DanceCourseCreator
./raspberry-pi-install.sh --configure-ssl
# Välj "Self-signed certificate"
```

## 📊 Prestanda-tips

### För Raspberry Pi 3/3B+:
- Använd Ethernet istället för WiFi när det är möjligt
- Överväg att öka swap-storleken (skriptet gör detta automatiskt)
- Stäng onödiga tjänster: `sudo systemctl disable [tjänst]`

### För Raspberry Pi 4/5:
- Installera 64-bit OS för bättre prestanda
- Använd USB 3.0 för extern lagring om möjligt
- Överväg aktiv kylning för kontinuerlig last

## 🔐 Säkerhet

### Grundläggande Säkerhet

1. **Ändra default-lösenord:**
   ```bash
   passwd
   ```

2. **Uppdatera systemet regelbundet:**
   ```bash
   sudo apt update && sudo apt upgrade -y
   ```

3. **Aktivera brandvägg:**
   ```bash
   sudo ufw enable
   sudo ufw allow 22/tcp  # SSH
   sudo ufw allow 80/tcp  # HTTP
   sudo ufw allow 443/tcp # HTTPS
   ```

4. **Använd SSL/HTTPS** för produktion (se Nginx-sektion ovan)

### JWT-säkerhet

En säker JWT-nyckel genereras automatiskt under installationen. För att regenerera:

```bash
# Generera ny nyckel
openssl rand -base64 48

# Uppdatera i appsettings.Production.json
nano ~/DanceCourseCreator/src/DanceCourseCreator.API/appsettings.Production.json
```

## 📝 Systemd-tjänst

Tjänsten heter `dancecourse` och kan hanteras med:

```bash
# Starta
sudo systemctl start dancecourse

# Stoppa
sudo systemctl stop dancecourse

# Starta om
sudo systemctl restart dancecourse

# Status
sudo systemctl status dancecourse

# Loggar
journalctl -u dancecourse -f

# Aktivera autostart
sudo systemctl enable dancecourse

# Inaktivera autostart
sudo systemctl disable dancecourse
```

## 🛠️ Avancerad Konfiguration

### Statisk IP-adress

För att konfigurera en statisk IP-adress (rekommenderat för servrar):

```bash
cd ~/DanceCourseCreator
./raspberry-pi-install.sh --configure-static-ip
```

### Custom Portar

För att använda andra portar, redigera konfigurationsfilerna:

```bash
# API
nano ~/DanceCourseCreator/src/DanceCourseCreator.API/appsettings.Production.json

# Web
nano ~/DanceCourseCreator/src/DanceCourseCreator.Web/appsettings.Production.json

# AppHost (Aspire Dashboard)
nano ~/DanceCourseCreator/src/DanceCourseCreator.AppHost/appsettings.Production.json
```

Starta sedan om tjänsten:
```bash
sudo systemctl restart dancecourse
```

## 📚 Ytterligare Resurser

- **GitHub Repository**: https://github.com/pownas/DanceCourseCreator
- **Issue Tracker**: https://github.com/pownas/DanceCourseCreator/issues
- **Main README**: [README.md](README.md)
- **Requirements Specification**: [Kravspecifikation.md](Kravspecifikation.md)

## 🤝 Support

Om du stöter på problem:

1. Kör debug-skriptet: `./raspberry-pi-debug.sh`
2. Kontrollera loggar: `journalctl -u dancecourse -n 100 --no-pager`
3. Sök i GitHub Issues
4. Skapa en ny issue med output från debug-skriptet

## 📄 Licens

MIT License - Se [LICENSE](LICENSE) för detaljer.

---

**Utvecklad för West Coast Swing-communityn** 💃🕺
