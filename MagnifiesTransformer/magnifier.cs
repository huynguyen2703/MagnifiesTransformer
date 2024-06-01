// Huy Quoc Nguyen
// CPSC 3200 P5 : Magnifier Transformer
/*
 *
 * -----------------------------------------CLASS INVARIANT AND INTERFACE INVARIANT-------------------------------------
 * Magnifier is a new added class in the transformers family that provides new abilities, though not establishing direct
 * relationship, several unchangeable characteristic of a real transformer are kept for a magnifier. A Magnifier object
 * encapsulates an integer as a size, a scale factor and a limit, the object will simulate scaling with numbers through
 * object's status. Specifically, the status we have is Up or Down, the size the object holds will be scaled up
 * if the maximum number of chances the client wish to set when creating an object is over the specified limit, which
 * will also be provided by the client, the size will be scaled down if the maximum number of chances to scale is below
 * the provided limit. Scaling will be performed through YieldSize(), the client does not need to provide external
 * resources because scaling is based on all current status the object. Magnifier is initially set to be in a valid
 * active state after instantiation, indicating that it is ready to perform action. During its lifetime, object can be
 * inactive or active, and the client are also given control whether to activate or deactivate the object through
 * Activate() or Deactivate(), yet actions may be rejected if object is not in an ideal state to be turned on or off
 * (ex : object is in shut down mode), with that being said, object may be automatically shut down and client is not
 * able to use the object unless they manually reset the object, the reason for this change in state is due to the
 * number of chances to scale a size is exhausted, hence it makes sense that object will be prevented to perform actions
 * in the wrong state, and it is good advice to reset the object to keep using it through Reset(). Additionally, the
 * client also choose to avoid resetting if they notice the chances to yield is almost up, they can choose to perform
 * ChangeMaxYield(), which can help them change the number of maximum yield chances before it runs out, requiring client
 * to make a reset. The client is likely will need to track multiple states, since a lot of them are used to determine
 * object's behavior in each call to YieldSize(). For that reason, Magnifier class provides numerous properties to help
 * the client track initial values and as well as current values of some certain states, this will help the client
 * navigate better how to make a Magnifier object function correctly.
 *
 * A Magnifier object is active as long as it is instantiated or reset, since invalid values are restraint using strong
 * type, the only time instantiation fails is a scale factor is less than zero, this makes sense because a scale factor
 * will directly affect size changes, a negative scale factor will result in negative size, which is not practical when
 * we talk about sizes. Object's states are manipulated via methods that can alter states, such as YieldSize(), Reset(),
 * Activate() or Deactivate(). Magnifier provides multiple statistics to help the client control actions. These are the
 * current size, scale factor, limit (the minimum chances required to scale up a size), maxYield (the maximum chances to
 * scale a size, will be determined by the client), it also provides the number of queries data, this will increment
 * after one call to YieldSize(), it also provides properties to check scale direction (Up or Down), object's state (
 * Active, Inactive, Shutdown). Also, the client can choose to verify through syntactical sugar boolean properties for
 * the states above (ex : IsUp, IsDown..etc..). There are a few methods that require the object to be in Active mode to
 * activate, such as YieldSize(), if an object is shut down, any effort to activate or deactivate the object will not
 * work, and has to be done through Reset() (notice this method will wipe all current data and reset, therefore it is
 * crucial that the client keeps track of the data with provided properties).
 * Error handling for Magnifier is done through Exceptions
 *
 * public Magnifier(uint  size, double scaleFactor, uint limit, uint maxYield) : Constructor
 * - allows the client to create an active Magnifier object. This is where dependencies injection is expected.
 * - the client will need to provide an initial size for encapsulation and scale factor to scale the size with that
 * - factor. The client needs to provide limit and maxYield, which is the minimum and maximum number of chances to scale
 * - the size, note the maxYield does not need to be always larger than limit, since these two values together will
 * - determine if the size should be scaled up or down, because they help decide the scale direction.
 *
 * YieldSize()
 * - requires an object to be in active mode to perform actions.
 * - as long as an object is instantiated successfully, then based on the limit and maxYield,
 * - a new scaled size will be returned and some statistics will be changed according to action.
 * - upon, yielding, scale direction will be determined based on limit and maxYield, which in turns will determine if it
 * - will scale up or down.
 * - object will not be in initial state if data is updated.
 * - if the maximum number of chances to scale is exhausted, the object will be shut down.
 * - resetting will help bring the object back to life.
 *
 * ChangeMaxYield(uint newMaxYield)
 * - requires an object to be in active mode to perform actions.
 * - if a newMaxYield is the same as the current max yield, no change will be made.
 * - else, maxYield will be updated to the newMaxYield
 * - object will not be in initial state if data is updated
 *
 * Reset()
 * - wipe out all data and put object into initial state.
 * - used to activate an object that is shut down.
 *
 * Activate()
 * - requires an object is not shut down.
 * - will throw exception if object is not in correct state.
 * - gives client the right to request state change.
 * - used to make an object active if not active.
 * - only accept request if state change is possible for object's current state.
 *
 * Deactivate()
 * - requires an object is not shut down.
 * - will throw exception if object is not in correct state.
 * - gives client the right to request state change.
 * - used to make an object inactive if not inactive.
 * - only accept request if state change is possible for object's current state.
 *
 *
 *  Magnifier also offers a variety of properties to help the client keep track of states and statistics.
 *
 *  Size : get the current size
 *  ScaleFactor : get the scale factor
 *  Limit : get the minimum number of chances to scale up (means will scale up only if a maxYield is larger than limit)
 *  MaxYield : get the maximum number of chances to yield a scaled size
 *  NumQueries : get the total number of queries
 *  GetInitialState : check if object is in initial state
 *  GetInitialYield : check the initial number of maxYield
 *  GetState : get the current state (Active, Inactive, Shutdown)
 *  GetDirection : get the current scale direction (Up, Down)
 *  IsActive : check if object is active
 *  IsInactive : check if object is inactive
 *  IsShutDown : check if object is shut down
 *  IsUp : check if scale direction is Up
 *  IsDown : check if scale direction is Down
 *  IsUnknown : check if scale direction is Unknown (happens when object just got created)
 */

