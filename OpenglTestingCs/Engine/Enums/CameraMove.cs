namespace OpenglTestingCs.Engine.Enums
{
    //Combine with Up | Left = up and left
    public enum CameraMove
    {
        None       = 0b_0000_0000_0000,
        Up         = 0b_0000_0000_0001,
        Down       = 0b_0000_0000_0010,
        Left       = 0b_0000_0000_0100,
        Right      = 0b_0000_0000_1000,
        Front      = 0b_0000_0001_0000,
        Back       = 0b_0000_0010_0000,
        TiltLeft   = 0b_0000_0100_0000,
        TiltRight  = 0b_0000_1000_0000,
        ExtraSpeed = 0b_0001_0000_0000,
    }
}
