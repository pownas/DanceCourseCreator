#!/bin/bash

set -e  # Exit on any error

# Colors for output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
BLUE='\033[0;34m'
NC='\033[0m' # No Color

# Logging functions
log_info() {
    echo -e "${BLUE}[INFO]${NC} $1"
}

log_success() {
    echo -e "${GREEN}[SUCCESS]${NC} $1"
}

log_warning() {
    echo -e "${YELLOW}[WARNING]${NC} $1"
}

log_error() {
    echo -e "${RED}[ERROR]${NC} $1"
}

log_section() {
    echo -e "\n${BLUE}========================================${NC}"
    echo -e "${BLUE} $1${NC}"
    echo -e "${BLUE}========================================${NC}\n"
}

# Check if running in Codespace
check_codespace() {
    if [[ -z "${CODESPACES}" ]]; then
        log_warning "This script is designed for GitHub Codespaces but can work in other Ubuntu environments"
    else
        log_info "Running in GitHub Codespaces environment"
    fi
}

# Step 1: Verify .NET 8 SDK
verify_dotnet_8() {
    log_section "Verifying .NET 8 SDK"
    
    log_info "Checking current .NET installation..."
    if command -v dotnet &> /dev/null; then
        log_info "Current .NET version: $(dotnet --version)"
        log_info "Installed SDKs:"
        dotnet --list-sdks
        
        # Check if .NET 8 is installed
        if dotnet --list-sdks | grep -q "8\."; then
            log_success ".NET 8 SDK is already installed"
            return 0
        else
            log_warning ".NET 8 SDK not found"
        fi
    else
        log_warning ".NET not found in PATH"
    fi
    
    log_info "Installing .NET 8 SDK using Microsoft's installation script..."
    curl -sSL https://dot.net/v1/dotnet-install.sh | bash /dev/stdin --version latest --channel 8.0
    
    log_info "Configuring PATH for .NET..."
    export PATH="$HOME/.dotnet:$PATH"
    echo 'export PATH="$HOME/.dotnet:$PATH"' >> ~/.bashrc
    
    log_info "Verifying .NET 8 installation..."
    dotnet --list-sdks
    log_success ".NET 8 SDK installation completed"
}

# Step 2: Restore and build project
setup_project() {
    log_section "Setting up Project Dependencies"
    
    log_info "Restoring project dependencies..."
    dotnet restore DanceCourseCreator.slnx
    
    log_info "Cleaning previous build artifacts..."
    dotnet clean DanceCourseCreator.slnx
    
    log_info "Building solution..."
    dotnet build DanceCourseCreator.slnx
    
    log_success "Project setup completed"
}

# Step 3: Install Entity Framework CLI tools
install_ef_tools() {
    log_section "Installing Entity Framework CLI Tools"
    
    log_info "Checking if Entity Framework tools are already installed..."
    if dotnet tool list --global | grep -q "dotnet-ef"; then
        log_success "Entity Framework tools are already installed"
        # Verify it's working
        if dotnet ef --version &> /dev/null; then
            return 0
        else
            log_warning "dotnet-ef is installed but not working, attempting to reinstall..."
            dotnet tool uninstall --global dotnet-ef 2>/dev/null || true
        fi
    fi
    
    log_info "Installing Entity Framework global tools..."
    if ! dotnet tool install --global dotnet-ef; then
        log_warning "Installation failed, clearing NuGet cache and retrying..."
        dotnet nuget locals all --clear
        
        # Try with explicit version matching .NET SDK
        local dotnet_version=$(dotnet --version | cut -d'.' -f1)
        log_info "Attempting installation with version ${dotnet_version}.0.0..."
        if ! dotnet tool install --global dotnet-ef --version ${dotnet_version}.0.0; then
            log_error "Failed to install Entity Framework tools after cache clear"
            log_error "Try manually: dotnet tool install --global dotnet-ef --version ${dotnet_version}.0.0"
            return 1
        fi
    fi
    
    log_info "Verifying Entity Framework tools installation..."
    if dotnet tool list --global | grep -q "dotnet-ef"; then
        log_success "Entity Framework tools installed successfully"
        # Use || true to prevent script from stopping if ef command has issues
        dotnet ef --version || log_warning "EF version check had issues but tool is installed"
    else
        log_error "Failed to install Entity Framework tools"
        return 1
    fi
    log_success "Entity Framework tools installation completed"
}

# Step 4: Configure HTTPS development certificates
configure_dev_certs() {
    log_section "Configuring HTTPS Development Certificates"
    
    log_info "Cleaning any existing development certificates..."
    dotnet dev-certs https --clean || log_warning "Failed to clean existing certificates (may not exist)"
    
    log_info "Generating new HTTPS development certificate..."
    dotnet dev-certs https --trust || log_warning "Failed to trust certificate (may require manual trust)"
    
    log_info "Verifying HTTPS certificate installation..."
    if dotnet dev-certs https --check; then
        log_success "HTTPS development certificate is properly configured"
    else
        log_warning "HTTPS certificate verification failed - applications may have SSL issues"
    fi
    
    log_success "HTTPS development certificates configuration completed"
}