namespace MagnifiesTransformer;

public class Magnifier : IMagnifier
{
    // data section
    private readonly uint _size;
    private double _yieldedSize;
    private readonly double _scaleFactor;
    private readonly uint _limit;
    private uint _maxYield;
    private readonly uint _extraMaxYield;
    protected uint _numQueries;
    protected bool _initialState;
    private State CurrentState;
    private ScaleDirection Direction;

    public enum State
    {
        // 0 : Active, 1 : Inactive, 2 : Shutdown
        Active,
        Inactive,
        ShutDown
    }

    public enum ScaleDirection
    {
        // 0 : Up, 1 : Down, Unknown : 2
        Up,
        Down,
        Unknown
    }

    // properties section
    public uint Size => _size;
    public double ScaleFactor => Math.Round(_scaleFactor, 2); // 2 decimal places
    public uint Limit => _limit;
    public uint MaxYield => _maxYield;
    public uint NumQueries => _numQueries;
    public bool GetInitialState => _initialState;
    public uint GetInitialYield => _extraMaxYield;
    public State GetState => CurrentState;
    public ScaleDirection GetDirection => Direction;
    public bool IsActive => CurrentState == State.Active;
    public bool IsInactive => CurrentState == State.Inactive;
    public bool IsShutDown => CurrentState == State.ShutDown;
    public bool IsUp => Direction == ScaleDirection.Up;
    public bool IsDown => Direction == ScaleDirection.Down;
    public bool IsUnknown => Direction == ScaleDirection.Unknown;


    // methods section
    /// PRECONDITIONS: a scale factor must be nonnegative <para></para>
    ///                a Transformer object passed in must be a valid Transformer object
    /// POSTCONDITIONS: object will be successfully created and set into a valid active state <para></para>
    ///                 all data fields initialized to reflect initial state
    ///
    public Magnifier(uint size, double scaleFactor, uint limit, uint maxYield)
    {
        GateCheck(scaleFactor);
        _size = size;
        _yieldedSize = _size;
        _scaleFactor = scaleFactor;
        _limit = limit;
        _maxYield = maxYield;
        _extraMaxYield = maxYield;
        _numQueries = 0;
        _initialState = true;
        CurrentState = State.Active;
        Direction = ScaleDirection.Unknown;
    }

