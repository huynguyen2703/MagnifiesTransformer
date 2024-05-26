namespace MagnifiesTransformer;

public class TMagnifier : Magnifier
{
    // data section
    private readonly Transformer _transformer;
    
    // properties section

    public uint TMagnifierGetNumQueries => _transformer.GetNumQueries;
    public uint TMagnifierGetHighData => _transformer.GetHighData;
    public uint TMagnifierGetLowData => _transformer.GetLowData;
    public bool IsTargetKnown => _transformer.IsTargetKnown;
    public bool TMagnifierSum => _transformer.IsSum;
    public bool TMagnifierDifference => _transformer.IsDifference;
    public bool TMagnifierProduct => _transformer.IsProduct;
    public bool TMagnifierModulo => _transformer.IsModulo;

    public bool TMagnifierUnknown => _transformer.IsUnknown;
    public Transformer.OperationType TMagnifierOperationType => _transformer.GetOperationType;
    
    
    // methods section
    public TMagnifier(int size, double scaleFactor, uint limit, uint maxYield, Transformer transformer) :
        base(size, scaleFactor, limit, maxYield)
    { 
        _transformer = transformer;
    }

    public double TMagnifierYieldSize()
    {
        double returnVal = YieldSize();
        if (GetState == State.ShutDown)
        {
            _transformer.ShutDown();
        }

        return returnVal;
    }
    public double TMagnifierTransform(int guessValue)
    {
        _numQueries += 1;
        _initialState = false;
        double returnVal = _transformer.Transform(guessValue);
        if (_transformer.IsShutDown)
        {
            ShutDown();
        }

        return returnVal;
    }

    public void  TMagnifierReset()
    {
        _transformer.Reset();
        Reset(); // magnifier version
    }

    public void TMagnifierActivate()
    {
        _transformer.Activate();
        Activate(); // magnifier version
    }

    public void TMagnifierDeactivate()
    {
        _transformer.Deactivate();
        Deactivate(); // magnifier version
    }
    public double GetAcceleratingFactor()
    {
        if (_transformer.GetType() != typeof(AccelerateTransformer))
        {
            throw new Exception("Object is not AccelerateTransformer");
        }

        return _transformer.GetAcceleratingFactor();
    }
    public double GetModuloFactor()
    {
        if (_transformer.GetType() != typeof(ViralTransformer))
        {
            throw new Exception("Object is not ViralTransformer");
        }

        return _transformer.GetModuloFactor();
    }
    public Type GetTransformerPower()
    {
        return _transformer.GetType();
    }
    
}


// TODO : BUILD DRIVER (keep build driver)
// TODO : START CODING P3 3500
// TODO : STUDY TURING MACHINE
