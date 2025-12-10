"# KeySender

A Windows Forms application for sending keystrokes to target windows when direct pasting is not available or reliable.

## Overview

KeySender is a utility tool designed to simulate keyboard input character by character to any application window. This is particularly useful in scenarios where the standard copy-paste functionality doesn't work, such as:

- KVM consoles and virtual machine interfaces
- Remote desktop sessions with clipboard restrictions
- Legacy applications that don't support clipboard operations
- Secure terminals or applications with paste restrictions
- Browser-based consoles and web terminals

## Features

- **Character-by-character input simulation**: Sends text as individual keystrokes with configurable delays
- **Two operation modes**:
  - **Manual Mode**: Switch to target application manually before sending
  - **Auto Mode**: Automatically brings target application to foreground
- **Process targeting**: Select from a list of running applications
- **Configurable delays**: 
  - Sendkey delay (between each character)
  - Application switch delay (time to wait after switching to target app)
- **Special character handling**: Properly escapes SendKeys special characters (`{`, `}`, `+`, `^`, `%`, `~`, `(`, `)`)
- **Optional Enter key**: Option to automatically press Enter at the end
- **Clipboard integration**: Automatically loads clipboard content on startup (if under 85 characters)
- **Always on top**: Window stays on top for easy access

## System Requirements

- Windows operating system
- .NET Framework 4.8 or later

## Usage

### Manual Mode (Default)
1. Enter or paste the text you want to send in the text box
2. Manually click on or switch to the target application window
3. Click the "Send" button or press Enter in KeySender
4. The text will be sent character by character to the active window

### Auto Mode
1. Switch to the "Auto" tab
2. Click "Refresh Processes" to get a list of running applications
3. Select the target application from the dropdown
4. Enter the text you want to send
5. Click "Send" - the application will automatically switch to the target and send the text

### Configuration Options

- **Sendkey delay (ms)**: Time to wait between each character (default: 100ms)
- **Application switch delay (ms)**: Time to wait after switching to target application (default: varies)
- **Press {Enter} at end**: Automatically sends Enter key after the text (enabled by default)
- **Titleless**: Show processes without window titles in the application list

## Building from Source

This is a Visual Studio C# Windows Forms project targeting .NET Framework 4.8.

### Prerequisites
- Visual Studio 2019 or later
- .NET Framework 4.8 SDK

### Build Steps
1. Open `KeySender.sln` in Visual Studio
2. Build the solution (Ctrl+Shift+B)
3. The executable will be generated in `bin\Debug\` or `bin\Release\`

## Technical Details

- Built with C# Windows Forms
- Uses `SendKeys.SendWait()` for keyboard simulation
- Implements Win32 API calls for window management
- Excludes system processes from the target application list for safety

## Use Cases

- **KVM/IPMI Consoles**: Send complex passwords or commands to server management interfaces
- **Virtual Machines**: Input text in VMs where clipboard sharing is disabled
- **Secure Terminals**: Work with applications that have clipboard restrictions
- **Legacy Systems**: Interface with older applications that don't support modern clipboard operations
- **Remote Sessions**: Send text in RDP/VNC sessions with clipboard issues
- **Web Consoles**: Input commands in browser-based terminal emulators

## Safety Features

- Excludes critical system processes from targeting
- Configurable delays prevent overwhelming target applications
- Always-on-top design ensures easy access and control

## License

This project is open source. See the repository for license details.

## Repository

https://github.com/saeedphr/keysender

---

*KeySender - Because sometimes Ctrl+V just isn't an option.*" 
