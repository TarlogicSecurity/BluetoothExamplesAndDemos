#!/usr/bin/env python

import usbbluetooth
from scapy_usbbluetooth import UsbBluetoothSocket
from rich.console import Console
from rich.prompt import IntPrompt
from scapy.layers.bluetooth import HCI_Hdr, HCI_Command_Hdr, HCI_Cmd_Reset, HCI_Event_Command_Complete


# Print a nice banner!
console = Console()
BANNER = r'''                                                                       [bold dark_cyan]
 __      _______  _____   ______                                      _             [bold cyan3]
 \ \    / / ____|/ ____| |  ____|      by Tarlogic Security          | |            [bold light_sea_green]
  \ \  / / (___ | |      | |__   _ __  _   _ _ __ ___   ___ _ __ __ _| |_ ___  _ __ [bold dark_turquoise]
   \ \/ / \___ \| |      |  __| | '_ \| | | | '_ ` _ \ / _ \ '__/ _` | __/ _ \| '__|[bold turquoise2]
    \  /  ____) | |____  | |____| | | | |_| | | | | | |  __/ | | (_| | || (_) | |   [bold steel_blue1]
     \/  |_____/ \_____| |______|_| |_|\__,_|_| |_| |_|\___|_|  \__,_|\__\___/|_|   
'''
console.print(BANNER)

# Select a controller
devices = usbbluetooth.list_devices()
for i, dev in enumerate(devices):
    console.print(f'{i}) {dev}')
dev_idx = IntPrompt().ask('Select a Bluetooth controller', console=console,
                          choices=[str(i) for i in range(len(devices))])
device = devices[dev_idx]
console.log(f"Using {device}")

# Open a socket
console.log("Opening socket...")
socket = UsbBluetoothSocket(device)

# Reset the controller
console.log("Resetting the controller...")
pkt = HCI_Hdr() / HCI_Command_Hdr() / HCI_Cmd_Reset()
response = socket.sr1(pkt, verbose=0)
if not HCI_Event_Command_Complete in response or response[HCI_Event_Command_Complete].status != 0:
    response.show()
    console.log("Could not reset controller!")
    exit(1)

# OCF can range from 0x00 to 0x3FF (10 bits)
opcodes = []
for ocf in range(0x400):  # 0 to 0x3FF
    pkt = HCI_Hdr() / HCI_Command_Hdr(ogf=0x3f, ocf=ocf, len=0)
    console.log(f"Testing opcode 0x{pkt[HCI_Command_Hdr].opcode:04x}...")
    response = socket.sr1(pkt, verbose=0, timeout=1)
    if not response or HCI_Event_Command_Complete in response and response[HCI_Event_Command_Complete].status != 1:
        opcodes.append(pkt[HCI_Command_Hdr].opcode)
        console.log("Possible working command.")
        if response:
            console.log("Logging response data:")
            response.show()

if len(opcodes) != 0:
    op_str = [f"0x{op:04x}" for op in opcodes]
    console.log(
        f"Here is a list of possible vendor opcodes: {', '.join(op_str)}")
