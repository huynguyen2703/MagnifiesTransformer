namespace MagnifiesTransformer;

public interface IMagnifier
{
    // Properties to get data (getters)
    public int Size { get; }
    
    public double ScaleFactor { get; }
    
    public uint Limit { get; }
    
    public bool GetInitialState { get; }
    
    public bool IsActive { get; }
    
    public bool IsInactive { get; }
    
    public bool IsShutDown { get; }
    
    public bool IsUp { get; }
    
    public bool IsDown { get; }
    
    public bool IsUnknown { get; }
    
    public Magnifier.State GetState { get; }
    
    public Magnifier.ScaleDirection GetDirection { get; }

    // Methods
    public double YieldSize();

    public void Reset();

    public void Activate();

    public void Deactivate();
}