    /// PRECONDITIONS: TMagnifier must be active <para></para>
    /// POSTCONDITIONS: object will be successfully created and set into a valid active state <para></para>
    ///                 all data fields initialized to reflect initial state
    public double YieldSize()
    {
        PreCheck();
        ToggleDirection();
        const int decimalPlaces = 2;
        _maxYield -= 1;
        _numQueries += 1;
        ShutDown();
        _initialState = false;
        if (IsUp)
        {
            _yieldedSize *= _scaleFactor;
        }
        else
        {
            _yieldedSize /= _scaleFactor;
        }

        return Math.Round(_yieldedSize, decimalPlaces);
    }

    public void ChangeMaxYield(uint newMaxYield)
    {
        PreCheck();
        if (newMaxYield != _extraMaxYield)
        {
            _initialState = false;
        }

        _maxYield = newMaxYield;
    }

    /// PRECONDITIONS : None <para></para>
    /// POSTCONDITIONS : All data will be wiped out and go back to their initial state. Object becomes active again.
    public void Reset()
    {
        _maxYield = _extraMaxYield;
        _initialState = true;
        _numQueries = 0;
        CurrentState = State.Active;
        Direction = ScaleDirection.Unknown;
    }

    /// PRECONDITIONS : An object must not be in shut down mode <para></para>
    /// POSTCONDITIONS : An object that is not already active will become active again.
    public void Activate()
    {
        CheckDead();
        if (IsInactive)
        {
            _initialState = false;
            CurrentState = State.Active;
        }
    }

    /// PRECONDITIONS : An object must not be in shut down mode <para></para>
    /// POSTCONDITIONS : An object that is not already inactive will become inactive again.
    public void Deactivate()
    {
        CheckDead();
        if (IsActive)
        {
            _initialState = false;
            CurrentState = State.Inactive;
        }
    }

    /// PRECONDITIONS : An object must not be in shut down mode or inactive mode. <para></para>
    /// POSTCONDITIONS : No exception is thrown
    private void PreCheck()
    {
        if (IsInactive || IsShutDown)
        {
            throw new Exception("Invalid Request");
        }
    }

    /// PRECONDITIONS : An object must not be in shut down mode. <para></para>
    /// POSTCONDITIONS : No exception is thrown
    private void CheckDead()
    {
        if (IsShutDown)
        {
            throw new Exception("Object is currently shut down");
        }
    }

    /// PRECONDITIONS : None <para></para>
    /// POSTCONDITIONS : Scale Direction will be updated
    private void ToggleDirection()
    {
        Direction = _maxYield > _limit ? ScaleDirection.Up : ScaleDirection.Down;
        _initialState = false;
    }

    /// PRECONDITIONS : None <para></para>
    /// POSTCONDITIONS : Object will not be in initial state and it will be shut down
    protected void ShutDown()
    {
        if (_maxYield == 0)
        {
            CurrentState = State.ShutDown;
            _initialState = false;
        }
    }

    // PRECONDITIONS : scale factor must be nonnegative <para></para>
    /// POSTCONDITIONS : No exception is thrown
    private void GateCheck(double scaleFactor)
    {
        // different from PreCheck -> GateCheck invoked before private data initializations
        if (scaleFactor < 0)
        {
            throw new Exception("scaleFactor must be nonnegative!");
        }
    }
}

