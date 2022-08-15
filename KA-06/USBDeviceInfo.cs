namespace KA_06
{
    public class UsbDeviceInfo
    {
        public UsbDeviceInfo(string deviceId, string pnpDeviceId, string description)
        {
            this.DeviceId = deviceId;
            this.PnpDeviceId = pnpDeviceId;
            this.Description = description;
        }
        public string DeviceId { get; }
        public string PnpDeviceId { get; }
        public string Description { get; }
    }
}
