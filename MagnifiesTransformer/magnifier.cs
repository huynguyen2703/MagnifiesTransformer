namespace MagnifiesTransformer;

public class Magnifier : IMagnifier
{
    // data section
    private readonly int _size;
    private readonly double _scaleFactor;
    private readonly uint _limit;
    private uint _maxYield;
    private readonly uint _extraMaxYield;
    protected uint _numQueries;
    protected bool _initialState;
    private State CurrentState;
    private ScaleDirection Direction;

    public enum State
    {   // 0 : Active, 1 : Inactive, 2 : Shutdown
        Active,
        Inactive,
        ShutDown
    }

    public enum ScaleDirection
    {   // 0 : Up, 1 : Down, Unknown : 2
        Up,
        Down,
        Unknown
    }

    // properties section
    public int Size => _size;
    public double ScaleFactor => _scaleFactor;
    public uint Limit => _limit;
    public uint MaxYield => _maxYield;
    public uint NumQueries => _numQueries;
    public bool GetInitialState => _initialState;
    public State GetState => CurrentState;
    public ScaleDirection GetDirection => Direction;
    public bool IsActive => CurrentState == State.Active;
    public bool IsInactive => CurrentState == State.Inactive;
    public bool IsShutDown => CurrentState == State.ShutDown;
    public bool IsUp => Direction == ScaleDirection.Up;

    public bool IsDown => Direction == ScaleDirection.Down;

    public bool IsUnknown => Direction == ScaleDirection.Unknown;


    // methods section
    public Magnifier(int size, double scaleFactor, uint limit, uint maxYield)
    {
        GateCheck(size);
        _size = size;
        _scaleFactor = scaleFactor;
        _limit = limit;
        _maxYield = maxYield;
        _extraMaxYield = maxYield;
        _numQueries = 0;
        _initialState = true;
        CurrentState = State.Active;
        Direction = ScaleDirection.Unknown;
    }

    public double YieldSize()
    {
        PreCheck();
        ToggleDirection();
        _maxYield -= 1;
        _numQueries += 1;
        ShutDown();
        _initialState = false;
        if (IsUp)
        {
            return _size * _scaleFactor;
        }

        return _size / _scaleFactor;
    }
    public void ChangeMaxYield(uint newMaxYield)
    {
        if (newMaxYield != _extraMaxYield)
        {
            _initialState = false;
        }
        _maxYield = newMaxYield;
    }
    public void Reset()
    {
        _maxYield = _extraMaxYield;
        _initialState = true;
        _numQueries = 0;
        CurrentState = State.Active;
        Direction = ScaleDirection.Unknown;
    }
    public void Activate()
    {
        CheckDead();
        if (IsInactive)
        {
            _initialState = false;
            CurrentState = State.Active;
        }
    }
    public void Deactivate()
    {
        CheckDead();
        if (IsActive)
        {
            _initialState = false;
            CurrentState = State.Inactive;
        }
    }
    private void PreCheck()
    {
        if (IsInactive || IsShutDown)
        {
            throw new Exception("Invalid Request");
        }
    }
    private void CheckDead()
    {
        if (IsShutDown)
        {
            throw new Exception("Object is currently shut down");
        }
    }
    private void ToggleDirection()
    {
        Direction = _maxYield > _limit ? ScaleDirection.Up : ScaleDirection.Down;
        _initialState = false;
    }
    protected void ShutDown()
    {
        if (_maxYield == 0)
        {
            CurrentState = State.ShutDown;
            _initialState = false;
        }
    }
    private void GateCheck(int size)
    {   // different from PreCheck -> GateCheck invoked before private data initializations
        if (size < 0)
        {
            throw new Exception("size or scaleFactor must be nonnegative");
        }
    }

}