/*
 * -----------------------------------------IMPLEMENTATION INVARIANT----------------------------------------------------
 * A Magnifier's states depends on the protected/private variables :
 *  _size :  initial size to be scaled.
 *  _yieldedSize : reflected size after YieldSize() is called.
 *  _scaleFactor : scale factor that will be used to scale a size.
 *  _limit : the minimum number of chances to scale up a size.
 *  _maxYield : the maximum number of chances to yield a size.
 *  _extraMaxYield : this stores the initial maxYield.
 *  _numQueries : the total number of queries to YieldSize().
 *  _initialState : a bool variable to check if object is in initial state.
 *  CurrentState : this variable stores the current state of the object.
 *  Direction : this variable stores the current scale direction of the object.
 *
 * Some of these values depend on each other and one may cause the other to change and it may change the entire state or
 * states of the object. Whether object is in active mode, inactive mode, or shut down will be based on these variables.
 * Statistics will also be stored in these variables.
 *
 * public Magnifier(uint size, double scaleFactor, uint limit, uint maxYield) : Constructor
 * - the constructor takes in four arguments and most of them are restricted with strong types to help the client
 * instantiate the object more easily without entering illegal values. The constructor requires to have a size, a scale
 * factor, a limit and a maxYield (notes and definitions on these values are provided more above)
 * - upon instantiation, we need to pass a gate check, which is invoked before data initializations, this GateCheck()
 * will conduct the final check on the only value that may be illegal which is scale factor, that is, a scale factor
 * needs to be nonnegative.
 *
 * 
 * YieldSize()
 * - client can use this method to scale the size they put when creating the object, though this method does not requires
 * any argument, it is client's responsibilities to control the states of the object, specifically, they need to take
 * good care of the values that get passed into the constructor as those values are key to yield a scaled size the
 * client hopes to see.
 * - the object is required to be in an active state, after this first security check, based on the values passed into
 * the constructor, scale direction will be determined (Up or Down), and the size will be scaled based on this direction.
 * - each time YieldSize() is called, we count that as one query and we also count that as one chance to yield is used,
 * hence the number of queries will go up and maxYield will go down.
 * - in this method, if maxYield becomes 0, object will be shut down after it performs its last yield, therefore, the
 * protected method ShutDown() is triggered in this method along with the last time the size to be scaled before object
 * is shut down.
 * - Notice that if scale direction is Up, we will multiply the current size with the scale factor and scale down means
 * dividing the current size by the scale factor so the result could contains decimal points.
 * - object will not be in initial state if invoked.
 * 
 * 
 * ChangeMaxYield(uint newMaxYield)
 * - client can use this method to explicitly change the maximum number of chances to yield a scaled size, this method
 * is only allowed if an object's maxYield is not exhausted (becomes 0), this method serves as an alternative if the
 * client wish to run the object more without having all data being wiped through Reset() to just bring the object back
 * to life.
 * - object will not be in initial state if invoked.
 * 
 * 
 * Reset()
 * - client can choose to reset an object, this method is extremely powerful, not only it will wipe out all accumulative
 * data, but it will bring object back to its initial state, which is active, so a current dead object can suddenly be
 * alive through this method.
 * - object will return to initial state if invoked.
 *
 * 
 * Activate()
 * - client has the right to request state changes through this method. Activate() is used to explicitly turn an object
 * that is in inactive mode to become active again. Therefore, upon request, object's current state need to be checked
 * if it is in valid state to change. This is done with helper CheckDead(), which checks if object is currently shut
 * down.
 * - object will not be in initial state if invoked.
 *
 * 
 * Deactivate()
 * - client has the right to request state changes through this method. Deactivate() is used to explicitly turn an
 * object that is in active mode to become inactive again. Therefore, upon request, object's current state need to be
 * checked if it is in valid state to change. This is done with helper CheckDead(), which checks if object is currently
 * shut down.
 * - object will not be in initial state if invoked.
 *
 * 
 * PreCheck()
 * - this is a helper method used for error processing, this method targets the methods that require Magnifier object
 * to be in active mode, therefore, it only checks if object is active, if it is not, it will throw an exception
 * indicating being in active mode must be met before performing certain actions.
 *
 * 
 * CheckDead()
 * - this is a helper method used for error processing, this method targets Activate() and Deactivate(), since
 * Activate() and Deactivate() can only toggle the object's two states, it cannot do that if an object is being shut
 * down due to maxYield is exhausted. The only way to bring object back to active mode is to reset everything through
 * Reset() and wipe out all data. Therefore, client may wish to track maxYield and invoke ChangeMaxYield() on time to
 * prevent object being shut down.
 *
 * 
 * ToggleDirection()
 * - this is a private method to help control scale direction, it will determine the scale direction based on limit and
 * maxYield that the client provides when constructing the object.
 * - object will not be in initial state if invoked.
 *
 * 
 * Shutdown()
 * - this is a protected method used for both Magnifier and its child (if possible) to control object's state, object
 * will be immediately shut down if the client uses up all chances to yield a scaled size (or maxYield becomes 0).
 * - object will not be in initial state if invoked.
 *
 * 
 * GateCheck(double scaleFactor)
 * - this is a helper method used for error processing, it will protect the object from being created with invalid scale
 * factor, since a scale factor cannot be negative, the data will pass this check, otherwise, an exception will be
 * thrown.
 */