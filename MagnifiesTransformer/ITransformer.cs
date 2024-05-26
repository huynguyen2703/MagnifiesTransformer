namespace MagnifiesTransformer;

public interface ITransformer
{
    // Properties to get data (getters)
    public bool IsActive { get;  }
    
    public bool IsInactive { get; }
    
    public bool IsShutDown { get; }
    
    public bool IsSum { get; }
    
    public bool IsDifference { get; }
    
    public bool IsProduct { get; }
    
    public bool IsModulo { get; }
    
    public bool IsUnknown { get; }
    
    public Transformer.State GetState { get; }
    
    public Transformer.OperationType GetOperationType { get; }
    
    // Methods
    public double Transform(int guessValue);

    public void Reset();

    public void Activate();

    public void Deactivate();
}