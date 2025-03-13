using BleSpammer;

class Program {
    //-------------------------------------------------------------------------------------------------------------------------------
    static void Main() {

        Console.CursorVisible = false;
        Console.Clear();

        BannerManager.Show();
        MenuManager.Instance.SelectUsbDevice();
    }
    //-------------------------------------------------------------------------------------------------------------------------------
}