# Step 5: Install Playwright browsers for E2E tests
install_playwright() {
    log_section "Installing Playwright Browsers for E2E Tests"
    
    log_info "Building E2E test project to prepare Playwright installation..."
    cd src/DanceCourseCreator.Tests.E2E
    dotnet build --verbosity quiet
    
    log_info "Checking if Playwright browsers are already installed..."
    if [ -d "bin/Debug/net8.0/.playwright/node/linux" ]; then
        log_success "Playwright browsers are already installed"
        cd ../..
        return 0
    fi
    
    log_info "Installing Playwright Chromium browser..."
    if command -v pwsh &> /dev/null; then
        pwsh bin/Debug/net8.0/playwright.ps1 install chromium
        log_success "Playwright browsers installed successfully"
    else
        log_warning "PowerShell (pwsh) not found"
        log_warning "Playwright browsers installation skipped"
        log_warning "You can install manually later with:"
        log_warning "  cd src/DanceCourseCreator.Tests.E2E"
        log_warning "  pwsh bin/Debug/net8.0/playwright.ps1 install chromium"
    fi
    
    cd ../..
    log_success "Playwright installation completed"
}

# Step 6: Make scripts executable
setup_scripts() {
    log_section "Setting up Project Scripts"
    
    log_info "Making project scripts executable..."
    chmod +x ./app-install.sh
    if [ -f "./app-start.sh" ]; then
        chmod +x ./app-start.sh
    fi
    if [ -f "./docs/run-e2e-tests.sh" ]; then
        chmod +x ./docs/run-e2e-tests.sh
    fi
    
    log_success "Project scripts are now executable"
}

# Step 7: Verify installation
verify_installation() {
    log_section "Verifying Installation"
    
    log_info "Checking .NET installation..."
    dotnet --version
    dotnet --list-sdks
    
    log_info "Checking Entity Framework tools..."
    dotnet tool list --global | grep dotnet-ef || log_warning "EF tools not found in global tools"
    
    log_info "Checking HTTPS development certificates..."
    if dotnet dev-certs https --check --quiet; then
        log_success "HTTPS development certificates are properly configured"
    else
        log_warning "HTTPS development certificates may need attention"
    fi
    
    log_info "Checking project build status..."
    dotnet build DanceCourseCreator.slnx --verbosity quiet
    
    log_success "All verifications completed successfully!"
}

# Step 8: Display usage information
show_usage_info() {
    log_section "Installation Complete - Usage Information"
    
    echo -e "${GREEN}✅ DanceCourseCreator environment is ready!${NC}\n"
    
    echo -e "${BLUE}Next Steps:${NC}"
    echo -e "  ${YELLOW}Start API Backend (Terminal 1):${NC}"
    echo -e "    cd src/DanceCourseCreator.API"
    echo -e "    dotnet run"
    echo -e "    ${BLUE}→ API will be available at: https://localhost:7177${NC}"
    echo -e ""
    echo -e "  ${YELLOW}Start Blazor Client (Terminal 2):${NC}"
    echo -e "    cd src/DanceCourseCreator.Client"
    echo -e "    dotnet run"
    echo -e "    ${BLUE}→ Client will be available at: https://localhost:5001${NC}"
    echo -e ""
    echo -e "${BLUE}Available commands:${NC}"
    echo -e "  ${YELLOW}Database Operations:${NC}"
    echo -e "    cd src/DanceCourseCreator.API"
    echo -e "    dotnet ef migrations add <MigrationName>"
    echo -e "    dotnet ef database update"
    echo -e ""
    echo -e "  ${YELLOW}Build and Test:${NC}"
    echo -e "    dotnet build DanceCourseCreator.slnx"
    echo -e "    dotnet test"
    echo -e ""
    echo -e "  ${YELLOW}Run E2E Tests:${NC}"
    echo -e "    ./docs/run-e2e-tests.sh"
    echo -e "    ${BLUE}(Make sure API and Client are running first)${NC}"
    echo -e ""
    echo -e "${BLUE}Project Structure:${NC}"
    echo -e "  • ${YELLOW}DanceCourseCreator.API${NC} - Backend Web API project"
    echo -e "  • ${YELLOW}DanceCourseCreator.Client${NC} - Blazor WebAssembly frontend"
    echo -e "  • ${YELLOW}DanceCourseCreator.AppHost${NC} - .NET Aspire orchestration (optional)"
    echo -e "  • ${YELLOW}DanceCourseCreator.ServiceDefaults${NC} - Shared service configurations"
    echo -e "  • ${YELLOW}DanceCourseCreator.Tests.E2E${NC} - Playwright end-to-end tests"
    echo -e ""
    echo -e "${BLUE}Installed Tools:${NC}"
    echo -e "  • .NET 8 SDK"
    echo -e "  • Entity Framework CLI tools"
    echo -e "  • HTTPS development certificates (trusted)"
    echo -e "  • Playwright browsers (for E2E testing)"
    echo -e ""
    echo -e "${BLUE}Database:${NC}"
    echo -e "  • SQLite database at: ${YELLOW}src/DanceCourseCreator.API/database.sqlite${NC}"
    echo -e "  • Automatically created and seeded on first API run"
    echo -e ""
    echo -e "${GREEN}Ready to start developing! 🎵💃🕺${NC}"
}

# Main execution
main() {
    log_section "DanceCourseCreator Environment Installation"
    log_info "Starting automated environment setup..."
    
    check_codespace
    verify_dotnet_8
    setup_project
    install_ef_tools
    configure_dev_certs
    install_playwright
    setup_scripts
    verify_installation
    show_usage_info
    
    log_success "Environment installation completed successfully!"
}

# Run main function
main "$@"
