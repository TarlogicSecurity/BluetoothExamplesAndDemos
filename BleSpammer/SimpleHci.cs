using UsbBluetooth;

/// <summary>  Minimum basic implementation of HCi  </summary>
internal static class SimpleHci {

    //-------------------------------------------------------------------------------------------------------------------------------
    // HCI Initialization commands
    /// <summary> BLUETOOTH CORE SPECIFICATION Version 6.0 | Vol 4, Part E Page 2040 - 7.3.2 Reset command </summary>
    public static byte[] CMD_RESET                     = new byte[] { 0x01, 0x03, 0x0C, 0x00 };

    /// <summary> BLUETOOTH CORE SPECIFICATION Version 6.0 | Vol 4, Part E Page 2037 - 7.3.1 Set Event Mask command </summary>
    public static byte[] CMD_SET_EVENT_MASK            = new byte[] { 0x01,0x01,0x0c,0x08,0xff,0xff,0xff,0xff,0xff,0xff,0xff, 0xff};

    /// <summary>BLUETOOTH CORE SPECIFICATION Version 6.0 | Vol 4, Part E Page 2136 - 7.3.69 Set Event Mask Page 2 command </summary>
    public static byte[] CMD_SET_EVENT_MASK_PAGE_2     = new byte[] { 0x01,0x63,0x0c,0x08,0xff,0xff,0xff,0xff,0xff,0xff,0xff,0xff};

    /// <summary> BLUETOOTH CORE SPECIFICATION Version 6.0 | Vol 4, Part E Page 2483 - 7.8.1 LE Set Event Mask command</summary>   
    public static byte[] CMD_LE_SET_EVENT_MASK         = new byte[] { 0x01,0x01,0x20,0x08,0xff,0xff,0xff,0xff,0xff,0xff,0xff,0xff};


    /// <summary> BLUETOOTH CORE SPECIFICATION Version 6.0 | Vol 4, Part E Page 2501 - 7.8.9 LE Set Advertising Enable command</summary>
    public static byte[] CMD_SET_ADVERTISING_ENABLE        = new byte[] { 0x01,0x0a,0x20,0x01,0x01 }; // 0x01 Advertising enabled.
    public static byte[] CMD_SET_ADVERTISING_ENABLE_STOP   = new byte[] { 0x01,0x0a,0x20,0x00,0x01 }; // 0x00 Advertising disabled.
    //-------------------------------------------------------------------------------------------------------------------------------

    //-------------------------------------------------------------------------------------------------------------------------------
    /// <summary> 
    /// Basic Bluetooth device initialization with HCI commands (Simplified)
    /// BLUETOOTH CORE SPECIFICATION Version 6.0 | Vol 6, Part D Page 3526 - 2.1 Initial setup
    /// </summary>
    /// <param name="device"></param>
    public static void Initialize(this UsbBluetoothDevice device) {
        device.Write(CMD_RESET);
        device.Write(CMD_SET_EVENT_MASK);
        device.Write(CMD_SET_EVENT_MASK_PAGE_2);
        device.Write(CMD_LE_SET_EVENT_MASK);
    }
    //-------------------------------------------------------------------------------------------------------------------------------

    //-------------------------------------------------------------------------------------------------------------------------------
    public static void StartAdvertisement(this UsbBluetoothDevice device,byte[] advertisementData) {
        byte[] addr = new byte[6];
        Random.Shared.NextBytes(addr);
        
        byte[] advertisinDataPacket = new byte[0x23] ;
        int i=0;
        advertisinDataPacket[i++] = 0x01;
        advertisinDataPacket[i++] = 0x08;
        advertisinDataPacket[i++] = 0x20;
        advertisinDataPacket[i++] = 0x20;
        if (advertisementData != null) Buffer.BlockCopy(advertisementData,0,advertisinDataPacket,i,advertisementData.Length);

        device.Write(new byte[] { 0x01,0x05,0x20,0x06,addr[5],addr[4],addr[3],addr[2],addr[1],addr[0] |= 0xC0 }); // CMD_SET_RANDOM_ADDRESS
        device.Write(new byte[] { 0x01,0x06,0x20,0x0f,0x00,0x02,0x00,0x08,0x02,0x02,0x01,0x8a,0xaf,0x69,0x3e,0x51,0xe9,0x03,0x00 }); // CMD_SET_ADVERTISING_PARAMETERS
        device.Write(advertisinDataPacket.ToArray()); // CMD_SET_ADVERTISING_DATA
        device.Write(CMD_SET_ADVERTISING_ENABLE); // CMD_SET_ADVERTISING_ENABLE

    }
    //-------------------------------------------------------------------------------------------------------------------------------

    //-------------------------------------------------------------------------------------------------------------------------------
    public static void StopAdvertisement(this UsbBluetoothDevice device) {
        device.Write(CMD_SET_ADVERTISING_ENABLE_STOP); // CMD_SET_ADVERTISING_ENABLE - Disable
    }
    //-------------------------------------------------------------------------------------------------------------------------------
}
